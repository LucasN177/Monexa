using Microsoft.AspNetCore.Components;
using monexa.Models;
using monexa.Models.Enums;
using MudBlazor;

namespace monexa.Pages;

public partial class Transactions : ComponentBase
{
     private List<Transaction> transactions = new();
    private List<Konto> accounts = new();
    private string searchString = "";
    private int? selectedAccount = null;
    private int? selectedCategory = null;
    private TransactionType? selectedType = null;

    protected override void OnInitialized()
    {
        LoadTestData();
    }

    private void LoadTestData()
    {
        accounts = new List<Konto>
        {
            new Konto { Id = 1, Name = "Hauptkonto" },
            new Konto { Id = 2, Name = "Sparkonto" },
            new Konto { Id = 3, Name = "Tagesgeldkonto" },
            new Konto { Id = 4, Name = "Kreditkarte Premium" },
            new Konto{ Id = 5, Name = "Depot" }
        };

        transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Description = "Gehalt Oktober",
                Amount = 3500.00m,
                Date = new DateTime(2024, 10, 1),
                Type = TransactionType.Einnahme,
                Category = TransactionCategory.Gehalt,
                AccountId = 1,
                Notes = "Monatliches Gehalt"
            },
            new Transaction
            {
                Id = 2,
                Description = "Miete",
                Amount = 950.00m,
                Date = new DateTime(2024, 10, 3),
                Type = TransactionType.Ausgabe,
                Category = TransactionCategory.Wohnen,
                AccountId = 1,
                Notes = "Kaltmiete Wohnung"
            },
            new Transaction
            {
                Id = 3,
                Description = "REWE Einkauf",
                Amount = 127.45m,
                Date = new DateTime(2024, 10, 5),
                Type = TransactionType.Ausgabe,
                Category = TransactionCategory.Lebensmittel,
                AccountId = 1
            },
            new Transaction
            {
                Id = 4,
                Description = "Strom & Gas",
                Amount = 145.00m,
                Date = new DateTime(2024, 10, 5),
                Type = TransactionType.Ausgabe,
                Category = TransactionCategory.Nebenkosten,
                AccountId = 1,
                Notes = "Monatlicher Abschlag"
            },
            new Transaction
            {
                Id = 5,
                Description = "Überweisung auf Sparkonto",
                Amount = 500.00m,
                Date = new DateTime(2024, 10, 6),
                Type = TransactionType.Umbuchung,
                Category = TransactionCategory.Sparen,
                AccountId = 1,
                ToAccountId = 2
            },
            new Transaction
            {
                Id = 6,
                Description = "Tankstelle",
                Amount = 65.80m,
                Date = new DateTime(2024, 10, 8),
                Type = TransactionType.Ausgabe,
                Category = TransactionCategory.Transport,
                AccountId = 4
            },
            new Transaction
            {
                Id = 7,
                Description = "Amazon Bestellung",
                Amount = 89.99m,
                Date = new DateTime(2024, 10, 10),
                Type = TransactionType.Ausgabe,
                Category = TransactionCategory.Shopping,
                AccountId = 4
            },
            new Transaction
            {
                Id = 8,
                Description = "Freelance Projekt",
                Amount = 750.00m,
                Date = new DateTime(2024, 10, 12),
                Type = TransactionType.Einnahme,
                Category = TransactionCategory.SonstigeEinnahmen,
                AccountId = 1,
                Notes = "Webdesign Projekt"
            },
            new Transaction
            {
                Id = 9,
                Description = "Restaurant",
                Amount = 45.50m,
                Date = new DateTime(2024, 10, 14),
                Type = TransactionType.Ausgabe,
                Category = TransactionCategory.Freizeit,
                AccountId = 4
            },
            new Transaction
            {
                Id = 10,
                Description = "Fitnessstudio",
                Amount = 39.90m,
                Date = new DateTime(2024, 10, 15),
                Type = TransactionType.Ausgabe,
                Category = TransactionCategory.Gesundheit,
                AccountId = 1,
                Notes = "Monatsbeitrag"
            }
        };
    }

    private IEnumerable<Transaction> GetFilteredTransactions()
    {
        var filtered = transactions.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            filtered = filtered.Where(t => 
                t.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                (t.Notes != null && t.Notes.Contains(searchString, StringComparison.OrdinalIgnoreCase)));
        }

        if (selectedAccount.HasValue)
        {
            filtered = filtered.Where(t => t.AccountId == selectedAccount.Value);
        }

        if (selectedCategory.HasValue)
        {
            filtered = filtered.Where(t => (int)t.Category == selectedCategory.Value);
        }

        if (selectedType.HasValue)
        {
            filtered = filtered.Where(t => t.Type == selectedType.Value);
        }

        return filtered.OrderByDescending(t => t.Date);
    }

    private IEnumerable<IGrouping<string, Transaction>> GetGroupedTransactions()
    {
        return GetFilteredTransactions()
            .GroupBy(t => t.Date.ToString("MMMM yyyy", new System.Globalization.CultureInfo("de-DE")));
    }

    private void ClearFilters()
    {
        searchString = "";
        selectedAccount = null;
        selectedCategory = null;
        selectedType = null;
    }

    private decimal GetMonthlyIncome()
    {
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;
        return transactions
            .Where(t => t.Type == TransactionType.Einnahme && 
                       t.Date.Month == currentMonth && 
                       t.Date.Year == currentYear)
            .Sum(t => t.Amount);
    }

    private decimal GetMonthlyExpenses()
    {
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;
        return transactions
            .Where(t => t.Type == TransactionType.Ausgabe && 
                       t.Date.Month == currentMonth && 
                       t.Date.Year == currentYear)
            .Sum(t => t.Amount);
    }

    private decimal GetMonthlySaldo()
    {
        return GetMonthlyIncome() - GetMonthlyExpenses();
    }

    private string GetAccountName(int accountId)
    {
        return accounts.FirstOrDefault(a => a.Id == accountId)?.Name ?? "Unbekannt";
    }

    private string GetFormattedAmount(Transaction transaction)
    {
        var prefix = transaction.Type == TransactionType.Einnahme ? "+" : "-";
        return $"{prefix} {transaction.Amount.ToString("C", new System.Globalization.CultureInfo("de-DE"))}";
    }

    private void DeleteTransaction(Transaction transaction)
    {
        transactions.Remove(transaction);
    }

    private Color GetTransactionTypeColor(TransactionType type)
    {
        return type switch
        {
            TransactionType.Einnahme => Color.Success,
            TransactionType.Ausgabe => Color.Error,
            TransactionType.Umbuchung => Color.Info,
            _ => Color.Default
        };
    }

    private Color GetCategoryColor(TransactionCategory category)
    {
        return category switch
        {
            TransactionCategory.Gehalt => Color.Success,
            TransactionCategory.Lebensmittel => Color.Primary,
            TransactionCategory.Wohnen => Color.Info,
            TransactionCategory.Transport => Color.Warning,
            TransactionCategory.Freizeit => Color.Secondary,
            _ => Color.Default
        };
    }

    private string GetCategoryIcon(TransactionCategory category)
    {
        return category switch
        {
            TransactionCategory.Gehalt => Icons.Material.Filled.AttachMoney,
            TransactionCategory.Lebensmittel => Icons.Material.Filled.ShoppingCart,
            TransactionCategory.Wohnen => Icons.Material.Filled.Home,
            TransactionCategory.Transport => Icons.Material.Filled.DirectionsCar,
            TransactionCategory.Freizeit => Icons.Material.Filled.SportsEsports,
            TransactionCategory.Gesundheit => Icons.Material.Filled.LocalHospital,
            TransactionCategory.Shopping => Icons.Material.Filled.ShoppingBag,
            TransactionCategory.Nebenkosten => Icons.Material.Filled.Receipt,
            TransactionCategory.Sparen => Icons.Material.Filled.Savings,
            TransactionCategory.SonstigeEinnahmen => Icons.Material.Filled.TrendingUp,
            TransactionCategory.SonstigeAusgaben => Icons.Material.Filled.MoreHoriz,
            _ => Icons.Material.Filled.Category
        };
    }

    private string GetCategoryText(TransactionCategory category)
    {
        return category switch
        {
            TransactionCategory.Gehalt => "Gehalt",
            TransactionCategory.Lebensmittel => "Lebensmittel",
            TransactionCategory.Wohnen => "Wohnen",
            TransactionCategory.Transport => "Transport",
            TransactionCategory.Freizeit => "Freizeit",
            TransactionCategory.Gesundheit => "Gesundheit",
            TransactionCategory.Shopping => "Shopping",
            TransactionCategory.Nebenkosten => "Nebenkosten",
            TransactionCategory.Sparen => "Sparen",
            TransactionCategory.SonstigeEinnahmen => "Sonstige Einnahmen",
            TransactionCategory.SonstigeAusgaben => "Sonstige Ausgaben",
            _ => category.ToString()
        };
    }

    // Model Classes
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public TransactionCategory Category { get; set; }
        public int AccountId { get; set; }
        public int? ToAccountId { get; set; }
        public string? Notes { get; set; }
    }

    
    
}