using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Dtos;
using BookApiProject.Models;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDto = new List<ReviewDto>();

            foreach(var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    HeadLine = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return Ok(reviewsDto);
        }

        [HttpGet("{reviewId}", Name = "GetReview")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return BadRequest(ModelState);

            var review = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewDto = new ReviewDto()
            {  
                Id = review.Id,
                HeadLine = review.Headline,
                ReviewText = review.ReviewText,
                Rating = review.Rating
            };

            return Ok(reviewDto);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type = typeof(Country))]
        public IActionResult CreateReview([FromBody]Review reviewToCreate)
        {
            if (reviewToCreate == null)
                return BadRequest(ModelState);

            var review = _reviewRepository.GetReviews().Where(r => r.Headline.Trim().ToUpper() ==
            reviewToCreate.Headline.Trim().ToUpper() && r.Id != reviewToCreate.Id).FirstOrDefault();

            if(review != null)
            {
                ModelState.AddModelError("", $"Review {reviewToCreate.Headline} already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.CreateReview(reviewToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating {reviewToCreate.Headline}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReview", new { reviewId = reviewToCreate.Id }, reviewToCreate);


        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReview(int reviewId,[FromBody]Review reviewToUpdate)
        {
            if (reviewToUpdate == null)
                return BadRequest(ModelState);

            if (reviewId != reviewToUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.UpdateReview(reviewToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {reviewToUpdate.Headline}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
                return NotFound();

            var countryToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {countryToDelete.Headline}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }
    }
}