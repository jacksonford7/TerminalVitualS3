
using Respuesta;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades
{



    namespace Entidades
    {
        [Serializable]
        [XmlRoot(ElementName = nameof(EventLog))]
        public class CertificadoEIR : Cls_Bil_Base
        {

            public Int64 Id { get; set; }
            public string ClientId { get; set; }
            public Int64 EventsId { get; set; }
            public Int64 UnitGkey { get; set; }
            public string Container { get; set; }
            public string weight { get; set; }
            public string seal1 { get; set; }
            public string seal2 { get; set; }
            public string seal3 { get; set; }
            public string seal4 { get; set; }

            public string mrn { get; set; }
            public string msn { get; set; }
            public string hsn { get; set; }
            public string bl { get; set; }
            public string numero_carga { get; set; }
            public string booking { get; set; }
            public string referencia { get; set; }
            public string categoria { get; set; }
            public DateTime EventDate { get; set; }
            public Int64? IdPase { get; set; }
            public string cedula_chofer { get; set; }
            public string Others { get; set; }
            public string Description { get; set; }
            public string placa { get; set; }
            public string dae { get; set; }
            public string sealcgsa { get; set; }
            public string empresa_tran { get; set; }
            public string Volumen { get; set; }
            public string TipoCarga { get; set; }
            public string nombre_chofer { get; set; }
            public string nombre_empresa { get; set; }
            public string cliente { get; set; }
            public string iso { get; set; }
            public string linea_naviera { get; set; }
            public string nave { get; set; }
            public string peso { get; set; }

            public string TEXTO1 { get; set; }
            public string TEXTO2 { get; set; }
            public string TITULO1 { get; set; }
            public string TITULO2 { get; set; }

            public CertificadoEIR()
            {

            }
            private static void OnInit(string Base)
            {
                sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
                parametros = new Dictionary<string, object>();
                nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
            }

            public List<CertificadoEIR> ver_certificado(int _id)
            {
                try
                {
                    string error = string.Empty;
                    parametros.Clear();
                    parametros.Add("Id", _id);


                    return sql_puntero.ExecuteSelectControl<CertificadoEIR>(sql_puntero.Conexion_Local, 5000, "[Bill].[ver_certificados_eir_TV]", parametros, out error);
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception("Ocurrió un error al consultar los sellos en la base de datos.", sqlEx);
                }
                catch (TimeoutException timeoutEx)
                {
                    throw new Exception("El tiempo de espera para la consulta de sellos ha expirado.", timeoutEx);
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocurrió un error inesperado al consultar los sellos.", ex);
                }
            }





        }
    }

}
