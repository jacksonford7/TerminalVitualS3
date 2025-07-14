using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceptioMtyStock
{
    public class Sellos : Cls_Bil_Base
    {
        private long _ROW_ID;
        private long _PRE_GATE_ID;
        private string _CHOFER;
        private string _PLACA;
        private string _CONTENEDOR;
        private string _SELLO_CGSA; 
        private string _TIPO_TRX;
        private string _PHOTO1;
        private string _PHOTO2;
        private string _PHOTO3;
        private string _PHOTO4;
        private DateTime _DATE;
        private string _USUARIO_CREA;
        private long _GKEY;

        public long ROW_ID { get => _ROW_ID; set => _ROW_ID = value; }
        public long PRE_GATE_ID { get => _PRE_GATE_ID; set => _PRE_GATE_ID = value; }
        public string CHOFER { get => _CHOFER; set => _CHOFER = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public string SELLO_CGSA { get => _SELLO_CGSA; set => _SELLO_CGSA = value; }
        public string TIPO_TRX { get => _TIPO_TRX; set => _TIPO_TRX = value; }
        public string PHOTO1 { get => _PHOTO1; set => _PHOTO1 = value; }
        public string PHOTO2 { get => _PHOTO2; set => _PHOTO2 = value; }
        public string PHOTO3 { get => _PHOTO3; set => _PHOTO3 = value; }
        public string PHOTO4 { get => _PHOTO4; set => _PHOTO4 = value; }
        public DateTime DATE { get => _DATE; set => _DATE = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public long GKEY { get => _GKEY; set => _GKEY = value; }

        #region "Constructores"
        public Sellos()
        {

            base.init();
        }
        #endregion

        #region "Metodos"

        private static void OnInit(string Base)
        {
            //sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            //parametros = new Dictionary<string, object>();
            //v_conexion = Extension.Nueva_Conexion("RECEPTIO");

            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.CONTENEDOR) )
            {
                msg = "Especifique el contenedor";
                return 0;
            }
           
            msg = string.Empty;
            return 1;
        }

        public static List<Sellos> ListSellos(DateTime? _desde ,DateTime? _hasta, string _container)
        {
            OnInit("RECEPTIO");
            string msg;
            parametros.Clear();
            parametros.Add("i_fechaDesde", _desde);
            parametros.Add("i_fechaHasta", _hasta);
            parametros.Add("i_contenedor", _container);
            return sql_puntero.ExecuteSelectControl<Sellos>(nueva_conexion, 2000, "sp_selloContainers", parametros, out msg);
        }

        public static Sellos Sello(long _id,  out string msg)
        {
            OnInit("RECEPTIO");
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectControl<Sellos>(nueva_conexion, 2000, "sp_selloContainersXId", parametros, out msg).FirstOrDefault();
        }
        #endregion
    }
}