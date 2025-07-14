using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite
{
    public class EDI_bookingDet : BillionEntidades.Cls_Bil_Base
    {
        public long? id { get; set; }
        public long? idBooking { get; set; }
        public int? qty { get; set; }
        public string equipmentType { get; set; }
        public string length { get; set; }
        public string height { get; set; }
        public string ISOgroup { get; set; }
        public string grade { get; set; }
        public string material { get; set; }
        public string commodity { get; set; }
        public string commodityDesc { get; set; }
        public string grossWeightKg { get; set; }
        public string tempRequired { get; set; }
        public string ventilationRequired { get; set; }
        public string ventilationUnit { get; set; }
        public string CO2Required { get; set; }
        public string O2Required { get; set; }
        public string humidityRequired { get; set; }
        public string overLongBack { get; set; }
        public string overLongFront { get; set; }
        public string overWideLeft { get; set; }
        public string overWideRight { get; set; }
        public string overHeight { get; set; }
        public bool? isOOG { get; set; }
        public string remarks { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }

        #region "Constructores"
        public EDI_bookingDet()
        {
            base.init();
        }
        #endregion

        #region "Metodos"
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        private int? PreValidations(out string msg)
        {

            if (this.qty <= 0)
            {
                msg = "Especifique el qty";
                return 0;
            }

            if (string.IsNullOrEmpty(this.height))
            {
                msg = "Especifique el height";
                return 0;
            }

            if (string.IsNullOrEmpty(this.length))
            {
                msg = "Especifique el length";
                return 0;
            }

            if (string.IsNullOrEmpty(this.ISOgroup))
            {
                msg = "Especifique el ISOgroup";
                return 0;
            }

            msg = string.Empty;
            return 1;
        }

        public static List<EDI_bookingDet> ListBookingDet(long _id)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_idbooking", _id);
            return sql_puntero.ExecuteSelectControl<EDI_bookingDet>(nueva_conexion, 2000, "EDI_BookingDet_consultar", parametros, out msg);
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            OnInit("N4Middleware");
            if (this.id > 0)
            {
                parametros.Add("i_id", this.id);
            }
            parametros.Add("i_idBooking", this.idBooking);                             
            parametros.Add("i_qty", this.qty);                                      
            parametros.Add("i_equipmentType", this.equipmentType);                                 
            parametros.Add("i_length", this.length);                                       
            parametros.Add("i_height", this.height);                                       
            parametros.Add("i_ISOgroup", this.ISOgroup);                                     
            parametros.Add("i_grade", this.grade);                                        
            parametros.Add("i_material", this.material);                         
            parametros.Add("i_commodity", this.commodity);                          
            parametros.Add("i_commodityDesc", this.commodityDesc);                         
            parametros.Add("i_grossWeightKg", this.grossWeightKg);                   
            parametros.Add("i_tempRequired", this.tempRequired);                          
            parametros.Add("i_ventilationRequired", this.ventilationRequired);                   
            parametros.Add("i_ventilationUnit", this.ventilationUnit);                      
            parametros.Add("i_CO2Required", this.CO2Required);                               
            parametros.Add("i_O2Required", this.O2Required);                           
            parametros.Add("i_humidityRequired", this.humidityRequired);                        
            parametros.Add("i_overLongBack", this.overLongBack);                               
            parametros.Add("i_overLongFront", this.overLongFront);                           
            parametros.Add("i_overWideLeft", this.overWideLeft);                         
            parametros.Add("i_overWideRight", this.overWideRight);                          
            parametros.Add("i_overHeight", this.overHeight);                           
            parametros.Add("i_isOOG", this.isOOG);                                   
            parametros.Add("i_remarks", this.remarks); 
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "EDI_bookingDet_insert", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? Save_Anula(out string OnError)
        {
            parametros.Clear();
            //if (this.id > 0)
            //{
            OnInit("N4Middleware");
            //}

            parametros.Add("i_id", this.id);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "EDI_bookingDet_estado", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
        #endregion
    }
}