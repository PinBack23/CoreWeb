using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.AccessTokenValidation;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());
                //.AddTestUsers(Config.GetUsers());

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    //options.ApiSecret = "secret";
                    options.ApiName = "api1";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Route Default Files (index.html) https://wipdeveloper.com/2016/01/09/asp-net-5-static-files-enable-default-files/
            app.UseDefaultFiles();
            //Route zu wwwroot
            app.UseStaticFiles();

            // api
            //app.MapWhen(context => context.Request.Path.Value.StartsWith("/api"), map =>
            //{
            //    map.UseAuthentication();
            //    //map.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            //    //{
            //    //    Authority = "http://localhost:5000",
            //    //    ScopeName = "api1",
            //    //    ScopeSecret = "secret",
            //    //    RequireHttpsMetadata = false,
            //    //    SaveToken = true,
            //    //    EnableCaching = true
            //    //});

            //    map.UseMvc();
            //});

            app.UseIdentityServer();

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //IdentityServerAuthenticationOptions identityServerValidationOptions = new IdentityServerAuthenticationOptions
            //{
            //    Authority = "http://localhost:5000",
            //    ApiSecret = "secret",
            //    ApiName = "api1",
            //    SupportedTokens = SupportedTokens.Both
            //};

            //app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            //{
            //    Authority = "http://localhost:5000",
            //    ScopeName = "api1",
            //    ScopeSecret = "secret",
            //    RequireHttpsMetadata = false,
            //    SaveToken = true,
            //    EnableCaching = true
            //});

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
