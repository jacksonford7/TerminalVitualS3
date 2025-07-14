using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Validaciones : Cls_Bil_Base
    {

        #region "Variables"

        private string _xmlContenedores = string.Empty;

        #endregion

        #region "Propiedades"

        public string xmlContenedores { get => _xmlContenedores; set => _xmlContenedores = value; }

        private static String v_mensaje = string.Empty;


        #endregion


        public Cls_Bil_Invoice_Validaciones()
        {
            init();
          
        }

        public Cls_Bil_Invoice_Validaciones( string _xmlContenedores)

        {
            this.xmlContenedores = _xmlContenedores;
          
        }


        /*valida que no exista una nota de credito aplicada a una factura*/
        public string Validacion_Contenedores(string _xmlContenedores)
        {
            string OnError;
            parametros.Clear();
            parametros.Add("XmlContenedor", _xmlContenedores);

            var db = sql_puntero.ExecuteSelectOnly(sql_puntero.Conexion_Local, 6000, "sp_Bil_Validacion_Contenedor", parametros);
            if (db == null)
            {
                OnError = string.Format("Error al validar contenedores..por favor informar a CGSA..");
                return null;
            }
            else
            {
                if (db.codigo == 1)
                {
                    OnError = db.mensaje;
                }
                else
                {
                    OnError = string.Empty;
                }

            }
            return OnError;

        }

        /*valida que no exista una nota de credito aplicada a una factura*/
        public string Validacion_Carga_Suelta(string _xmlContenedores)
        {
            string OnError;
            parametros.Clear();
            parametros.Add("XmlContenedor", _xmlContenedores);

            var db = sql_puntero.ExecuteSelectOnly(sql_puntero.Conexion_Local, 6000, "sp_Bil_Validacion_Carga_Cfs", parametros);
            if (db == null)
            {
                OnError = string.Format("Error al validar carga suelta..por favor informar a CGSA..");
                return null;
            }
            else
            {
                if (db.codigo == 1)
                {
                    OnError = db.mensaje;
                }
                else
                {
                    OnError = string.Empty;
                }

            }
            return OnError;

        }

        /*valida que no exista una nota de credito aplicada a una factura*/
        public string Validacion_Carga_Suelta_FF(string _xmlContenedores)
        {
            string OnError;
            parametros.Clear();
            parametros.Add("XmlContenedor", _xmlContenedores);

            var db = sql_puntero.ExecuteSelectOnly(sql_puntero.Conexion_Local, 6000, "sp_Bil_Validacion_Carga_Cfs_FF", parametros);
            if (db == null)
            {
                OnError = string.Format("Error al validar carga suelta..por favor informar a CGSA..");
                return null;
            }
            else
            {
                if (db.codigo == 1)
                {
                    OnError = db.mensaje;
                }
                else
                {
                    OnError = string.Empty;
                }

            }
            return OnError;

        }

        /*validaciones facturas break bulk*/
        public string Validacion_Break_Bulk(string _xmlContenedores)
        {
            string OnError;
            parametros.Clear();
            parametros.Add("XmlContenedor", _xmlContenedores);

            var db = sql_puntero.ExecuteSelectOnly(sql_puntero.Conexion_Local, 6000, "sp_Bil_Validacion_Carga_Brbk", parametros);
            if (db == null)
            {
                OnError = string.Format("Error al validar carga Break Bulk..por favor informar a CGSA..");
                return null;
            }
            else
            {
                if (db.codigo == 1)
                {
                    OnError = db.mensaje;
                }
                else
                {
                    OnError = string.Empty;
                }

            }
            return OnError;

        }
    }
}
