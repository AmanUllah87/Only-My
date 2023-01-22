using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Pharmacy
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string LotNo { get; set; }
        public double PurchasePrice { get; set; }
        public double SalePrice { get; set; }
        public double WholeSalePrice { get; set; }
        public double Qty { get; set; }
        public double BQty { get; set; }
        

        public double Vat { get; set; }
        public double VatPc { get; set; }
        public double TaxPc { get; set; }
        public double Tax { get; set; }
        public double UnitTotal { get; set; }
      
        public double Tp { get; set; }
        public DateTime ExpireDate { get; set; }



        public int ReOrderQty { get; set; }
        public int IsDiscItem { get; set; }
        public double LessAmt { get; set; }

        public GroupModel Group { get; set; }
        public SupplierModel Supplier { get; set; }

    }
}
