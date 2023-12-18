using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using GrapeCity.Documents.Word;
using System.Reflection;

namespace Colt.Application.Common.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IOrderService _orderService;

        public DocumentService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<string> CreateInvoiceAsync(int orderId)
        {
            var order = await _orderService.GetByIdWithCustomerAsync(orderId, CancellationToken.None);

            var docName = $"{order.CustomerName} - {order.Id}.docx";
            string inputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources", "InvoiceTemplate.docx");

            var outputPath = $"C:\\Users\\zhere\\OneDrive\\Робочий стіл\\Invoices\\{order.DeliveryDate:dd.MMM yyyy}";

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            ProcessFile(order, inputPath, outputPath, docName);

            return docName;
        }

        private void ProcessFile(OrderDto order, string inputPath, string outputPath, string fileName)
        {
            var doc = new GcWordDocument();
            doc.Load(inputPath);

            doc.DataTemplate.DataSources.Add("ds", order);

            doc.DataTemplate.Process();

            var filePath = Path.Combine(outputPath, fileName);

            doc.Save(filePath);

            RemoveParagraph(filePath);
        }

        private void RemoveParagraph(string outputPath)
        {
            using (var wordDoc = WordprocessingDocument.Open(outputPath, true))
            {
                var mainPart = wordDoc.MainDocumentPart;
                var doc = mainPart.Document;
                var paragraphs = doc.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ToList();

                paragraphs[0].Remove();

                doc.Save();
            }
        }
    }
}
