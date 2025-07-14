using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite
{
    public class EDI_hazard : BillionEntidades.Cls_Bil_Base
    {
        public long? id { get; set; }
        public long? idbooking { get; set; }
        public string un_na_number { get; set; }
        public string imdgClass { get; set; }
        public string hazardNumberType { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }

        #region "Constructores"
        public EDI_hazard()
        {
            base.init();
        }
        #endregion

        #region "Metodos"
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        private int? PreValidations(out string msg)
        {
            if (string.IsNullOrEmpty(this.imdgClass))
            {
                msg = "Especifique el IMDG Class";
                return 0;
            }

            msg = string.Empty;
            return 1;
        }

        public static List<EDI_hazard> ListHazard(long _id)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_idbooking", _id);
            return sql_puntero.ExecuteSelectControl<EDI_hazard>(nueva_conexion, 2000, "EDI_Hazard_consultar", parametros, out msg);
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            OnInit("N4Middleware");
            if (this.id > 0)
            {
                parametros.Add("i_id", this.id);
            }

            parametros.Add("i_idbooking", this.idbooking);
            parametros.Add("i_un_na_number", this.un_na_number);
            parametros.Add("i_imdgClass", this.imdgClass);
            parametros.Add("i_hazardNumberType", this.hazardNumberType); 
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "EDI_hazard_insert", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? Save_Anula(out string OnError)
        {
            parametros.Clear();
            //if (this.id > 0)
            //{
            OnInit("N4Middleware");
            //}

            parametros.Add("i_id", this.id);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "EDI_hazard_estado", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        #endregion
    }
}
