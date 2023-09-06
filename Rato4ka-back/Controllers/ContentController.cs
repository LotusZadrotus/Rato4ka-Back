using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rato4ka_back.Models;

namespace Rato4ka_back.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> _logger;
        private readonly DBContext _context;

        public ContentController(ILogger<ContentController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Contents>> GetAllContent()
        {
            try
            {
                var toReturn = await _context.Contents.Select(v=>new
                {
                    id = v.Id,
                    name = v.Name,
                    desc = v.Decription,
                    image = v.Image,
                    releaseDate = v.ReleaseDate
                }).ToListAsync();
                if (toReturn.Count == 0)
                {
                    return new NotFoundObjectResult(toReturn);
                }

                return new JsonResult(toReturn);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new BadRequestResult();
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Contents>> GetContents(int page)
        {
            try
            {
                var toReturn = await _context.Contents.Select(v=>new
                {
                    id = v.Id,
                    name = v.Name,
                    // tags = v.TagsIds,
                    desc = v.Decription,
                    releaseDate = v.ReleaseDate
                }).OrderBy(x=>x.id).Skip((page-1)*10).Take(10).ToListAsync();
                if (toReturn.Count == 0)
                {
                    return new NotFoundObjectResult(toReturn);
                }

                return new JsonResult(toReturn);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e,"");
                return new BadRequestResult();
            }
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Contents>> GetContentById(int id)
        {
            try
            {
                var toReturn = await _context.Contents.Where(x=>x.Id==id).Include(v => v.User).Select(x=>new
                    {
                        id = x.Id,
                        name = x.Name,
                        createdAt = x.CreatedAt,
                        releaseDate = x.ReleaseDate,
                        //tags = x.TagsIds == null ? x.TagsIds : new []{""},
                        description = x.Decription,
                        User = new {x.User.Id, x.User.Name, x.User.Avatar}
                    }).ToListAsync();
                return new JsonResult(toReturn);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("[action]/{name}")]
        public async Task<ActionResult<Contents>> GetContentByName(string name)
        {
            try
            {
                Regex regex = new Regex(@$"[{name}]");
                _logger.LogInformation(regex.ToString());
                var toReturn = await _context.Contents.Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    releaseDate = x.ReleaseDate,
                    desc = x.Decription,
                    image = x.Image
                }).ToListAsync();
                toReturn = toReturn.Where(x => regex.IsMatch(x.name)).ToList();
                if (toReturn.Count == 0)
                {
                    return new NotFoundObjectResult(toReturn);
                }
                return new JsonResult(toReturn);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Something went wrong in method GetContentByName: {e.Message}");
                return new BadRequestResult();
                
            }
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public async Task<ActionResult> CreateContent([FromBody] Contents content)
        {
            try
            {
                if (content.Name is null)
                    return new BadRequestObjectResult("Name is null");

                content.CreatorId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
                content.User = null;
                content.CreatedAt = null;
                await _context.Contents.AddAsync(content);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, e, message:"");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public async Task<ActionResult> UpdateContent([FromBody] Contents content)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
                if (!content.CreatorId.Equals(userId)) return new BadRequestObjectResult("U aren't owner of content");
                _context.Entry(content).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, e, message:"Error in method UpdateContent");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DeleteContent(int id)
        {
            try
            {
                // _logger.Log(LogLevel.Information, User.IsInRole("Admin").ToString());
                // _logger.Log(LogLevel.Information, User.Claims.First(x=>x.Type=="id").Value);
                // // foreach (var claim in User.Claims)
                // // {
                // //     _logger.Log(LogLevel.Information, claim.Type + claim.Value);
                // // }
                int userId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
                var item = await _context.Contents.FirstOrDefaultAsync( x =>
                    x.Id == id && x.CreatorId == userId );
                if (item is null)
                {
                    return new NotFoundObjectResult($"Can't find content with id: {id} or you are not owner");
                }

                _context.Remove(item);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Content was deleted");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Warning, e, message:"");
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpGet]
        [Route("image/{url}")]
        public async Task<ActionResult> GetImage(string url)
        {
            try
            {
                var item = await _context.Contents.Select(x=>new {x.Id, x.Image}).FirstAsync();
                if (item.Image == null)
                {
                    var info = new FileInfo(@"assets/defaul-img.jpg");
                    byte[] data = new byte[info.Length];
                    await using (FileStream fs = info.OpenRead())
                    {
                        fs.Read(data, 0, data.Length);
                    }
                    
                    return File(data, "image/jpeg");
                }
                return File(item.Image, "image/jpeg");
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}