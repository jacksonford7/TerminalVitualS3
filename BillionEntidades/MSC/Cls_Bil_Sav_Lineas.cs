using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Bil_Sav_Lineas : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID;
      
        private string _LINEA = string.Empty;
        private string _DESCRIPCION = string.Empty;
        private bool _ESTADO;
        private bool _VALIDA_WS;
        private string _RUTA_WS = string.Empty;
        private string _CODIGOADUANA = string.Empty;

        private static Int64? lm = -3;

        private Int64 _DEPOT;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }


        public string LINEA { get => _LINEA; set => _LINEA = value; }

        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }

        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }

        public bool VALIDA_WS { get => _VALIDA_WS; set => _VALIDA_WS = value; }

        public string RUTA_WS { get => _RUTA_WS; set => _RUTA_WS = value; }

        public string CODIGOADUANA { get => _CODIGOADUANA; set => _CODIGOADUANA = value; }

        private static String v_mensaje = string.Empty;

        public Int64 DEPOT { get => _DEPOT; set => _DEPOT = value; }

        #endregion

        public Cls_Bil_Sav_Lineas()
        {
            init();

          

        }


        public static List<Cls_Bil_Sav_Lineas> Consulta_Linea_Sav(string LINEA, Int64 ID_DEPOT, out string OnError)
        {
            parametros.Clear();
            parametros.Add("LINEA", LINEA);
            parametros.Add("ID_DEPOT", ID_DEPOT);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Sav_Lineas>(sql_puntero.Conexion_Local, 6000, "Sav_Carga_Lineas", parametros, out OnError);

        }

        public bool PopulateMyData(out string OnError)
        {
          
            parametros.Clear();
            parametros.Add("CODIGOADUANA", this.CODIGOADUANA);

            var t = sql_puntero.ExecuteSelectOnly<Cls_Bil_Sav_Lineas>(sql_puntero.Conexion_Local, 6000, "Sav_Carga_Lineas_CodigoAduana", parametros);
            if (t == null)
            {
                OnError = string.Format("No existe información con el código de aduana a consultar: {0}", this.CODIGOADUANA);
                return false;
            }

            this.ID = t.ID;
            this.LINEA = t.LINEA;
            this.DESCRIPCION = t.DESCRIPCION;
            this.ESTADO = t.ESTADO;
            this.VALIDA_WS = t.VALIDA_WS;
            this.RUTA_WS = t.RUTA_WS;
            this.CODIGOADUANA = t.CODIGOADUANA;

            OnError = string.Empty;
            return true;
        }

        public bool PopulateMyData_Id_Linea(out string OnError)
        {

            parametros.Clear();
            parametros.Add("LINEA", this.LINEA);
            parametros.Add("DEPOT", this.DEPOT);
            var t = sql_puntero.ExecuteSelectOnly<Cls_Bil_Sav_Lineas>(sql_puntero.Conexion_Local, 6000, "Sav_Carga_Id_Linea", parametros);
            if (t == null)
            {
                OnError = string.Format("No existe información con el código de línea a consultar: {0}", this.LINEA);
                return false;
            }

            this.ID = t.ID;
         
            OnError = string.Empty;
            return true;
        }

    }
}
