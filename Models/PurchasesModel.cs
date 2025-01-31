namespace XetuxProfit.Models
{
	public class PurchasesModel
    {
        public string header_id { get; set; }
        public string number_document { get; set; }
        public string number_control { get; set; }
        public string date_document_string { get; set; }
        public string date_reception_string { get; set; }
        public string supplier_id { get; set; }
        public string supplier_name { get; set; }
        public string supplier_rif { get; set; }
        public decimal sub_total_bill { get; set; }
        public decimal tax_percentage_bill { get; set; }
        public decimal tax_bill { get; set; }
        public decimal tax_other_amount { get; set; }
        public decimal discount_bill { get; set; }
        public decimal net_bill { get; set; }
        public decimal total_bill { get; set; }
        public decimal currency_bill { get; set; }
        public string currency_name { get; set; }
        public string item_id { get; set; }
        public string product_name { get; set; }
        public decimal product_quantity { get; set; }
        public decimal product_cost_unit { get; set; }
        public decimal product_cost_total { get; set; }
        public decimal tax_value_item { get; set; }
        public decimal item_tax { get; set; }
        public decimal item_tax_other { get; set; }
        public decimal item_discount { get; set; }
        public decimal item_net_price { get; set; }
        public decimal item_total { get; set; }
        public decimal exchange_date_day { get; set; }
        public decimal exchange_date_day_date_document { get; set; }
    }
}