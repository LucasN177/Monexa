using MudBlazor;

namespace monexa.Models;

public class FinancialStat
{
    public string Title { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string TrendText { get; set; } = string.Empty;
    public Color TrendColor { get; set; }
    public string Icon { get; set; } = string.Empty;
    public Color IconColor { get; set; }
}