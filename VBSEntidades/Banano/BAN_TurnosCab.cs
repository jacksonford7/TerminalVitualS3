using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace VBSEntidades.Banano
{
    public class BAN_TurnosCab : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? id { get; set; }
        public string idNave { get; set; }
        public string nave { get; set; }
        public string idAgente { get; set; }
        public string Agente { get; set; }
        public string mrn { get; set; }
        public DateTime eta { get; set; }
        public DateTime? fecha { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public List<BAN_TurnosDet> Detalle { get; set; }
        public long idPlantillaCab { get; set; }
        #endregion

        public BAN_TurnosCab() : base()
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

        public static List<BAN_TurnosCab> listadotarjaCab(string _estado, out string OnError)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_estado", _estado);
            return sql_puntero.ExecuteSelectControl<BAN_TurnosCab>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_TURNOCAB", parametros, out OnError);
        }

        public static BAN_TurnosCab GetTurnoCab(long _id)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_TurnosCab>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_TURNOCAB", parametros);
            //if (obj != null)
            //{
            //    try { obj.Estados = estados.GetEstado(obj.estado); } catch { }
            //}

            return obj;
        }

        public static BAN_TurnosCab GetTurnoCab(string _nave, string _idAgente, string _mrn, out string OnError)
        {
            OnError = string.Empty;
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_nave", _nave);
            parametros.Add("i_idAgente", _idAgente);
            parametros.Add("i_mrn", _mrn);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_TurnosCab>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_TURNOCABESP", parametros);
            if (obj != null)
            {
                try
                {
                    //obj.Estados = estados.GetEstado(obj.estado);
                    obj.Detalle = BAN_TurnosDet.listadoTurnoDet(long.Parse(obj.id.ToString()), out OnError);
                }
                catch { OnError = ""; }

            }
            else
            {
                OnError = "";
            }

            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            //OnInit("N4Middleware");

            //using (var scope = new TransactionScope())
            //{
                parametros.Clear();
                parametros.Add("i_id", this.id);
                parametros.Add("i_idNave", this.idNave);
                parametros.Add("i_nave", this.nave);
                parametros.Add("i_idAgente", this.idAgente);
                parametros.Add("i_Agente", this.Agente);
                parametros.Add("i_mrn", this.mrn);
                parametros.Add("i_eta", this.eta);
                parametros.Add("i_fecha", this.fecha);
                parametros.Add("i_estado", this.estado);
                parametros.Add("i_usuarioCrea", this.usuarioCrea);
                parametros.Add("i_usuarioModifica", this.usuarioModifica);
                parametros.Add("i_idPlantillaCab", this.idPlantillaCab);

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "BAN_INSERTAR_TURNO_CAB", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return null;
                }
                OnError = string.Empty;

                //foreach (var oItem in Detalle)
                //{
                //    oItem.idTarja = db.Value;
                //    oItem.usuarioCrea = this.usuarioCrea;
                //    var dbItem = oItem.Save_Update(out OnError);

                //    if (!dbItem.HasValue || dbItem.Value < 0)
                //    {
                //        return null;
                //    }
                //}
                //scope.Complete();
                return db.Value;
            //}


        }

        
    }
}
