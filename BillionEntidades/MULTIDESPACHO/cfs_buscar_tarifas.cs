using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class cfs_buscar_tarifas : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;

        private Int64 _ID;
        private string _TIPO = string.Empty;
        private int _CANTIDAD ;
        private decimal _FACTOR;
        private decimal _VALOR;
        private string _INVOICETYPE;
        private string _CODIGO_TARIJA_N4;
        private bool _ESTADO;
        private decimal _IVA;

        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;

        private decimal _VALOR_IVA;
        private decimal _VALOR_TOTAL;
        private string _DESCRIPCION = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
        public string TIPO { get => _TIPO; set => _TIPO = value; }
        public int CANTIDAD { get => _CANTIDAD; set => _CANTIDAD = value; }
        public decimal FACTOR { get => _FACTOR; set => _FACTOR = value; }
        public decimal VALOR { get => _VALOR; set => _VALOR = value; }
        public string INVOICETYPE { get => _INVOICETYPE; set => _INVOICETYPE = value; }
        public string CODIGO_TARIJA_N4 { get => _CODIGO_TARIJA_N4; set => _CODIGO_TARIJA_N4 = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public decimal IVA { get => _IVA; set => _IVA = value; }

        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }

        public decimal VALOR_IVA { get => _VALOR_IVA; set => _VALOR_IVA = value; }
        public decimal VALOR_TOTAL { get => _VALOR_TOTAL; set => _VALOR_TOTAL = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }
        #endregion



        public cfs_buscar_tarifas()
        {
            init();

    

        }

        public static cfs_buscar_tarifas GetServicio(int _cantidad)
        {
          
            parametros.Clear();
            parametros.Add("cantidad", _cantidad);
            return sql_puntero.ExecuteSelectOnly<cfs_buscar_tarifas>(sql_puntero.Conexion_Local, 6000, "cfs_buscar_tarifas", parametros);
        }




        public static Int64? SaveTrace( string _rucEmpresa, string _tipo, string _invoicetype, Int64 _idtarifa, string _tarifaN4, string _xmlRequest, string _xmlResponse, bool _estado, string _user, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_RUCEMPRESA", _rucEmpresa);
            parametros.Add("i_TIPO", _tipo);
            parametros.Add("i_INVOICETYPE", _invoicetype);
            parametros.Add("i_IDTARIFA", _idtarifa);
            parametros.Add("i_TARIFAN4", _tarifaN4);
            parametros.Add("i_REQUESTXML", _xmlRequest);
            parametros.Add("i_RESPONSEXML", _xmlResponse);
            parametros.Add("i_ESTADO", _estado);
            parametros.Add("i_USUARIO", _user);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "CFS_ADD_TRACE_FACTURA", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }





    }
}
