using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TRANSP_Doc_Colaborador : Cls_Bil_Base
    {
        private static Int64? lm = -3;

        #region "Propiedades"

        public string NOMINA_COD { get; set; }
        public Int64 ID_SOLICITUD { get; set; }    
        public string COD_SOLICITUD { get; set; }
        public string DESC_SOLICITUD { get; set; }
        public string TIPO_SOLCIITUD { get; set; }      
        public Int64 ID_DOCUMENTO { get; set; }
        public string COD_DOCUMENTO { get; set; }
        public string DESC_DOCUMENTO { get; set; }
        public string EXT_DOCUMENTO { get; set; }
        public string RUTA { get; set; }
        public DateTime? FECHA_CADUCA { get; set; }
        public string ESTADO { get; set; }
        public string RUTA_FINAL { get; set; }
        #endregion



        public Cls_TRANSP_Doc_Colaborador()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_TRANSP_Doc_Colaborador> Carga_Documento_Colaborador( out string OnError)
        {
            OnInit("Portal_Sca");
            return sql_puntero.ExecuteSelectControl<Cls_TRANSP_Doc_Colaborador>(nueva_conexion, 7000, "TRANSP_CONSULTA_DOCUMENTO_COLABORADOR", parametros, out OnError);

        }

        public Int64? Save_Colaborador(out string OnError)
        {

            OnInit("Portal_Sca");

            parametros.Clear();
            parametros.Add("IDSOLICITUD", this.ID_SOLICITUD);
            parametros.Add("IDDOCEMP", this.ID_DOCUMENTO);
            parametros.Add("RUTADOCUMENTO", this.RUTA_FINAL);
            parametros.Add("USUARIOING", this.IV_USUARIO_CREA);
           

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "TRANSP_REGISTRA_DOC_SOL_COLABORADOR", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


    }
}
