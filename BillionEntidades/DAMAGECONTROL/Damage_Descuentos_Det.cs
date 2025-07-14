using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Damage_Descuentos_Det : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _DESC_ID;
        private int _DESC_FILA;
        private Int64 _LIN_ID;
        private string _DESC_LIN_DESCRIP;     
        private decimal _DESC_PORCENTAJE;
      
        #endregion

        #region "Propiedades"
        public Int64 DESC_ID { get => _DESC_ID; set => _DESC_ID = value; }
        public int DESC_FILA { get => _DESC_FILA; set => _DESC_FILA = value; }
        public Int64 LIN_ID { get => _LIN_ID; set => _LIN_ID = value; }
        public string DESC_LIN_DESCRIP { get => _DESC_LIN_DESCRIP; set => _DESC_LIN_DESCRIP = value; }
        public decimal DESC_PORCENTAJE { get => _DESC_PORCENTAJE; set => _DESC_PORCENTAJE = value; }
   
        #endregion

        public Damage_Descuentos_Det()
        {
            init();
        }


        public Damage_Descuentos_Det(Int64 _DESC_ID, int _DESC_FILA, Int64 _LIN_ID,
         string _DESC_LIN_DESCRIP, decimal _DESC_PORCENTAJE )
        {
            this.DESC_ID = _DESC_ID;
            this.DESC_FILA = _DESC_FILA;
            this.LIN_ID = _LIN_ID;
            this.DESC_LIN_DESCRIP = _DESC_LIN_DESCRIP;
            this.DESC_PORCENTAJE = _DESC_PORCENTAJE;

          

        }



        private int? PreValidations(out string msg)
        {

            if (this.LIN_ID <= 0)
            {
                msg = "Especifique el id de la línea naviera";
                return 0;
            }

          

            msg = string.Empty;
            return 1;
        }

        public Int64? Save(out string OnError)
        {

            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

           // OnInit();

            parametros.Clear();
            parametros.Add("DESC_ID", this.DESC_ID);
            parametros.Add("DESC_FILA", this.DESC_FILA);
            parametros.Add("LIN_ID", this.LIN_ID);
            parametros.Add("DESC_LIN_DESCRIP", this.DESC_LIN_DESCRIP);
            parametros.Add("DESC_PORCENTAJE", this.DESC_PORCENTAJE);
        

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "DAMAGE_DESCUENTO_DET", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        #region "Detalle de politicas de descuentos"
        public static List<Damage_Descuentos_Det> Detalle_Descuentos(Int64 DESC_ID, out string OnError)
        {

            parametros.Clear();
            parametros.Add("DESC_ID", DESC_ID);
            return sql_puntero.ExecuteSelectControl<Damage_Descuentos_Det>(sql_puntero.Conexion_Local, 5000, "DAMAGE_CARGAR_DETALLE_DESCUENTO", parametros, out OnError);

        }



        #endregion

    }
}
