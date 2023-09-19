using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace project_k.DataModel.ProductModels;

[Table("users")]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("first_name")]
    [StringLength(25)]
    public string? FirstName { get; set; }

    [Column("last_name")]
    [StringLength(25)]
    public string? LastName { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
