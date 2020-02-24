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
            // �����֤�м��IdentityServer4.AccessTokenValidation Nuget���� API ����,���м������Ҫ�����ǣ�
            //  ��֤�����������ȷ�������Կ����εķ����ߣ�IdentityServer��;
            //  ��֤�����Ƿ�����ڸ� api��Ҳ���� Scope����
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5000";
                    // HTTPS�ڿ��������ر�
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

            // Ӧ��������֤�Ͽɹܵ�
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                // ȫ�̷��ʶ���Ҫ��֤
                .RequireAuthorization();
            });
        }
    }
}
