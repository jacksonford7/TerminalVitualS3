using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public class Cls_Politicas_Datos : Cls_Bil_Base
    {
        private static String v_mensaje = string.Empty;

        #region "Variables"

        private static Int64? lm = -3;
        private Int64 _ID;
        private string _usuario = string.Empty;
        private string _ruc = string.Empty;
        private DateTime? _fecha_acepta;
        private string _descripcion = string.Empty;
        private string _empresa = string.Empty;
        private DateTime? _fecha_registro;
        private string _tipo = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
        public string usuario { get => _usuario; set => _usuario = value; }
        public string ruc { get => _ruc; set => _ruc = value; }    
        public DateTime? fecha_acepta { get => _fecha_acepta; set => _fecha_acepta = value; }
        public string descripcion { get => _descripcion; set => _descripcion = value; }
        public string empresa { get => _empresa; set => _empresa = value; }
        public DateTime? fecha_registro { get => _fecha_registro; set => _fecha_registro = value; }
        public string tipo { get => _tipo; set => _tipo = value; }
        #endregion

        public Cls_Politicas_Datos()
        {
            init();

        }
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

       

        #region "Registra Aceptacion"
        private Int64? Save_Acepta(out string OnError)
        {
            
            parametros.Clear();

            parametros.Add("usuario", this.usuario);
            parametros.Add("ruc", this.ruc);
         
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_bil_acepta_politica", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Acepta(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Acepta(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al registrar aceptación de la política ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Acepta), "SaveTransaction_Acepta", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Registra No Aceptacion"
        private Int64? Save_NoAcepta(out string OnError)
        {

            parametros.Clear();
            parametros.Add("usuario", this.usuario);
            parametros.Add("ruc", this.ruc);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_bil_noacepta_politica", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_NoAcepta(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_NoAcepta(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al registrar no aceptación de la política ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_NoAcepta), "SaveTransaction_NoAcepta", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        public static List<Cls_Politicas_Datos> Tiene_Politica(string pusuario, out string OnError)
        {
            parametros.Clear();
            parametros.Add("usuario", pusuario);
            return sql_puntero.ExecuteSelectControl<Cls_Politicas_Datos>(sql_puntero.Conexion_Local, 6000, "sp_bil_tiene_politica", parametros, out OnError);

        }


        public static List<Cls_Politicas_Datos> Listado_Politicas(DateTime fecha_desde, DateTime fecha_hasta, string tipo, out string OnError)
        {


            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("tipo", tipo);
            return sql_puntero.ExecuteSelectControl<Cls_Politicas_Datos>(sql_puntero.Conexion_Local, 6000, "sp_bil_listado_acepta_politicas", parametros, out OnError);

        }

    }
}
