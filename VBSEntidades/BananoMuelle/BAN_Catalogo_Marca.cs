using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_Catalogo_Marca : Cls_Bil_Base
    {
        #region "Propiedades"
        public int id { get; set; }
        public int idLinea { get; set; }
        public string nombre { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public  BAN_Catalogo_Linea Linea { get; set; }
        #endregion

        public BAN_Catalogo_Marca()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Catalogo_Marca> ConsultarListaMarca(string RucLinea, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_rucCliente", RucLinea);
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Marca>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Catalogo_Marca_Consultar]", parametros, out OnError);
        }

        public static BAN_Catalogo_Marca GetMarca(long _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Marca>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Catalogo_Marca_Consultar]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_idLinea", this.idLinea);
            parametros.Add("i_nombre", this.nombre);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_estado", this.estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Catalogo_Marca_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

    }
}
