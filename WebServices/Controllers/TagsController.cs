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
    [Route("api/Tags")]
    public class TagsController : Controller
    {
        private readonly IValidator<TagDto> _validator;
        private readonly IRepositoryManager _repository;
        public TagsController(IValidator<TagDto> validator, IRepositoryManager repository)
        {
            _validator = validator;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags([FromQuery] TagParameters tagsParameters)
        {
            var tags = await _repository.Tag.GetAllTagsAsync(tagsParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(tags.MetaData));
            var tagDto = tags.Select(c => new TagDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Date = c.Date,
                Location = c.Location,
                Attachments = c.Attachments,
                CreatedAt = c.CreatedAt
            }).ToList();
            return Ok(tagDto);
        }

        [HttpGet("{id}", Name = "TagById")]
        public async Task<IActionResult> GetTag(Guid id)
        {
            var tag = await _repository.Tag.GetTagsByIdAsync(id, trackChanges: false);
            if (tag == null) return NotFound();
            var tagDto = new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                Date = tag.Date,
                Attachments = tag.Attachments,
                Location = tag.Location,
                CreatedAt = tag.CreatedAt
            };
            return Ok(tagDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] TagDto tag)
        {
            var validationResult = await _validator.ValidateAsync(tag);
            if (!validationResult.IsValid) return UnprocessableEntity(validationResult.Errors.Select(e => e.ErrorMessage));

            var tagEntity = new Tag
            {
                Name = tag.Name,
                Description = tag.Description,
                Date = tag.Date,
                Attachments = tag.Attachments,
                Location = tag.Location,
                CreatedAt = tag.CreatedAt
            };
            _repository.Tag.CreateTag(tagEntity);
            await _repository.SaveAsync();

            return CreatedAtRoute("TagById", new { id = tagEntity.Id }, tagEntity);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(Guid id, [FromBody] TagDto tag)
        {
            var validationResult = await _validator.ValidateAsync(tag);
            if (!validationResult.IsValid) return UnprocessableEntity(validationResult.Errors.Select(e => e.ErrorMessage));

            var tagEntity = await _repository.Tag.GetTagsByIdAsync(id, trackChanges: true);
            if (tagEntity == null) return NotFound();

            tagEntity.Name = tagEntity.Name;
            tagEntity.Description = tagEntity.Description;
            tagEntity.Date = tagEntity.Date;
            tagEntity.Attachments = tagEntity.Attachments;
            tagEntity.Location = tagEntity.Location;
            tagEntity.CreatedAt = tagEntity.CreatedAt;

            _repository.Tag.UpdateTag(tagEntity);
            await _repository.SaveAsync();

            return CreatedAtRoute("TagById", new { id = tagEntity.Id }, tagEntity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var tag = await _repository.Tag.GetTagsByIdAsync(id, trackChanges: false);
            if (tag == null) return NotFound();
            _repository.Tag.DeleteTag(tag);
            await _repository.SaveAsync();
            return NoContent();
        }


    }
}
