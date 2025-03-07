using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XetuxProfit.Controller;
using XetuxProfit.Models;

namespace XetuxProfit
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            LogController.WriteLog("INFO", "INICIO MIGRACION");

            // URL Ventas
            string url_v = "http://26.6.138.104:9090/xconnect/api/ExtractionData/SalesFull?dateFrom=20241230&dateEnd=20251230";
            // string url_v = "http://190.216.234.233:8080/xconnect/api/ExtractionData/SalesFull?dateFrom=20220101&dateEnd=20220131";
            
            // URL Compras
            string url_c = "http://26.6.138.104:9090/xconnect/api/ExtractionData/PurchaseFull?dateFrom=20241230&dateEnd=20251230";
            // string url_c = "http://190.216.234.233:8080/xconnect/api/ExtractionData/PurchaseFull?dateFrom=20220101&dateEnd=20220131";

            try
            {
                // Realizando HTTP Request Ventas
                string json = await client.GetStringAsync(url_v);

                // Parseando JSON Ventas
                List<SalesModel> items_v = JsonConvert.DeserializeObject<List<SalesModel>>(json);

                List<SalesModel> invoices = items_v.Where(i => i.type_doc == "FACT").OrderBy(i => i.bill_id).ToList(); // Facturas de Venta
                List<SalesModel> notes = items_v.Where(i => i.type_doc == "NC").OrderBy(i => i.bill_id).ToList(); // Notas de Credito

                foreach (SalesModel note in notes)
                {
                    if (note.fact_notec.Contains("."))
                        note.fact_notec = note.fact_notec.Split('.')[0];
                }

                bool skip = false; // Controla si se omite la insercion de una factura
                int skipped_inv = 0, skipped_not = 0;

                // IDs unicos de facturas de venta
                var invoices_v_ids = invoices.Select(i => i.bill_id).Distinct().ToList();
                foreach (string id in invoices_v_ids) // Itero por cada numero de factura de venta
                {
                    // Verifico si existe la factura
                    if (!InvoiceController.Exists(id, true))
					{
                        // Renglones de esa factura de venta
                        var rengs = invoices.Where(i => i.bill_id == id).ToList();

                        // Verifico si el cliente esta ya creado
                        if (!ClientController.Exists(rengs[0].customer_id))
                            ClientController.Add(rengs[0]);

                        // Verifico si los articulos estan creados
                        foreach (var r in rengs)
                        {
                            if (!ProductController.Exists(r.product_code))
                                ProductController.Add(r);

                            if (r.product_sale_price == 0) // Ajuste para items con precio = 0
								r.product_sale_price = Convert.ToDecimal(0.000001);

                            if (r.product_net_price == 0) // Ajuste para items con precio = 0
                                r.product_net_price = Convert.ToDecimal(0.000001);

                            // Verifico los montos del producto
                            skip = CheckAndLogPrice("Factura Venta", r.bill_id, r.product_code, r.quantity, "quantity          ") ||
                                   CheckAndLogPrice("Factura Venta", r.bill_id, r.product_code, r.product_sale_price, "product_sale_price") ||
                                   CheckAndLogPrice("Factura Venta", r.bill_id, r.product_code, r.product_net_price, "product_net_price ");

                            if (skip)
                                break;
                        }

                        if (!skip)
                            InvoiceController.Add(rengs);
                        else
                            skipped_inv++;
					}
					else
					{
                        // LogController.WriteLog("INFO", $"Factura Venta {id} ya existe");
					}
                }

                // IDs unicos de notas de credito
                var notes_ids = notes.Select(n => n.fact_notec).Distinct().ToList();
				foreach (string id in notes_ids)
				{
                    // Verifico si existe la factura
                    if (!NoteController.Exists(id))
                    {
                        // Renglones de esa factura de venta
                        var reng = notes.Single(n => n.fact_notec == id);

                        // Verifico si el cliente esta ya creado
                        if (!ClientController.Exists(reng.customer_id))
                            ClientController.Add(reng);

                        skip = CheckAndLogPrice("Nota de Credito", reng.fact_notec, reng.product_code, reng.exchange_date_day, "exchange_date_day          ");

                        if (!skip)
                            NoteController.Add(reng);
                        else
                            skipped_not++;
                    }
                    else
                    {
                        // LogController.WriteLog("INFO", $"Nota de credito {id} ya existe");
                    }
                }

                LogController.WriteLog("SUCCESS", $"Total Facturas de Venta recibidas: {invoices_v_ids.Count} (Insertadas: {invoices_v_ids.Count - skipped_inv}, Omitidas: {skipped_inv})");
                LogController.WriteLog("SUCCESS", $"Total Notas de Credito recibidas: {notes_ids.Count} (Insertadas: {notes_ids.Count - skipped_not}, Omitidas: {skipped_not})");

                // Realizando HTTP Request Compras
                json = await client.GetStringAsync(url_c);
                skip = false;
                skipped_inv = 0;

                // Parseando JSON Compras
                List<PurchasesModel> items_c = JsonConvert.DeserializeObject<List<PurchasesModel>>(json);

                // IDs unicos de facturas de compra
                var invoices_c_ids = items_c.Select(i => i.header_id).Distinct().ToList();
                foreach (string id in invoices_c_ids)
				{
                    // Verifico si existe la factura
                    if (!InvoiceController.Exists(id, false))
					{
                        // Renglones de esa factura de venta
                        var rengs = items_c.Where(i => i.header_id == id).ToList();

                        // Verifico si el proveedor esta ya creado
                        if (!SupplierController.Exists(rengs[0].supplier_id))
                            SupplierController.Add(rengs[0]);

                        // Verifico si los articulos estan creados
                        foreach (var r in rengs)
						{
                            if (!ProductController.Exists(r.item_id))
                                ProductController.Add(r);

                            // Verifico los montos del producto
                            skip = CheckAndLogPrice("Factura Compra", r.header_id, r.item_id, r.product_quantity, "product_quantity") ||
                                   CheckAndLogPrice("Factura Compra", r.header_id, r.item_id, r.product_cost_unit, "product_cost_unit") ||
                                   CheckAndLogPrice("Factura Compra", r.header_id, r.item_id, r.product_cost_total, "product_cost_total");

                            if (skip)
                                break;
                        }

                        if (!skip)
                            InvoiceController.Add(rengs);
                        else
                            skipped_inv++;
                    }
					else
					{
                        // LogController.WriteLog("INFO", $"Factura Compra {id} ya existe");
                    }
                }

                LogController.WriteLog("SUCCESS", $"Total Facturas de Compra recibidas: {invoices_c_ids.Count} (Insertadas: {invoices_c_ids.Count - skipped_inv}, Omitidas: {skipped_inv})");

                // Detener aplicacion antes de cerrar
                Console.WriteLine("Presiona una tecla para continuar...");
            }
            catch (Exception ex)
            {
                LogController.WriteLog("ERROR", $"{ex.Message}\n{ex.StackTrace}");
			}
			finally
            {
                LogController.WriteLog("INFO", "FIN MIGRACION");
                LogController.WriteLog("LINE", string.Empty);
            }
        }

        private static bool CheckAndLogPrice(string doc, string billId, string productCode, decimal price, string priceType)
        {
            if (price <= 0)
            {
                LogController.WriteLog("WARNING", $"{doc} {billId} / Producto {productCode} / {priceType} = {price}");
                return true;
            }

            return false;
        }
    }
}