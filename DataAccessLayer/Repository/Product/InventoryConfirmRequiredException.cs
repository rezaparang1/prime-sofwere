using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Product
{
    public class InventoryConfirmRequiredException : Exception
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public int RequestedValue { get; }
        public int CurrentInventory { get; }

        public InventoryConfirmRequiredException(string message, int productId, string productName, int requested, int current)
            : base(message)
        {
            ProductId = productId;
            ProductName = productName;
            RequestedValue = requested;
            CurrentInventory = current;
        }
    }
}
