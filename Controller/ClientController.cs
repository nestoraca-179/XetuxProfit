using System;
using System.Data.Entity;
using System.Linq;
using XetuxProfit.Data;
using XetuxProfit.Models;

namespace XetuxProfit.Controller
{
	public class ClientController
	{
		private static readonly string GENERIC_ID = "01";
		private static readonly string USER_ID    = "SYNC";
		private static readonly string SERVER_ID  = "SYNC SERVER";
		private static readonly string CO_TAB_ID  = "7";
		private static readonly string TIP_PER_ID = "1";
		private static readonly string COUNTRY_ID = "VE";

		public static bool Exists(string id)
		{
			return new ProfitAdmCustomModel().saCliente.AsNoTracking().Any(c => c.co_cli.Trim() == id);
		}

		public static void Add(SalesModel reng)
		{
			using (ProfitAdmCustomModel context = new ProfitAdmCustomModel())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						var sp = context.pInsertarCliente(reng.customer_id, null, null, null, reng.customer_name, GENERIC_ID, GENERIC_ID, GENERIC_ID, null, false, true, 
							true, false, false, false, false, false, false, false, reng.customer_address, null, null, null, null, reng.customer_phone, null, null, 
							DateTime.Now, GENERIC_ID, null, 0, 0, 0, null, GENERIC_ID, 0, 0, 0, null, 0, reng.customer_identification, false, null, null, null, GENERIC_ID, 
							null, null, null, null, null, null, null, null, null, USER_ID, SERVER_ID, null, null, null, false, 1, null, CO_TAB_ID, TIP_PER_ID, COUNTRY_ID,
							null, null, null, false, false, 0, null, null, null, null);
						sp.Dispose();

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();

						while (ex.InnerException != null)
							ex = ex.InnerException;

						throw new Exception($"CLIENT {reng.customer_id} -> {ex.Message}");
					}
				}
			}
		}
	}
}