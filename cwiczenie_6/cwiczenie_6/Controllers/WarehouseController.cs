using cwiczenie_6.Repositories;
using cwiczenie_6.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenie_6.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseRepository _warehouseRepository;
    
    public WarehouseController(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct(int idProduct, int idWarehouse, int amount, DateTime createdAt)
    {
        if (_warehouseRepository.getProduct(idProduct) == null) return NotFound("idProduct is null");
        if(_warehouseRepository.getWarehouse(idWarehouse) == null) return NotFound("idWarehouse is null");
        if(amount < 0) return NotFound("Amount cannot be less than zero");

        var order = _warehouseRepository.getOrder(idProduct, amount);
        if (order == null) return NotFound("There is no record in the Order table with idProduct and Amount that match our request");
        if (order.CreatedAt > createdAt) return NotFound("The order creation date is later than the creation date in the request");

        if (order.FulfilledAt != null) return NotFound("FulfilledAt in the Order is not null");
        if (_warehouseRepository.getProduct_Warehouse(order.IdOrder) != null) return NotFound("IdOrder in the Product_Warehouse table is not null");

        _warehouseRepository.UpdateOrder(order, DateTime.Now);
        var product_warehouse = new Product_Warehouse
        {
            IdProduct = idProduct,
            IdWarehouse = idWarehouse,
            IdOrder = order.IdOrder,
            Amount = amount,
            Price = _warehouseRepository.getProduct(idProduct).Price * amount,
            CreatedAt = DateTime.Now
        };
        int id = _warehouseRepository.createProduct_Warehouse(product_warehouse);

        if (id == -1) return StatusCode(StatusCodes.Status400BadRequest);
        return StatusCode(StatusCodes.Status201Created);
    }
}
