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
            services.ConfigureRepositories(ConnectionString, dataBase);
            services.ConfigureJWTAuthentication();

            services.ConfigureSwagger();
            services.ConfigureCors();


            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserRepository userRepo, CategoryRepository categoryRepo,ArticleRepository articleRepo)
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

                var user = new Entity.ProjectUser.User
                {
                    FirstName = "Mustafa",
                    LastName = "Ceylan",
                    Email = "m.mustafaceylan19@gmail.com",
                    Password = "123456",
                    FullName = "Mustafa Ceylan",
                    Image = ""
                };
                await userRepo.AddAsync(user);

                var educationItem = new Entity.Definition.Category
                {
                    Title = "Eðitim",
                    Slug = "egitim",
                    OrderNo = 1, ArticleCount=1
                };
                var softwareItem = new Entity.Definition.Category
                {
                    Title = "Yazýlým",
                    Slug = "yazilim",
                    OrderNo = 2, ArticleCount=1
                };

                var healthItem = new Entity.Definition.Category
                {
                    Title = "Saðlýk",
                    Slug = "saglik",
                    OrderNo = 3, ArticleCount=1
                };

                await categoryRepo.AddAsync(new List<Entity.Definition.Category> { educationItem, softwareItem, healthItem });

                var owner = new Entity.ProjectUser.UserMini
                {

                    Email = user.Email,
                    Image = user.Image,
                    FullName = user.FullName,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName

                };
                var educationArticle = new Entity.Application.Article
                {
                    Image = "https://setav.org/assets/uploads/2019/04/A272-kapakgo%CC%88rsel.jpg",
                    Owner = owner,
                    Category = educationItem,
                    CategoryID = educationItem.Id,
                    OwnerID = owner.Id,
                    PublisDate = DateTime.Now,
                    Slug = "egitim-nedir",
                    Title = "Eðitim Nedir?",
                    Summary = "Eðitim; okullar, kurslar ve üniversiteler vasýtasýyla bireylere hayatta gerekli olan bilgi ve kabiliyetlerin sistematik bir þekilde verilmesi.",
                    Detail = "Eðitim, önceden belirlenmiþ amaçlar doðrultusunda insanlarýn düþüncelerinde, tutum ve davranýþlarýnda ve yaþamlarýnda belirli iyileþtirme ve geliþtirmeler saðlamaya yarayan sistematik bir süreçtir. Bu süreçten geçen insanýn kazandýðý yeni bilgi, beceri ve tutum onun birey olma ve ait olma bilincini artýrýr, kiþiliðini geliþtirir ve onu daha deðerli kýlar.Eðitim insanýn içinde yaþadýðý bireysel,kurumsal ve toplumsal alanlarý bütünleþtirir.Ýnsanýn mevcut performansýyla arzulanan performansý arasýndaki farký kapatmasýna yardýmcý olur.Sahip olduklarýyla sahip olmak istedikleri arasýndaki farký azaltmasýný ve kiþisel bütünlüðe ulaþmasýný kolaylaþtýrýr."
                };
                var sofwareArticle = new Entity.Application.Article
                {
                    Image = "https://i2.cnnturk.com/i/cnnturk/75/800x400/5de10182bf214412f429b06c.jpg",
                    Owner = owner,
                    Category = softwareItem,
                    CategoryID = softwareItem.Id,
                    OwnerID = owner.Id,
                    PublisDate = DateTime.Now,
                    Slug = "yazilim-nedir",
                    Title = "Yazýlým Nedir?",
                    Summary = "Yazýlým, deðiþik ve çeþitli görevler yapma amaçlý tasarlanmýþ elektronik aygýtlarýn birbirleriyle haberleþebilmesini ve uyumunu saðlayarak görevlerini ya da kullanýlabilirliklerini geliþtirmeye yarayan makine komutlarýdýr.",
                    Detail = "En basit tanýmýyla yazýlým belirlenmiþ bir iþin yapýlmasýný saðlayan komutlar bütünü olarak nitelendirilir. Söz konusu komutlar iþlemci sayesinde iþlev gören bir olay þekline evrilir. Esasen yazýlým bilgisayar donanýmýnýn iþlev görmesini saðlayan programlar ve kodlamalar bütünüdür. Yazýlým sayesinde günlük hayat oldukça kolaylaþmýþtýr. Yazýlým kullanýlarak akýllý telefon, televizyon, tablet, bilgisayar, sanayide kullanýlan makine ve ekipmanlar çalýþýr. Bunlara ek olarak otomotiv, eðitim, biliþim, saðlýk, eðlence, pazarlama, inþaat, uzay sanayisi ve reklamcýlýk gibi pek çok sektörde de yazýlým kullanýlmaktadýr. Yazýlým hayatýn her anýnda varlýk gösteren bir teknolojidir."
                };
                var healthArticle = new Entity.Application.Article
                {
                    Image = "https://www.mediaclick.com.tr/mp-include/uploads/2019/04/yazilim-nedir-4.jpg",
                    Owner = owner,
                    Category = healthItem,
                    CategoryID = healthItem.Id,
                    OwnerID = owner.Id,
                    PublisDate = DateTime.Now,
                    Slug = "saglik-nedir",
                    Title = "Saðlýk Nedir?",
                    Summary = "Saðlýk, sadece hastalýk ve sakatlýk durumunun olmayýþý deðil kiþinin bedenen, ruhen ve sosyal yönden tam bir iyilik halidir. Dünya Saðlýk Örgütü saðlýðý, \"sadece hastalýklarýn ve rahatsýzýklarýn olmayýþý deðil,bir bütün olarak fiziki,ruhi ve sosyal açýdan iyi olma hali\" olarak açýklar.",
                    Detail = "aðlýklý olmak, mutlu ve insanca yaþamakla eþ deðerdir bir kavramdýr. Gerçekten de saðlýk, mutlu bir yaþamýn deðiþmez ve öncelikli özelliðidir.Bu nedenle bir yakýnýmýzdan ayrýlýp giderken “saðlýcakla Kal.” ya da olumsuz bir davranýþla karþýlaþtýðýmýzda “Saðlýk olsun.” , “Her þeyin baþý saðlýk.” gibi sözlerle saðlýklý olmanýn her þeyden önce geldini vurgulamak isteriz.Çünkü,insan olarak çalýþmak,üretmek,böylece kendimize ve çevremize yararlý olmak temel amacýmýzdýr.Bunun da ancak saðlýklý bir yaþamla saðlanabileceðini biliriz ama saðlýklý olmak ne demektir ? Hangi insanlar saðlýklýdýr yada deðildir ?Saðlýk kavramý kiþilere,toplumlara, ülkelere ve zamana göre deðiþebilir.Bazý insanlar sýkça karþýlaþtýklarý baþ aðrýlarýný önemsiz bir durum olarak görebilir.Oysa baþ aðrýsý yaþanan ortamýn havasýnýn kirliliði,gürültü kirliliði vb.bir durumdan kaynaklanabilir.Beyin tümörü gibi önemli bir hastalýðýn belirtiside olabilir."
                };
                await articleRepo.AddAsync(new List<Entity.Application.Article> { educationArticle,sofwareArticle,healthArticle});
            }









            #endregion


        }
    }
}
