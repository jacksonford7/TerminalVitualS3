using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_CabeceraMsc : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID;
        private DateTime? _FECHA = null;  
        private string _SESION =string.Empty;
        private string _ARCHIVO = string.Empty;

        private string _xmlCabecera;
        private string _xmlDetalle;

        private static Int64? lm = -3;

        private string _CONTENEDOR = string.Empty;
        private DateTime _FECHA_DESPACHO;
        private string _LINEA = string.Empty;

        private string _bl = string.Empty;
        private string _cliente = string.Empty;
        private string _tipo = string.Empty;
        private string _nave = string.Empty;
        private string _viaje = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
      
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }
       
        public string SESION { get => _SESION; set => _SESION = value; }

        public string ARCHIVO { get => _ARCHIVO; set => _ARCHIVO = value; }

        private static String v_mensaje = string.Empty;

        public string xmlCabecera { get => _xmlCabecera; set => _xmlCabecera = value; }
        public string xmlDetalle { get => _xmlDetalle; set => _xmlDetalle = value; }

        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public DateTime FECHA_DESPACHO { get => _FECHA_DESPACHO; set => _FECHA_DESPACHO = value; }
        public string LINEA { get => _LINEA; set => _LINEA = value; }

        public string bl { get => _bl; set => _bl = value; }
        public string cliente { get => _cliente; set => _cliente = value; }
        public string tipo { get => _tipo; set => _tipo = value; }
        public string nave { get => _nave; set => _nave = value; }
        #endregion



        public List<Cls_Bil_DetalleMsc> Detalle { get; set; }
      

      

        public Cls_Bil_CabeceraMsc()
        {
            init();

            this.Detalle = new List<Cls_Bil_DetalleMsc>();
           
        }

        public Cls_Bil_CabeceraMsc(Int64 _ID, DateTime? FECHA, 
                                      string _USUARIO_CREA, DateTime? _FECHA_CREA)

        {
            this.ID = _ID;
         
            this.FECHA = _FECHA;
           
            this.IV_USUARIO_CREA = _USUARIO_CREA;
            this.IV_FECHA_CREA = _FECHA_CREA;
           

            this.Detalle = new List<Cls_Bil_DetalleMsc>();
          
        }


        #region "Graba"
        private Int64? Save(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCabecera", this.xmlCabecera);
            parametros.Add("xmlDetalle", this.xmlDetalle);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_Contenedores_Msc", parametros, out OnError);
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
                var id = this.Save(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar contenedores ****";
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
        #endregion


        #region "Graba"
        private Int64? Save_Lineas(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCabecera", this.xmlCabecera);
            parametros.Add("xmlDetalle", this.xmlDetalle);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_Contenedores_Lineas", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Lineas(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Lineas(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar contenedores ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Lineas), "SaveTransaction_Lineas", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        public static List<Cls_Bil_CabeceraMsc> Despacho_Contenedor(string CONTENEDOR, out string OnError)
        {
            parametros.Clear();
            parametros.Add("CONTENEDOR", CONTENEDOR);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_CabeceraMsc>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Existe_Contenedor_Msc", parametros, out OnError);

        }

        public static List<Cls_Bil_CabeceraMsc> Despacho_Contenedor_Linea(string CONTENEDOR, string LINEA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("CONTENEDOR", CONTENEDOR);
            parametros.Add("LINEA", LINEA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_CabeceraMsc>(sql_puntero.Conexion_Local, 6000, "Sav_Bil_Existe_Contenedor_Linea", parametros, out OnError);

        }

    }
}
