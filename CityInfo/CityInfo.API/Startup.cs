namespace CityInfo.API
{
    using AutoMapper;
    using CityInfo.API.Entities;
    using CityInfo.API.Models;
    using CityInfo.API.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;

    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        // For ASP.Net Core 1.x
        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appSettings.json", false, true)
        //        .AddJsonFile($"appSettings.{env.EnvironmentName}.json", true, true);

        //    Configuration = builder.Build();
        //}

        // For ASP.Net Core 2.x
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    // Let our service support xml as response type, default is json.
                    new XmlDataContractSerializerOutputFormatter()));

#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            // If use way 2 to provide connection string to DBContext, it's unnecessary to add "o => o.UseSqlServer(connectionString)" here.
            var connectionString = Configuration["connectionStrings:cityInfoDBConnectionString"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {
            //loggerFactory.AddConsole();
            //loggerFactory.AddDebug();

            //loggerFactory.AddProvider(new NLogLoggerProvider());
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            cityInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            app.UseMvc();

            Mapper.Initialize(cfg =>
            {
                // Way 1
                cfg.CreateMap<City, CityWithoutPointsOfInterestDto>();
                cfg.CreateMap<City, CityDto>();
                cfg.CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
                cfg.CreateMap<PointOfInterest, PointOfInterestDto>();
                cfg.CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
                cfg.CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();
                // cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>(); // Don't need for AutoMapper 7.x.x
                // Way 2
                //cfg.CreateMap(typeof(City), typeof(CityWithoutPointsOfInterestDto));
            });

            // Convention-based routing
            // Typically for using MVC framework to build a web application with HTML-returning views.
            // For web api, using attribute-based routing.
            //app.UseMvc(config =>
            //{
            //    config.MapRoute(
            //        name: "Default",
            //        template: "{controller}/{action}/{id?}",
            //        defaults: new { controller = "Home", action = "Index" });
            //});

            //app.Run(async (context) =>
            //{
            //    throw new Exception("example exception");
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
