using System.ComponentModel.DataAnnotations;

namespace project_k;

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string Country { get; set; }
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }
    [Compare("PasswordConfirmation", ErrorMessage = "Passwords do not match.")]
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
    [CreditCard(ErrorMessage = "Invalid credit card number.")]
    public string CreditCardNumber { get; set; }
    [FileExtensions(Extensions = ".pdf, .doc, .docx", ErrorMessage = "Invalid file format. Only PDF and Word documents are allowed.")]
    public string FilePath { get; set; }
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; }
    [Url(ErrorMessage = "Invalid URL.")]
    public string Website { get; set; }

}

public enum Gender
{
    Male,
    Female,
    Other
}