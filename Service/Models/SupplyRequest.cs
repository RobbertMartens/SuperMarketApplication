using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class SupplyRequest
    {
        public List<ProvisioningProduct> ProvisionProducts { get; set; }
    }

    public class ProvisioningProduct
    {
        public int Barcode { get; set; }
        public int Amount { get; set; }
    }
}
