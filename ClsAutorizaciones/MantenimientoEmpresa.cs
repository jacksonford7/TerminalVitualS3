using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsAutorizaciones
{
    public class MantenimientoEmpresa : Base
    {

        #region "Variables"

        private Int64 _ID = 0;
        private string _ID_EMPRESA = string.Empty;
        private string _RAZON_SOCIAL = string.Empty;
        private string _USUARIO_CRE = string.Empty;
        private bool _ESTADO = false;
        private string _LINEA_NAVIERA = string.Empty;
        private DateTime _FECHA_CREA ;

        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
        public string RAZON_SOCIAL { get => _RAZON_SOCIAL; set => _RAZON_SOCIAL = value; }
        public string USUARIO_CRE { get => _USUARIO_CRE; set => _USUARIO_CRE = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string LINEA_NAVIERA { get => _LINEA_NAVIERA; set => _LINEA_NAVIERA = value; }
        public DateTime FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public MantenimientoEmpresa()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.ID_EMPRESA))
            {
                msg = "Debe especificar el ruc de la empresa de transporte";
                return 0;
            }

            if (string.IsNullOrEmpty(this.RAZON_SOCIAL))
            {
                msg = "Debe especificar el nombre de la empresa de transporte";
                return 0;
            }

            if (string.IsNullOrEmpty(this.LINEA_NAVIERA))
            {
                msg = "Debe especificar la línea naviera";
                return 0;
            }
            if (string.IsNullOrEmpty(this.USUARIO_CRE))
            {
                msg = "Especifique el usuario creador del registro";
                return 0;
            }

            msg = string.Empty;
            return 1;

        }

        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

            OnInit();

            parametros.Clear();
            parametros.Add("ID_EMPRESA", this.ID_EMPRESA);
            parametros.Add("RAZON_SOCIAL", this.RAZON_SOCIAL);
            parametros.Add("USUARIO_CRE", this.USUARIO_CRE);
            parametros.Add("ESTADO", this.ESTADO);
            parametros.Add("LINEA_NAVIERA", this.LINEA_NAVIERA);

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "RVA_GRABA_AUTORIZACION_EMPRESA", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

        public bool Delete(out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID", this.ID);
            parametros.Add("USUARIO_CRE", this.USUARIO_CRE);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "RVA_ELIMINA_AUTORIZACION_EMPRESA", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }


        public bool Existe_EmpresaTransporte(out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("ID_EMPRESA", this.ID_EMPRESA);
            parametros.Add("LINEA_NAVIERA", this.LINEA_NAVIERA);

            var t = sql_pointer.ExecuteSelectOnly<MantenimientoEmpresa>(v_conexion, 6000, "RVA_VALIDA_EMPRESA_TRANSPORTE", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }
            this.ID_EMPRESA = t.ID_EMPRESA;
            this.RAZON_SOCIAL = t.RAZON_SOCIAL;
          
            OnError = string.Empty;
            return true;
        }

        public static List<MantenimientoEmpresa> Listado_Empresas(string LINEA_NAVIERA, out string OnError)
        {
            OnInit();
         
            parametros.Clear();
            parametros.Add("LINEA_NAVIERA", LINEA_NAVIERA);
            return sql_pointer.ExecuteSelectControl<MantenimientoEmpresa>(v_conexion, 6000, "RVA_LISTADO_EMPRESA_TRANSPORTE", parametros, out OnError);
        }

        /*listado para mostrar buscador de grupos creados*/
        public static List<MantenimientoEmpresa> Buscador_Empresas(string criterio, string LINEA_NAVIERA, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("LINEA_NAVIERA", LINEA_NAVIERA);
            parametros.Add("criterio", criterio);
            return sql_pointer.ExecuteSelectControl<MantenimientoEmpresa>(v_conexion, 6000, "RVA_BUSCADOR_EMPRESA_TRANSPORTE", parametros, out OnError);
        }

        public static List<MantenimientoEmpresa> ComboBox_Empresas(string LINEA_NAVIERA, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("LINEA_NAVIERA", LINEA_NAVIERA);
            return sql_pointer.ExecuteSelectControl<MantenimientoEmpresa>(v_conexion, 6000, "RVA_LISTA_EMPRESA_TRANSPORTE_RPT", parametros, out OnError);
        }


    }
}
