using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSiteUnitLogic.Cls_pase_puerta
{
    public class ContenedorPase : Cls_Bil_Base
    {
        private string conN4;
        private bool estaConectado = false;
        public string contenedor { get; set; }
        public string bl { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public string documento { get; set; }
        public string pase { get; set; }
        public string tturno { get; set; }
        public string tinicio { get; set; }
        public string tfin { get; set; }
        public string importador { get; set; }
        public string sn { get; set; }
        public string ruc { get; set; }
        public string empresa { get; set; }
        public string placa { get; set; }
        public string licencia { get; set; }
        public string conductor { get; set; }
        public string item { get; set; }
        public string provincia { get; set; }
        public Int64? gkey { get; set; }
        public string telefono { get; set; }
        public decimal? neto { get; set; }
        public decimal? bruto { get; set; }
        public string sello1 { get; set; }
        public string sello2 { get; set; }
        public string sello3 { get; set; }
        public string sello4 { get; set; }
        public string selloCGSA { get; set; }
        public string buque { get; set; }
        public string aduana { get; set; }
        public string iso { get; set; }
        public string line { get; set; }
        public string ubicacion { get; set; }

        public ContenedorPase() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }
        public static List<ContenedorPase> ObtenePasesPuerta(long id)
        {
            try
            {
                OnInit("APPCGSA");
                parametros.Clear();

                parametros.Add("Gkey", id);


                // Reemplaza solo la parte de Data Source

                string controlError;
                var listaIv = sql_puntero.ExecuteSelectControl<ContenedorPase>(
                    nueva_conexion,
                    4000,
                    "[pdf].[ObtenerIdPaseXGkey]",
                    parametros,
                    out controlError
                );

                return listaIv;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al consultar las facturas.", ex);
            }
        }



        public static ContenedorPase ObtenePaseRegistroUnificado(long id, long gkey1)
        {
            try
            {
                OnInit("APPCGSA");
                parametros.Clear();
                parametros.Add("IdPase", id);

                string controlError;
                var datosCGSA = sql_puntero.ExecuteSelectControl<ContenedorPase>(
                    nueva_conexion,
                    4000,
                    "[pdf].[ObtenerRegistroPase]",
                    parametros,
                    out controlError
                );

                if (datosCGSA == null || datosCGSA.Count == 0)
                    throw new Exception("No se encontraron datos en APPCGSA");

                var dato1 = datosCGSA.First();
                if (!dato1.gkey.HasValue)
                    throw new Exception("GKEY no encontrado en el resultado de APPCGSA");

                long gkey = dato1.gkey.Value;

                // Paso 2: Datos desde N4
                OnInit("N5");
                parametros.Clear();
                parametros.Add("Idcarga", gkey1);

                var datosN4 = sql_puntero.ExecuteSelectControl<ContenedorPase>(
                    nueva_conexion,
                    4000,
                    "[pdf].[ObtenerRegistroPase]",
                    parametros,
                    out controlError
                );

                var dato2 = datosN4?.FirstOrDefault();

                if (dato2 != null)
                {
                    dato1.sello1 = dato2.sello1;
                    dato1.sello2 = dato2.sello2;
                    dato1.sello3 = dato2.sello3;
                    dato1.sello4 = dato2.sello4;
                    dato1.selloCGSA = dato2.selloCGSA;
                    dato1.buque = dato2.buque;
                    dato1.aduana = dato2.aduana;
                    dato1.iso = dato2.iso;
                    dato1.line = dato2.line;
                    dato1.ubicacion = dato2.ubicacion;
                }

                return dato1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener y unir los datos del pase", ex);
            }
        }

    }
}
