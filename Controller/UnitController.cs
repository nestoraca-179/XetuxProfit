using System;
using System.Data.Entity;
using System.Linq;
using XetuxProfit.Data;

namespace XetuxProfit.Controller
{
	public class UnitController
	{
		private static readonly string USER_ID   = "SYNC";
		private static readonly string SERVER_ID = "SYNC SERVER";

		public static bool Exists(string id)
		{
			return new ProfitAdmCustomModel().saUnidad.AsNoTracking().Any(u => u.co_uni.Trim() == id);
		}

		public static void Add(string id)
		{
			using (ProfitAdmCustomModel context = new ProfitAdmCustomModel())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						var sp = context.pInsertarUnidad(id, id, null, null, null, null, null, null, null, null, USER_ID, null, SERVER_ID, null, null);
						sp.Dispose();

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();

						while (ex.InnerException != null)
							ex = ex.InnerException;

						throw new Exception($"UNIT {id} -> {ex.Message}");
					}
				}
			}
		}
	}
}