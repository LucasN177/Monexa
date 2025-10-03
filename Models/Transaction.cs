using MudBlazor;

namespace monexa.Models;

public class Transaction
{
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Color CategoryColor { get; set; }
    public string Account { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}