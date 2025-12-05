using BusinessLogicLayer.Repository.Bank;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Invoices
{
    public class InvoicesService : Interface.Invoices.IInvoicesService
    {
        private readonly DataAccessLayer.Interface.Invoices.IInvoicesRepository _InvoicesRepository;
        private readonly ILogger<InvoicesService> _logger;

        public InvoicesService(DataAccessLayer.Interface.Invoices.IInvoicesRepository invoicesRepository, ILogger<InvoicesService> logger)
        {
            _InvoicesRepository = invoicesRepository;
            _logger = logger;
        }

        public async Task<string> Create(BusinessEntity.Invoices.Invoices invoice)
        {
            _logger.LogInformation("Request to add new Invoice: {@Invoice}", invoice);

            var message = await _InvoicesRepository.AddInvoice(invoice);

            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }

    }
}
