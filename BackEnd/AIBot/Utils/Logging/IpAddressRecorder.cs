using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;


namespace Buaa.AIBot.Utils.Logging
{
    public class IpAddressRecorder
    {
        private HttpContextAccessor accessor;

        public ILogger<IpAddressRecorder> Logger { get; }

        public HttpContext HttpContext { get => accessor.HttpContext; }

        public IPAddress Remote { get => HttpContext.Connection.RemoteIpAddress; }

        public IPAddress Local { get => HttpContext.Connection.LocalIpAddress; }

        public IpAddressRecorder(ILogger<IpAddressRecorder> logger)
        {
            this.accessor = new HttpContextAccessor();
            this.Logger = logger;
        }
    }
}
