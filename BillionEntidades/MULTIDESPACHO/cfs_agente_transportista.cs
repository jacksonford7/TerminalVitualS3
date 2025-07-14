using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class cfs_agente_transportista : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;

        private Int64 _ID;
        private int _IDUSUARIO;
        private string _RUC_AGENTE = string.Empty;
        private string _DESC_AGENTE;
        private string _RUC_TRANSPORTE;
        private string _DESC_TRANSPORTE;

        private bool _ESTADO;
     
        private string _USUARIO_ING = string.Empty;
        private string _USARIO_MOD = string.Empty;
        private DateTime? _FECHA_ING;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
        public int IDUSUARIO { get => _IDUSUARIO; set => _IDUSUARIO = value; }
        public string RUC_AGENTE { get => _RUC_AGENTE; set => _RUC_AGENTE = value; }
        public string DESC_AGENTE { get => _DESC_AGENTE; set => _DESC_AGENTE = value; }
        public string RUC_TRANSPORTE { get => _RUC_TRANSPORTE; set => _RUC_TRANSPORTE = value; }
        public string DESC_TRANSPORTE { get => _DESC_TRANSPORTE; set => _DESC_TRANSPORTE = value; }

        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
     
        public string USUARIO_ING { get => _USUARIO_ING; set => _USUARIO_ING = value; }
        public string USARIO_MOD { get => _USARIO_MOD; set => _USARIO_MOD = value; }

        public DateTime? FECHA_ING { get => _FECHA_ING; set => _FECHA_ING = value; }
        #endregion



        public cfs_agente_transportista()
        {
            init();
        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.RUC_AGENTE))
            {
                msg = "Debe especificar el agente";
                return 0;
            }
            if (string.IsNullOrEmpty(this.RUC_TRANSPORTE))
            {
                msg = "Debe especificar la empresa de transporte";
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
            parametros.Add("IDUSUARIO", this.IDUSUARIO);
            parametros.Add("RUC_AGENTE", this.RUC_AGENTE);
            parametros.Add("DESC_AGENTE", this.DESC_AGENTE);
            parametros.Add("RUC_TRANSPORTE", this.RUC_TRANSPORTE);
            parametros.Add("DESC_TRANSPORTE", this.DESC_TRANSPORTE);
            parametros.Add("USUARIO_ING", this.USUARIO_ING);


            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "bil_agente_transportista", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

        public static List<cfs_agente_transportista> Listado_Transportistas(string pRUC_AGENTE, out string OnError)
        {
            parametros.Clear();
            parametros.Add("RUC_AGENTE", pRUC_AGENTE);
            return sql_puntero.ExecuteSelectControl<cfs_agente_transportista>(sql_puntero.Conexion_Local, 7000, "bil_lista_agente_transportista", parametros, out OnError);

        }

        public static List<cfs_agente_transportista> CboTransportista(string pRUC_AGENTE, out string OnError)
        {
            parametros.Clear();
            parametros.Add("RUC_AGENTE", pRUC_AGENTE);
            return sql_puntero.ExecuteSelectControl<cfs_agente_transportista>(sql_puntero.Conexion_Local, 7000, "bil_carga_transportista", parametros, out OnError);

        }

        public bool Delete(out string OnError)
        {
           
            parametros.Clear();
            parametros.Add("ID", this.ID);
            parametros.Add("USARIO_MOD", this.USARIO_MOD);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "bil_inactiva_agente_transportista", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

    }
}
