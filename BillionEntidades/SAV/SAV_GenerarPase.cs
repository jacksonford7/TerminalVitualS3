using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class SAV_GenerarPase : Cls_Bil_Base
    {



        #region "Propiedades"
        public Int64 id { get; set; }
        public string turno_id { get; set; }
        public DateTime turno_fecha { get; set; }
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

        public Int64? n4_unit_key { get; set; }
        public string n4_message { get; set; }
        public int? n4_codigo { get; set; }

        public string documento_id { get; set; }
        public string documento_estado { get; set; }
        public DateTime documento_fecha { get; set; }

        public int? deposito_id { get; set; }
        public string deposito_nombre { get; set; }
        public int? turno_numero { get; set; }
        public string unidad_dae { get; set; }

        public string ruc_cliente { get; set; }
        public string name_cliente { get; set; }
        public string ruc_asume { get; set; }
        public string name_asume { get; set; }
        public Int64? id_asignacion { get; set; }



        private static String v_mensaje = string.Empty;


        #endregion


        #region "Propiedes "
      
        private static Int64? lm = -3;

        #endregion


    

        public SAV_GenerarPase()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        private Int64? Save_Pase(out string OnError)
        {
            OnInit("SERVICE");
      


            parametros.Clear();
            parametros.Add("turno_id", this.turno_id);
            parametros.Add("turno_fecha", this.turno_fecha);
            parametros.Add("turno_hora", this.turno_hora);
            parametros.Add("unidad_id", this.unidad_id);
            parametros.Add("unidad_tamano", this.unidad_tamano);
            parametros.Add("unidad_linea", this.unidad_linea);
            parametros.Add("unidad_booking", this.unidad_booking);
            parametros.Add("unidad_referencia", this.unidad_referencia);
            parametros.Add("unidad_estatus", "MTY");
            parametros.Add("unidad_key", this.unidad_key);
            parametros.Add("chofer_licencia", this.chofer_licencia);
            parametros.Add("chofer_nombre", this.chofer_nombre);
            parametros.Add("vehiculo_placa", this.vehiculo_placa);
            parametros.Add("vehiculo_desc", this.vehiculo_desc);
            parametros.Add("creado_usuario", this.creado_usuario);
            parametros.Add("n4_unit_key", null);
            parametros.Add("n4_message", this.n4_message);
            parametros.Add("n4_codigo", null);
            parametros.Add("unidad_dae", this.unidad_dae);
            parametros.Add("turno_numero", this.turno_numero);
            parametros.Add("ruc_cliente", this.ruc_cliente);
            parametros.Add("name_cliente", this.name_cliente);
            parametros.Add("ruc_asume", this.ruc_asume);
            parametros.Add("name_asume", this.name_asume);
            parametros.Add("id_asignacion", this.id_asignacion);
            parametros.Add("deposito_id", this.deposito_id);
            parametros.Add("deposito_nombre", this.deposito_nombre);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 8000, "agregar_preaviso_documento", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


        public Int64? SaveTransaction(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Pase(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar pase puerta REPCONTVER ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        public Int64? cancelar_turno(string turno_id, string unidad_id, string modifica, string causa, out string novedad)
        {
            OnInit("SERVICE");
     
            parametros.Clear();
            parametros.Add("turno_id", turno_id);
            parametros.Add("unidad_id", unidad_id);
            parametros.Add("cancela_usuario", modifica);
            parametros.Add("cancela_razon", causa);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 8000, "cancelar_preaviso", parametros, out novedad);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }

           
        novedad = string.Empty;
        return db.Value;
            
          
        }



        public bool? ExisteTurnoEnCGSA(string turno, out string OnError)
        {
            //true es valido, false caso contrario
            OnInit("SERVICE");

            parametros.Clear();
            parametros.Add("turno", turno);
            var rs = sql_puntero.ComandoSelectEscalar(nueva_conexion, 5000, "select [dbo].[fx_far_preaviso_existe](@turno)", parametros, out OnError);

            if (rs != null)
            {
                return Boolean.Parse(rs.ToString());
            }

            OnError = string.Empty;
            return false;
            
            
        }


        public bool? ExisteConteendorTurnoEnCGSA(string unidad_id, out string OnError)
        {
            //true es valido, false caso contrario
            OnInit("SERVICE");

            parametros.Clear();
            parametros.Add("unidad_id", unidad_id);
            var rs = sql_puntero.ComandoSelectEscalar(nueva_conexion, 5000, "select [dbo].[fx_far_preaviso_existe_contenedor](@unidad_id)", parametros, out OnError);

            if (rs != null)
            {
                return Boolean.Parse(rs.ToString());
            }

            OnError = string.Empty;
            return false;


        }



    }
}
