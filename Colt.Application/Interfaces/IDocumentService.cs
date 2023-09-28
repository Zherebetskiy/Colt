namespace Colt.Application.Interfaces
{
    public interface IDocumentService
    {
        Task<string> CreateInvoiceAsync(int orderId);
    }
}
