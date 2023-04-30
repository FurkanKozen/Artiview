using Artiview.Article.Application.Adapters;
using Artiview.Article.Application.Dtos;
using Artiview.Article.Application.Repositories;
using Artiview.Article.Domain.Entities;
using Artiview.Article.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Artiview.Article.WebApi.UnitTest.Controllers
{
    public class ArticleControllerTests
    {
        private ILogger<ArticleController> subLogger;
        private IArticleRepository subArticleRepository;
        private IApiAdapter subApiAdapter;

        public ArticleControllerTests()
        {
            this.subLogger = Substitute.For<ILogger<ArticleController>>();
            this.subArticleRepository = Substitute.For<IArticleRepository>();
            this.subApiAdapter = Substitute.For<IApiAdapter>();
        }

        private ArticleController CreateArticleController()
        {
            return new ArticleController(
                this.subLogger,
                this.subArticleRepository,
                this.subApiAdapter);
        }

        [Fact]
        public async Task CreateArticle_ArticleIsValid_Status200()
        {
            var articleController = this.CreateArticleController();
            CreateArticleReqDto createArticleReqDto = new()
            {
                Title = "A title",
                Author = "An author",
                ArticleContent = "Content of the article"
            };

            var result = await articleController.CreateArticle(createArticleReqDto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateArticle_ArticleIsInvalid_Status400()
        {
            var articleController = this.CreateArticleController();
            CreateArticleReqDto createArticleReqDto = new();

            var result = await articleController.CreateArticle(createArticleReqDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteArticle_ArticleDoesNotHaveReview_Status200()
        {
            var articleController = this.CreateArticleController();
            Guid id = Guid.NewGuid();

            subApiAdapter.AnyReviewByArticleIdAsync(id).Returns(false);
            var result = await articleController.DeleteArticle(id);

            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task DeleteArticle_ArticleDoesNotExist_Status400()
        {
            var articleController = this.CreateArticleController();
            Guid id = Guid.NewGuid();

            subApiAdapter.AnyReviewByArticleIdAsync(id).Returns(false);
            subArticleRepository.DeleteArticleAsync(id).Returns(_ => throw new ArgumentException());
            var result = await articleController.DeleteArticle(id);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteArticle_ArticleHasReviews_Status422()
        {
            var articleController = this.CreateArticleController();
            Guid id = Guid.NewGuid();

            subApiAdapter.AnyReviewByArticleIdAsync(id).Returns(true);
            var result = await articleController.DeleteArticle(id);

            var actualStatusCode = (result as StatusCodeResult).StatusCode;
            Assert.Equal(422, actualStatusCode);
        }

        [Fact]
        public async Task UpdateArticle_ArticleIsValid_Status200()
        {
            var articleController = this.CreateArticleController();
            var id = Guid.NewGuid();
            UpdateArticleReqDto updateArticleReqDto = new() { Id = id };

            subArticleRepository.GetArticleByIdAsync(updateArticleReqDto.Id).Returns(new ArticleEntity { Id = id });
            var result = await articleController.UpdateArticle(updateArticleReqDto);

            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task UpdateArticle_UpdateRequestIsNull_Status400()
        {
            var articleController = this.CreateArticleController();
            UpdateArticleReqDto updateArticleReqDto = null;

            var result = await articleController.UpdateArticle(updateArticleReqDto);

            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task UpdateArticle_RequestedArticleDoesNotExist_Status422()
        {
            var articleController = this.CreateArticleController();
            UpdateArticleReqDto updateArticleReqDto = new();

            subArticleRepository.GetArticleByIdAsync(Guid.NewGuid()).Returns((ArticleEntity)null);
            var result = await articleController.UpdateArticle(updateArticleReqDto);

            var actualStatusCode = (result as StatusCodeResult).StatusCode;
            Assert.Equal(422, actualStatusCode);
        }
    }
}
