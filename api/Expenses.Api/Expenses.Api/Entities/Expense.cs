using Expenses.Api.Database;
using Expenses.Api.Model;

namespace Expenses.Api.Entities;

public sealed class Expense
{
    private readonly IDatabaseContext _context;
    
    public ExpenseInfo Info { get; }
    public IReadOnlyList<ExpenseCreditInfo> Creditors { get; }
    public IReadOnlyList<ExpenseDebtInfo> Debtors { get; }
    
    public Expense(
        IDatabaseContext context, 
        ExpenseInfo info, 
        IReadOnlyList<ExpenseCreditInfo> creditors,
        IReadOnlyList<ExpenseDebtInfo> debtors)
    {
        _context = context;
        Info = info;
        Creditors = creditors;
        Debtors = debtors;
    }

    public static async Task<Expense> CreateAsync(IDatabaseContext context, ExpenseInfo info, IReadOnlyList<ExpenseCreditInfo> creditors, IReadOnlyList<ExpenseDebtInfo> debtors)
    {
        using var transaction = context.Connection.BeginTransaction();
        await context.Expenses.CreateAsync(info);
        foreach (var creditor in creditors)
            await context.ExpenseCredits.CreateAsync(creditor);
        foreach (var debtor in debtors)
            await context.ExpenseDebts.CreateAsync(debtor);
        transaction.Commit();
        
        return new Expense(context, info, creditors, debtors);
    }
}