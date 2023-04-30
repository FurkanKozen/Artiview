using Artiview.Review.Application.Adapters;
using Artiview.Review.Application.Dtos;
using Artiview.Review.Application.Repositories;
using Artiview.Review.Domain.Entities;
using Artiview.Review.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Artiview.Review.WebApi.UnitTest.Controllers
{
    public class ReviewControllerTests
    {
        private ILogger<ReviewController> subLogger;
        private IReviewRepository subReviewRepository;
        private IApiAdapter subApiAdapter;

        public ReviewControllerTests()
        {
            this.subLogger = Substitute.For<ILogger<ReviewController>>();
            this.subReviewRepository = Substitute.For<IReviewRepository>();
            this.subApiAdapter = Substitute.For<IApiAdapter>();
        }

        private ReviewController CreateReviewController()
        {
            return new ReviewController(
                this.subLogger,
                this.subReviewRepository,
                this.subApiAdapter);
        }

        [Fact]
        public async Task CreateReview_ReviewIsValid_Status200()
        {
            var reviewController = this.CreateReviewController();
            Guid articleId = Guid.NewGuid();
            CreateReviewReqDto createReviewReqDto = new()
            {
                ArticleId = articleId,
                ReviewContent = "A review content",
                Reviewer = "A reviewer"
            };

            subApiAdapter.AnyArticleByIdAsync(articleId).Returns(true);
            var result = await reviewController.CreateReview(
                createReviewReqDto);

            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task CreateReview_ReviewIsInvalid_Status200()
        {
            var reviewController = this.CreateReviewController();
            Guid articleId = Guid.NewGuid();
            CreateReviewReqDto createReviewReqDto = new()
            {
                ArticleId = articleId,
            };

            subApiAdapter.AnyArticleByIdAsync(articleId).Returns(true);
            var result = await reviewController.CreateReview(
                createReviewReqDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateReview_ReviewDoesNotBelongToArticle_Status422()
        {
            var reviewController = this.CreateReviewController();
            Guid articleId = Guid.NewGuid();
            CreateReviewReqDto createReviewReqDto = new()
            {
                ArticleId = articleId,
            };

            subApiAdapter.AnyArticleByIdAsync(articleId).Returns(false);
            var result = await reviewController.CreateReview(
                createReviewReqDto);

            var actualStatusCode = (result as StatusCodeResult).StatusCode;
            Assert.Equal(422, actualStatusCode);
        }

        [Fact]
        public async Task DeleteReview_ReviewExists_Status200()
        {
            var reviewController = this.CreateReviewController();

            var result = await reviewController.DeleteReview(Guid.Empty);

            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task DeleteReview_ReviewDoesNotExist_Status400()
        {
            var reviewController = this.CreateReviewController();

            subReviewRepository.DeleteReviewAsync(Guid.Empty).ReturnsForAnyArgs(_ => throw new ArgumentException());
            var result = await reviewController.DeleteReview(Guid.Empty);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateReview_ReviewIsValid_Status200()
        {
            var reviewController = this.CreateReviewController();
            UpdateReviewReqDto updateReviewReqDto = new() { Id = Guid.Empty };

            subReviewRepository.GetReviewByIdAsync(Guid.Empty).ReturnsForAnyArgs(new ReviewEntity());
            var result = await reviewController.UpdateReview(updateReviewReqDto);

            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task UpdateReview_UpdateRequestIsNull_Status400()
        {
            var reviewController = this.CreateReviewController();
            UpdateReviewReqDto updateReviewReqDto = null;

            var result = await reviewController.UpdateReview(updateReviewReqDto);

            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task UpdateReview_RequestedReviewDoesNotExist_Status422()
        {
            var reviewController = this.CreateReviewController();
            UpdateReviewReqDto updateReviewReqDto = new();

            subReviewRepository.GetReviewByIdAsync(Guid.Empty).ReturnsForAnyArgs((ReviewEntity)null);
            var result = await reviewController.UpdateReview(updateReviewReqDto);

            var actualStatusCode = (result as StatusCodeResult).StatusCode;
            Assert.Equal(422, actualStatusCode);
        }
        [Fact]
        public async Task UpdateReview_RequestedArticleIdDoesNotExist_Status422()
        {
            var reviewController = this.CreateReviewController();
            var articleId = Guid.NewGuid();
            UpdateReviewReqDto updateReviewReqDto = new() { ArticleId = articleId };

            subApiAdapter.AnyArticleByIdAsync(articleId).Returns(false);
            var result = await reviewController.UpdateReview(updateReviewReqDto);

            var actualStatusCode = (result as StatusCodeResult).StatusCode;
            Assert.Equal(422, actualStatusCode);
        }
    }
}
