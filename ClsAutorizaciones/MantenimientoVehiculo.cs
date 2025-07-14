using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;
using System.Data;
using System.Reflection;

namespace ClsAutorizaciones
{
    public class MantenimientoVehiculo : Base
    {
        #region "Variables"

        private Int64 _ID = 0;
        private string _ID_VEHICULO = string.Empty;
        private string _PLACA = string.Empty;
        private string _TAG = string.Empty;
        private string _STATUS = string.Empty;
        private DateTime? _LICENCIA_EXPIRACION;
        private DateTime? _CABEZAL_EXPIRACION;
        private string _ID_EMPRESA = string.Empty;
        private string _RAZON_SOCIAL = string.Empty;
        private string _LICENCIA_EXPIRACION_MENSAJE = string.Empty;
        private string _CABEZAL_EXPIRACION_MENSAJE = string.Empty;
        private string _USUARIO_CRE = string.Empty;
        private bool _ESTADO = false;
        private string _LINEA_NAVIERA = string.Empty;
        private DateTime _FECHA_CREA;
        private string _MENSAJE = string.Empty;
        private string _MOTIVO = string.Empty;

        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public string ID_VEHICULO { get => _ID_VEHICULO; set => _ID_VEHICULO = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
        public string TAG { get => _TAG; set => _TAG = value; }
        public string STATUS { get => _STATUS; set => _STATUS = value; }


        public DateTime? LICENCIA_EXPIRACION { get => _LICENCIA_EXPIRACION; set => _LICENCIA_EXPIRACION = value; }
        public DateTime? CABEZAL_EXPIRACION { get => _CABEZAL_EXPIRACION; set => _CABEZAL_EXPIRACION = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
        public string RAZON_SOCIAL { get => _RAZON_SOCIAL; set => _RAZON_SOCIAL = value; }
        public string LICENCIA_EXPIRACION_MENSAJE { get => _LICENCIA_EXPIRACION_MENSAJE; set => _LICENCIA_EXPIRACION_MENSAJE = value; }
        public string CABEZAL_EXPIRACION_MENSAJE { get => _CABEZAL_EXPIRACION_MENSAJE; set => _CABEZAL_EXPIRACION_MENSAJE = value; }

        public string USUARIO_CRE { get => _USUARIO_CRE; set => _USUARIO_CRE = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string LINEA_NAVIERA { get => _LINEA_NAVIERA; set => _LINEA_NAVIERA = value; }
        public DateTime FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        public string MOTIVO { get => _MOTIVO; set => _MOTIVO = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public MantenimientoVehiculo()
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

            /*if (string.IsNullOrEmpty(this.ID))
            {
                msg = "Debe especificar el Vehículo";
                return 0;
            }*/

            if (string.IsNullOrEmpty(this.PLACA))
            {
                msg = "Debe especificar la placa Vehículo";
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
            parametros.Add("ID_VEHICULO", this.ID_VEHICULO);
            parametros.Add("PLACA", this.PLACA);
            parametros.Add("TAG", this.TAG);
            parametros.Add("STATUS", this.STATUS);
            parametros.Add("LICENCIA_EXPIRACION", this.LICENCIA_EXPIRACION);
            parametros.Add("CABEZAL_EXPIRACION", this.CABEZAL_EXPIRACION);
            parametros.Add("ID_EMPRESA", this.ID_EMPRESA);
            parametros.Add("USUARIO_CRE", this.USUARIO_CRE);
            parametros.Add("ESTADO", this.ESTADO);
            parametros.Add("LINEA_NAVIERA", this.LINEA_NAVIERA);
            parametros.Add("MENSAJE", this.MENSAJE);
            parametros.Add("MOTIVO", this.MOTIVO);
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "RVA_GRABA_NO_AUTORIZACION_VEHICULO", parametros, out OnError);
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
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "RVA_ELIMINA_AUTORIZACION_VEHICULO", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }


        /*listado para mostrar vehiculos excluidos y poder filtrar*/
        public static List<MantenimientoVehiculo> Listado_Vehiculos(string criterio, string LINEA_NAVIERA, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("LINEA_NAVIERA", LINEA_NAVIERA);
            parametros.Add("criterio", criterio);
            return sql_pointer.ExecuteSelectControl<MantenimientoVehiculo>(v_conexion, 6000, "RVA_LISTAR_VEHICULOS_NOAUTORIZADOS", parametros, out OnError);
        }

       

        public bool Existe_Vehiculo(out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("ID_EMPRESA",this.ID_EMPRESA);
            parametros.Add("ID_VEHICULO", this.ID_VEHICULO);
            parametros.Add("PLACA", this.PLACA);
            parametros.Add("LINEA_NAVIERA", this.LINEA_NAVIERA);

            var t = sql_pointer.ExecuteSelectOnly<MantenimientoVehiculo>(v_conexion, 6000, "RVA_VALIDA_VEHICULO", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }

            this.ID = t.ID;
            this.ID_VEHICULO = t.ID_VEHICULO;
            this.PLACA = t.PLACA;

            OnError = string.Empty;
            return true;
        }

        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
    }
}
