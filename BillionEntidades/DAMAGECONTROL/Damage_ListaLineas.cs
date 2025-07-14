using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using ServicesEntities;
using AccesoDatos;

namespace BillionEntidades
{
    public class Damage_ListaLineas : Cls_Bil_Base
    {
        public Int64 LIN_ID { get; set; }
        public string LIN_CODIGO { get; set; }
        public string LIN_DESCRIP { get; set; }
        public bool LIN_ESTADO { get; set; }
        public string LIN_USER_CREA { get; set; }
        public DateTime? LIN_DATE_CREA { get; set; }
        public string LIN_USER_UPD { get; set; }
        public DateTime? LIN_DATE_UPD { get; set; }

        public string LIN_ESTADO2 { get; set; }

        public Damage_ListaLineas()
        {
            init();

        }

        public ResultadoOperacion<List<Damage_ListaLineas>> GetListaLineas()
        {
            parametros.Clear();

            var rp = BDOpe.ComandoSelectALista<Damage_ListaLineas>(sql_puntero.Conexion_Local, "DAMAGE_LISTAR_LINEAS", null);
            return rp.Exitoso ? ResultadoOperacion<List<Damage_ListaLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Damage_ListaLineas>>.CrearFalla(rp.MensajeProblema);

        }

        public string GetLineaValida(string pista)
        {
            parametros.Clear();
            parametros.Add("NOMBRE_PARAMETRO ", pista);

            var rp = BDOpe.ComandoTransaccionString(sql_puntero.Conexion_Local, "DAMAGE_EXISTE_LINEA", parametros);

            return rp.Resultado;

        }

        public ResultadoOperacion<List<Damage_ListaLineas>> GuardarLinea(Damage_ListaLineas obj)
        {
            parametros.Clear();
            parametros.Add("LIN_CODIGO ", obj.LIN_CODIGO);
            parametros.Add("LIN_DESCRIP ", obj.LIN_DESCRIP);
            parametros.Add("LIN_USER_CREA ", obj.LIN_USER_CREA);





            var rp = BDOpe.ComandoSelectALista<Damage_ListaLineas>(sql_puntero.Conexion_Local, "DAMAGE_INSERT_LINEA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Damage_ListaLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Damage_ListaLineas>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<Damage_ListaLineas>> EditarLinea(Damage_ListaLineas obj)
        {
            parametros.Clear();
            parametros.Add("LIN_ID", obj.LIN_ID);
            parametros.Add("LIN_CODIGO ", obj.LIN_CODIGO);
            parametros.Add("LIN_DESCRIP ", obj.LIN_DESCRIP);
            parametros.Add("LIN_USER_UPD ", obj.LIN_USER_UPD);
            parametros.Add("LIN_ESTADO ", obj.LIN_ESTADO);

            var rp = BDOpe.ComandoSelectALista<Damage_ListaLineas>(sql_puntero.Conexion_Local, "DAMAGE_UPDATE_LINEA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Damage_ListaLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Damage_ListaLineas>>.CrearFalla(rp.MensajeProblema);

        }

        public static List<Damage_ListaLineas> ComboLineas(out string OnError)
        {
           
            parametros.Clear();

            return sql_puntero.ExecuteSelectControl<Damage_ListaLineas>(sql_puntero.Conexion_Local, 6000, "DAMAGE_COMBO_LINEAS", null, out OnError);

        }

    }
}
