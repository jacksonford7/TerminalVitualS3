using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_excluir_numero_carga : Cls_Bil_Base
    {


        #region "Variables"

        private Int64 _ID;
        private string _MRN;
        private string _MSN;
        private string _HSN;

        private string _USUARIO;
        private string _RUC;
        private string _BODEGA;
        private int? _TIPO_PRODUCTO;

        private bool _ESTADO;

        private string _OBSERVACION;
        private static Int64? lm = -3;

        #endregion

        #region "Propiedades"
        public int? TIPO_PRODUCTO { get => _TIPO_PRODUCTO; set => _TIPO_PRODUCTO = value; }
        public Int64 ID { get => _ID; set => _ID = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }

        public string USUARIO { get => _USUARIO; set => _USUARIO = value; }
        public string RUC { get => _RUC; set => _RUC = value; }
        public string BODEGA { get => _BODEGA; set => _BODEGA = value; }

        public string OBSERVACION { get => _OBSERVACION; set => _OBSERVACION = value; }

        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        
        #endregion



        public brbk_excluir_numero_carga()
        {
            init();


        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.USUARIO))
            {
                msg = "Debe especificar el usuario";
                return 0;
            }
            if (this.TIPO_PRODUCTO == 0)
            {
                msg = "Debe especificar el ID del tipo de producto";
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
            parametros.Add("ID", this.ID);
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("USUARIO", this.USUARIO);
            parametros.Add("RUC", this.RUC);
            parametros.Add("BODEGA", this.BODEGA);
            parametros.Add("TIPO_PRODUCTO", this.TIPO_PRODUCTO);
            parametros.Add("OBSERVACION", this.OBSERVACION);

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "[BRBK].[REGISTRA_EXCLUIR_NUMERO_CARGA]", parametros, out OnError);
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
