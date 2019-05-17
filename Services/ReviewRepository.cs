﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private BookDbContext _reviewContext;
        public ReviewRepository(BookDbContext reviewContext)
        {
            _reviewContext = reviewContext;
        }

        public bool CreateReview(Review review)
        {
            _reviewContext.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _reviewContext.Remove(review);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.OrderBy(r => r.ReviewText).ToList();
        }

        public bool ReviewExist(int reviewId)
        {
            return _reviewContext.Reviews.Any(r => r.Id == reviewId);
        }

        public bool Save()
        {
            var saved = _reviewContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _reviewContext.Update(review);
            return Save();
        }
    }
}
