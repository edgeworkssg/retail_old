using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPIntegration.API.SYNERGIX.request
{
    public class Sales
    {
        public string company_code;
        public InvoiceHeader ar_invoice_header;
        public IList<InvoiceDetail> ar_invoice_detail;
    }

    public class InvoiceDetail
    {
        public int line_item_no; 
        public string item_type;
        public string item_code;
        public string item_remarks;
        public decimal qty;
        public string uom_type;
        public string pack_size_code;
        public decimal unit_price;
        public decimal base_extended_amt;
        public decimal discount_amt;
        public decimal pre_tax_amt;
        public decimal sales_tax_amt;
        public bool is_rounding;
        public string dr_cr_type;
    }

    public class InvoiceHeader
    {
        public string invoice_no;
        public string invoice_date;
        public string customer_code;
        public string currency_code;
        public decimal exchange_rate;
        public string sales_tax_code;
        public string external_remarks;
        public decimal total_discount_amt; 
        public decimal total_pre_tax_amt;
        public decimal total_sales_tax_amt;
        public decimal total_after_tax_amt;
        //public int payment_no;
        public decimal payment_amt;
        public bool generate_receipt;
    }
}
