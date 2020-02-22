using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace IdentityStudy.ConsoleClient
{
    /// <summary>
    /// 可信客户端认证方法一
    /// 客户端授权模式访问API资源
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            // 添加 IdentityModel NuGet 程序包到你的客户端项目
            // 从元数据中发现端口  
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // 请求以获得令牌
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            // 调用API
            var tokenClient = new HttpClient();
            tokenClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await tokenClient.GetAsync("https://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            Console.ReadKey();
        }
    }
}
