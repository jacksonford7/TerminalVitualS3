using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public  class Cls_Bil_Log_Carbono_Expo : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID;
        private string _RUC = string.Empty;
        private Int64 _GKEY;
        private string _BKG = string.Empty;
        private string _CNTR = string.Empty;
        private string _DAE = string.Empty;
        private bool _CARBONO ;
        private string _USUARIOING = string.Empty;

        private string _XMLDAE = string.Empty;
        private string _nombre = string.Empty;
        private string _email1 = string.Empty;
        private string _email2 = string.Empty;
        private string _email3 = string.Empty;
        

        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID= value; }
        public string RUC { get => _RUC; set => _RUC = value; }
        public Int64 GKEY { get => _GKEY; set => _GKEY = value; }
        public string BKG { get => _BKG; set => _BKG = value; }
        public string CNTR { get => _CNTR; set => _CNTR = value; }
        public string DAE { get => _DAE; set => _DAE = value; }
        public bool CARBONO { get => _CARBONO; set => _CARBONO = value; }
        public string USUARIOING { get => _USUARIOING; set => _USUARIOING = value; }

        public string XMLDAE { get => _XMLDAE; set => _XMLDAE = value; }
        public string nombre { get => _nombre; set => _nombre = value; }
        public string email1 { get => _email1; set => _email1 = value; }
        public string email2 { get => _email2; set => _email2 = value; }
        public string email3 { get => _email3; set => _email3 = value; }

        #endregion


        public Cls_Bil_Log_Carbono_Expo()
        {
            init();
          
        }

        public Cls_Bil_Log_Carbono_Expo(string _RUC, Int64 _GKEY, string _BKG, string _CNTR, string _DAE, bool _CARBONO, string _USUARIOING)

        {
            this.RUC = _RUC;
            this.GKEY = _GKEY;
            this.BKG = _BKG;
            this.CNTR = _CNTR;
            this.DAE = _DAE;
            this.CARBONO = _CARBONO;
            this.USUARIOING = _USUARIOING;

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        private int? PreValidationsTransaction(out string msg)
        {

            
            if (string.IsNullOrEmpty(this.RUC))
            {
                msg = "Debe especificar el usuario";
                return 0;

            }

          


            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {
       
            parametros.Clear();

            parametros.Add("RUC", this.RUC);
            parametros.Add("GKEY", this.GKEY);
            parametros.Add("BKG", this.BKG);
            parametros.Add("CNTR", this.CNTR);
            parametros.Add("DAE", this.DAE);
            parametros.Add("CARBONO", this.CARBONO);
            parametros.Add("USUARIOING", this.USUARIOING);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "bil_inserta_traza_carbono", parametros, out OnError);
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
                    OnError = "*** Error: al registrar trza de carbono neutro exportaciones ****";
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


        #region "Graba Tablas de csl_services"
        private Int64? Save_New( out string OnError)
        {
            OnInit("SERVICE");

            parametros.Clear();
            parametros.Add("XMLDAE", this.XMLDAE);
            parametros.Add("ruc", this.RUC);
            parametros.Add("nombre", this.nombre);
            parametros.Add("email1", this.email1);
            parametros.Add("email2", this.email2);
            parametros.Add("email3", this.email3);
            parametros.Add("usuario", this.USUARIOING);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 8000, "graba_asiv_carbono", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_New(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_New(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar carbono neutro ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction_New", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


    }
}
