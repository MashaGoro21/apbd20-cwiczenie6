using System.ComponentModel.DataAnnotations;

namespace cwiczenie_6.Models.DTO;

public class Warehouse
{
    [Required]
    public int IdWarehouse { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Address { get; set; }
}