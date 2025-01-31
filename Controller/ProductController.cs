using System;
using System.Data.Entity;
using System.Linq;
using XetuxProfit.Data;
using XetuxProfit.Models;

namespace XetuxProfit.Controller
{
	public class ProductController
	{
		private static readonly string GENERIC_ID = "01";
		private static readonly string USER_ID    = "SYNC";
		private static readonly string SERVER_ID  = "SYNC SERVER";
		private static readonly string TYPE_ID    = "V";
		private static readonly string TAX_ID     = "1";
		private static readonly string WARRANTY   = "n/a";
		private static readonly string UNI_ID     = "UNI";

		public static bool Exists(string id)
		{
			return new ProfitAdmCustomModel().saArticulo.AsNoTracking().Any(a => a.co_art.Trim() == id);
		}

		public static void Add(SalesModel reng)
		{
			using (ProfitAdmCustomModel context = new ProfitAdmCustomModel())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						var sp = context.pInsertarArticulo(reng.product_code, DateTime.Now, reng.product_name, TYPE_ID, false, null, GENERIC_ID, GENERIC_ID, GENERIC_ID,
							GENERIC_ID, GENERIC_ID, null, null, null, false, false, false, false, 0, 0, TAX_ID, null, null, null, null, WARRANTY, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
							0, 0, null, true, null, null, 0, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, null, USER_ID, null, SERVER_ID, null, null);
						sp.Dispose();

						var sp_u = context.pInsertarUnidadArticuloRenglon(reng.product_code, UNI_ID, 1, false, 1, true, true, true, true, false, false, false, 0, null, null,
							null, null, null, null, null, null, USER_ID, null, SERVER_ID, null, null);
						sp_u.Dispose();

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();

						while (ex.InnerException != null)
							ex = ex.InnerException;

						throw new Exception($"PRODUCT {reng.product_code} -> {ex.Message}");
					}
				}
			}
		}

		public static void Add(PurchasesModel reng)
		{
			using (ProfitAdmCustomModel context = new ProfitAdmCustomModel())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						var sp = context.pInsertarArticulo(reng.item_id, DateTime.Now, reng.product_name, TYPE_ID, false, null, GENERIC_ID, GENERIC_ID, GENERIC_ID,
							GENERIC_ID, GENERIC_ID, null, null, null, false, false, false, false, 0, 0, TAX_ID, null, null, null, null, WARRANTY, 0, 0, 0, 0, 0, 0, 0, 0, 0,
							0, 0, null, true, null, null, 0, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, null, USER_ID, null, SERVER_ID, null, null);
						sp.Dispose();

						var sp_u = context.pInsertarUnidadArticuloRenglon(reng.item_id, UNI_ID, 1, false, 1, true, true, true, true, false, false, false, 0, null, null,
							null, null, null, null, null, null, USER_ID, null, SERVER_ID, null, null);
						sp_u.Dispose();

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();

						while (ex.InnerException != null)
							ex = ex.InnerException;

						throw new Exception($"PRODUCT {reng.item_id} -> {ex.Message}");
					}
				}
			}
		}
	}
}