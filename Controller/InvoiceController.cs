using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using XetuxProfit.Data;
using XetuxProfit.Models;

namespace XetuxProfit.Controller
{
	public class InvoiceController
	{
		private static readonly string GENERIC_ID  = "01";
		private static readonly string USER_ID     = "SYNC";
		private static readonly string SERVER_ID   = "SYNC SERVER";
		private static readonly string CURR_ID     = "USD";
		private static readonly string UNI_ID      = "UNI";
		private static readonly string TYPE_DOC_ID = "FACT";

		public static bool Exists(string id, bool sell)
		{
			if (sell)
				return new ProfitAdmCustomModel().saFacturaVenta.AsNoTracking().Any(i => i.doc_num.Trim() == id);
			else
				return new ProfitAdmCustomModel().saFacturaCompra.AsNoTracking().Any(i => i.doc_num.Trim() == id);
		}

		public static void Add(List<SalesModel> rengs)
		{
			using (ProfitAdmCustomModel context = new ProfitAdmCustomModel())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						// PRIMER RENGLON PARA DATOS DE FACTURA
						SalesModel reng = rengs[0];

						// PARSEANDO FECHA
						string[] date_parts = reng.bill_datetime_string.Split('-');
						int year = int.Parse(date_parts[0]);
						int month = int.Parse(date_parts[1]);
						int day = int.Parse(date_parts[2]);
						DateTime date_inv = new DateTime(year, month, day);

						// MONTOS EN BSD
						decimal total_bruto = reng.subtotal * reng.exchange_date_day;
						decimal monto_imp = reng.tax_value * reng.exchange_date_day;
						decimal total_neto = reng.total * reng.exchange_date_day;
						decimal saldo = total_neto;

						// FACTURA DE VENTA
						var sp = context.pInsertarFacturaVenta(reng.bill_id, null, reng.customer_id, GENERIC_ID, CURR_ID, GENERIC_ID, GENERIC_ID, GENERIC_ID, date_inv, 
							date_inv, DateTime.Now, false, "0", reng.exchange_date_day, reng.sale_voucher, "0", 0, "0", 0, saldo, total_bruto, monto_imp, 0, 0, 0, 0, 0, 
							total_neto, null, null, null, false, false, null, reng.fiscal_printer_serial, null, null, false, null, null, null, null, null, null, null, 
							null, USER_ID, null, null, null, SERVER_ID);
						sp.Dispose();

						int n = 1;
						foreach (SalesModel r in rengs)
						{
							// MONTOS EN BSD (R)
							string tip_imp = r.product_tax_value > 0 ? "1" : "7";
							decimal prec_vta_r = Math.Round(r.product_sale_price, 2) * reng.exchange_date_day;
							decimal monto_imp_r = Math.Round(r.product_tax_value, 2) * reng.exchange_date_day;
							decimal reng_neto_r = Math.Round(r.product_net_price, 2) * reng.exchange_date_day;

							// RENGLON
							var sp_r = context.pInsertarRenglonesFacturaVenta(n, r.bill_id, r.product_code, null, UNI_ID, null, GENERIC_ID, GENERIC_ID, tip_imp, null, null, 
								r.quantity, 0, prec_vta_r, null, 0, reng_neto_r, r.quantity, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, null, null, r.tax_percentage, 0, 0, monto_imp_r, 
								0, 0, 0, 0, 0, null, null, null, USER_ID, null, null, SERVER_ID);
							sp_r.Dispose();
							n++;
						}

						// DOCUMENTO DE VENTA
						var sp_d = context.pInsertarDocumentoVenta(TYPE_DOC_ID, reng.bill_id, reng.customer_id, GENERIC_ID, CURR_ID, null, null, reng.exchange_date_day, 
							"FACTURA " + reng.customer_id, DateTime.Now, date_inv, date_inv, false, true, false, TYPE_DOC_ID, reng.bill_id, null, monto_imp, saldo, 
							total_bruto, 0, null, null, 0, total_neto, 0, 0, "1", 0, 0, 0, 0, null, reng.sale_voucher, null, 0, 0, 0, 0, 0, 0, 0, null, false, reng.fiscal_printer_serial, 
							null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, null, USER_ID, SERVER_ID);
						sp_d.Dispose();

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();

						while (ex.InnerException != null)
							ex = ex.InnerException;

						throw new Exception($"SALE INVOICE {rengs[0].bill_id} -> {ex.Message}");
					}
				}
			}
		}

		public static void Add(List<PurchasesModel> rengs)
		{
			using (ProfitAdmCustomModel context = new ProfitAdmCustomModel())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						// PRIMER RENGLON PARA DATOS DE FACTURA
						PurchasesModel reng = rengs[0];

						// PARSEANDO FECHA
						string[] date_parts = reng.date_document_string.Split('-');
						int year = int.Parse(date_parts[0]);
						int month = int.Parse(date_parts[1]);
						int day = int.Parse(date_parts[2]);
						DateTime date_inv = new DateTime(year, month, day);

						// MONTOS EN BSD
						decimal total_bruto = reng.net_bill * reng.exchange_date_day_date_document;
						decimal monto_imp = reng.tax_bill * reng.exchange_date_day_date_document;
						decimal total_neto = reng.total_bill * reng.exchange_date_day_date_document;
						decimal saldo = total_neto;

						// FACTURA DE COMPRA
						var sp = context.pInsertarFacturaCompra(reng.header_id, reng.number_document, string.Empty, reng.supplier_id, GENERIC_ID, CURR_ID, GENERIC_ID, reng.number_control, 
							null, date_inv, date_inv, DateTime.Now, false, "0", reng.exchange_date_day_date_document, null, saldo, total_bruto, total_neto, 0, 0, 0, 0, 0, 
							monto_imp, 0, 0, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null, USER_ID, null, SERVER_ID, true);
						sp.Dispose();

						int n = 1;
						foreach (PurchasesModel r in rengs)
						{
							// MONTOS EN BSD (R)
							string tip_imp = r.item_tax > 0 ? "1" : "7";
							decimal cost_unit_r = Math.Round(r.product_cost_unit, 2) * reng.exchange_date_day;
							decimal monto_imp_r = Math.Round(r.item_tax, 2) * reng.exchange_date_day;
							decimal reng_neto_r = Math.Round(r.product_cost_total, 2) * reng.exchange_date_day;

							// RENGLON
							var sp_r = context.pInsertarRenglonesFacturaCompra(n, r.header_id, r.item_id, null, UNI_ID, null, GENERIC_ID, tip_imp, null, null, null, null, 
								null, null, reng_neto_r, cost_unit_r, 0, reng.product_quantity, 0, 0, reng.tax_value_item, 0, 0, monto_imp_r, 0, 0, 0, 0, 0, 0, null, false, 
								0, 0, 0, 0, 0, 0, 0, 0, 0, reng.product_quantity, 0, null, null, USER_ID, null, null, SERVER_ID, 0, 0, 0, null);
							sp_r.Dispose();
							n++;
						}

						// DOCUMENTO DE COMPRA
						var sp_d = context.pInsertarDocumentoCompra(TYPE_DOC_ID, reng.header_id, reng.number_document, CURR_ID, reng.supplier_id, GENERIC_ID, TYPE_DOC_ID, null, 
							reng.header_id, null, null, null,false, true, 0, "FACTURA " + reng.supplier_id, "1", null, null, DateTime.Now, date_inv, date_inv, total_neto, reng.exchange_date_day_date_document, 
							reng.tax_percentage_bill, 0, 0, monto_imp, 0, 0, total_bruto, 0, 0, saldo, 0, 0, 0, 0, null, null, null, 0, 0, null, null, reng.number_control, 
							null, null, null, null, null, null, null, null, null, null, null, USER_ID, SERVER_ID, true);
						sp_d.Dispose();

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();

						while (ex.InnerException != null)
							ex = ex.InnerException;

						throw new Exception($"PURCHASE INVOICE {rengs[0].header_id} -> {ex.Message}");
					}
				}
			}
		}
	}
}