using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace ROBOT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //设置读取指定位置nlog.config文件
            NLogBuilder.ConfigureNLog("XmlConfig/nlog.config");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
               //.ConfigureServices(services =>
               //{
               //    services.AddHostedService<VideosWatcher>();
               //});
               //配置使用Nlog
               .UseNLog();

    }
}
