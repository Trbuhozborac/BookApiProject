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
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });

            }

            return Ok(categoriesDto);
        }

        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound();

            var category = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);

        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody]Category CategoryToCreate)
        {
            if (CategoryToCreate == null)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == CategoryToCreate.Name
            .Trim().ToUpper()).FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", $"Category {CategoryToCreate.Name} already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CreateCategory(CategoryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating {CategoryToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = CategoryToCreate.Id }, CategoryToCreate);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]Category updateCategoryInfo)
        {
            if (updateCategoryInfo == null)
                return BadRequest(ModelState);

            if (categoryId != updateCategoryInfo.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.UpdateCategory(updateCategoryInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updateCategoryInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound(ModelState);

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {categoryToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



    }
}