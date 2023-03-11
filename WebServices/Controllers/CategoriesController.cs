using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures.Parameters;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebServices.Controllers
{
    [ApiController]
    [Route("api/Categories")]
    public class CategoriesController : Controller
    {
        private readonly IValidator<CategoryDto> _validator;
        private readonly IRepositoryManager _repository;
        public CategoriesController(IValidator<CategoryDto> validator, IRepositoryManager repository)
        {
            _validator = validator;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryParameters categoryParameters)
        {
            var categories = await _repository.Category.GetAllCategoriesAsync(categoryParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categories.MetaData));
            var categoryDto = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Image = c.Image,
            }).ToList();
            return Ok(categoryDto);
        }

        [HttpGet("{id}", Name = "CategoryById")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(id, trackChanges: false);
            if (category == null) return NotFound();
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Image = category.Image,
                CreatedAt = category.CreatedAt
            };
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category)
        {
            var validationResult = await _validator.ValidateAsync(category);
            if (!validationResult.IsValid) return UnprocessableEntity(validationResult.Errors.Select(e => e.ErrorMessage));

            var categoryEntity = new Category
            {
                Name = category.Name,
                Description = category.Description,
                Image = category.Image
            };
            _repository.Category.CreateCategory(categoryEntity);
            await _repository.SaveAsync();

            return CreatedAtRoute("CategoryById", new { id = categoryEntity.Id }, categoryEntity);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryDto category)
        {
            var validationResult = await _validator.ValidateAsync(category);
            if (!validationResult.IsValid) return UnprocessableEntity(validationResult.Errors.Select(e => e.ErrorMessage));

            var categoryEntity = await _repository.Category.GetCategoryByIdAsync(id, trackChanges: true);
            if (categoryEntity == null) return NotFound();

            categoryEntity.Name = category.Name;
            categoryEntity.Description = category.Description;
            categoryEntity.Image = category.Image;

            _repository.Category.UpdateCategory(categoryEntity);
            await _repository.SaveAsync();

            return CreatedAtRoute("CategoryById", new { id = categoryEntity.Id }, categoryEntity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(id, trackChanges: false);
            if (category == null) return NotFound();
            _repository.Category.DeleteCategory(category);
            await _repository.SaveAsync();
            return NoContent();
        }
        

    }
}
