using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client
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
            services.AddControllersWithViews();
            // Microsoft.AspNetCore.Authentication.OpenIDConnet Nuget包
            // 关闭了 JWT 身份信息类型映射，这样就允许 well-known 身份信息（比如，“sub” 和 “idp”） 无干扰地流过。
            // 这个身份信息类型映射的 “清理” 必须在调用 UseOpenIdConnectAuthentication() 之前完成
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // 指向 Identity Server，指定一个客户端 ID 并且告诉它哪个中间件将会负责本地登陆（也就是 cookies 中间件）
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";

                    // OpenID Connect中间件会自动为您保存令牌（标识，访问和刷新）
                    options.SaveTokens = true;

                    // 同意界面现在要求你提供额外的API和offline access访问作用域
                    // OpenID Connect 包含了一个叫做 “混合流（Hybrid flowe）” 的流，它为我们提供了两方面优点 
                    //  身份令牌通过浏览器频道来传输，这样客户端就能够在做任何工作前验证它；
                    //  如果验证成功了，客户端就会打开一个后端通道来连接令牌服务以检索访问令牌。
                    options.Scope.Add("api1");
                    options.Scope.Add("offline_access");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
            });
        }
    }
}
