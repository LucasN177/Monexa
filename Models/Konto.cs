using System.ComponentModel.DataAnnotations;
using monexa.Models.Enums;

namespace monexa.Models;

public class Konto
{
    public int Id { get; set; }
        
    [Required(ErrorMessage = "Kontoname ist erforderlich")]
    public string Name { get; set; } = "";
        
    public AccountType Type { get; set; }
        
    public string IBAN { get; set; } = "";
        
    public decimal Balance { get; set; }
        
    [Required]
    public string Currency { get; set; } = "EUR";
        
    [Required(ErrorMessage = "Bank ist erforderlich")]
    public string BankName { get; set; } = "";
        
    public bool IsActive { get; set; } = true;
        
    public string Description { get; set; } = "";
}