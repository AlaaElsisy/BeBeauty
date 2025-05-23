using AutoMapper;
using BeBeauty.DTOs.OrdersDTos;
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
    public class OrderController : ControllerBase
    {
        private readonly OrderRepo orderRepo;
        private readonly IMapper mapper;
        public OrderController(OrderRepo orderRepo, IMapper mapper)
        {
            this.orderRepo = orderRepo;
            this.mapper = mapper;
        }


            [HttpGet]
            public IActionResult GetAllOrders()
            {
                try
                {
                    var allOrders = orderRepo.GetAll();

                    if (allOrders == null)
                    {
                        return NotFound("No orders found");
                    }

                    List<DisplayOrder> orders = new List<DisplayOrder>();
                    foreach (var order in allOrders)
                    {
                   
                    var orderDto = mapper.Map<DisplayOrder>(order);
                        orders.Add(orderDto);
                    }

                    if (orders.Any())
                    {
                        return Ok(orders);
                    }
                    return NotFound();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet("{id}")]
            public IActionResult GetOrderById(int id)
            {
                try
                {
                    if (id <= 0)
                    {
                        return BadRequest("Invalid order ID");
                    }

                    var order = orderRepo.GetById(id);

                    if (order == null)
                    {
                        return NotFound("No order found");
                    }

                    var orderDto = mapper.Map<DisplayOrder>(order);
                    return Ok(orderDto);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPost]
            public IActionResult AddOrder([FromBody] AddOrder orderDto)
            {
                try
                {
                    if (orderDto == null)
                    {
                        return BadRequest("Invalid order data");
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

                    var order = mapper.Map<Order>(orderDto);
                    order.OrderDate = DateTime.UtcNow;

                orderRepo.Add(order);
                orderRepo.Save();

                    var result = mapper.Map<DisplayOrder>(order);
                    return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPut("{id}")]
            public IActionResult UpdateOrder(int id, [FromBody] DisplayOrder orderDto)
            {
                try
                {
                    if (orderDto == null)
                    {
                        return BadRequest("Invalid order data");
                    }

                    if (id != orderDto.OrderId)
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

                    var existingOrder = orderRepo.GetById(id);
                    if (existingOrder == null)
                    {
                        return NotFound("No order found");
                    }

                    var updatedOrder = mapper.Map(orderDto, existingOrder);
                orderRepo.Update(updatedOrder);
                orderRepo.Save();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteOrder(int id)
            {
                try
                {
                    if (id <= 0)
                    {
                        return BadRequest("Invalid order ID");
                    }

                    var order = orderRepo.GetById(id);
                    if (order == null)
                    {
                        return NotFound("No order found");
                    }

                    DisplayOrder displayOrder = mapper.Map<DisplayOrder>(order);
                orderRepo.Delete(id);
                orderRepo.Save();

                    return Ok(displayOrder);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet("user/{userId}")]
            public IActionResult GetOrdersByUser(string userId)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        return BadRequest("Invalid user ID");
                    }

                    var orders = orderRepo.GetOrdersByUserId(userId);
                    if (orders == null || !orders.Any())
                    {
                        return NotFound("No orders found for this user");
                    }

                    var orderDtos = mapper.Map<List<DisplayOrder>>(orders);
                    return Ok(orderDtos);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet("status/{status}")]
            public IActionResult GetOrdersByStatus(string status)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(status))
                    {
                        return BadRequest("Status cannot be empty");
                    }

                    var orders = orderRepo.GetOrdersByStatus(status);
                    if (orders == null || !orders.Any())
                    {
                        return NotFound($"No {status} orders found");
                    }

                    var orderDtos = mapper.Map<List<DisplayOrder>>(orders);
                    return Ok(orderDtos);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet("all")]
            public IActionResult GetAllOrdersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
            {
                try
                {
                    var allOrders = orderRepo.GetAll().ToList();

                    if (!allOrders.Any())
                    {
                        return NotFound("No orders found");
                    }

                    var pagedOrders = allOrders
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var orderDtos = mapper.Map<List<DisplayOrder>>(pagedOrders);

                    var response = new
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalCount = allOrders.Count,
                        Data = orderDtos
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


