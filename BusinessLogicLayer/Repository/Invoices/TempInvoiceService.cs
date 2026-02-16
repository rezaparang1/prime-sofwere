using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Invoices;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Invoices
{
    public class TempInvoiceService : ITempInvoiceService
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public TempInvoiceService(IMemoryCache cache)
        {
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // زمان انقضا
        }

        public Task SaveItemsAsync(int invoiceId, List<InvoiceItemDto> items)
        {
            _cache.Set($"invoice-items-{invoiceId}", items, _cacheOptions);
            return Task.CompletedTask;
        }

        public Task<List<InvoiceItemDto>> GetItemsAsync(int invoiceId)
        {
            _cache.TryGetValue($"invoice-items-{invoiceId}", out List<InvoiceItemDto> items);
            return Task.FromResult(items ?? new List<InvoiceItemDto>());
        }

        public Task RemoveAsync(int invoiceId)
        {
            _cache.Remove($"invoice-items-{invoiceId}");
            return Task.CompletedTask;
        }
    }
}
