using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Catalogo_Bodega : Cls_Bil_Base
    {
        #region "Propiedades"
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int idTipo { get; set; }
        public int capacidadBox { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public BAN_Catalogo_TipoBodega oTipoBodega { get; set; }
        #endregion

        public BAN_Catalogo_Bodega()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Catalogo_Bodega> ConsultarLista(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Catalogo_Bodega_Consultar]", null, out OnError);
        }


        public static BAN_Catalogo_Bodega GetEntidad(int _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Catalogo_Bodega_Consultar]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_codigo", this.codigo);
            parametros.Add("i_nombre", this.nombre);
            parametros.Add("i_idTipo", this.idTipo);
            parametros.Add("i_capacidadBox", this.capacidadBox);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_estado", this.estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "BAN_Catalogo_Bodega_Insertar", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

    }
}
