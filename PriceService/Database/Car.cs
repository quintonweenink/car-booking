using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceService.Database;

public class Car
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CarId { get; set; } 
    
    [Column(TypeName="money")]
    public decimal Price { get; set; }
}