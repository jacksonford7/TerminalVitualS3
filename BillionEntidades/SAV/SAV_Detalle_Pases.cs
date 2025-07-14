using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class SAV_Detalle_Pases : Cls_Bil_Base
    {


        #region "Propiedades"
        public int fila { get; set; }
        public string turno_id { get; set; }
        public DateTime? turno_fecha { get; set; }
        public string turno_hora { get; set; }
        public string unidad_id { get; set; }
        public string unidad_tamano { get; set; }
        public string unidad_linea { get; set; }
        public string unidad_booking { get; set; }
        public string unidad_referencia { get; set; }
        public string unidad_estatus { get; set; }
        public Int64? unidad_key { get; set; }

        public string chofer_licencia { get; set; }
        public string chofer_nombre { get; set; }
        public string vehiculo_placa { get; set; }
        public string vehiculo_desc { get; set; }

        public string creado_usuario { get; set; }
        public DateTime? creado_fecha { get; set; }

        public Int64? n4_unit_key { get; set; }
        public string n4_message { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string documento_id { get; set; }
        public string estado_pago { get; set; }
        public string estado { get; set; }

        public string ruc_cliente { get; set; }
        public string name_cliente { get; set; }
        public string ruc_asume { get; set; }
        public string name_asume { get; set; }
        public int cantidad { get; set; }
        public string ruc_facturar { get; set; }
        public string cliente_facturar { get; set; }
        public string numero_factura { get; set; }

        public string asume_ruc_facturar { get; set; }
        public string asume_cliente_facturar { get; set; }
        public Int64? id_facturar { get; set; }
        public bool visto { get; set; }
        public string invoice_type { get; set; }
        #endregion


        public SAV_Detalle_Pases()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }



       

    }
}
