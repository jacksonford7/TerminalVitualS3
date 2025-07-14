using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace MonitorVacios.Entidades
{
   public class Contenedor : Base
    {
        #region "Variables"

        private Int64 _ID=0;
        private DateTime? _FECHA ;
        private string _AUTORIZACION = string.Empty;
        private string _REFERENCIA = string.Empty;
        private Int64 _SECUENCIA = 0;
        
        private string _CONTENEDOR = string.Empty;
        private Int64 _GKEY = 0;
        private string _GRUPO = string.Empty;
        private string _REF_FINAL = string.Empty;
        private string _LINEA_NAVIERA = string.Empty;
        private string _MENSAJE = string.Empty;
        private string _PROCESO = string.Empty;
        private string _ESTADO_PROCESO = string.Empty;
        private Int32 _Tipo = 0;
        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }

        public string AUTORIZACION { get => _AUTORIZACION; set => _AUTORIZACION = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }

        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public Int64 GKEY { get => _GKEY; set => _GKEY = value; }

        public string GRUPO { get => _GRUPO; set => _GRUPO = value; }
        public string REF_FINAL { get => _REF_FINAL; set => _REF_FINAL = value; }
   
        public string LINEA_NAVIERA { get => _LINEA_NAVIERA; set => _LINEA_NAVIERA = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        public string PROCESO { get => _PROCESO; set => _PROCESO = value; }
        public string ESTADO_PROCESO { get => _ESTADO_PROCESO; set => _ESTADO_PROCESO = value; }
        public Int32 Tipo { get => _Tipo; set => _Tipo = value; }

        #endregion

        private static String v_mensaje = string.Empty;


        public Contenedor()
        {
            init();
           
            OnInit();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        public static List<Contenedor> CONSULTA_CONTENEDORES(int registros, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("registros", registros);
            //RVA_PROCESAR_CONTENEDORES
            return sql_pointer.ExecuteSelectControl<Contenedor>(v_conexion, 6000, "RVA_PROCESAR_CONTENEDORES", parametros, out OnError);

        }

        public bool Marcar(out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID", this.ID);
            parametros.Add("SECUENCIA", this.SECUENCIA);
            parametros.Add("MENSAJE", this.MENSAJE);
            parametros.Add("PROCESO", this.PROCESO);
            parametros.Add("ESTADO_PROCESO", this.ESTADO_PROCESO);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "RVA_MARCAR_CONTENEDOR", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        public bool Valida_Linea(out string OnError)
        {
            parametros.Clear();
            parametros.Add("Tipo", this.Tipo);
            parametros.Add("RVA_LINE", this.LINEA_NAVIERA);
           
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "sp_procesos", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

    }
}
