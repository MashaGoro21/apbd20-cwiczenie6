using cwiczenie_6.Models.DTO;

namespace cwiczenie_6.Repositories;

public interface IWarehouseRepository
{
    public Order? getOrder(int id, int amount);
    public int UpdateOrder(Order order, DateTime fulfilledAt);

    public Product? getProduct(int id);

    public Warehouse? getWarehouse(int id);

    public Product_Warehouse? getProduct_Warehouse(int id);

    public int createProduct_Warehouse(Product_Warehouse product_Warehouse);
}