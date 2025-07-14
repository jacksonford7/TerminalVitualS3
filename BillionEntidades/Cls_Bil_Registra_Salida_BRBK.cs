using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Registra_Salida_BRBK : Cls_Bil_Base
    {
        #region "Variables"



        private Int64? _ID_PPWEB;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _FACTURA = string.Empty;
        private DateTime? _FECHA_SALIDA;
        #endregion

        #region "Propiedades"


        public Int64? ID_PPWEB { get => _ID_PPWEB; set => _ID_PPWEB = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public string FACTURA { get => _FACTURA; set => _FACTURA = value; }
        public DateTime? FECHA_SALIDA { get => _FECHA_SALIDA; set => _FECHA_SALIDA = value; }

        #endregion

        public Cls_Bil_Registra_Salida_BRBK()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        /*poblar */
        public bool PopulateMyData(out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);


            var t = sql_puntero.ExecuteSelectOnly<Cls_Bil_Registra_Salida_BRBK>(nueva_conexion, 6000, "VBS_INFORMACION_CARGA_BBK", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos de la carga";
                return false;
            }

            this.ID_PPWEB = t.ID_PPWEB;
            this.MRN = t.MRN;
            this.MSN = t.MSN;
            this.HSN = t.HSN;
            this.FACTURA = t.FACTURA;
            this.FECHA_SALIDA = t.FECHA_SALIDA;
          
           
            OnError = string.Empty;
            return true;
        }

        private int? PreValidations(out string msg)
        {
           
            if (string.IsNullOrEmpty(this.MRN))
            {
                msg = "Debe especificar el MRN";
                return 0;
            }
            if (string.IsNullOrEmpty(this.MSN))
            {
                msg = "Debe especificar el MSN";
                return 0;
            }

            if (string.IsNullOrEmpty(this.HSN))
            {
                msg = "Debe especificar el HSN";
                return 0;
            }

            if (!this.FECHA_SALIDA.HasValue)
            {
                msg = "Debe especificar la fecha de salida de la carga";
                return 0;
            }

            msg = string.Empty;
            return 1;

        }

        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

            OnInit("N4Middleware");

            parametros.Clear();
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("FACTURA", this.FACTURA);
            parametros.Add("FECHA_SALIDA", this.FECHA_SALIDA);
            parametros.Add("USUARIO_REGISTRA", this.IV_USUARIO_CREA);
         
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "VBS_GRABA_FECHA_CARGA_BBK", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

    }

}
