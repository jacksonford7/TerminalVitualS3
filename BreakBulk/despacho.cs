using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace BreakBulk
{
    public class despacho : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idDespacho { get; set; }
        public long idTarjaDet { get; set; }
        public string pase { get; set; }
        public tarjaDet tarjaDet { get; set; }
        public string bl { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public string placa { get; set; }
        public string idchofer { get; set; }
        public string chofer { get; set; }
        public decimal cantidad { get; set; }
        public string observacion { get; set; }
        public string estado { get; set; }
        public estados Estados { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public despacho() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<despacho> listadoDespacho(long _idTarjaDet, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idTarjaDet", _idTarjaDet);
            return sql_puntero.ExecuteSelectControl<despacho>(nueva_conexion, 4000, "[brbk].consultarDespacho", parametros, out OnError);
        }

        public static despacho GetDespacho(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idDespacho", _id);
            var obj = sql_puntero.ExecuteSelectOnly<despacho>(nueva_conexion, 4000, "[brbk].consultarDespacho", parametros);
            try
            {
                obj.tarjaDet = tarjaDet.GetTarjaDet(obj.idTarjaDet);
                obj.Estados = estados.GetEstado(obj.estado);
            }
            catch { }
            return obj;
        }
    }
}
