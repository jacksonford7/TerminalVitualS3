using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Catalogo_Ubicacion : Cls_Bil_Base
    {
        #region "Propiedades"
        public int? id { get; set; }
        public int? idBodega { get; set; }
        public int? idBloque { get; set; }
        public int? idFila { get; set; }
        public int? idAltura { get; set; }
        public int? idProfundidad { get; set; }
        public string barcode { get; set; }
        public string descripcion { get; set; }
        public int? capacidadBox { get; set; }
        public int? mt2 { get; set; }
        public bool? disponible { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public BAN_Catalogo_Bloque oBloque { get; set; }
        public BAN_Catalogo_Fila oFila { get; set; }
        public BAN_Catalogo_Altura oAltura { get; set; }
        public BAN_Catalogo_Profundidad oProfundidad { get; set; }
        #endregion

        public BAN_Catalogo_Ubicacion()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Catalogo_Ubicacion> ConsultarLista(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Ubicacion>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Catalogo_Ubicacion_Consultar]", null, out OnError);
        }


        public static BAN_Catalogo_Ubicacion GetEntidad(int _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Ubicacion>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Catalogo_Ubicacion_Consultar]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_idBodega", this.idBodega);
            parametros.Add("i_idBloque", this.idBloque);
            parametros.Add("i_idFila", this.idFila);
            parametros.Add("i_idAltura", this.idAltura);
            parametros.Add("i_idProfundidad", this.idProfundidad);
            parametros.Add("i_barcode", this.barcode);
            parametros.Add("i_descripcion", this.descripcion);
            parametros.Add("i_capacidadBox", this.capacidadBox);
            parametros.Add("i_mt2", this.mt2);
            parametros.Add("i_disponible", this.disponible);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Catalogo_Ubicacion_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }
    }
}
