using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    public class Crane_Turn : Base
    {
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }
        public Int64 id { get; set; } //identity-->No grabar ni validar
        public Int64 vessel_visit_id { get; set; } //la secuencia del documento
        public Int64 vessel_crane_id { get; set; } //la grua de base de datos
        public int turno_number { get; set; } //secuencia interna
        public int task_id { get; set; } //numero de tarea de tareas
        public Int64 crane_id { get; set; }
        public string crane_name { get; set; }
        public DateTime? turn_time_start { get; set; } //n4
        public DateTime? turn_time_end { get; set; } //n4
        public DateTime? turn_time_meet { get; set; }//manual
        public string opc_id { get; set; }
        public string opc_name { get; set; }
        public bool vlock { get; set; }
        public bool vunlock { get; set; }
        public Int64 grupo_id { get; set; }
        public string grupo_name { get; set; }
        public string vlock_text { get; set; }
        public string vunlock_text { get; set; }

        public List<Vessel_Operator> turn_operators { get; set; } //cuadrilleros grabados
        //indica el estado del turno
        public string status { get; set; }
        public string t_status { get; set; }
        //PC_I_Turn
        public Crane_Turn()
        {
            base.init();
            this.turn_operators = new List<Vessel_Operator>();
        }
        public static List<Crane_Turn> ListCraneTurn(long _vessel_visit_id, out string OnError)
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Crane_Turn>(sql_pointer.basic_con, 2000, "PC_C_Turn", new Dictionary<string, object>() { { "i_vessel_visit_id", _vessel_visit_id } }, out OnError);
        }

        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            if (this.Validaturno(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("i_vessel_crane_id", vessel_crane_id); //es el identity FK de VesselCrane
            parametros.Add("N4_crane_name", crane_name); //es le nombre de la grua N4 (QC1,QC2)
            parametros.Add("N4_crane_id", crane_id); //es le gkey de la grua N4
            parametros.Add("i_vessel_visit_id", vessel_visit_id);//es la secuencia de la cabecera
            parametros.Add("i_turn_time_start", turn_time_start); //inicia
            parametros.Add("i_turn_time_end", turn_time_end); //termina
            parametros.Add("i_turn_time_meet", turn_time_meet); // es una hora sugerida opcional
            parametros.Add("i_turno_number", turno_number); //secuencia de numero del turno
            parametros.Add("i_task_id", task_id); //opcional tarea que va a hacer este turno
            parametros.Add("i_create_user", Create_user); //quien lo crea
            parametros.Add("i_grupo_id", grupo_id); //grupo
            parametros.Add("i_opc_id", opc_id); //
            parametros.Add("i_opc_name", opc_name); //
            parametros.Add("i_vlock", vlock); //
            parametros.Add("i_vunlock", vunlock); //
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_I_Turn", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            // PC_I_Turn
        }

        public Int64? Validaturno(out string OnError)
        {

      
            parametros.Clear();
            parametros.Add("i_vessel_visit_id", vessel_visit_id);
            parametros.Add("i_vessel_crane_id", vessel_crane_id);
            parametros.Add("i_grupo_id", grupo_id);
            parametros.Add("i_turn_time_start", turn_time_start); //inicia
            parametros.Add("i_turn_time_end", turn_time_end); //termina
            parametros.Add("i_vlock", vlock); //
            parametros.Add("i_vunlock", vunlock); //
            var db = sql_pointer.ExecuteSelectOnly(sql_pointer.basic_con, 4000, "PC_C_ValidaturnoOPC", parametros);
            if (db == null)
            {
                //null error en base de datos, el mensaje ya fue en onError
                OnError = "Error al validar turno (sin mensaje)";
                return 0;
            }
            else
            {
                if (db.code == 1)
                {
                    OnError = string.Empty;
                }
                else
                {
                    OnError = db.message;
                    return 0;
                }

            }

            //      OnError = string.Empty;
            return 1;

        }

        private int? PreValidations(out string msg)
        {
            //1.->Validaciones de objetos.
            if (this.vessel_crane_id <= 0)
            {
                msg = "Especifique el id de la grua";
                return 0;
            }

            if (string.IsNullOrEmpty(this.crane_name))
            {
                msg = "Especifique el nombre de la grua";
                return 0;
            }

            if (this.vessel_visit_id <= 0)
            {
                msg = "Especifique el id de la visita";
                return 0;
            }
            if (turno_number < 1)
            {
                turno_number = -1;
            }
            if (!this.turn_time_meet.HasValue)
            {
                msg = "Especifique la fecha de inicio de trabajo de la grúa para cada turno (meet)";
                return 0;
            }
            if (task_id == 0)
            {
                task_id = 1;
            }
            if (this.turn_time_start.HasValue && this.turn_time_end.HasValue)
            {
                if (this.turn_time_end < this.turn_time_start)
                {
                    msg = "La fecha de inicio de turno no puede ser mayor a la fecha final de turno ";
                    return 0;
                }
            }
            msg = string.Empty;
            return 1;
        }


        public bool? EliminarTurnoOPC(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;

            try
            {
                //recuperar toda la info del turno ir a la base y ejecutar los comandos.
                if (this.id <= 0)
                {
                    OnError = "no se puede anular turno, falta el id de la transacción";
                    return result_;
                }
                if (string.IsNullOrEmpty(this.Mod_user))
                {
                    OnError = "no se puede anular turno, falta el usuario de la transacción";
                    return result_;
                }

                //PC_D_Turn_Cuadrilla   
                parametros.Clear();
                parametros.Add("i_id", id);
                parametros.Add("i_mod_user", this.Mod_user);
                string Oe = string.Empty;
                var e = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_D_Turn_OPC", parametros, out Oe);
                if (e.HasValue && e.Value > 0)
                {
                    OnError = string.Empty;
                    return true;
                }
                OnError = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(Crane_Turn), nameof(LiberarTurnoCuadrilla), result_, this.id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }

        }



        /// <summary>
        /// Obtiene los oepradores de un turno ya grabado
        /// </summary>
        public void PopulateMyVesselOperators()
        {
            OnInit();
            string OnError;
            this.turn_operators = sql_pointer.ExecuteSelectControl<Vessel_Operator>(sql_pointer.basic_con, 2000, "PO_C_Cuadrilla", new Dictionary<string, object>() { { "turn_id", this.id } }, out OnError);
        }
        //por cada fila de gruas le pasas estos datos y te genera turnos
        public static List<Crane_Turn> GenerateTurns(Int64 _gruaId, DateTime _start, int _qtWork)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("crane_id", _gruaId);
            parametros.Add("desde", _start);
            parametros.Add("worqt", _qtWork);
            return sql_pointer.ExecuteSelectControl<Crane_Turn>(sql_pointer.basic_con, 2000, "PC_G_Turn", parametros, out msg);
        }

        public static List<Crane_Turn> ListaTurnoOC(Int64 _i_vessel_visit_id, Int64 _crane_id, string _opc_id)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_vessel_visit_id", _i_vessel_visit_id);
            parametros.Add("i_crane_id", _crane_id);
            parametros.Add("i_opc_id", _opc_id);
            return sql_pointer.ExecuteSelectControl<Crane_Turn>(sql_pointer.basic_con, 2000, "PC_C_Turn_OPC", parametros, out msg);
        }

        public Vessel_Visit getVesselVisit()
        {
            parametros.Clear();
            parametros.Add("i_turno", this.id);
            return sql_pointer.ExecuteSelectOnly<Vessel_Visit>(sql_pointer.basic_con, 2000, "PC_C_Turn_Vessel_Visit", parametros);

        }
        public bool? SaveMyVesselOperators(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return result_;
                }

                using (var scope = new System.Transactions.TransactionScope())
                {
                    foreach (var i in this.turn_operators)
                    {
                        var il= i.Save(out OnError);
                        if (!il.HasValue || il.Value <= 0)
                        {
                            return false;
                        }
                        this.id = il.Value;
                    } //bucle de las gruas
                    //fin de la transaccion
                    scope.Complete();
                    OnError = string.Empty;
                    return true;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(Crane_Turn), nameof(SaveMyVesselOperators), result_, this.id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }


        }
        public bool? SaveMyVesselOperators(out string OnError, bool vlock, bool vunlock)
        {
            bool result_ = false;
            string resultado_others = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return result_;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    foreach (var i in this.turn_operators)
                    {
                        var il = i.Save(out OnError);
                        if (!il.HasValue || il.Value <= 0)
                        {
                            return false;
                        }
                        i.id = il.Value;
                    } //bucle de las gruas
                    //fin de la transaccion
                    //actualiza el turno
                    update_lock_unlock(out OnError, vlock, vunlock);
                    scope.Complete();
                    OnError = string.Empty;
                    return true;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(Crane_Turn), nameof(SaveMyVesselOperators), result_, this.id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }


        }
        private int? PreValidationsTransaction(out string msg)
        {
            if (this.turn_operators == null && this.turn_operators.Count <= 0)
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
            if (this.turn_operators.Count < t)
            {
                msg = string.Format("La cuadrilla debe tener al menos {0} personas",t);
                return 0;
            }
            msg = string.Empty;
            return 1;
        }

        public int? LiberarTurno()
        {
            //recuperar toda la info del turno ir a la base y ejecutar los comandos.
            if (this.id <= 0)
            {
                return 0;
            }
            if (string.IsNullOrEmpty(this.Mod_user))
            {
                return null;
            }

            //PC_D_Turn_Cuadrilla   
            parametros.Clear();
            parametros.Add("i_id", id);
            parametros.Add("i_mod_user",this.Mod_user);
            string Oe = string.Empty;
            var e= sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con,4000, "PC_D_Turn_Cuadrilla", parametros, out Oe);
            if (e.HasValue && e.Value > 0)
            {
                return 1;
            }
            return null;

        }
        public Int64? RemoveMyVesselOperators(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;
            try
            {
                //UPDATE MASIVO A CRUDRILLA
                //PC_D_Cuadrilla
               return  Vessel_Operator.RemoveFullCuadrilla(this.id, this.Mod_user, out OnError);
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(Crane_Turn), nameof(RemoveMyVesselOperators), result_, this.id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }


        }
        private Int64? update_lock_unlock(out string OnError, bool vlock, bool vunlock)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id); 
            parametros.Add("i_vlock", vlock); 
            parametros.Add("i_vunlock", vunlock); 
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_U_Turn_Lock_Unlock", parametros, out OnError);
            if (!db.HasValue || db.Value < 0){ return null;  }
            OnError = string.Empty;
            return db.Value;
        }
        public bool IsSetted()
        {
            //ir y revisar si el estado del turno ha cambiado sino ya fue tomado
           string er;
            parametros.Clear();
            parametros.Add("i_turn_id", this.id);
            var db = sql_pointer.EscalarFunction(sql_pointer.basic_con, 4000, "select [dbo].[fx_turn_locked](@i_turn_id)", parametros, out er);
            var r= db as bool?;
            if(!r.HasValue)
            {
                return false;
            }
            return r.Value;
        }
        public static List<Crane_Turn> LockUnlockTurns(Int64 turn_id)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_turn_id", turn_id);
            return sql_pointer.ExecuteSelectControl<Crane_Turn>(sql_pointer.basic_con, 2000, "PC_C_Turn_Amarre", parametros, out msg);
        }
        public static List<Crane_Turn> LockUnlockTurnsID(Int64 vv_id, Int64 turn_id)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("referencia_id", vv_id);
            parametros.Add("turn_id", turn_id);
            return sql_pointer.ExecuteSelectControl<Crane_Turn>(sql_pointer.basic_con, 2000, "PC_C_Turn_Amarre_ID", parametros, out msg);
        }

        #region "Cancela turnos"

        public static List<Crane_Turn> GetListTurnReference(string _reference, out string OnError)
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Crane_Turn>(sql_pointer.basic_con, 2000, "PC_C_Turn_Resumen", new Dictionary<string, object>() { { "reference", _reference } }, out OnError);
        }

        public static List<Crane_Turn> GetListturnReferenceCombo(string _reference, out string OnError)
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Crane_Turn>(sql_pointer.basic_con, 2000, "PC_C_Turn_Resumen_Combo", new Dictionary<string, object>() { { "reference", _reference } }, out OnError);
        }

        public static List<Crane_Turn> GetTurn(Int64 _Id, out string OnError)
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Crane_Turn>(sql_pointer.basic_con, 2000, "PC_C_Turn_Id", new Dictionary<string, object>() { { "Id", _Id } }, out OnError);
        }

        public bool? LiberarTurnoCuadrilla(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;

            try
            {
                //recuperar toda la info del turno ir a la base y ejecutar los comandos.
                if (this.id <= 0)
                {
                    OnError = "no se puede anular turno, falta el id de la transacción";
                    return result_;
                }
                if (string.IsNullOrEmpty(this.Mod_user))
                {
                    OnError = "no se puede anular turno, falta el usuario de la transacción";
                    return result_;
                }

                if (!this.turn_time_meet.HasValue)
                {
                    OnError = "Especifique la fecha de feinalización del trabajo del turno";
                    return result_;
                }
               
                if (this.turn_time_meet.HasValue && this.turn_time_end.HasValue)
                {
                    if (this.turn_time_meet > this.turn_time_end)
                    {
                        OnError = "La fecha ingresada de finalización del turno no puede ser mayor que la fecha actual..";
                        return result_;
                    }
                }

                //PC_D_Turn_Cuadrilla   
                parametros.Clear();
                parametros.Add("i_id", id);
                parametros.Add("i_mod_user", this.Mod_user);
                parametros.Add("i_turn_time_meet", this.turn_time_meet);
                string Oe = string.Empty;
                var e = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_D_Turn_Cuadrilla_Desactiva", parametros, out Oe);
                if (e.HasValue && e.Value > 0)
                {
                    OnError = string.Empty;
                    return true;
                }
                OnError = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(Crane_Turn), nameof(LiberarTurnoCuadrilla), result_, this.id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }

        }


        #endregion
    }
}
