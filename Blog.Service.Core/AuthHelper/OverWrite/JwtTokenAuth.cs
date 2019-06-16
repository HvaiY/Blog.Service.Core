using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Service.Core.AuthHelper.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Core.AuthHelper.OverWrite
{
    /// <summary>
    /// Jwt 认证的中间件
    /// </summary>
    public class JwtTokenAuth
    {
        // 注入的请求委托 next ，中间件需要一个next才能将管道正常走下去
        private readonly RequestDelegate _next;

        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //检查请求头是否包含 Authorization
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }

            var tokenHeather = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
            try
            {
                //if (tokenHeather.Length >= 128)
                //{
                TokenModelJwt tm = JwtHelper.SerializeJwt(tokenHeather);
                //授权 claim 关键
                var claimList = new List<Claim>();
                var claim = new Claim(ClaimTypes.Role, tm.Role);
                claimList.Add(claim);
                var identity = new ClaimsIdentity(claimList);
                var principal = new ClaimsPrincipal(identity);
                httpContext.User = principal;
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now} middleware wrong:{e.Message}");
            }

            return _next(httpContext);
        }
    }

    // 该类相当于将上面的中间件取了别名称 让配置中直接使用
    public static class MiddlewareHelpers
    {
        public static IApplicationBuilder UseJwtTokenAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtTokenAuth>();
            //也可以直接在config中直接配置 使用 app.UseMiddleware<JwtTokenAuth>();
        }
    }
}