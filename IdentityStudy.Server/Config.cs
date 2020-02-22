using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityStudy.Server
{
    public class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        // 注册令牌控制访问Scope
        public static IEnumerable<ApiResource> Apis =>
                new List<ApiResource>
                {
                    new ApiResource("api1", "My API")
                };

        // 注册客户端
        public static IEnumerable<Client> Clients =>
                new List<Client>
                {
                    // 客户端认证模式
                    new Client
                    {
                        ClientId = "client",

                        // 没有交互性用户，使用 clientid/secret 实现认证
                        AllowedGrantTypes = GrantTypes.ClientCredentials,

                        // 用于认证的密码
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        // 客户端有权访问的范围（Scopes）
                        AllowedScopes = { "api1" }
                    },
                    // 资源所有者密码授权客户端定义
                    new Client
                    {
                        ClientId = "ro.client",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "api1" }
                    }
                };
    }
}
