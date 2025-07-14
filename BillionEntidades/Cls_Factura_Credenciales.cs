using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Factura_Credenciales : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _IDSOLICITUD;
        private string _NUMERO_FACTURA = string.Empty;
       
        #endregion

        #region "Propiedades"

        public Int64 IDSOLICITUD { get => _IDSOLICITUD; set => _IDSOLICITUD = value; }
        public string NUMERO_FACTURA { get => _NUMERO_FACTURA; set => _NUMERO_FACTURA = value; }


        #endregion

        public Cls_Factura_Credenciales()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("Portal_Sca");
        }

        public bool PopulateMyData(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("IDSOLICITUD", this.IDSOLICITUD);

            var t = sql_puntero.ExecuteSelectOnly<Cls_Factura_Credenciales>(nueva_conexion, 6000, "SCA_NUMERO_FACTURA", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener el número de la factura";
                return false;
            }

            this.IDSOLICITUD = t.IDSOLICITUD;
            this.NUMERO_FACTURA = t.NUMERO_FACTURA;
           

            OnError = string.Empty;
            return true;
        }

    }
}
