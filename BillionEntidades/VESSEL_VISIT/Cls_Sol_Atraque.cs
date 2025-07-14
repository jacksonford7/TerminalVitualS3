using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using System.Data;
using System.Reflection;


namespace BillionEntidades
{
    public class Cls_Sol_Atraque : Cls_Bil_Base
    {
        private static String v_mensaje = string.Empty;

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID;
        private string _referencia = string.Empty;
        private string _cae_manifiestoImpo_ant = string.Empty;
        private string _cae_manifiestoExpo_ant = string.Empty;
        private string _nave_codigoViajeIn_ant = string.Empty;
        private string _nave_codigoViajeOut_ant = string.Empty;
        private DateTime? _veo_eta_ant;
        private DateTime? _veo_ets_ant;
        private int _veo_horasUsoMuelle_ant;

        private string _embarqueplanificado_ant;
        private string _cutoffbbk_ant;
        private int _contador;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
        public string referencia { get => _referencia; set => _referencia = value; }
        public string cae_manifiestoImpo_ant { get => _cae_manifiestoImpo_ant; set => _cae_manifiestoImpo_ant = value; }
        public string cae_manifiestoExpo_ant { get => _cae_manifiestoExpo_ant; set => _cae_manifiestoExpo_ant = value; }
        public string nave_codigoViajeIn_ant { get => _nave_codigoViajeIn_ant; set => _nave_codigoViajeIn_ant = value; }
        public string nave_codigoViajeOut_ant { get => _nave_codigoViajeOut_ant; set => _nave_codigoViajeOut_ant = value; }
        public DateTime? veo_eta_ant { get => _veo_eta_ant; set => _veo_eta_ant = value; }
        public DateTime? veo_ets_ant { get => _veo_ets_ant; set => _veo_ets_ant = value; }
        public int veo_horasUsoMuelle_ant { get => _veo_horasUsoMuelle_ant; set => _veo_horasUsoMuelle_ant = value; }

        public string embarqueplanificado_ant { get => _embarqueplanificado_ant; set => _embarqueplanificado_ant = value; }
        public string cutoffbbk_ant { get => _cutoffbbk_ant; set => _cutoffbbk_ant = value; }
        public int contador { get => _contador; set => _contador = value; }

        #endregion

        public Cls_Sol_Atraque()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public bool Update(out string OnError)
        {
            OnInit("PORTAL_CGSA");
            parametros.Clear();
            parametros.Add("referencia", this.referencia);
            parametros.Add("cae_manifiestoImpo_ant", this.cae_manifiestoImpo_ant);
            parametros.Add("cae_manifiestoExpo_ant", this.cae_manifiestoExpo_ant);
            parametros.Add("nave_codigoViajeIn_ant", this.nave_codigoViajeIn_ant);

            parametros.Add("nave_codigoViajeOut_ant", this.nave_codigoViajeOut_ant);
            parametros.Add("veo_eta_ant", this.veo_eta_ant);
            parametros.Add("veo_ets_ant", this.veo_ets_ant);
            parametros.Add("veo_horasUsoMuelle_ant", this.veo_horasUsoMuelle_ant);

            parametros.Add("embarqueplanificado_ant", this.embarqueplanificado_ant);
            parametros.Add("cutoffbbk_ant", this.cutoffbbk_ant);


           
            var db = sql_puntero.ExecuteInsertUpdateDelete(nueva_conexion, 6000, "sol_atra_modifica", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return false;
            }

            OnError = string.Empty;
            
            return true;
        }
    }
}
