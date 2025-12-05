using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface.Invoices;
using BusinessLogicLayer.Repository.Bank;
using BusinessLogicLayer.Repository.Invoices;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime_Software.DTO.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Invoices
{
    [Route("api/Invoices/Invoices")]
    [ApiController]
    [Authorize]

    public class Invoices : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Invoices.IInvoicesService _InvoicesService;
        private readonly ILogger<Invoices> _logger;
        public Invoices(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Invoices.IInvoicesService InvoicesService, ILogger<Invoices> logger)
        {
            _currentUser = currentUser;
            _InvoicesService = InvoicesService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] BusinessEntity.Invoices.Invoices model)
        {
            var result = await _InvoicesService.Create(model);
            return Ok(result); 
        }

    }
}
