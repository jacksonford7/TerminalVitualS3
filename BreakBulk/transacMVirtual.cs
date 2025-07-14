using BillionEntidades;
using N4.Exportacion;
using Respuesta;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakBulk
{
    public class transacMVirtual : Cls_Bil_Base
    {
        public string SEAL_1 { get; set; }
        public string SEAL_2 { get; set; }
        public string SEAL_3 { get; set; }
        public string SEAL_4 { get; set; }
        public DateTime FECHA { get; set; }
      

        public transacMVirtual() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        public static List<transacMVirtual> GetSellos(int _id)
        {
            try
            {
                OnInit("N4Middleware");
                parametros.Clear();
                parametros.Add("gkey", _id);

                string controlError;
                var listaSellos = sql_puntero.ExecuteSelectControl<transacMVirtual>(
                    nueva_conexion,
                    4000,
                    "[Bill].[consultar_sellos]",
                    parametros,
                    out controlError
                );

                if (!string.IsNullOrEmpty(controlError))
                {
                    throw new Exception($"Error en la consulta de sellos: {controlError}");
                }

                return listaSellos ?? new List<transacMVirtual>(); 
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

        public static int GetAforo(int gkey)
        {
            try
            {
                OnInit("N5"); 
                parametros.Clear();
                parametros.Add("gkey", gkey);

                int resultado = sql_puntero.ExecuteSelectOnlyValue<int>(
                    nueva_conexion,      
                    4000,                
                    "[dbo].[mb_unit_aforo]", 
                    parametros
                );

                Console.WriteLine($"Resultado del SP: {resultado}"); // Debug

                return resultado; // Devuelve el COUNT del SP
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Error en la base de datos al consultar aforo.", sqlEx);
            }
            catch (TimeoutException timeoutEx)
            {
                throw new Exception("Tiempo de espera agotado para la consulta de aforo.", timeoutEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al consultar aforo.", ex);
            }
        }

        public static List<Bloqueos> GetBloqueos(int gkey, string booking)
        {
            try
            {
                OnInit("N5"); 
                parametros.Clear();
                parametros.Add("cntr", booking);
                parametros.Add("key", gkey);

                string controlError;
                var resultado = sql_puntero.ExecuteSelectControl<Bloqueos>(
                    nueva_conexion,      
                    4000,                
                    "[dbo].[pc_unit_hold]", 
                    parametros,
                    out controlError
                );

                if (!string.IsNullOrEmpty(controlError))
                {
                    throw new Exception($"Error en la consulta de bloqueos: {controlError}");
                }

            

                return resultado; 
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Error en la base de datos al consultar bloqueos.", sqlEx);
            }
            catch (TimeoutException timeoutEx)
            {
                throw new Exception("Tiempo de espera agotado para la consulta de bloqueos.", timeoutEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al consultar bloqueos.", ex);
            }
        }
   
        public class Bloqueos 
        {
            public long Gkey { get; set; }
            public string Id { get; set; }
            public string Creador { get; set; }
            public DateTime FECHA { get; set; }
            public string Proposito { get; set; }

            public string Description { get; set; }
            public bool Estado { get; set; }
        }
    }
}
