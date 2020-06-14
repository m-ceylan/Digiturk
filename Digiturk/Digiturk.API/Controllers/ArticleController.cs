using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using Digiturk.Models.API.Request.Article;
using Digiturk.Repository.Application;
using Digiturk.Repository.Definition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.VisualBasic;
using Digiturk.Models.API.Response.Article;
using Microsoft.AspNetCore.Authorization;
using Digiturk.API.Code;
using Digiturk.Repository.ProjectUser;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Digiturk.Models.Base.Response;

namespace Digiturk.API.Controllers
{

    public class ArticleController : BaseController<ArticleController>
    {
        private readonly ArticleRepository repo;
        private readonly CategoryRepository categoryRepo;
        private readonly UserRepository userRepo;

        public ArticleController(
            ArticleRepository repo,
            CategoryRepository categoryRepo,
            UserRepository userRepo
            )
        {
            this.repo = repo;
            this.categoryRepo = categoryRepo;
            this.userRepo = userRepo;
        }
        [HttpPost]
        public async Task<ActionResult<BaseAPIResponse<LoadArticleResponse>>> Load([FromBody] LoadArticleRequest request)
        {

            var response = new BaseAPIResponse<LoadArticleResponse>();
            response.Data = new LoadArticleResponse();
            var query = repo.GetBy(x => x.PublisDate != null);

            if (string.IsNullOrWhiteSpace(request.SearchTerm))
                query = query.Where(x => x.Title.Contains(request.SearchTerm));

            response.Data.Items = await query.OrderByDescending(x => x.PublisDate).Skip(request.Skip).Take(request.Take).ToListAsync();


            return Ok(response);
        }
        [HttpPost]

        public async Task<ActionResult<BaseAPIResponse<GetArticleResponse>>> Get([FromBody] GetArticleRequest request)
        {
            var response = new BaseAPIResponse<GetArticleResponse>();
            response.Data = new GetArticleResponse();
            response.Data.Item = await repo.GetByIdAsync(request.Id);
            if (response.Data.Item == null)
                return NotFound();

            return Ok(response);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BaseAPIResponse<bool>>> Create([FromBody]CreateArticleRequest request)
        {
            var response = new BaseAPIResponse<bool>();

            var category = await categoryRepo.GetByIdAsync(request.CategoryID);
            if (category == null)
                return BadRequest();

            var owner = await userRepo.GetByIdAsync(CurrentUserID);

            if (owner == null)
                return Forbid();

            var ownerMini = new Entity.ProjectUser.UserMini
            {
                Email = owner.Email,
                FirstName = owner.FirstName,
                FullName = owner.FirstName + " " + owner.LastName,
                LastName = owner.LastName,
                Image = owner.Image,
                Id = owner.Id
               
        };

            category.ArticleCount++;

            var item = new Entity.Application.Article
            {
                CategoryID = request.CategoryID,
                Detail = request.Detail,
                Image = request.Image,
                OwnerID = ownerMini.Id,
                Title = request.Title,
                Summary = request.Summary,
                PublisDate = request.PublisDate,
                Owner=ownerMini,
                Category=category,
                Slug = ToUrlSlug(request.Title)
        };
            await repo.AddAsync(item);
            await categoryRepo.UpdateAsync(category);

            response.Message = "Kayıt başarıyla eklendi.";

            response.Data = true;
            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BaseAPIResponse<bool>>> Update([FromBody]UpdateArticleRequest request)
        {
            var response = new BaseAPIResponse<bool>();

            var item = await repo.GetByIdAsync(request.Id);

            if (item == null)
                return NotFound();

            if (item.OwnerID == CurrentUserID)
                return Forbid();


            item.Detail = request.Detail;
            item.Image = request.Image;
            item.PublisDate = request.PublisDate;
            item.Title = request.Title;
            item.Slug = ToUrlSlug(item.Title);



            if (item.CategoryID!=request.CategoryID)
            {
                var oldCategory = await categoryRepo.GetByIdAsync(item.CategoryID);

                var category = await categoryRepo.GetByIdAsync(request.CategoryID);
                if (category == null)
                    return BadRequest();

                item.Category = category;
                item.CategoryID = item.CategoryID;
                category.ArticleCount++;
                if (oldCategory.ArticleCount>0)
                    oldCategory.ArticleCount--;
                await categoryRepo.UpdateAsync(oldCategory);
                await categoryRepo.UpdateAsync(category);
           
            }


            await repo.UpdateAsync(item);


            response.Message = "Kayıt başarıyla güncellendi.";
            response.Data = true;
            return Ok(response);

        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BaseAPIResponse<bool>>> Delete([FromBody]UpdateArticleRequest request)
        {
            var response = new BaseAPIResponse<bool>();


            var item =await repo.GetByIdAsync(request.Id);
            if (item == null)
                return NotFound();

            if (item.OwnerID != CurrentUserID)
                return Forbid();

            var category = await categoryRepo.GetByIdAsync(item.CategoryID);
            await repo.DeleteAsync(item);

            if (category.ArticleCount>0)
                category.ArticleCount--;

            await categoryRepo.UpdateAsync(category);

            response.Message = "Kayıt başarıyla silindi.";
            response.Data = true;
            return Ok(response);
        }
      
        
        private  string ToUrlSlug(string text)
        {
            return Regex.Replace(Regex.Replace(Regex.Replace(text.Trim().ToLower().Replace(" ", "-").Replace("ö", "o").Replace(".", "").Replace("ç", "c").Replace("ş", "s").Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u"), @"\s+", " "), @"\s", ""), @"[^a-z0-9\s-]", "");
        }


    }
}
