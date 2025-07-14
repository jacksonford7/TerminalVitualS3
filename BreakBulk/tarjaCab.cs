using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BreakBulk
{
    public class tarjaCab : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idTarja { get; set; }
        public string idNave { get; set; }
        public string nave { get; set; }
        public string carrier_id { get; set; }
        public string idAgente { get; set; }
        public string Agente { get; set; }
        public string mrn { get; set; }
        public DateTime eta { get; set; }
        public DateTime? fecha { get; set; }
        public string estado { get; set; }
        public estados Estados { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public List<tarjaDet> Detalle { get; set; }
        #endregion

        public tarjaCab(): base()
        {
            //init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = null;
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<tarjaCab> listadotarjaCab(string _estado,out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_estado", _estado);
            return sql_puntero.ExecuteSelectControl<tarjaCab>(nueva_conexion, 4000, "[brbk].consultartarjaCab", parametros, out OnError);
        }

        public static tarjaCab GetTarjaCab(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<tarjaCab>(nueva_conexion, 4000, "[brbk].consultartarjaCab", parametros);
            if (obj != null)
            {
                try { obj.Estados = estados.GetEstado(obj.estado); } catch { }
            }
           
            return obj;
        }

        public static tarjaCab GetTarjaCab(string _nave,string _idAgente, string _mrn, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_nave", _nave);
            parametros.Add("i_idAgente", _idAgente);
            parametros.Add("i_mrn", _mrn);
            var obj = sql_puntero.ExecuteSelectOnly<tarjaCab>(nueva_conexion, 4000, "[brbk].consultartarjaCabEsp", parametros);
            if (obj != null)
            {
                try { obj.Estados = estados.GetEstado(obj.estado);
                    obj.Detalle = tarjaDet.listadoTarjaDet(long.Parse(obj.idTarja.ToString()), out OnError);
                } catch { OnError = ""; }
                
            }
            else
            {
                OnError = "";
            }
            
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            OnInit("N4Middleware");

            using (var scope = new System.Transactions.TransactionScope())
            {
                parametros.Clear();
                parametros.Add("i_idTarja", this.idTarja);
                parametros.Add("i_idNave", this.idNave);
                parametros.Add("i_carrier_id", this.carrier_id);
                parametros.Add("i_nave", this.nave);
                parametros.Add("i_idAgente", this.idAgente);
                parametros.Add("i_Agente", this.Agente);
                parametros.Add("i_mrn", this.mrn);
                parametros.Add("i_eta", this.eta);
                parametros.Add("i_fecha", this.fecha);
                parametros.Add("i_estado", this.estado);
                parametros.Add("i_usuarioCrea", this.usuarioCrea);
                parametros.Add("i_usuarioModifica", this.usuarioModifica);

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].insertarTarjaCab", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return null;
                }
                OnError = string.Empty;

                foreach (var oItem in Detalle)
                {
                    oItem.idTarja = db.Value;
                    oItem.usuarioCrea = this.usuarioCrea;
                    var dbItem = oItem.Save_Update(out OnError);

                    if (!dbItem.HasValue || dbItem.Value < 0)
                    {
                        return null;
                    }
                }
                scope.Complete();
                return db.Value;
            }
            
            
        }

        public Int64? Save_Delete(out string OnError)
        {
            OnInit("N4Middleware");

            using (var scope = new System.Transactions.TransactionScope())
            {
                parametros.Clear();
                parametros.Add("i_idTarja", this.idTarja);
                parametros.Add("i_usuarioCrea", this.usuarioCrea);

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].deleteTarjaCab", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }


        }
    }
}
