using Artiview.Article.Application.Adapters;
using Artiview.Article.Application.Dtos;
using Artiview.Article.Application.Repositories;
using Artiview.Article.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artiview.Article.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleRepository _articleRepository;
        private readonly IApiAdapter _apiAdapter;

        public ArticleController(ILogger<ArticleController> logger, IArticleRepository articleRepository, IApiAdapter apiAdapter)
        {
            _logger = logger;
            _articleRepository = articleRepository;
            _apiAdapter = apiAdapter;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateArticle(CreateArticleReqDto createArticleReqDto)
        {
            try
            {
                var entityToInsert = new ArticleEntity(
                    createArticleReqDto.Title,
                    createArticleReqDto.Author,
                    createArticleReqDto.ArticleContent
                );
                await _articleRepository.CreateArticleAsync(entityToInsert);
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            var doesArticleHaveReviews = await _apiAdapter.AnyReviewByArticleIdAsync(id);
            if (doesArticleHaveReviews)
                return StatusCode(StatusCodes.Status422UnprocessableEntity);

            try
            {
                await _articleRepository.DeleteArticleAsync(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateArticle(UpdateArticleReqDto updateArticleReqDto)
        {
            if (updateArticleReqDto == null)
                return BadRequest();

            var entityToUpdate = await _articleRepository.GetArticleByIdAsync(updateArticleReqDto.Id);
            if (entityToUpdate == null)
                return StatusCode(StatusCodes.Status422UnprocessableEntity);

            entityToUpdate.Update(
                updateArticleReqDto.Title,
                updateArticleReqDto.Author,
                updateArticleReqDto.ArticleContent,
                updateArticleReqDto.PublishDate,
                updateArticleReqDto.StarCount);

            await _articleRepository.UpdateArticleAsync(entityToUpdate);
            return Ok();
        }

        [EnableQuery]
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<GetArticleResDto>>> GetArticles()
        {
            var articles = await _articleRepository.GetArticlesAsync();
            var result = articles.Select(a => new GetArticleResDto
            {
                Id = a.Id,
                ArticleContent = a.ArticleContent,
                Author = a.Author,
                PublishDate = a.PublishDate,
                Title = a.Title,
                StarCount = a.StarCount
            }).ToArray();

            return Ok(result);
        }

        [HttpGet]
        [Route("anyArticleById")]
        public async Task<ActionResult<bool>> AnyArticleById(Guid id)
        {
            var anyArticle = await _articleRepository.AnyArticleByIdAsync(id);
            return Ok(anyArticle);
        }
    }
}
