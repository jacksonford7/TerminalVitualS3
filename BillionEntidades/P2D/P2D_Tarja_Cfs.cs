using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Tarja_Cfs : Cls_Bil_Base
    {
        #region "Variables"

       
        private decimal _VALOR = 0;
        private decimal _m3 = 0;
        private decimal _pesokg = 0;

        private string _mrn;
        private string _msn;
        private string _hsn;
        private string _ruc;
        private string _contenedor;
        private string _consignee_id;
        private string _consignee_name;
        private string _agent_id;
        private bool _apilable = false;

        private decimal _cantidad = 0;
       
        #endregion

        #region "Propiedades"
      
        public decimal VALOR { get => _VALOR; set => _VALOR = value; }
        public decimal m3 { get => _m3; set => _m3 = value; }
        public decimal pesokg { get => _pesokg; set => _pesokg = value; }

        public string mrn { get => _mrn; set => _mrn = value; }
        public string msn { get => _msn; set => _msn = value; }
        public string hsn { get => _hsn; set => _hsn = value; }
        public string ruc { get => _ruc; set => _ruc = value; }

        public string contenedor { get => _contenedor; set => _contenedor = value; }
        public string consignee_id { get => _consignee_id; set => _consignee_id = value; }
        public string consignee_name { get => _consignee_name; set => _consignee_name = value; }
        public string agent_id { get => _agent_id; set => _agent_id = value; }

        public decimal cantidad { get => _cantidad; set => _cantidad = value; }
        public bool apilable { get => _apilable; set => _apilable = value; }
        #endregion

        public P2D_Tarja_Cfs()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N4Middleware");
        }

        /*poblar */
        public bool PopulateMyData(out string OnError)
        {

            OnInit();

            parametros.Clear();
            parametros.Add("mrn", this.mrn);
            parametros.Add("msn", this.msn);
            parametros.Add("hsn", this.hsn);
            parametros.Add("ruc", this.ruc);
            parametros.Add("codigo", null);
            parametros.Add("validar", true);
            parametros.Add("apilable", apilable);

            var t = sql_puntero.ExecuteSelectOnly<P2D_Tarja_Cfs>(nueva_conexion, 6000, "[P2D].[validacion_p2d_impo_mrn_ruc]", parametros);
            if (t == null)
            {
                OnError = string.Format("No existe información con el número de carga ingresado: {0}-{1}-{2}", this.mrn, this.msn, this.hsn);
                return false;
            }

            this.contenedor = t.contenedor;
            this.m3 = t.m3;
            this.pesokg = t.pesokg;
            this.cantidad = t.cantidad;
            this.consignee_id = t.consignee_id;
            this.consignee_name = t.consignee_name;
            this.agent_id = t.agent_id;

            OnError = string.Empty;
            return true;
        }
    }
}
