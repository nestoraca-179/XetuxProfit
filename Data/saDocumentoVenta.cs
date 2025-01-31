//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XetuxProfit.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class saDocumentoVenta
    {
        public string co_tipo_doc { get; set; }
        public string nro_doc { get; set; }
        public string co_cli { get; set; }
        public string co_ven { get; set; }
        public string co_mone { get; set; }
        public string mov_ban { get; set; }
        public decimal tasa { get; set; }
        public string observa { get; set; }
        public System.DateTime fec_reg { get; set; }
        public System.DateTime fec_emis { get; set; }
        public System.DateTime fec_venc { get; set; }
        public bool anulado { get; set; }
        public bool aut { get; set; }
        public bool contrib { get; set; }
        public string doc_orig { get; set; }
        public Nullable<int> tipo_origen { get; set; }
        public string nro_orig { get; set; }
        public string nro_che { get; set; }
        public decimal saldo { get; set; }
        public decimal total_bruto { get; set; }
        public string porc_desc_glob { get; set; }
        public decimal monto_desc_glob { get; set; }
        public string porc_reca { get; set; }
        public decimal monto_reca { get; set; }
        public decimal total_neto { get; set; }
        public decimal monto_imp { get; set; }
        public decimal monto_imp2 { get; set; }
        public decimal monto_imp3 { get; set; }
        public string tipo_imp { get; set; }
        public string tipo_imp2 { get; set; }
        public string tipo_imp3 { get; set; }
        public decimal porc_imp { get; set; }
        public decimal porc_imp2 { get; set; }
        public decimal porc_imp3 { get; set; }
        public string num_comprobante { get; set; }
        public Nullable<System.DateTime> feccom { get; set; }
        public Nullable<int> numcom { get; set; }
        public string n_control { get; set; }
        public string dis_cen { get; set; }
        public decimal comis1 { get; set; }
        public decimal comis2 { get; set; }
        public decimal comis3 { get; set; }
        public decimal comis4 { get; set; }
        public decimal comis5 { get; set; }
        public decimal comis6 { get; set; }
        public decimal adicional { get; set; }
        public string salestax { get; set; }
        public bool ven_ter { get; set; }
        public string impfis { get; set; }
        public string impfisfac { get; set; }
        public string imp_nro_z { get; set; }
        public decimal otros1 { get; set; }
        public decimal otros2 { get; set; }
        public decimal otros3 { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }
        public string campo3 { get; set; }
        public string campo4 { get; set; }
        public string campo5 { get; set; }
        public string campo6 { get; set; }
        public string campo7 { get; set; }
        public string campo8 { get; set; }
        public string co_us_in { get; set; }
        public string co_sucu_in { get; set; }
        public System.DateTime fe_us_in { get; set; }
        public string co_us_mo { get; set; }
        public string co_sucu_mo { get; set; }
        public System.DateTime fe_us_mo { get; set; }
        public string revisado { get; set; }
        public string trasnfe { get; set; }
        public byte[] validador { get; set; }
        public System.Guid rowguid { get; set; }
        public string co_cta_ingr_egr { get; set; }
    
        public virtual saCliente saCliente { get; set; }
    }
}
