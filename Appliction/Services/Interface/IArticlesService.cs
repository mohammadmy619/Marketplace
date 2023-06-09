using MarcketDataLayer.DTOs.Articles;
using MarcketDataLayer.Entities.Articles;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
   public interface IArticlesService
    {

        public List<ArticlesGroups> GetArticlesGroups();
        public Task<FilterArticlesDTO> FilterArticles(FilterArticlesDTO filter);

        public Task<FilterArticlesGroupsDto> FilterArticlesGroups(FilterArticlesGroupsDto filter);
        public Task<AddorEditArticleGroupResul> AddorEditArticleGroup(AddorEditArticleGroupDTO Group, IFormFile img);
        public Task<ArticlesResult> AddorEDiteArticles(AddOrEditeArticlesDTO articles, long selectedGroups, IFormFile Img, IFormFile mp3, IFormFile video);

        public Task<AddorEditArticleGroupDTO> GetGroupsByID(long GroupsByID);

        public Task<AddOrEditeArticlesDTO> GetArticlesByID(long ArticleID);

        public Task<Articles> GetArticlesDetail(long ArticleID);

        public Task<bool> DeleteArticleGroupByid(long GroupID);

        public Task<bool> DeleteArticleByid(long Articleid);

        public Task<List<Articles>> RelatedArticles(long Articleid);

        public Task<bool> AddComment(long Articleid,long UserId,string text,string name,string Email);


        public Task<FilterCommentDTO> FilterComment(FilterCommentDTO filter);

        public Task<bool> DeleteCommentyid(long Commentid);

        public Task<List<ArticlesGroups>> ArticlesGroupsforIndex();


    }
}
