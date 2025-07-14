using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_AISV_ValidaBooking : Cls_Bil_Base
    {
        public string booking { get; set; }
        public string line { get; set; }
        public int aisv_cant_bult { get; set; }
        public Int64 dispone { get; set; }
        public Int64 reserva { get; set; }
        public string bnumber { get; set; }



        public Cls_AISV_ValidaBooking()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public bool PopulateMyData_DisponibilidadBooking(out string OnError)
        {
            OnInit("N5");
            parametros.Clear();
            parametros.Add("booking", this.booking);
            parametros.Add("line", this.line);

            var t = sql_puntero.ExecuteSelectOnly<Cls_AISV_ValidaBooking>(nueva_conexion, 6000, "sp_booking_disponibilidad", parametros);
            if (t == null)
            {
                OnError = "No existe información del booking para validar..";
                return false;
            }


            this.bnumber = t.bnumber;
            this.dispone = t.dispone;
            this.reserva = t.reserva;

            OnError = string.Empty;
            return true;
        }

        public bool PopulateMyData_Aisv(out string OnError)
        {
            OnInit("SERVICE");
            parametros.Clear();
            parametros.Add("booking", this.booking);
            parametros.Add("line", this.line);

            var t = sql_puntero.ExecuteSelectOnly<Cls_AISV_ValidaBooking>(nueva_conexion, 6000, "sp_consulta_booking_aisv", parametros);
            if (t == null)
            {
                OnError = "No existe información del booking para validar..";
                return false;
            }


            this.aisv_cant_bult = t.aisv_cant_bult;


            OnError = string.Empty;
            return true;
        }



    }
}
