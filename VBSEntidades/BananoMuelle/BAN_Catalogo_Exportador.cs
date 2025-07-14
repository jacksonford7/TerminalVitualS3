using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_Catalogo_Exportador : Cls_Bil_Base
    {
        #region "Propiedades"
        public int? id { get; set; }
        public int? idLinea { get; set; }
        public string ruc { get; set; }
        public string nombre { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public BAN_Catalogo_Linea Linea { get; set; }
        #endregion

        public BAN_Catalogo_Exportador()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }
        public static List<BAN_Catalogo_Exportador> ConsultarListaExportador(string RucLinea, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_rucCliente", RucLinea);
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Exportador>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Catalogo_Exportador_Consultar]", parametros, out OnError);
        }

        public static BAN_Catalogo_Exportador GetExportaador(int _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Exportador>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Catalogo_Exportador_Consultar]", parametros);
            return obj;
        }

        public static BAN_Catalogo_Exportador GetExportaadorPorRucLinea(int _idLinea, string ruc)
        {
            parametros.Clear();
            parametros.Add("i_idLinea", _idLinea);
            parametros.Add("i_ruc", ruc);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Exportador>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Catalogo_Exportador_ConsultarXRucLinea]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_idLinea", this.idLinea);
            parametros.Add("i_ruc", this.ruc);
            parametros.Add("i_nombre", this.nombre);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_estado", this.estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Catalogo_Exportador_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
    }
}
