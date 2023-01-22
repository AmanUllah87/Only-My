using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Pharmacy
{
    public class SalesModel
    {
        public int BillId { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; }
        public SupplierModel Supplier { get; set; }
        public PatientModel Customer { get; set; }


        public double TotalItem { get; set; }
        public double NetAmount { get; set; }
        public double TotAmount { get; set; }


        public double TotalVat { get; set; }
        public double TotalComision { get; set; }
        public double TotalLess { get; set; }
        public double TotalPaid { get; set; }
        public string Remarks { get; set; }
        public List<ItemModel> ItemModels { get; set; }




    }
}
