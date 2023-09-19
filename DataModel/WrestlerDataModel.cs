using System.ComponentModel.DataAnnotations;

public class WrestlerDataModel
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Brand { get; set; }
}