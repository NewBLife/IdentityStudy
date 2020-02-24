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
            // Microsoft.AspNetCore.Authentication.OpenIDConnet Nuget��
            // �ر��� JWT �����Ϣ����ӳ�䣬���������� well-known �����Ϣ�����磬��sub�� �� ��idp���� �޸��ŵ�������
            // ��������Ϣ����ӳ��� ������ �����ڵ��� UseOpenIdConnectAuthentication() ֮ǰ���
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // ָ�� Identity Server��ָ��һ���ͻ��� ID ���Ҹ������ĸ��м�����Ḻ�𱾵ص�½��Ҳ���� cookies �м����
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

                    // OpenID Connect�м�����Զ�Ϊ���������ƣ���ʶ�����ʺ�ˢ�£�
                    options.SaveTokens = true;

                    // ͬ���������Ҫ�����ṩ�����API��offline access����������
                    // OpenID Connect ������һ������ ���������Hybrid flowe���� ��������Ϊ�����ṩ���������ŵ� 
                    //  �������ͨ�������Ƶ�������䣬�����ͻ��˾��ܹ������κι���ǰ��֤����
                    //  �����֤�ɹ��ˣ��ͻ��˾ͻ��һ�����ͨ�����������Ʒ����Լ����������ơ�
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
