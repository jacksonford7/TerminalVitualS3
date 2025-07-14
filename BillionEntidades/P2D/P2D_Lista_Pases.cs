using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Lista_Pases : Cls_Bil_Base
    {
        #region "Variables"

        private string _ORDEN;
        private Int64 _ID_PASE;
        private string _ESTADO = string.Empty;
        private DateTime _FECHA_TURNO;
       
        private string _TRANSPORTISTA_DESC = string.Empty;
        private string _CLIENTE = string.Empty;
        private string _PLACA = string.Empty;
        private string _CARGA = string.Empty;
        private string _USUARIO_CREA = string.Empty;
        private string _ESTADO_PASE = string.Empty;
        //'CARGA NO AUTORIZADA'  
        #endregion

        #region "Propiedades"
        public string ORDEN { get => _ORDEN; set => _ORDEN = value; }
        public Int64 ID_PASE { get => _ID_PASE; set => _ID_PASE = value; }
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public DateTime FECHA_TURNO { get => _FECHA_TURNO; set => _FECHA_TURNO = value; }
        
        public string TRANSPORTISTA_DESC { get => _TRANSPORTISTA_DESC; set => _TRANSPORTISTA_DESC = value; }
        public string CLIENTE { get => _CLIENTE; set => _CLIENTE = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
        public string CARGA { get => _CARGA; set => _CARGA = value; }

        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string ESTADO_PASE { get => _ESTADO_PASE; set => _ESTADO_PASE = value; }
        private static String v_mensaje = string.Empty;

        #endregion


        public P2D_Lista_Pases()
        {
            init();
        }

        public static List<P2D_Lista_Pases> Listado_Pases(string ORDEN, out string OnError)
        {
            parametros.Clear();
            parametros.Add("ORDEN", ORDEN);
         
            return sql_puntero.ExecuteSelectControl<P2D_Lista_Pases>(sql_puntero.Conexion_Local, 4000, "P2D_BUSCAR_ORDEN", parametros, out OnError);

        }

        public bool Delete(out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID_PASE", this.ID_PASE);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "P2S_QUITAR_ORDEN", parametros, out OnError);
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
