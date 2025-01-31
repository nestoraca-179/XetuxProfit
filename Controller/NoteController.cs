using System;
using System.Data.Entity;
using System.Linq;
using XetuxProfit.Data;
using XetuxProfit.Models;

namespace XetuxProfit.Controller
{
	public class NoteController
	{
		private static readonly string GENERIC_ID = "01";
		private static readonly string USER_ID = "SYNC";
		private static readonly string SERVER_ID = "SYNC SERVER";
		private static readonly string CURR_ID = "USD";
		private static readonly string TYPE_DOC_ID = "N/CR";

		public static bool Exists(string id)
		{
			return new ProfitAdmCustomModel().saDocumentoVenta.AsNoTracking().Any(i => i.co_tipo_doc == TYPE_DOC_ID && i.nro_doc.Trim() == id);
		}

		public static void Add(SalesModel reng)
		{
			using (ProfitAdmCustomModel context = new ProfitAdmCustomModel())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						// PARSEANDO FECHA
						string[] date_parts = reng.bill_datetime_string.Split('-');
						int year = int.Parse(date_parts[0]);
						int month = int.Parse(date_parts[1]);
						int day = int.Parse(date_parts[2]);
						DateTime date_not = new DateTime(year, month, day);

						// MONTOS EN BSD
						decimal total_bruto = reng.subtotal * reng.exchange_date_day;
						decimal monto_imp = reng.tax_value * reng.exchange_date_day;
						decimal total_neto = reng.total * reng.exchange_date_day;
						decimal saldo = total_neto;

						var sp = context.pInsertarDocumentoVenta(TYPE_DOC_ID, reng.fact_notec, reng.customer_id, GENERIC_ID, CURR_ID, null, null, reng.exchange_date_day,
							"N/CR " + reng.customer_id, DateTime.Now, date_not, date_not, false, true, false, TYPE_DOC_ID, reng.bill_id, null, monto_imp, saldo,
							total_bruto, 0, null, null, 0, total_neto, 0, 0, "1", 0, 0, 0, 0, null, reng.sale_voucher, null, 0, 0, 0, 0, 0, 0, 0, null, false, reng.fiscal_printer_serial,
							null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, null, USER_ID, SERVER_ID);
						sp.Dispose();

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();

						while (ex.InnerException != null)
							ex = ex.InnerException;

						throw new Exception($"CREDIT NOTE {reng.customer_id} -> {ex.Message}");
					}
				}
			}
		}
	}
}