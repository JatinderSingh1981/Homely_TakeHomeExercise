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
            services.AddOptions();
            services.AddResponseCaching();
            services.AddDbContext<DataContext>();
            services.Configure<AppSettings>(Configuration);
            
            services.AddControllers();
            services.AddAutoMapper(typeof(PropertyListingMapper).Assembly);

            #region DI
            services.AddTransient<PropertyListingResponse>();

            services.AddTransient<IPropertyListingBusiness, PropertyListingBusiness>();
            services.AddTransient<IPropertyListingRepository, PropertyListingRepository>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context)
        {
            if (env.IsDevelopment())
            {
                context.Database.Migrate();
                app.UseDeveloperExceptionPage();
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
