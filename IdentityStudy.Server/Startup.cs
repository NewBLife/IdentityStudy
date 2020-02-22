using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityStudy.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加IdentityServer4 Nuget包
            // 使用内存存储，密钥，客户端和资源来配置身份服务器。
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(Config.GetUsers());
            //AddTestUsers 扩展方法在背后做了以下几件事：
            //  为资源所有者密码授权添加支持
            //  添加对用户相关服务的支持，这服务通常为登录 UI 所使用（我们将在下一个快速入门中用到登录 UI）
            //  为基于测试用户的身份信息服务添加支持（你将在下一个快速入门中学习更多与之相关的东西）
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
    }
}
