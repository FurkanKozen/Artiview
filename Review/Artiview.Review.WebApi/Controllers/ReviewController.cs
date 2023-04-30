using Artiview.Review.Application.Adapters;
using Artiview.Review.Application.Dtos;
using Artiview.Review.Application.Repositories;
using Artiview.Review.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artiview.Review.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewRepository _reviewRepository;
        private readonly IApiAdapter _apiAdapter;
        public ReviewController(ILogger<ReviewController> logger, IReviewRepository reviewRepository, IApiAdapter apiAdapter)
        {
            _logger = logger;
            _reviewRepository = reviewRepository;
            _apiAdapter = apiAdapter;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateReview(CreateReviewReqDto createReviewReqDto)
        {
            var doesArticleExist = await _apiAdapter.AnyArticleByIdAsync(createReviewReqDto.ArticleId);
            if (!doesArticleExist)
                return StatusCode(StatusCodes.Status422UnprocessableEntity);

            try
            {
                var entityToInsert = new ReviewEntity(
                    createReviewReqDto.ArticleId,
                    createReviewReqDto.Reviewer,
                    createReviewReqDto.ReviewContent);
                await _reviewRepository.CreateReviewAsync(entityToInsert);
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            try
            {
                await _reviewRepository.DeleteReviewAsync(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateReview(UpdateReviewReqDto updateReviewReqDto)
        {
            if (updateReviewReqDto == null)
                return BadRequest();

            if (updateReviewReqDto.ArticleId.HasValue)
            {
                var doesArticleExist = await _apiAdapter.AnyArticleByIdAsync(updateReviewReqDto.ArticleId.Value);
                if (!doesArticleExist)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity);
            }

            var entityToUpdate = await _reviewRepository.GetReviewByIdAsync(updateReviewReqDto.Id);
            if (entityToUpdate == null)
                return StatusCode(StatusCodes.Status422UnprocessableEntity);

            entityToUpdate.Update(
                updateReviewReqDto.ArticleId,
                updateReviewReqDto.Reviewer,
                updateReviewReqDto.ReviewContent);

            await _reviewRepository.UpdateReviewAsync(entityToUpdate);
            return Ok();
        }

        [EnableQuery]
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<GetReviewResDto>>> GetReviews()
        {
            var reviews = await _reviewRepository.GetReviewsAsync();
            var result = reviews.Select(r => new GetReviewResDto
            {
                ArticleId = r.ArticleId,
                Id = r.Id,
                ReviewContent = r.ReviewContent,
                Reviewer = r.Reviewer
            }).ToArray();

            return Ok(result);
        }

        [HttpGet]
        [Route("anyReviewByArticleId")]
        public async Task<ActionResult<bool>> AnyReviewByArticleId(Guid articleId)
        {
            var anyArticle = await _reviewRepository.AnyReviewByArticleIdAsync(articleId);
            return Ok(anyArticle);
        }
    }
}
