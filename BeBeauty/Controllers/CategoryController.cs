using AutoMapper;
using BeBeauty.DTOs.CatecoryDTos;
using BeBeauty.Models;
using BeBeauty.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeBeauty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        public GenericRepo<Category> CategoryRepo { get; }
        public IMapper Mapper { get; }

        public CategoryController(GenericRepo<Category> categoryRepo, IMapper mapper)
        {
            CategoryRepo = categoryRepo;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                var allCategories = CategoryRepo.GetAll();

                if (allCategories == null)
                {
                    return NotFound("No categories found");
                }

                List<DisplayCategory> categories = new List<DisplayCategory>();
                foreach (var category in allCategories)
                {
                    var categoryDto = Mapper.Map<DisplayCategory>(category);
                    categories.Add(categoryDto);
                }

                if (categories.Any())
                {
                    return Ok(categories);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid category ID");
                }

                var category = CategoryRepo.GetById(id);

                if (category == null)
                {
                    return NotFound("No category found");
                }

                var categoryDto = Mapper.Map<DisplayCategory>(category);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddCategory([FromBody] DisplayCategory categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest("Invalid category data");
                }
                var existing = CategoryRepo.GetAll()
                     .Any(c => c.Name.ToLower() == categoryDto.Name.ToLower());

                if (existing)
                {
                    ModelState.AddModelError("Name", "Category name must be unique.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(kvp => kvp.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    return BadRequest(new { Errors = errors });
                }

                var category = Mapper.Map<Category>(categoryDto);
                CategoryRepo.Add(category);
                CategoryRepo.Save();
                return CreatedAtAction("GetCategoryById", new { id = category.CategoryId }, category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] DisplayCategory categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest("Invalid category data");
                }

                if (id != categoryDto.CategoryId)
                {
                    return BadRequest("ID doesn't match body ID");
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(kvp => kvp.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(new { Errors = errors });
                }

                var existingCategory = CategoryRepo.GetById(id);
                if (existingCategory == null)
                {
                    return NotFound("No category found");
                }

                var updatedCategory = Mapper.Map(categoryDto, existingCategory);
                CategoryRepo.Update(updatedCategory);
                CategoryRepo.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid category ID");
                }

                var category = CategoryRepo.GetById(id);
                if (category == null)
                {
                    return NotFound("No category found");
                }

                DisplayCategory displayCategory = Mapper.Map<DisplayCategory>(category);
                CategoryRepo.Delete(id);
                CategoryRepo.Save();
                return Ok(displayCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("search")]
        //public IActionResult SearchCategories([FromQuery] string searchTerm)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(searchTerm))
        //        {
        //            return BadRequest("Search term cannot be empty.");
        //        }

        //        var categories = CategoryRepo.GetCategoriesByName(searchTerm);
        //        if (categories == null || !categories.Any())
        //        {
        //            return NotFound("No categories found matching the search term.");
        //        }

        //        var categoryDtos = Mapper.Map<List<DisplayCategory>>(categories);
        //        return Ok(categoryDtos);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet("all")]
        //public IActionResult GetAllCategoriesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    try
        //    {
        //        var allCategories = CategoryRepo.GetAll().ToList();

        //        if (!allCategories.Any())
        //            return NotFound("No categories found.");

        //        var pagedCategories = allCategories
        //            .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //        var categoryDtos = Mapper.Map<List<DisplayCategory>>(pagedCategories);

        //        var response = new
        //        {
        //            PageNumber = pageNumber,
        //            PageSize = pageSize,
        //            TotalCount = allCategories.Count,
        //            Data = categoryDtos
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
