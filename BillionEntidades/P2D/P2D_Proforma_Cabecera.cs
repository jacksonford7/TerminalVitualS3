using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Proforma_Cabecera : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID_PROFORMA;
        private DateTime? _FECHA = null;
        private string _RUC_USUARIO = string.Empty;
        private string _NOMBRES = string.Empty;
        private string _APELLIDOS = string.Empty;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private int _CANT_BULTOS = 0;
        private Int64? _ID_CIUDAD;
        private Int64? _ID_ZONA;
        private Int64? _ID_TARIFA;
        private int? _ID_TARIFA_SECUEN;
        private string _DIR_ENTREGA = string.Empty;
        private decimal _TOTAL_M3 = 0;
        private decimal _TOTAL_TN = 0;
        private decimal _TOTAL_PAGAR = 0;
        private bool _ESTADO = false;
       
        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;

        private string _xmlCabecera;
        private string _xmlDetalle;

        private decimal _CANT_CALCULAR = 0;

        private decimal? _LATITUD;
        private decimal? _LONGITUD;
        private string _UBICACION = string.Empty;
        private bool _APILABLE = false;
        private bool _EXPRESS = false;

        #endregion

        #region "Propiedades"

        public Int64 ID_PROFORMA { get => _ID_PROFORMA; set => _ID_PROFORMA = value; }
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }
        public string RUC_USUARIO { get => _RUC_USUARIO; set => _RUC_USUARIO = value; }
        public string NOMBRES { get => _NOMBRES; set => _NOMBRES = value; }
        public string APELLIDOS { get => _APELLIDOS; set => _APELLIDOS = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public int CANT_BULTOS { get => _CANT_BULTOS; set => _CANT_BULTOS = value; }
        public Int64? ID_CIUDAD { get => _ID_CIUDAD; set => _ID_CIUDAD = value; }
        public Int64? ID_ZONA { get => _ID_ZONA; set => _ID_ZONA = value; }
        public Int64? ID_TARIFA { get => _ID_TARIFA; set => _ID_TARIFA = value; }
        public int? ID_TARIFA_SECUEN { get => _ID_TARIFA_SECUEN; set => _ID_TARIFA_SECUEN = value; }

        public string DIR_ENTREGA { get => _DIR_ENTREGA; set => _DIR_ENTREGA = value; }
     

        public decimal TOTAL_M3 { get => _TOTAL_M3; set => _TOTAL_M3 = value; }
        public decimal TOTAL_TN { get => _TOTAL_TN; set => _TOTAL_TN = value; }
        public decimal TOTAL_PAGAR { get => _TOTAL_PAGAR; set => _TOTAL_PAGAR = value; }

        public decimal CANT_CALCULAR { get => _CANT_CALCULAR; set => _CANT_CALCULAR = value; }

        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }


        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        
        private static String v_mensaje = string.Empty;

        public string xmlCabecera { get => _xmlCabecera; set => _xmlCabecera = value; }
        public string xmlDetalle { get => _xmlDetalle; set => _xmlDetalle = value; }

        public decimal? LATITUD { get => _LATITUD; set => _LATITUD = value; }
        public decimal? LONGITUD { get => _LONGITUD; set => _LONGITUD = value; }
        public string UBICACION { get => _UBICACION; set => _UBICACION = value; }
        public bool APILABLE { get => _APILABLE; set => _APILABLE = value; }
        public bool EXPRESS { get => _EXPRESS; set => _EXPRESS = value; }
        #endregion



        public List<P2D_Proforma_Detalle> Detalle { get; set; }
      

        public P2D_Proforma_Cabecera()
        {
            init();

            this.Detalle = new List<P2D_Proforma_Detalle>();
         
        }

        public P2D_Proforma_Cabecera(Int64 _ID_PROFORMA,
         DateTime? _FECHA ,
         string _RUC_USUARIO ,
         string _NOMBRES ,
         string _APELLIDOS,
         string _MRN ,
         string _MSN ,
         string _HSN ,
         int _CANT_BULTOS ,
         Int64? _ID_CIUDAD,
         Int64? _ID_ZONA,
         Int64? _ID_TARIFA,
         int? _ID_TARIFA_SECUEN,
         string _DIR_ENTREGA,                   
         decimal _TOTAL_M3,
         decimal _TOTAL_TN,
         decimal _TOTAL_PAGAR,
         decimal _CANT_CALCULAR,
         bool _ESTADO,
         string _USUARIO_CREA,
         string _USUARIO_MOD,
         bool _EXPRESS)

        {
            this.ID_PROFORMA = _ID_PROFORMA;
            this.FECHA = _FECHA;
            this.RUC_USUARIO = _RUC_USUARIO;
            this.NOMBRES = _NOMBRES;
            this.APELLIDOS = _APELLIDOS;
            this.MRN = _MRN;
            this.MSN = _MSN;
            this.HSN = _HSN;
            this.CANT_BULTOS = _CANT_BULTOS;
            this.ID_CIUDAD = _ID_CIUDAD;
            this.ID_ZONA = _ID_ZONA;
            this.ID_TARIFA = _ID_TARIFA;
            this.ID_TARIFA_SECUEN = _ID_TARIFA_SECUEN;
            this.TOTAL_M3 = _TOTAL_M3;
            this.TOTAL_TN = _TOTAL_TN;
            this.CANT_CALCULAR = _CANT_CALCULAR;
            this.ESTADO = _ESTADO;
            this.TOTAL_PAGAR = _TOTAL_PAGAR;
            this.USUARIO_CREA = _USUARIO_CREA;
            this.USUARIO_MOD = _USUARIO_MOD;
            this.EXPRESS = _EXPRESS;

            this.Detalle = new List<P2D_Proforma_Detalle>();
            
        }

        #region "Graba Proforma"
        private Int64? Save(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlDetalle", this.xmlDetalle);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "P2D_PROCESA_PROFORMA", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar proforma ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Graba Simulador"
        private Int64? Save_Simulador(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlDetalle", this.xmlDetalle);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "P2D_PROCESA_SIMULADOR", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Simulador(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Simulador(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar proforma ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Simulador), "SaveTransaction_Simulador", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        public bool Delete(out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID_PROFORMA", this.ID_PROFORMA);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "P2D_ANULA_PROFORMA", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

    }
}
