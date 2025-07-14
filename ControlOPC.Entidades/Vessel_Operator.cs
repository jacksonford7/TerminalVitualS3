using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlOPC.Entidades
{
    /// <summary>
    /// Representa cada cuadrillero de un turno
    /// </summary>
   public class Vessel_Operator :Base
    {
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }

        public Vessel_Operator()
        {
            base.init();
        }
        public Int64 id { get; set; } // identity
        public Int64 turn_id { get; set; } // id del turno
        public string opc_id { get; set; } // ruc de la opc
        public string opc_name { get; set; } // ruc de la opc
        public string operator_id { get; set; } //ci del operador
        public string operator_names { get; set; } //nombres del operador
        public string operator_apellidos { get; set; } //nombres del operador
        public int cuadrilla_number { get; set; } //numero generico
        public string description { get; set; }//nota x

        /// <summary>
        /// Retorna todos los operarios de un turno
        /// </summary>
        /// <param name="_turn_id"></param>
        /// <param name="OnError"></param>
        /// <returns></returns>
        public static List<Vessel_Operator> ListVesselOperators(long _turn_id, out string OnError)
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Vessel_Operator>(sql_pointer.basic_con, 2000, "PC_C_Cuadrilla", new Dictionary<string, object>() { { "turn_id", _turn_id } }, out OnError);
        }
        /// <summary>
        /// Guarda un operario del turno
        /// </summary>
        /// <param name="OnError"></param>
        /// <returns></returns>
        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("i_turn_id", turn_id); 
            parametros.Add("i_opc_id", opc_id);
            parametros.Add("i_opc_name", opc_name);
            parametros.Add("i_operator_id", operator_id); 
            parametros.Add("i_operator_names", operator_names);
            parametros.Add("i_operator_apellidos", operator_apellidos); 
            parametros.Add("i_cuadrilla_number", cuadrilla_number);
            parametros.Add("i_description",description ); 
            parametros.Add("i_create_user", Create_user); 
           var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_I_Cuadrilla", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            // PC_I_Cuadrilla
        }

        public Int64? Save(bool vlock, bool vunlock , out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("i_turn_id", turn_id);
            parametros.Add("i_opc_id", opc_id);
            parametros.Add("i_opc_name", opc_name);
            parametros.Add("i_operator_id", operator_id);
            parametros.Add("i_operator_names", operator_names);
            parametros.Add("i_operator_apellidos", operator_apellidos);
            parametros.Add("i_cuadrilla_number", cuadrilla_number);
            parametros.Add("i_description", description);
            parametros.Add("i_create_user", Create_user);
            //nuevo
            parametros.Add("i_vlock", vlock);
            parametros.Add("i_vunlock", vunlock);
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_I_Cuadrilla", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            // PC_I_Cuadrilla
        }
        private int? PreValidations(out string msg)
        {
            //1.->Validaciones de objetos.
            if (this.turn_id <= 0)
            {
                msg = "Especifique el id del turno";
                return 0;
            }

            if (string.IsNullOrEmpty(this.opc_id))
            {
                msg = "Especifique el ruc de la opc";
                return 0;
            }
            if (string.IsNullOrEmpty(this.operator_id))
            {
                msg = "Especifique la ci/pasaporte del operario";
                return 0;
            }
            if (string.IsNullOrEmpty(this.operator_names))
            {
                msg = "Especifique los nombres del operario";
                return 0;
            }

            if (string.IsNullOrEmpty(this.operator_apellidos))
            {
                msg = "Especifique los apellidos del operario";
                return 0;
            }
            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Especifique el usuario creador";
                return 0;
            }

            msg = string.Empty;
            return 1;
        }

        public static bool HasActiveTurns(string ci, DateTime inicio, DateTime fin, out string message)
        {

            OnInit();
            if (string.IsNullOrEmpty(ci))
            {
                message = string.Empty;
                return false;
            }
            
            //PC_V_Cuadrillero_Turno
            parametros.Clear();
            parametros.Add("i_inicio",inicio);
            parametros.Add("i_ci",ci);
            parametros.Add("i_fin",fin);
            var dm = sql_pointer.ExecuteSelectOnly(sql_pointer.basic_con, 4000, "PC_V_Cuadrillero_Turno", parametros);
            if (dm.code < 0)
            {
                message = dm.message;
                return true;
            }
            message = string.Empty;
            return false;
        }

        internal static Int64? RemoveFullCuadrilla(Int64 turno_id, string mod_user, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_turn_id", turno_id);
            parametros.Add("i_mod_user", mod_user);
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_D_Cuadrilla", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }



        public static List<Opc> get_local_opcs()
        {
            OnInit();
            string OnError;
            return sql_pointer.ExecuteSelectControl<Opc>(sql_pointer.basic_con, 4000, "PO_L_OPC", null, out OnError);
        }
    }
}
