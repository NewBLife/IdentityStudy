using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EFcoreServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // add default data,��һ��������ʱ�����
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
