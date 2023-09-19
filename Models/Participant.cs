using System.ComponentModel.DataAnnotations;

public class Participant
{
    [Required]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed")]
    public string FirstName { get; set; }
    [Required]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed")]
    public string LastName { get; set; }
    [StringLength(50, MinimumLength = 2)]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string? Email { get; set; }
    public string Country { get; set; }
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }
}

public enum Gender
{
    Male,
    Female,
    Other
}