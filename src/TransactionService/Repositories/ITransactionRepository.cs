using TransactionService.Models;

namespace TransactionService.Repositories
{
    public interface ITransactionRepository
    {
        Task SaveTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Transaction transaction);
    }
}