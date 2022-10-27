using System;
using System.Text;

namespace Rato4ka_back.Exceptions
{
    public class ServiceException: Exception
    {
        private readonly string[] _params;
        public ServiceException(string msg, params string[] param)
            : base(msg)
        {
            _params = param;
        }
        public String GetDetailInfo()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0}{1}Additional parameters info:{2}", Message, Environment.NewLine, Environment.NewLine);
            foreach(var param in _params)
            {
                builder.AppendLine(param);
            }
            return builder.ToString();
        }
    }
}
