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
                    Title = "E�itim",
                    Slug = "egitim",
                    OrderNo = 1, ArticleCount=1
                };
                var softwareItem = new Entity.Definition.Category
                {
                    Title = "Yaz�l�m",
                    Slug = "yazilim",
                    OrderNo = 2, ArticleCount=1
                };

                var healthItem = new Entity.Definition.Category
                {
                    Title = "Sa�l�k",
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
                    Title = "E�itim Nedir?",
                    Summary = "E�itim; okullar, kurslar ve �niversiteler vas�tas�yla bireylere hayatta gerekli olan bilgi ve kabiliyetlerin sistematik bir �ekilde verilmesi.",
                    Detail = "E�itim, �nceden belirlenmi� ama�lar do�rultusunda insanlar�n d���ncelerinde, tutum ve davran��lar�nda ve ya�amlar�nda belirli iyile�tirme ve geli�tirmeler sa�lamaya yarayan sistematik bir s�re�tir. Bu s�re�ten ge�en insan�n kazand��� yeni bilgi, beceri ve tutum onun birey olma ve ait olma bilincini art�r�r, ki�ili�ini geli�tirir ve onu daha de�erli k�lar.E�itim insan�n i�inde ya�ad��� bireysel,kurumsal ve toplumsal alanlar� b�t�nle�tirir.�nsan�n mevcut performans�yla arzulanan performans� aras�ndaki fark� kapatmas�na yard�mc� olur.Sahip olduklar�yla sahip olmak istedikleri aras�ndaki fark� azaltmas�n� ve ki�isel b�t�nl��e ula�mas�n� kolayla�t�r�r."
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
                    Title = "Yaz�l�m Nedir?",
                    Summary = "Yaz�l�m, de�i�ik ve �e�itli g�revler yapma ama�l� tasarlanm�� elektronik ayg�tlar�n birbirleriyle haberle�ebilmesini ve uyumunu sa�layarak g�revlerini ya da kullan�labilirliklerini geli�tirmeye yarayan makine komutlar�d�r.",
                    Detail = "En basit tan�m�yla yaz�l�m belirlenmi� bir i�in yap�lmas�n� sa�layan komutlar b�t�n� olarak nitelendirilir. S�z konusu komutlar i�lemci sayesinde i�lev g�ren bir olay �ekline evrilir. Esasen yaz�l�m bilgisayar donan�m�n�n i�lev g�rmesini sa�layan programlar ve kodlamalar b�t�n�d�r. Yaz�l�m sayesinde g�nl�k hayat olduk�a kolayla�m��t�r. Yaz�l�m kullan�larak ak�ll� telefon, televizyon, tablet, bilgisayar, sanayide kullan�lan makine ve ekipmanlar �al���r. Bunlara ek olarak otomotiv, e�itim, bili�im, sa�l�k, e�lence, pazarlama, in�aat, uzay sanayisi ve reklamc�l�k gibi pek �ok sekt�rde de yaz�l�m kullan�lmaktad�r. Yaz�l�m hayat�n her an�nda varl�k g�steren bir teknolojidir."
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
                    Title = "Sa�l�k Nedir?",
                    Summary = "Sa�l�k, sadece hastal�k ve sakatl�k durumunun olmay��� de�il ki�inin bedenen, ruhen ve sosyal y�nden tam bir iyilik halidir. D�nya Sa�l�k �rg�t� sa�l���, \"sadece hastal�klar�n ve rahats�z�klar�n olmay��� de�il,bir b�t�n olarak fiziki,ruhi ve sosyal a��dan iyi olma hali\" olarak a��klar.",
                    Detail = "a�l�kl� olmak, mutlu ve insanca ya�amakla e� de�erdir bir kavramd�r. Ger�ekten de sa�l�k, mutlu bir ya�am�n de�i�mez ve �ncelikli �zelli�idir.Bu nedenle bir yak�n�m�zdan ayr�l�p giderken �sa�l�cakla Kal.� ya da olumsuz bir davran��la kar��la�t���m�zda �Sa�l�k olsun.� , �Her �eyin ba�� sa�l�k.� gibi s�zlerle sa�l�kl� olman�n her �eyden �nce geldini vurgulamak isteriz.��nk�,insan olarak �al��mak,�retmek,b�ylece kendimize ve �evremize yararl� olmak temel amac�m�zd�r.Bunun da ancak sa�l�kl� bir ya�amla sa�lanabilece�ini biliriz ama sa�l�kl� olmak ne demektir ? Hangi insanlar sa�l�kl�d�r yada de�ildir ?Sa�l�k kavram� ki�ilere,toplumlara, �lkelere ve zamana g�re de�i�ebilir.Baz� insanlar s�k�a kar��la�t�klar� ba� a�r�lar�n� �nemsiz bir durum olarak g�rebilir.Oysa ba� a�r�s� ya�anan ortam�n havas�n�n kirlili�i,g�r�lt� kirlili�i vb.bir durumdan kaynaklanabilir.Beyin t�m�r� gibi �nemli bir hastal���n belirtiside olabilir."
                };
                await articleRepo.AddAsync(new List<Entity.Application.Article> { educationArticle,sofwareArticle,healthArticle});
            }









            #endregion


        }
    }
}
