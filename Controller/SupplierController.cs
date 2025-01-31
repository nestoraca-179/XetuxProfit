using System;
using System.Data.Entity;
using System.Linq;
using XetuxProfit.Data;
using XetuxProfit.Models;

namespace XetuxProfit.Controller
{
	public class SupplierController
	{
		private static readonly string GENERIC_ID = "01";
		private static readonly string USER_ID = "SYNC";
		private static readonly string SERVER_ID = "SYNC SERVER";
		private static readonly string CO_TAB_ID = "7";
		private static readonly string TIP_PER_ID = "1";
		private static readonly string COUNTRY_ID = "VE";

		public static bool Exists(string id)
		{
			return new ProfitAdmCustomModel().saProveedor.AsNoTracking().Any(c => c.co_prov.Trim() == id);
		}

		public static void Add(PurchasesModel reng)
		{
			using (ProfitAdmCustomModel context = new ProfitAdmCustomModel())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						var sp = context.pInsertarProveedor(reng.supplier_id, reng.supplier_name, GENERIC_ID, GENERIC_ID, false, string.Empty, string.Empty, string.Empty, 
							null, null, DateTime.Now, GENERIC_ID, 0, null, GENERIC_ID, 0, 0, 0, reng.supplier_rif, true, null, null, null, GENERIC_ID, null, int.Parse(TIP_PER_ID), 
							null, CO_TAB_ID, TIP_PER_ID, COUNTRY_ID, null, null, null, null, null, false, false, 0, null, null, null, null, null, null, null, null, USER_ID, 
							null, SERVER_ID, null, null, null, null, null, false);
						sp.Dispose();

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();

						while (ex.InnerException != null)
							ex = ex.InnerException;

						throw new Exception($"SUPPLIER {reng.supplier_id} -> {ex.Message}");
					}
				}
			}
		}
	}
}