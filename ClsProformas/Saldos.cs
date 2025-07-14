using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClsProformas;
using ControlOPC.Entidades;

namespace ClsProformas
{
    public class Saldos : Base
    {

        #region "Variables"

        private string _ruc = string.Empty;
        private string _leyenda = string.Empty;
        private decimal _saldo_final = 0;
         

        #endregion

        #region "Propiedades"

        public string ruc { get => _ruc; set => _ruc = value; }
        public string leyenda { get => _leyenda; set => _leyenda = value; }
        public decimal saldo_final { get => _saldo_final; set => _saldo_final = value; }


        #endregion

        /*conexion proformas*/
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("validar");
        }

        private static void OnInit_N4()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }


        public Saldos()
        {
            OnInit();
   
        }

        /*busca un nivel en especifico de la base de datos de nota de credito*/
        public static List<Saldos> Get_Saldo(string _id_ruc, long deposito)
        {
            OnInit_N4();
            string msg;
            parametros.Clear();
            parametros.Add("i_ruc", _id_ruc);
            parametros.Add("deposito", deposito);
            return sql_pointer.ExecuteSelectControl<Saldos>(v_conexion, 4000, "SP_SALDO_CREDITO_ZAL", parametros, out msg);
        }

    }
}
