using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EFcoreServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // add default data,第一次启动的时候调用
            // SeedData.EnsureSeedData(host.Services);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
