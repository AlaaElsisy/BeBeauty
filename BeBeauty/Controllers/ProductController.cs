using AutoMapper;
using BeBeauty.DTOs.ProductsDTos;
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
    public class ProductController : ControllerBase
    {

        public ProductRepo ProductRepo { get; }
        public IMapper mapper { get; }
        public ProductController(ProductRepo productRepo, IMapper _mapper)
        {
            mapper = _mapper;
            ProductRepo = productRepo;
        }


        [HttpGet]

        public IActionResult GetAllProducts()
        {
            try
            {
                var allproducts = ProductRepo.GetAll();

                if (allproducts == null)
                {
                    return NotFound("No products found");
                }
                List<Displayproduct> products = new List<Displayproduct>();
                foreach (var product in allproducts)
                {
                    var productDto = mapper.Map<Displayproduct>(product);
                    products.Add(productDto);
                }
                if (products.Any())
                {

                    return Ok(products);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




        }
        [HttpGet("{id}")]
        public IActionResult GetProductByid(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid product ID");
                }
                var product1 = ProductRepo.GetById(id);

                if (product1 == null)
                {
                    return NotFound("No product found");
                }

                var productDto = mapper.Map<Displayproduct>(product1);
                return Ok(productDto);



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




        }


        [HttpPost]
        public IActionResult AddProduct([FromBody] AddProduct productDto)
        {
            try
            {
                if (productDto == null)
                {
                    return BadRequest("Invalid product data");
                }
                if (productDto.SalePrice > productDto.Price)
                {
                    ModelState.AddModelError(nameof(productDto.SalePrice), "Sale Price must be less than or equal to Price.");

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


                var product = mapper.Map<Product>(productDto);
                ProductRepo.Add(product);
                ProductRepo.Save();
                return CreatedAtAction("GetProductByid", new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Displayproduct productDto)
        {
            try
            {

                if (productDto == null)
                {
                    return BadRequest("Invalid product data");
                }
                if (id != productDto.ProductId)
                {
                    return BadRequest("id dosent match body id");
                }

                if (productDto.SalePrice > productDto.Price)
                {
                    ModelState.AddModelError(nameof(productDto.SalePrice), "Sale Price must be less than or equal to Price.");
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
                var existingProduct = ProductRepo.GetById(id);
                if (existingProduct == null)
                {
                    return NotFound("No product found");
                }
                var updatedProduct = mapper.Map(productDto, existingProduct);
                ProductRepo.Update(updatedProduct);
                ProductRepo.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid product ID");
                }


                var product = ProductRepo.GetById(id);
                if (product == null)
                {
                    return NotFound("No product found");
                }
                Displayproduct displayproduct = mapper.Map<Displayproduct>(product);
                ProductRepo.Delete(id);
                ProductRepo.Save();
                return Ok(displayproduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("category/{categoryId}")]
        public IActionResult GetProductbyCategory(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                {
                    return BadRequest("Invalid category ID.");
                }

               
                var products = ProductRepo.GetProductsByCategoryId(categoryId);

                if (products == null || !products.Any())
                {
                    return NotFound("No products found in this category.");
                }

            
                var productDtos = mapper.Map<List<Displayproduct>>(products);

                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/search/productname")]
        public IActionResult SearchProducts([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Search term cannot be empty.");
                }

                var products = ProductRepo.GetProductsByName(searchTerm);
                if (products == null || !products.Any())
                {
                    return NotFound("No products found matching the search term.");
                }

                var productDtos = mapper.Map<List<Displayproduct>>(products);
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("all")]
        public IActionResult GetAllProductspages([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var allProducts = ProductRepo.GetAll().ToList();

                if (!allProducts.Any())
                    return NotFound("No products found.");

                var pagedProducts = allProducts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var productDtos = mapper.Map<List<Displayproduct>>(pagedProducts);

                var response = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = allProducts.Count,
                    Data = productDtos
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }


}
