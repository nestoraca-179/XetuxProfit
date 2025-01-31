namespace XetuxProfit.Models
{
	public class SalesModel
    {
        public string journal_id { get; set; }
        public string order_id { get; set; }
        public string bill_id { get; set; }
        public string bill_number { get; set; }
        public long bill_datetime_long { get; set; }
        public string bill_datetime_string { get; set; }
        public decimal amount_igtf { get; set; }
        public string type_doc { get; set; }
        public string fact_notec { get; set; }
        public string status { get; set; }
        public string sale_voucher { get; set; }
        public string customer_id { get; set; }
        public string suborder_id { get; set; }
        public decimal subtotal { get; set; }
        public decimal discount { get; set; }
        public decimal net_price { get; set; }
        public decimal tax_value { get; set; }
        public decimal service_value { get; set; }
        public decimal tips { get; set; }
        public decimal total { get; set; }
        public decimal tax_percentage { get; set; }
        public decimal exchange_date_day { get; set; }
        public string fiscal_printer_serial { get; set; }
        public string station_bill_id { get; set; }
        public string station_name { get; set; }
        public string customer_name { get; set; }
        public string customer_identification { get; set; }
        public decimal quantity { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_bar_code { get; set; }
        public string product_uoms_name { get; set; }
        public decimal product_sale_price { get; set; }
        public decimal product_net_price { get; set; }
        public decimal product_subtotal { get; set; }
        public decimal product_tax_value { get; set; }
        public decimal product_total { get; set; }
        public string product_type_code { get; set; }
        public string product_type_name { get; set; }
        public decimal product_stock { get; set; }
        public decimal product_discount { get; set; }
        public decimal sale_price_1 { get; set; }
        public decimal sale_price_2 { get; set; }
        public string family_code { get; set; }
        public string family_name { get; set; }
        public long journal_start { get; set; }
        public long journal_end { get; set; }
        public string customer_type { get; set; }
        public string customer_address { get; set; }
        public string customer_phone { get; set; }
        public int tax_id { get; set; }
    }
}