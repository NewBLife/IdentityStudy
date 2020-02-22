using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityStudy.UserClientAuthServer
{
    public class Config
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            },
            new TestUser{SubjectId = "88421113", Username = "bob", Password = "bob",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                }
            }
        };

        // 与OAuth 2.0类似，OpenID Connect也使用Scope概念。
        // 同样，Scope代表您想要保护的内容以及客户端想要访问的内容。
        // 与OAuth相比，OIDC中的Scope不仅代表API资源，还代表用户ID，姓名或电子邮件地址等身份资源。
        public static IEnumerable<IdentityResource> Ids =>
                new List<IdentityResource>
                {
                new IdentityResources.OpenId(),//标准 openid（subject id）
                new IdentityResources.Profile()//（名字，姓氏等）
                };

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
                    },
                    // OIDC 隐式流客户端（MVC）
                    new Client
                    {
                        ClientId = "mvc",
                        ClientSecrets = { new Secret("secret".Sha256()) },

                        AllowedGrantTypes = GrantTypes.Code,
                        // 授权确认页开启
                        RequireConsent = true,
                        RequirePkce = true,
                
                        // 登录成功回调处理地址，处理回调返回的数据
                        RedirectUris = { "https://localhost:5005/signin-oidc" },

                        // where to redirect to after logout
                        PostLogoutRedirectUris = { "https://localhost:5005/signout-callback-oidc" },

                        AllowedScopes = new List<string>
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile
                        }
                    }
                };
    }
}
