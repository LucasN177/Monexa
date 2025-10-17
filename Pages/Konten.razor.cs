using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using monexa.Models;
using monexa.Models.Enums;
using MudBlazor;

namespace monexa.Pages;

public partial class Konten : ComponentBase
{
    private List<Konto> accounts = new();
    private string searchString = "";
    private bool dialogVisible = false;
    private bool isEditMode = false;
    private Konto currentAccount = new();
    private MudForm form;
    private bool formValid;
    
    private DialogOptions dialogOptions = new() 
    { 
        MaxWidth = MaxWidth.Medium, 
        FullWidth = true,
        CloseButton = true
    };

    protected override void OnInitialized()
    {
        LoadTestData();
    }

    private void LoadTestData()
    {
        accounts = new List<Konto>
        {
            new Konto
            {
                Id = 1,
                Name = "Hauptkonto",
                Type = AccountType.Girokonto,
                IBAN = "DE89 3704 0044 0532 0130 00",
                Balance = 5432.50m,
                Currency = "EUR",
                BankName = "Sparkasse",
                IsActive = true,
                Description = "Hauptkonto für alltägliche Transaktionen"
            },
            new Konto
            {
                Id = 2,
                Name = "Sparkonto",
                Type = AccountType.Sparkonto,
                IBAN = "DE89 3704 0044 0532 0130 01",
                Balance = 15000.00m,
                Currency = "EUR",
                BankName = "Sparkasse",
                IsActive = true,
                Description = "Rücklagen für größere Anschaffungen"
            },
            new Konto
            {
                Id = 3,
                Name = "Tagesgeldkonto",
                Type = AccountType.Tagesgeld,
                IBAN = "DE89 5001 0517 0123 4567 89",
                Balance = 25000.00m,
                Currency = "EUR",
                BankName = "ING",
                IsActive = true,
                Description = "Tagesgeld mit 3,5% Zinsen"
            },
            new Konto
            {
                Id = 4,
                Name = "Kreditkarte Premium",
                Type = AccountType.Kreditkarte,
                IBAN = "DE89 1001 0010 0123 4567 89",
                Balance = -856.30m,
                Currency = "EUR",
                BankName = "Deutsche Bank",
                IsActive = true,
                Description = "Kreditkarte für Reisen und Online-Einkäufe"
            },
            new Konto
            {
                Id = 5,
                Name = "Depot",
                Type = AccountType.Depot,
                IBAN = "DE89 6001 0075 0123 4567 89",
                Balance = 42500.75m,
                Currency = "EUR",
                BankName = "Trade Republic",
                IsActive = true,
                Description = "Wertpapierdepot für langfristigen Vermögensaufbau"
            },
            new Konto
            {
                Id = 6,
                Name = "Altes Girokonto",
                Type = AccountType.Girokonto,
                IBAN = "DE89 2005 0550 1234 5678 90",
                Balance = 127.85m,
                Currency = "EUR",
                BankName = "Hamburger Sparkasse",
                IsActive = false,
                Description = "Nicht mehr genutztes Konto"
            }
        };
    }

    private decimal GetTotalBalance()
    {
        return accounts.Where(a => a.IsActive).Sum(a => a.Balance);
    }

    private IEnumerable<Konto> GetFilteredAccounts()
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return accounts;

        return accounts.Where(a => 
            a.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            a.BankName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            a.IBAN.Contains(searchString, StringComparison.OrdinalIgnoreCase));
    }

    private void OpenCreateDialog()
    {
        currentAccount = new Konto { IsActive = true, Currency = "EUR" };
        isEditMode = false;
        dialogVisible = true;
        NavigationManager.NavigateTo("konten/neu");
    }

    private void OpenEditDialog(Konto account)
    {
        currentAccount = new Konto
        {
            Id = account.Id,
            Name = account.Name,
            Type = account.Type,
            IBAN = account.IBAN,
            Balance = account.Balance,
            Currency = account.Currency,
            BankName = account.BankName,
            IsActive = account.IsActive,
            Description = account.Description
        };
        isEditMode = true;
        dialogVisible = true;
        NavigationManager.NavigateTo($"konten/{account.Id}/bearbeiten");
    }

    private void CloseDialog()
    {
        dialogVisible = false;
        currentAccount = new();
    }

    private void SaveAccount()
    {
        if (isEditMode)
        {
            var existingAccount = accounts.FirstOrDefault(a => a.Id == currentAccount.Id);
            if (existingAccount != null)
            {
                existingAccount.Name = currentAccount.Name;
                existingAccount.Type = currentAccount.Type;
                existingAccount.IBAN = currentAccount.IBAN;
                existingAccount.Balance = currentAccount.Balance;
                existingAccount.Currency = currentAccount.Currency;
                existingAccount.BankName = currentAccount.BankName;
                existingAccount.IsActive = currentAccount.IsActive;
                existingAccount.Description = currentAccount.Description;
            }
        }
        else
        {
            currentAccount.Id = accounts.Any() ? accounts.Max(a => a.Id) + 1 : 1;
            accounts.Add(currentAccount);
        }
        
        CloseDialog();
    }

    private void DeleteAccount(Konto account)
    {
        // In der echten Anwendung: Bestätigungsdialog zeigen
        accounts.Remove(account);
    }

    private void ViewDetails(Konto account)
    {
        // Hier würde Navigation zur Detailseite erfolgen
    }

    private Color GetAccountTypeColor(AccountType type)
    {
        return type switch
        {
            AccountType.Girokonto => Color.Primary,
            AccountType.Sparkonto => Color.Success,
            AccountType.Tagesgeld => Color.Info,
            AccountType.Kreditkarte => Color.Warning,
            AccountType.Depot => Color.Secondary,
            _ => Color.Default
        };
    }

    private string GetAccountTypeText(AccountType type)
    {
        return type switch
        {
            AccountType.Girokonto => "Girokonto",
            AccountType.Sparkonto => "Sparkonto",
            AccountType.Tagesgeld => "Tagesgeldkonto",
            AccountType.Kreditkarte => "Kreditkarte",
            AccountType.Depot => "Depot",
            _ => type.ToString()
        };
    }
}