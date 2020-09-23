using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
namespace ROBOT
{
    public class NLogHelper:INLogHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<NLogHelper> _logger;

        public NLogHelper(IHttpContextAccessor httpContextAccessor, ILogger<NLogHelper> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public void LogError(Exception ex)
        {
           
        }
    }
}
