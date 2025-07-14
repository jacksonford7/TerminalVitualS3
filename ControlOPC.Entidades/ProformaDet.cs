using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlOPC.Entidades
{
    public class ProformaDet : Base
    {
        private Int64 mProforma_id = 0;
        private int mLine = 0;
        private Int64 mVessel_crane_id = 0;
        private Int64 mVessel_crane_gkey = 0;
        private int mTurn_id = 0;
        private int mConcepto_id = 0;
        private string mConcepto_name = "";
        private DateTime? mTurn_time_start ;
        private DateTime? mTurn_time_end ;
        private decimal total_horas = 0;
        private decimal precio_hora = 0;
        private decimal total = 0;
        private bool mValida = false; 

        #region "Propiedades"

        public Int64 Proforma_id
        {
            get
            {
                return mProforma_id;
            }
            set
            {
                mProforma_id = value;
            }
        }

        public int Line
        {
            get
            {
                return mLine;
            }
            set
            {
                mLine = value;
            }
        }

        public Int64 Vessel_crane_id
        {
            get
            {
                return mVessel_crane_id;
            }
            set
            {
                mVessel_crane_id = value;
            }
        }

        public Int64 Vessel_crane_gkey
        {
            get
            {
                return mVessel_crane_gkey;
            }
            set
            {
                mVessel_crane_gkey = value;
            }
        }

        public int Turn_id
        {
            get
            {
                return mTurn_id;
            }
            set
            {
                mTurn_id = value;
            }
        }

        public int Concepto_id
        {
            get
            {
                return mConcepto_id;
            }
            set
            {
                mConcepto_id = value;
            }
        }

        public string Concepto_name
        {
            get
            {
                return mConcepto_name;
            }
            set
            {
                mConcepto_name = value;
            }
        }

        public DateTime? Turn_time_start { get => mTurn_time_start; set => mTurn_time_start = value; }
        public DateTime? Turn_time_end { get => mTurn_time_end; set => mTurn_time_end = value; }
        public decimal Total_horas { get => total_horas; set => total_horas = value; }
        public decimal Precio_hora { get => precio_hora; set => precio_hora = value; }
        public decimal Total { get => total; set => total = value; }


        public bool Valida
        {
            get
            {
                return mValida;
            }
            set
            {
                mValida = value;
            }
        }

        #endregion

        public ProformaDet()
        {
            base.init();
        }

        public static List<ProformaDet> ListProformasDet(Int64 _proforma_id, out string OnError)
        {
            return sql_pointer.ExecuteSelectControl<ProformaDet>(sql_pointer.basic_con, 2000, "PC_C_ProformaDet", new Dictionary<string, object>() { { "i_proforma_id", _proforma_id } }, out OnError);
        }

        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("i_proforma_id", Proforma_id);
            parametros.Add("i_line", Line);
            parametros.Add("i_vessel_crane_id", Vessel_crane_id);
            parametros.Add("i_vessel_crane_gkey", Vessel_crane_gkey);
            parametros.Add("i_turn_id", Turn_id);
            parametros.Add("i_concepto_id", Concepto_id);
            parametros.Add("i_concepto_name", Concepto_name);
            parametros.Add("i_turn_time_start", Turn_time_start);
            parametros.Add("i_turn_time_end", Turn_time_end);
            parametros.Add("i_total_horas", Total_horas);
            parametros.Add("i_precio_hora", Precio_hora);
            parametros.Add("i_total", Total);
            parametros.Add("i_create_user", Create_user);
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_I_ProformaDet", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            // PC_I_Turn
        }

        private int? PreValidations(out string msg)
        {
            //1.->Validaciones de objetos.
            if (this.Proforma_id <= 0)
            {
                msg = "Especifique el id de la proforma";
                return 0;
            }

            if (this.Valida)
            {
                if (this.Vessel_crane_id <= 0)
                {
                    msg = "Especifique el id de la grúa";
                    return 0;
                }

                if (this.Turn_id <= 0)
                {
                    msg = "Especifique el id del turno";
                    return 0;
                }
            }
           

            if (this.Concepto_id<= 0)
            {
                msg = "Especifique el id del concepto";
                return 0;
            }




            msg = string.Empty;
            return 1;
        }
    }
}
