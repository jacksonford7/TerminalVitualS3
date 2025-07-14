using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_STC_Imagenes : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _id = 0;
        private string _ruc = string.Empty;
        private Int64 _gkey = 0;
        private string _contenedor = string.Empty;
        private string _mrn = string.Empty;
        private string _msn = string.Empty;
        private string _hsn = string.Empty;
        private string _imagen = string.Empty;
        private string _usuario = string.Empty;
        private string _nombre = string.Empty;
        private string _extension = string.Empty;
        private string _unidad = string.Empty;
        private string _id_importador = string.Empty;
        private static Int64? lm = -3;
        private string _xmlCabecera;
        private string _carga = string.Empty;
        private string _imagen_original = string.Empty;
        private DateTime? _fecha;
        #endregion

        #region "Propiedades"


        public Int64 id { get => _id; set => _id = value; }
        public string ruc { get => _ruc; set => _ruc = value; }
        public Int64 gkey { get => _gkey; set => _gkey = value; }
        public string contenedor { get => _contenedor; set => _contenedor = value; }
        public string mrn { get => _mrn; set => _mrn = value; }
        public string msn { get => _msn; set => _msn = value; }
        public string hsn { get => _hsn; set => _hsn = value; }
        public string imagen { get => _imagen; set => _imagen = value; }
        public string usuario { get => _usuario; set => _usuario = value; }
        public string nombre { get => _nombre; set => _nombre = value; }
        public string extension { get => _extension; set => _extension = value; }
        public string unidad { get => _unidad; set => _unidad = value; }
        public string id_importador { get => _id_importador; set => _id_importador = value; }
        public string xmlCabecera { get => _xmlCabecera; set => _xmlCabecera = value; }
        public string carga { get => _carga; set => _carga = value; }
        public string imagen_original { get => _imagen_original; set => _imagen_original = value; }
        public DateTime? fecha { get => _fecha; set => _fecha = value; }
        #endregion

        public Cls_STC_Imagenes()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_STC_Imagenes> Info_Aforo(string CONTENEDOR, out string OnError)
        {
            OnInit("STC");
            parametros.Clear();
            parametros.Add("CONTENEDOR", CONTENEDOR);

            return sql_puntero.ExecuteSelectControl<Cls_STC_Imagenes>(nueva_conexion, 6000, "ST_CONSULTA_AFORO_CONT", parametros, out OnError);

        }

        public static List<Cls_STC_Imagenes> Consulta_Imagenes(Int64 gkey, string mrn, string msn, string hsn, out string OnError)
        {
            OnInit("STC");
            parametros.Clear();
            parametros.Add("gkey", gkey);
            parametros.Add("mrn", mrn);
            parametros.Add("msn", msn);
            parametros.Add("hsn", hsn);
            return sql_puntero.ExecuteSelectControl<Cls_STC_Imagenes>(nueva_conexion, 6000, "stc_consulta_imagenes", parametros, out OnError);

        }

        #region "Proceso Grabar"
        private Int64? Save(out string OnError)
        {
            OnInit("STC");
            parametros.Clear();
            parametros.Add("xmlCab", this.xmlCabecera);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 8000, "stc_subir_imagenes", parametros, out OnError);
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
                    OnError = "*** Error: al grabar imagen ****";
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
        #endregion




    }
}
