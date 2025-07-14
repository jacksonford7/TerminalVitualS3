using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Inspeccion_Buques : Cls_Bil_Base
    {


        #region "Variables"

        private Int64? _SECUENCIA;
        private string _NAVE;
        private string _DESC_NAVE;
        private string _REFERENCIA;
        private string _LINEA;
        private string _ID_LINEA;
        private string _BANDERA;
        private DateTime? _FECHA_INSPE;
        private string _OBSERVACION;
        private DateTime? _FECHA_CREA;
        private string _RUC_USUARIO;
        private bool _ESTADO;
        private string _RUTA_PDF;
        private string _USUARIO_CREA;

        private static Int64? lm = -3;

        #endregion

        #region "Propiedades"

        public Int64? SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public string NAVE { get => _NAVE; set => _NAVE = value; }
        public string DESC_NAVE { get => _DESC_NAVE; set => _DESC_NAVE = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
       
        public string LINEA { get => _LINEA; set => _LINEA = value; }
        public string ID_LINEA { get => _ID_LINEA; set => _ID_LINEA = value; }
        public string BANDERA { get => _BANDERA; set => _BANDERA = value; }
        public DateTime? FECHA_INSPE { get => _FECHA_INSPE; set => _FECHA_INSPE = value; }
        public string OBSERVACION { get => _OBSERVACION; set => _OBSERVACION = value; }
        public DateTime? FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }
        public string RUC_USUARIO { get => _RUC_USUARIO; set => _RUC_USUARIO = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string RUTA_PDF { get => _RUTA_PDF; set => _RUTA_PDF = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        #endregion



        public Cls_Inspeccion_Buques()
        {
            init();


        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.NAVE))
            {
                msg = "Debe especificar la nave";
                return 0;
            }
            if (string.IsNullOrEmpty(this.REFERENCIA))
            {
                msg = "Debe especificar la referencia";
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

         

            parametros.Clear();
            parametros.Add("NAVE", this.NAVE);
            parametros.Add("DESC_NAVE", this.DESC_NAVE);
            parametros.Add("REFERENCIA", this.REFERENCIA);
            parametros.Add("LINEA", this.LINEA);
            parametros.Add("ID_LINEA", this.ID_LINEA);
            parametros.Add("BANDERA", this.BANDERA);
            parametros.Add("FECHA_INSPE", this.FECHA_INSPE);
            parametros.Add("OBSERVACION", this.OBSERVACION);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            parametros.Add("RUC_USUARIO", this.RUC_USUARIO);
            parametros.Add("RUTA_PDF", this.RUTA_PDF);

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "bil_registra_inspeccion", parametros, out OnError);
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
