using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace project_k.DataModel.ProductModels;

[Table("product")]
public partial class Product
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("product_name")]
    [StringLength(50)]
    public string? ProductName { get; set; }

    [Column("category")]
    [StringLength(10)]
    public string? Category { get; set; }

    [Column("price")]
    public int? Price { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Products")]
    public virtual User? User { get; set; }
}
