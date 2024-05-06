using cwiczenie_6.Models.DTO;
using Microsoft.Data.SqlClient;
namespace cwiczenie_6.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public Order? getOrder(int id, int amount)
    {
        var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdOrder, IdProduct, Amount, CreatedAt, FulfilledAt FROM `Order` WHERE IdProduct = @IdProduct AND Amount = @Amount";
        cmd.Parameters.AddWithValue("@IdProduct", id);
        cmd.Parameters.AddWithValue("@Amount", amount);
        
        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return null;
        
        var order = new Order
        {
            IdOrder = (int)dr["IdOrder"],
            IdProduct = (int)dr["IdProduct"],
            Amount = (int)dr["Amount"],
            CreatedAt = (DateTime)dr["CreatedAt"],
            FulfilledAt = dr["FulfilledAt"] == DBNull.Value ? null : (DateTime?)dr["FulfilledAt"]
        };
        
        return order;

    }
    
    public int UpdateOrder(Order order, DateTime fulfilledAt)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE `Order` SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder AND Amount = @Amount";
        cmd.Parameters.AddWithValue("@IdOrder", order.IdOrder);
        cmd.Parameters.AddWithValue("@Amount", order.Amount);
        cmd.Parameters.AddWithValue("@FulfilledAt", fulfilledAt);
        
        return cmd.ExecuteNonQuery();
    }
    
    public Product? getProduct(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdProduct, Name, Description, Price FROM Product WHERE IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", id);
        
        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return null;
        
        var product = new Product
        {
            IdProduct = (int)dr["IdProduct"],
            Name = (string)dr["Name"],
            Description = (string)dr["Description"],
            Price = (decimal)dr["Price"]
        };
        
        return product;
    }
    

    public Warehouse? getWarehouse(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdWarehouse, Name, Address FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", id);
        
        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return null;
        
        var warehouse = new Warehouse
        {
            IdWarehouse = (int)dr["IdWarehouse"],
            Name = (string)dr["Name"],
            Address = (string)dr["Address"]
        };
        
        return warehouse;
    }

    public Product_Warehouse? getProduct_Warehouse(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdProductWarehouse, IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt FROM Product_Warehouse WHERE IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("@IdOrder", id);
        
        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return null;
        
        var product_Warehouse = new Product_Warehouse
        {
            IdProductWarehouse = (int)dr["IdProductWarehouse"],
            IdWarehouse = (int)dr["IdWarehouse"],
            IdProduct = (int)dr["IdProduct"],
            IdOrder = (int)dr["IdOrder"],
            Amount = (int)dr["Amount"],
            Price = (decimal)dr["Price"],
            CreatedAt = (DateTime)dr["CreatedAt"]
        };
        
        return product_Warehouse;
    }

    public int createProduct_Warehouse(Product_Warehouse product_Warehouse)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
        command.Parameters.AddWithValue("@IdWarehouse", product_Warehouse.IdWarehouse);
        command.Parameters.AddWithValue("@IdProduct", product_Warehouse.IdProduct);
        command.Parameters.AddWithValue("@IdOrder", product_Warehouse.IdOrder);
        command.Parameters.AddWithValue("@Amount", product_Warehouse.Amount);
        command.Parameters.AddWithValue("@Price", product_Warehouse.Price);
        command.Parameters.AddWithValue("@CreatedAt", product_Warehouse.CreatedAt);
        Console.WriteLine("Inserting");
        command.ExecuteNonQuery();
        
        using var cmd = new SqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = "SELECT IdProductWarehouse FROM Product_Warehouse WHERE IdWarehouse = @IdWarehouse AND IdProduct = @IdProduct AND IdOrder = @IdOrder AND Amount = @Amount AND Price = @Price";
        cmd.Parameters.AddWithValue("@IdWarehouse", product_Warehouse.IdWarehouse);
        cmd.Parameters.AddWithValue("@IdProduct", product_Warehouse.IdProduct);
        cmd.Parameters.AddWithValue("@IdOrder", product_Warehouse.IdOrder);
        cmd.Parameters.AddWithValue("@Amount", product_Warehouse.Amount);
        cmd.Parameters.AddWithValue("@Price", product_Warehouse.Price);
        
        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return -1;
        
        return (int)dr["IdProductWarehouse"];
    }
}

