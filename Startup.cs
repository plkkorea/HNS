using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HNS.BusinessSvcs;
using HNS.Entities;
using HNS.Repositories;
using HNS.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HNS
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();

            services.AddDbContext<HNSEntities>(options => options.UseSqlServer(Configuration.GetConnectionString("HNSDbConnection")));

            services.AddIdentity<ApplicationUser, ApplicationUserRole>(options => {
                //// Password settings  
                //options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                //options.Password.RequiredUniqueChars = 1;
            })
                .AddEntityFrameworkStores<HNSEntities>().AddDefaultTokenProviders();

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            #region DataService Resolver
            services.AddScoped<INoticeRepository, NoticeRepository>();
            services.AddScoped<IFaqRepository, FaqRepository>();
            services.AddScoped<IQnaRepository, QnaRepository>();
            services.AddScoped<ITechNoteRepository, TechNoteRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<ISmartXRelatedRepository, SmartXRelatedRepository>();
            services.AddScoped<IPopupLayerRepository, PopupLayerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IHeaderScriptRepository, HeaderScriptRepository>();
            services.AddScoped<IVisitorsInfoRepository, VisitorsInfoRepository>();
            services.AddScoped<IProductCompareRepository, ProductCompareRepository>();
            services.AddScoped<ISiteContentRepository, SiteContentRepository>();

            #endregion

            #region BusinessService Resolver
            services.AddScoped<INoticeService, NoticeService>();
            services.AddScoped<IFaqService, FaqService>();
            services.AddScoped<IQnaService, QnaService>();
            services.AddScoped<ITechNoteService, TechNoteService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<ISmartXRelatedService, SmartXRelatedService>();
            services.AddScoped<IPopupLayerService, PopupLayerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IHeaderScriptService, HeaderScriptService>();
            services.AddScoped<IVisitorsInfoService, VisitorsInfoService>();
            services.AddScoped<IDbRecordService, DbRecordService>();
            services.AddScoped<IProductCompareService, ProductCompareService>();
            services.AddScoped<ISiteContentService, SiteContentService>();
            #endregion

            #region ViewModel Resolver
            services.AddScoped<INoticeViewModel, NoticeViewModel>();
            services.AddScoped<IFaqViewModel, FaqViewModel>();
            services.AddScoped<IQnaViewModel, QnaViewModel>();
            services.AddScoped<ITechNoteViewModel, TechNoteViewModel>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            CultureInfo newCulture = new CultureInfo("ko-KR");//  (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
