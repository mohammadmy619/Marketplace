using MarcketAppliction.Extensions;
using MarcketAppliction.Services.Interface;
using MarcketAppliction.Utils;
using MarcketDataLayer.DTOs.Articles;
using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.Articles;
using MarketPlace.DataLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Implementations
{

    public class  ArticlesService : IArticlesService
    {
        #region Cunstractor
        private IGenericRepository<Articles> _ArticlesRepository;

        private IGenericRepository<ArticlesGroups> _ArticlesGroupsRepository;
        private IGenericRepository<ArticlesComment> _ArticlesCommentRepository;

        public ArticlesService(IGenericRepository<Articles> ArticlesRepository, IGenericRepository<ArticlesGroups> articlesGroupsRepository, IGenericRepository<ArticlesComment> ArticlesCommentRepository)
        {
            _ArticlesRepository = ArticlesRepository;
            _ArticlesGroupsRepository = articlesGroupsRepository;
            _ArticlesCommentRepository = ArticlesCommentRepository;
        }
        #endregion

        public async Task<AddorEditArticleGroupResul> AddorEditArticleGroup(AddorEditArticleGroupDTO Group, IFormFile img)
        {
            if (Group.ArticleGroupID == 0 || Group.ArticleGroupID == null)
            {
                var articlesGroups = new ArticlesGroups
                {
                    GroupTitle = Group.GroupTitle,
                    IsDelete = false,
                    GroupDescriptison = Group.GroupDescriptison,
                    CreateDate = DateTime.Now

                };
                if (img != null && img.IsImage())
                {
                    articlesGroups.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                    img.AddImageToServer(articlesGroups.ImageName, PathExtension.ArticlesGroupsOriginServer, 200, 200, PathExtension.ArticlesGroupsThumbServer);
                }
                await _ArticlesGroupsRepository.AddEntity(articlesGroups);

                await _ArticlesGroupsRepository.SaveChanges();

                return AddorEditArticleGroupResul.succses;
            }
            var getarticlesGroups = await _ArticlesGroupsRepository.GetQuery().Where(s => s.Id == Group.ArticleGroupID).SingleOrDefaultAsync();

            if (getarticlesGroups != null)
            {
                getarticlesGroups.GroupTitle = Group.GroupTitle;
                getarticlesGroups.GroupDescriptison = Group.GroupDescriptison;

                if (img != null && img.IsImage())
                {
                    var x = getarticlesGroups.ImageName;
                    getarticlesGroups.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                    img.AddImageToServer(getarticlesGroups.ImageName, PathExtension.ArticlesGroupsOriginServer, 200, 200, PathExtension.ArticlesGroupsThumbServer, x);
                }

                _ArticlesGroupsRepository.EditEntity(getarticlesGroups);

                await _ArticlesGroupsRepository.SaveChanges();

                return AddorEditArticleGroupResul.succses;
            }



            return AddorEditArticleGroupResul.Error;
        }
        public async Task<AddOrEditeArticlesDTO> GetArticlesByID(long ArticleID)
        {
            var getArticle = await _ArticlesRepository.GetQuery().Where(x => x.Id == ArticleID).Select(x => new AddOrEditeArticlesDTO
            {

                ArticleId = x.Id,

                Title = x.Title,

                Text = x.Text,

                ImageAlt = x.ImageAlt,

                name = x.name,

                Tags = x.Tags,

                ArticlesGroupsId = x.ArticlesGroupsId,

                Mp3name = x.Mp3name,

                ImageName = x.ImageName,

                Videoname = x.Videoname,



            }).AsQueryable().AsNoTracking().SingleOrDefaultAsync();
            if (getArticle == null) { return null; }
            return getArticle;
        }

        public async Task<AddorEditArticleGroupDTO> GetGroupsByID(long GroupsByID)
        {
            var getArticle = await _ArticlesGroupsRepository.GetQuery().Where(x => x.Id == GroupsByID).Select(x=> new AddorEditArticleGroupDTO
            {
                ArticleGroupID = x.Id,
                GroupDescriptison = x.GroupDescriptison,
                GroupTitle = x.GroupTitle,
                imgname = x.ImageName,

            }).SingleOrDefaultAsync();


            if (getArticle == null) { return null; }
            return getArticle;




        }

        public async Task<ArticlesResult> AddorEDiteArticles(AddOrEditeArticlesDTO articles, long selectedGroups, IFormFile Img, IFormFile mp3, IFormFile video)
        {
            if (articles.ArticleId == null || articles.ArticleId == 0)
            {
                var newArticles = new Articles
                {
                    Visit = 1000,
                    ImageAlt = articles.ImageAlt,
                    DateTime = DateTime.Now,
                    name = articles.name,
                    Tags = articles.Tags,
                    ArticlesGroupsId = selectedGroups,
                    Text = articles.Text,
                    Title = articles.Title,
                    IsDelete = false,

                    CreateDate = DateTime.Now
                };
                if (Img != null && Img.IsImage())
                {
                    newArticles.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);



                    Img.AddImageToServer(newArticles.ImageName, PathExtension.ArticlesOriginServer, 200, 200, PathExtension.ArticlesThumbServer);


                }
                if (mp3 != null && mp3.FileName != null)
                {
                    //newArticles.Mp3name = Guid.NewGuid().ToString() + Path.GetExtension(mp3.FileName);

                    //var filepath = Path.Combine("wwwroot/mp3/Articles/");
                    //using(var stream = new FileStream(filepath, FileMode.Create))
                    //{
                    //    await mp3.CopyToAsync(stream);
                    //}


                    string imagePath = "";
                    newArticles.Mp3name = NameGenerator.GenerateUniqCode() + Path.GetExtension(mp3.FileName);
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mp3/Articles", newArticles.Mp3name);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        mp3.CopyTo(stream);
                    }
                    //using (var strem = System.IO.File.Create(PathExtension.ArticlesOriginServer))
                }

                if (video != null && video.FileName != null)
                {
                    string imagePath = "";
                    newArticles.Videoname = NameGenerator.GenerateUniqCode() + Path.GetExtension(video.FileName);
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/video/Articles/", newArticles.Videoname);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        video.CopyTo(stream);
                    }
                    //using (var strem = System.IO.File.Create(PathExtension.ArticlesVideoOriginServer))
                    //{
                    //    newArticles.Videoname = Guid.NewGuid().ToString() + Path.GetExtension(video.FileName);


                    //    await video.CopyToAsync(strem);
                    //}
                }

                await _ArticlesRepository.AddEntity(newArticles);

                await _ArticlesRepository.SaveChanges();

                return ArticlesResult.succses;
            }
            else
            {
                var editarticles = await _ArticlesRepository.GetQuery().Where(s => s.Id == articles.ArticleId).SingleOrDefaultAsync();

                editarticles.Id = (long)articles.ArticleId;
                editarticles.ImageAlt = articles.ImageAlt;
                editarticles.ArticlesGroupsId = selectedGroups;
                editarticles.Tags = articles.Tags;
                editarticles.Text = articles.Text;
                editarticles.Title = articles.Title;
                editarticles.name = articles.name;

                if (Img != null && Img.IsImage())
                {
                    var deleteimg = editarticles.ImageName;
                    editarticles.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);
                    Img.AddImageToServer(editarticles.ImageName, PathExtension.ArticlesOriginServer, 200, 200, PathExtension.ArticlesThumbServer, deleteimg);


                }
                if (mp3 != null && mp3.FileName != null)
                {
                    if(editarticles.Mp3name != null)
                    {
                        string mp3Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mp3/Articles", editarticles.Mp3name);



                        if (File.Exists(mp3Path))
                        {
                            File.Delete(mp3Path);
                        }

                    }

                    string imagePath = "";
                    editarticles.Mp3name = NameGenerator.GenerateUniqCode() + Path.GetExtension(mp3.FileName);
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mp3/Articles", editarticles.Mp3name);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        mp3.CopyTo(stream);
                    }
                    //using (var strem = System.IO.File.Create(PathExtension.ArticlesOriginServer))
                }

                if (video != null && video.FileName != null)
                {
                    if (editarticles.Videoname != null)
                    {

                        string VideoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/video/Articles/", editarticles.Videoname);



                        if (File.Exists(VideoPath))
                        {
                            File.Delete(VideoPath);
                        }

                    }
                 

                    string imagePath = "";
                    editarticles.Videoname = NameGenerator.GenerateUniqCode() + Path.GetExtension(video.FileName);
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/video/Articles/", editarticles.Videoname);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        video.CopyTo(stream);
                    }
         
                }

                _ArticlesRepository.EditEntity(editarticles);
                await _ArticlesRepository.SaveChanges();

                return ArticlesResult.succses;


            }




            return ArticlesResult.error;
        }

        public async Task<bool> DeleteArticleByid(long Articleid)
        {
            var GetArticles = await _ArticlesRepository.GetQuery().Where(x => x.Id == Articleid).SingleOrDefaultAsync();

            if (GetArticles == null) return false;

            UploadImageExtension.DeleteImage(GetArticles.ImageName, PathExtension.ArticlesOriginServer, PathExtension.ArticlesThumbServer);
            if (GetArticles.Mp3name != null)
            {
                string mp3Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mp3/Articles", GetArticles.Mp3name);



                if (File.Exists(mp3Path))
                {
                    File.Delete(mp3Path);
                }

            }
            if (GetArticles.Videoname != null)
            {
                string VideoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/video/Articles/", GetArticles.Videoname);



                if (File.Exists(VideoPath))
                {
                    File.Delete(VideoPath);
                }
            }





            _ArticlesRepository.DeleteEntity(GetArticles);

            await _ArticlesRepository.SaveChanges();


            return true;


        }

        public async Task<bool> DeleteArticleGroupByid(long GroupID)
        {
            var GetArticleGroup = await _ArticlesGroupsRepository.GetQuery().Where(x => x.Id == GroupID).SingleOrDefaultAsync();

            if (GetArticleGroup == null) return false;

            UploadImageExtension.DeleteImage(GetArticleGroup.ImageName, PathExtension.ArticlesGroupsOriginServer, PathExtension.ArticlesGroupsThumbServer);

            _ArticlesGroupsRepository.DeleteEntity(GetArticleGroup);

            await _ArticlesGroupsRepository.SaveChanges();


            return true;
        }

        public async Task<FilterArticlesDTO> FilterArticles(FilterArticlesDTO filter)
        {
            var query = _ArticlesRepository.GetQuery().Where(x => !x.IsDelete).Include(u => u.ArticlesGroups).AsNoTracking().AsQueryable();


            if (!string.IsNullOrEmpty(filter.search))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.search}%") || EF.Functions.Like(s.ImageAlt, $"%{filter.search}%") || EF.Functions.Like(s.Tags, $"%{filter.search}%") ||
                EF.Functions.Like(s.Text, $"%{filter.search}%") ||
                EF.Functions.Like(s.name, $"%{filter.search}%") ||
                EF.Functions.Like(s.ArticlesGroups.GroupTitle, $"%{filter.search}%") ||
                EF.Functions.Like(s.ArticlesGroups.GroupDescriptison, $"%{filter.search}%"));

            if(!string.IsNullOrEmpty(filter.GroupName))
            {
                query = query.Where(x => x.ArticlesGroups.GroupTitle == filter.GroupName);
            }
            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetArticles(allEntities);
        }

     
        public async Task<FilterArticlesGroupsDto> FilterArticlesGroups(FilterArticlesGroupsDto filter)
        {
            var query =  _ArticlesGroupsRepository.GetQuery().Where(x => !x.IsDelete).Include(u => u.Articles).AsNoTracking().AsQueryable();


            if (!string.IsNullOrEmpty(filter.search))
                query = query.Where(s => EF.Functions.Like(s.GroupTitle, $"%{filter.search}%") || EF.Functions.Like(s.GroupDescriptison, $"%{filter.search}%"));

            //|| s.GroupDescriptison, $"%{filter.search}%"

            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetArticlesGroups(allEntities);
        }

        public List<ArticlesGroups> GetArticlesGroups()
        {
            var GetArticlesGroups = _ArticlesGroupsRepository.GetQuery().AsQueryable().AsNoTracking().ToList();


            return GetArticlesGroups;
        }

        public async Task<Articles> GetArticlesDetail(long ArticleID)
        {
            var GetArticlesDetail = await _ArticlesRepository.GetQuery().Where(s => s.Id == ArticleID).Include(x => x.ArticlesGroups).AsNoTracking().FirstOrDefaultAsync();

            GetArticlesDetail.Visit += 1;
            GetArticlesDetail.ArticlesGroups.GroupDescriptison += "   ";

            _ArticlesRepository.EditEntity(GetArticlesDetail);

          await  _ArticlesRepository.SaveChanges();
            return await _ArticlesRepository.GetQuery().Where(x => x.Id == ArticleID &&x.IsDelete==false).Include(r=>r.ArticlesGroups).Include(s=>s.ArticlesComment).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<List<Articles>> RelatedArticles(long Articleid)
        {
            var RelatedArticles = await _ArticlesRepository.GetQuery().Where(x => x.Id != Articleid &&!x.IsDelete).Include(x => x.ArticlesGroups).Include(x=>x.ArticlesComment).AsNoTracking().ToListAsync();

            return RelatedArticles;

        }

        public async Task<bool> AddComment(long Articleid, long UserId, string text, string name, string Email)
        {
            if(Articleid !=0 &&text != null)
            {
                var newComment = new ArticlesComment
                {
                    ArticlesId = Articleid,
                    UserId = UserId,
                    Text = text,
                    Name = name,
                    Email = Email,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    DateTime = DateTime.Now,
                };

             await   _ArticlesCommentRepository.AddEntity(newComment);
              await  _ArticlesCommentRepository.SaveChanges();
                return true;
            }

            return false;
            
        }

        public async Task<FilterCommentDTO> FilterComment(FilterCommentDTO filter)
        {
            var query = _ArticlesCommentRepository.GetQuery().Where(x => !x.IsDelete).Include(u => u.Articles).Include(s=>s.User).AsQueryable();


            if (!string.IsNullOrEmpty(filter.search))
                query = query.Where(s => EF.Functions.Like(s.Text, $"%{filter.search}%") || EF.Functions.Like(s.Name, $"%{filter.search}%") || EF.Functions.Like(s.Email, $"%{filter.search}%") ||
                EF.Functions.Like(s.Articles.name, $"%{filter.search}%") ||
                EF.Functions.Like(s.Articles.Text, $"%{filter.search}%") ||
                EF.Functions.Like(s.Articles.Title, $"%{filter.search}%") ||
                EF.Functions.Like(s.Articles.Tags, $"%{filter.search}%"));

         
            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetComment(allEntities);
        }

        public async Task<bool> DeleteCommentyid(long Commentid)
        {
            var res =await _ArticlesCommentRepository.GetQuery().Where(s => s.Id == Commentid).SingleOrDefaultAsync();

            if (res == null) return false;

          await  _ArticlesCommentRepository.DeleteEntity(res.Id);

            await _ArticlesCommentRepository.SaveChanges();

            return true;
            
        }

        public async Task<List<ArticlesGroups>>  ArticlesGroupsforIndex()
        {
            var groups = await _ArticlesGroupsRepository.GetQuery().Where(x=>x.IsDelete==false).OrderBy(x=>x.CreateDate).ToListAsync();

            return groups;
        }
    }
}
