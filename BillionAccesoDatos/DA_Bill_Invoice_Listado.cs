using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using BillionEntidades;

namespace BillionAccesoDatos
{
   public  class DA_Bill_Invoice_Listado : Cls_Bil_Base
    {
        public DA_Bill_Invoice_Listado()
        {
            init();

        }

        /*carga todas las facturas por rango de fechas*/
        public List<Cls_Bil_Invoice_Listado> BackOffice_Listado_Facturas(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, out string OnError)
        {
           
            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", IV_FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", IV_FECHA_HASTA);

            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Listado>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Rpt_BackOffice_Listado_Factura", parametros, out OnError);

        }

        /*carga todas las facturas por rango de fechas*/
        public  List<Cls_Bil_Invoice_Listado> Listado_Facturas(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {
            
            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", IV_FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", IV_FECHA_HASTA);
            parametros.Add("IV_USUARIO", Usuario);
            parametros.Add("IV_RUC", Agente);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Listado>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Rpt_Listado_Factura", parametros, out OnError);

        }

        /*carga todas las facturas por rango de fechas*/
        public List<Cls_Bil_Invoice_Listado> Listado_Facturas_cfs(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {

            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", IV_FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", IV_FECHA_HASTA);
            parametros.Add("IV_USUARIO", Usuario);
            parametros.Add("IV_RUC", Agente);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Listado>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Rpt_Listado_Factura_cfs", parametros, out OnError);

        }

        /*carga todas las facturas por rango de fechas*/
        public List<Cls_Bil_Invoice_Listado> BackOffice_Listado_Facturas_Expo(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, out string OnError)
        {

            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", IV_FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", IV_FECHA_HASTA);

            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Listado>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Rpt_BackOffice_Listado_Factura_expo", parametros, out OnError);

        }

        /*carga todas las facturas por rango de fechas*/
        public List<Cls_Bil_Invoice_Listado> Listado_Facturas_Brbk(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {

            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", IV_FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", IV_FECHA_HASTA);
            parametros.Add("IV_USUARIO", Usuario);
            parametros.Add("IV_RUC", Agente);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Listado>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Rpt_Listado_Factura_brbk", parametros, out OnError);

        }

    }
}
