using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlOPC.Entidades
{
    public class OPCGrupo :Base
    {
        public Int64 id { get; set; }
        public string grupo_id { get; set; }
        public string opc_id { get; set; }
        public string grupo_name { get; set; }
        public List<OPCOperador> Operadores { get; set; }
        public OPCGrupo(string _opc)
        {
            this.opc_id = _opc;
            this.Operadores = new List<OPCOperador>();
            OnInit();
        }
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }

        public OPCGrupo(Int64 _id)
        {
            this.id = _id;
            OnInit();
            this.Operadores = new List<OPCOperador>();
        }
        public OPCGrupo()
        {
            OnInit();
            this.Operadores = new List<OPCOperador>();
        }
        /// <summary>
        /// Cragar mis operadores
        /// </summary>
        public void PopulateMyOperators()
        {
            OnInit();
            string OnError;
            this.Operadores = sql_pointer.ExecuteSelectControl<OPCOperador>(sql_pointer.basic_con, 2000, "PC_C_GrupoOperador", new Dictionary<string, object>() { { "i_idgrupo", this.id } }, out OnError);
        }
        private int? PreValidationsTransaction(out string msg)
        {
            //validaciones de cabecera.
            if (string.IsNullOrEmpty(this.grupo_name))
            {
                msg = "Debe especificar el nombre del grupo (ejemplo: A, 1 , A1)";
                return 0;

            }
            //todo->aqui debe revisar que el numero no haya sido tomado, la secuencia

            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Debe especificar el usuario que crea este grupo (crea_user)";
                return 0;
            }


            if (string.IsNullOrEmpty(this.opc_id))
            {
                msg = "Debe especificar el numero de ruc de la OPC";
                return 0;
            }

            //cuenta solo los activos
            var real_operadors = this.Operadores.Where(d => d.active.HasValue && d.active.Value).Count();

            if (this.Operadores == null && real_operadors  <= 0)
            {
                msg = "No se puede agregar operadores ya que la lista es nula o vacía";
                return 0;
            }

            var c = app_configurations.get_configuration("CUADRILLA");
            int t = 0;
            if (c == null || !int.TryParse(c.value, out t))
            {
                t = 2;
            }
            if (real_operadors < t)
            {
                msg = string.Format("La cuadrilla debe tener al menos {0} personas", t);
                return 0;
            }
            msg = string.Empty;
            return 1;
        }
        public Int64? SaveTransaction(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }
                    this.id = id.Value;
                    //si no falla la cabecera entonces añada los items
                    foreach (var i in this.Operadores)
                    {
                        i.Create_user = this.Create_user;
                        i.id_grupo = id.Value;
                        var il = i.Save(out OnError);
                        if (!il.HasValue || il.Value <= 0)
                        {
                            return 0;
                        }
                        i.id = il.Value;
                    }
                    //fin de la transaccion
                    scope.Complete();
                    OnError = string.Empty;
                    return this.id;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(OPCGrupo), nameof(SaveTransaction), result_, this.id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }
        }
        private Int64? Save(out string OnError)
        {
            bool u = false;
            parametros.Clear();
            parametros.Add("i_grupo_name", this.grupo_name); //numero del grupo
            parametros.Add("i_opc_id", this.opc_id); // ruc
            parametros.Add("i_create_user", Create_user); // quien crea
            if (this.id > 0)
            {
                parametros.Add("i_id", this.id); // id unico
                u = true;
            }
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_M_Grupo", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)  {return null; }
            OnError = string.Empty;
            this.id = u? this.id: db.Value;
            return this.id;
        }
        public string next_value(string opc_id)
        {
            //EscalarFunction
            parametros.Clear();
            parametros.Add("i_opc_id", opc_id);
            string er;
            var db = sql_pointer.EscalarFunction(sql_pointer.basic_con, 4000, "SELECT [dbo].[fx_grupo_siguiente](@i_opc_id)", parametros, out er);
            return string.Format("G{0}", db);
        }
        public static List<OPCGrupo> ListGrupos(string opc_id, out string OnError)
        {
            OnInit();
            parametros.Clear();
            if (!string.IsNullOrEmpty(opc_id))
            {
                parametros.Add("i_opc_id",opc_id);
            }
            return sql_pointer.ExecuteSelectControl<OPCGrupo>(sql_pointer.basic_con, 2000, "PC_C_Grupo_Lista", parametros, out OnError);
        }
        public bool PopulateMyData(out string OnError)
        {
            //cargar todos los datos este documento
            if (this.id <= 0 )
            {
                OnError = "Debe establecer el campo ID del grupo";
                return false;
            }
            parametros.Clear();
             parametros.Add("i_idgrupo", this.id);
  
            var t = sql_pointer.ExecuteSelectOnly<OPCGrupo>(sql_pointer.basic_con, 4000, "PC_C_Grupo", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos del grupo";
                return false;
            }
            this.grupo_id = t.grupo_id;
            this.grupo_name = t.grupo_name;
            this.Create_date = t.Create_date;
            this.Create_user = t.Create_user;
            this.Mod_data = t.Mod_data;
            this.Mod_user = t.Mod_user;
            this.Mod_date = t.Mod_date;
            this.opc_id = t.opc_id;
             
            OnError = string.Empty;
            return true;
        }
        public bool Delete()
        {
            parametros.Clear();
            string OnError;
            parametros.Add("i_idgrupo", this.id);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_D_Grupo", parametros, out OnError);
                if (!db.HasValue || db.Value < 0) { return false; }
                var b = OPCOperador.Delete(this.id);

                if (!b)
                {
                    return false;
                }
                 scope.Complete();
            }
            return true;
        }
    }
}
