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
            // ���IdentityServer4 Nuget��
            // ʹ���ڴ�洢����Կ���ͻ��˺���Դ��������ݷ�������
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(Config.GetUsers());
            //AddTestUsers ��չ�����ڱ����������¼����£�
            //  Ϊ��Դ������������Ȩ���֧��
            //  ��Ӷ��û���ط����֧�֣������ͨ��Ϊ��¼ UI ��ʹ�ã����ǽ�����һ�������������õ���¼ UI��
            //  Ϊ���ڲ����û��������Ϣ�������֧�֣��㽫����һ������������ѧϰ������֮��صĶ�����
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
