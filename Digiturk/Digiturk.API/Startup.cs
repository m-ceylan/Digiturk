using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digiturk.API.Code;
using Digiturk.Entity.Application;
using Digiturk.Repository.Application;
using Digiturk.Repository.Definition;
using Digiturk.Repository.ProjectUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Digiturk.API
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

            string ConnectionString = this.Configuration.GetConnectionString("mongoDB");
            string dataBase = "digiturkArticle";

            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(opts => opts.SuppressModelStateInvalidFilter = true);
            services.ConfigureRepositories(ConnectionString,dataBase);
            services.ConfigureJWTAuthentication();
          
            services.ConfigureSwagger();
            services.ConfigureCors();


            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env,UserRepository userRepo,CategoryRepository categoryRepo)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API Version (V 1.0)");
            });

            app.UseMvc();
            #region Default Data

            if (!await userRepo.AnyAsync(x => x.Email == "m.mustafaceylan19@gmail.com"))
            {
                await userRepo.AddAsync(new Entity.ProjectUser.User
                {
                    FirstName = "Mustafa",
                    LastName = "Ceylan",
                    Email = "m.mustafaceylan19@gmail.com",
                    Password = "123456"

                });
            }
            
            if (!await categoryRepo.AnyAsync(x=>x.Title=="Eðitim"))
            {

                var educationItem = new Entity.Definition.Category {
                 Title="Eðitim",
                  Slug="egitim",
                   OrderNo=1
                };
                var softwareItem = new Entity.Definition.Category
                {
                    Title = "Yazýlým",
                    Slug = "yazilim",
                    OrderNo = 2
                };

                var healthItem = new Entity.Definition.Category
                {
                    Title = "Saðlýk",
                    Slug = "saglik",
                    OrderNo = 3
                };

                await categoryRepo.AddAsync(new List<Entity.Definition.Category> { educationItem,softwareItem,healthItem });

            }






            #endregion


        }
    }
}
