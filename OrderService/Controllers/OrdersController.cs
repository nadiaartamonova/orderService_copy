using CommonModels.Models.Order;
using CommonModels.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Data;
using OrderService.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrdersService _service;

        public OrdersController(OrderContext context, ILogger<OrdersController> logger, 
            IOrdersService service)
        {
            _context = context;
            _logger = logger;
            this._service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            _logger.LogInformation("Getting all orders");
            return await _context.Orders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id, bool addClient, bool addProduct)
        {
            
            var order = await _context.Orders.FindAsync(id);
            /*
            if (addClient)
            {
                order.Client = await ClientService.GetAsync(order.Client_Id);
            }
            if (addProduct)
            {
                var productIds = order.Rows.Select(x => x.ProductId).ToList();
                ICollection<Product> products = await ProductService.GetProductsAsync(productIds);
                foreach (var item in products)
                {
                    
                }
            }
            */
            if (order == null)
            {
                _logger.LogWarning($"Order with id {id} not found");
                return NotFound();
            }

            return order;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                _logger.LogWarning($"Order with id {id} not found");
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order with id {id} deleted successfully");
            return Ok(new { Message = "Deleted successfully" });
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for order");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for order");
                return BadRequest(ModelState);
            }

            await _service.CreateOrder(order);

            _logger.LogInformation("Order created successfully");
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var storedOrder = await _context.Orders.FindAsync(id);
            if (storedOrder == null)
            {
                _logger.LogWarning($"Order with id {id} not found");
                return NotFound();
            }

            storedOrder.Client_Id = order.Client_Id;
            storedOrder.Products_List_Id = order.Products_List_Id;
            storedOrder.Status_Name = order.Status_Name;
            storedOrder.Update_Date = DateTime.UtcNow;

            _context.Entry(storedOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation($"Order with id {id} updated successfully");
            return NoContent();
        }

    }


}
