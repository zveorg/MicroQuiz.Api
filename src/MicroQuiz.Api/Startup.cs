using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using MicroQuiz.Api.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace MicroQuiz.Api
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
            var authConfig = new AuthenticationConfig();
            Configuration.Bind("Authentication", authConfig);

            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = authConfig.Authority;
                o.ApiName = authConfig.ApiName;
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = authConfig.ApiSecret;
            };

            services.AddAuthentication()
                .AddIdentityServerAuthentication(authConfig.AuthenticationProviderKey, options);

            services
                .AddOcelot(Configuration)
                .AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("MicroQuiz API (Gateway)");
                });
            });

            app.UseOcelot().Wait();
        }
    }
}
