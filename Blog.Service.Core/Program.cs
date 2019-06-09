using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Blog.Service.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // .UseKestrel() //web寄宿服务  都内置了可以查看源码->CreateDefaultBuilder
                // .UseContentRoot(Directory.GetCurrentDirectory())
                // .UseIISIntegration() //调试用服务器
                .UseStartup<Startup>();
    }
}