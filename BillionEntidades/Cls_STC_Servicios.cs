using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public  class Cls_STC_Servicios : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _id_registro;
        private string _ruc = string.Empty;
        private DateTime? _fecha_registro = null;
        private string _empresa = string.Empty;
        private string _email = string.Empty;
       
        #endregion

        #region "Propiedades"

        public Int64 id_registro { get => _id_registro; set => _id_registro = value; }
        public string ruc { get => _ruc; set => _ruc = value; }
        public DateTime? fecha_registro { get => _fecha_registro; set => _fecha_registro = value; }
        public string empresa { get => _empresa; set => _empresa = value; }
        public string email { get => _email; set => _email = value; }
      

        #endregion

        public List<Cls_Bil_Invoice_Detalle> Detalle { get; set; }
        public List<Cls_Bil_Invoice_Servicios> DetalleServicios { get; set; }

        public Cls_STC_Servicios()
        {
            init();
          
        }

        public Cls_STC_Servicios(Int64 _id_registro,string _ruc, DateTime? _fecha_registro, string _empresa ,string _email)

        {
            this.id_registro = _id_registro;
            this.ruc = _ruc;
            this.fecha_registro = _fecha_registro;
            this.empresa = _empresa;
            this.email = _email;
           

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        private int? PreValidationsTransaction(out string msg)
        {

            
            if (string.IsNullOrEmpty(this.ruc))
            {
                msg = "Debe especificar el ruc";
                return 0;

            }

            if (string.IsNullOrEmpty(this.empresa))
            {
                msg = "Debe especificar el nombre de la empresa";
                return 0;

            }
            if (string.IsNullOrEmpty(this.email))
            {
                msg = "Debe especificar un email";
                return 0;

            }


            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {
            OnInit("STC");

            parametros.Clear();

            parametros.Add("ruc", this.ruc);
            parametros.Add("empresa", this.empresa);
            parametros.Add("email", this.email);
          
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "stc_registra_empresa", parametros, out OnError);
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
                    OnError = "*** Error: al registrar empresa ****";
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

    }
}
