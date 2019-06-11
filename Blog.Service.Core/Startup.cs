using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Blog.Service.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // 设置 apiVersion
            services.AddApiVersioning(option => option.ReportApiVersions = true);
            #region Swagger 服务配置

            // 获取安装 Swashbuckle.AspNetCore 包
#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info()
                {
                    Version = "v0.1.0",
                    Title = "Blog.Core.API",
                    Description = "Blog 采用NetCore 开发的API",
                    TermsOfService = "NONE",
                    Contact = new Contact() { Name = "Blog.Core", Email = "337646685@qq.com", Url = "待定" }
                });

                // 增加Swagger 文档api说明支持
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Blog.Service.Core.xml");
                Console.WriteLine(xmlPath);
                c.IncludeXmlComments(xmlPath, true); //默认第二个参数是false 是控制器的注释
            });

            //注册服务 Swagger 使用
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";

                // 注意: 只有在通过url段进行版本控制时，才需要此选项。替代格式还可以用于控制路由模板中API版本的格式。
                options.SubstituteApiVersionInUrl = true;
            });
#else
            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();
#endif

            #endregion Swagger 服务配置
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region Swagger 中间件启用

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                //Todo 无法获取版本暂时写死
                foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{apiVersionDescription.GroupName}/swagger.json", apiVersionDescription.GroupName.ToLowerInvariant());
                }
                //写死的swagger首页
                //  c.SwaggerEndpoint($"/swagger/v1/swagger.json", "ApiHelp V1");
                //c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件，
                ////这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问localhost:8001/index.html即可
                //c.IndexStream = () =>
                //    GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Blog.Service.Code.index.html");
                c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件，
            });

            #endregion Swagger 中间件启用

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}