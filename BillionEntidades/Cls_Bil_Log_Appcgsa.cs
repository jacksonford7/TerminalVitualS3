using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public  class Cls_Bil_Log_Appcgsa : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID;
        private string _USUARIO = string.Empty;
        private string _ID_AGENTE = string.Empty;
        private string _AGENTE = string.Empty;
        private string _ID_IMPORTADOR = string.Empty;
        private string _IMPORTADOR = string.Empty;
        private string _NUMERO_CARGA = string.Empty;
        private bool _SERVICIO;
        
       
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID= value; }
        public string USUARIO { get => _USUARIO; set => _USUARIO = value; }
        public string ID_AGENTE { get => _ID_AGENTE; set => _ID_AGENTE = value; }
        public string AGENTE { get => _AGENTE; set => _AGENTE = value; }
        public string ID_IMPORTADOR { get => _ID_IMPORTADOR; set => _ID_IMPORTADOR = value; }
        public string IMPORTADOR { get => _IMPORTADOR; set => _IMPORTADOR = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public bool SERVICIO { get => _SERVICIO; set => _SERVICIO = value; }

      
        #endregion

    
        public Cls_Bil_Log_Appcgsa()
        {
            init();
          
        }

        public Cls_Bil_Log_Appcgsa(string _USUARIO, string _ID_AGENTE, string _AGENTE, string _ID_IMPORTADOR, string _IMPORTADOR, string _NUMERO_CARGA, bool _SERVICIO)

        {
            this.USUARIO = _USUARIO;
            this.ID_AGENTE = _ID_AGENTE;
            this.AGENTE = _AGENTE;
            this.ID_IMPORTADOR = _ID_IMPORTADOR;
            this.IMPORTADOR = _IMPORTADOR;
            this.NUMERO_CARGA = _NUMERO_CARGA;
            this.SERVICIO = _SERVICIO;

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        private int? PreValidationsTransaction(out string msg)
        {

            
            if (string.IsNullOrEmpty(this.USUARIO))
            {
                msg = "Debe especificar el usuario";
                return 0;

            }

          


            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {
            //OnInit("STC");

            parametros.Clear();

            parametros.Add("USUARIO", this.USUARIO);
            parametros.Add("ID_AGENTE", this.ID_AGENTE);
            parametros.Add("AGENTE", this.AGENTE);
            parametros.Add("ID_IMPORTADOR", this.ID_IMPORTADOR);
            parametros.Add("IMPORTADOR", this.IMPORTADOR);
            parametros.Add("NUMERO_CARGA", this.NUMERO_CARGA);
            parametros.Add("SERVICIO", this.SERVICIO);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "bil_inserta_traza_appcgsa", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al registrar trza de appcgsa ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        //valida si tiene el servicio desactivado
        public static List<Cls_Bil_Log_Appcgsa> Valida_TieneServicio(string ruc, out string OnError)
        {
         
            parametros.Clear();
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Log_Appcgsa>(sql_puntero.Conexion_Local, 6000, "bil_valida_servicio_appcgsa", parametros, out OnError);

        }

        //valida si tiene el servicio por primera vez
        public static List<Cls_Bil_Log_Appcgsa> Valida_TieneServicio_PrimeraVez(string ruc, out string OnError)
        {

            parametros.Clear();
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Log_Appcgsa>(sql_puntero.Conexion_Local, 6000, "bil_valida_servicio_appcgsa_cfs", parametros, out OnError);

        }

        //valida si tiene el servicio por primera vez
        public static List<Cls_Bil_Log_Appcgsa> Valida_TieneServicio_PrimeraVez_Brbk(string ruc, out string OnError)
        {

            parametros.Clear();
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Log_Appcgsa>(sql_puntero.Conexion_Local, 6000, "bil_valida_servicio_appcgsa_brbk", parametros, out OnError);

        }


        //metodo para verificar si es agente
        public static bool? VerificaSiEsAgente(string login, out string OnError)
        {
            OnInit("SERVICE");
            parametros.Clear();
            parametros.Add("login", login);
            return sql_puntero.ExecuteSelectOnlyBool(nueva_conexion, 6000, "sp_user_AGENTE", parametros, out OnError);

        }
    }
}
