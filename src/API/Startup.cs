using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Business.API;
using Common.API;
using ViewModels.API;
using Mapper.API;
using DBContext.API;
using Microsoft.EntityFrameworkCore;
using Repository.API;
using Infrastructure.API;

namespace API
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
            services
                .AddDbContext<DataContext>()
                .AddAutoMapper(typeof(PropertyListingMapper).Assembly);

            #region DI
            //Moved all the services to new extension methods
            services.AddInfrastructure(Configuration);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context)
        {
            if (env.IsDevelopment())
            {
                context.Database.Migrate();
                app.UseDeveloperExceptionPage();

                #region Add Swagger only in Development mode

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Take Home Exercise - API");
                });
                #endregion
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
