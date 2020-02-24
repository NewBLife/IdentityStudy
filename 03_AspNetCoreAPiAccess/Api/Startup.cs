using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // 添加认证中间件IdentityServer4.AccessTokenValidation Nuget包到 API 宿主,该中间件的主要工作是：
            //  验证输入的令牌以确保它来自可信任的发布者（IdentityServer）;
            //  验证令牌是否可用于该 api（也就是 Scope）。
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5000";
                    // HTTPS在开发环境关闭
                    options.RequireHttpsMetadata = false;

                    options.Audience = "api1";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // 应用启动认证认可管道
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                // 全程访问都需要认证
                .RequireAuthorization();
            });
        }
    }
}
