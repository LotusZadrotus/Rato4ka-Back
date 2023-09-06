namespace Rato4ka_back.DTO
{
    public record TokenDTO
    {
        public string Login { get; set; }
        public string Token { get; set; }
        public TokenDTO(string token, string login) { 
            Login = login;
            Token = token;
        }
    }
}
