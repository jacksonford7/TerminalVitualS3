using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClsProformas;
using ControlOPC.Entidades;

namespace ClsProformas
{
    public class Archivos_Zal : Base
    {

        #region "Variables"

        private string _ruc = string.Empty;
        private Int64 _id_ppzal = 0;
        private string _booking = string.Empty;
        private string _referencia = string.Empty;
        private string _fecha_salida = string.Empty;
        private string _turno = string.Empty;
        private string _placa = string.Empty;
        private string _chofer = string.Empty;
        private string _liquidacion = string.Empty;
        private string _estado_pago = string.Empty;
        private string _xmlDocumentos = string.Empty;
        private string _ruta_documento = string.Empty;
        private string _usr_ing_archivo = string.Empty;
        private string _contenedor = string.Empty;
        private int _fila = 0;
       

        #endregion

        #region "Propiedades"

        public Int64 id_ppzal { get => _id_ppzal; set => _id_ppzal = value; }
        public string booking { get => _booking; set => _booking = value; }
        public string referencia { get => _referencia; set => _referencia = value; }
        public string fecha_salida { get => _fecha_salida; set => _fecha_salida = value; }
        public string turno { get => _turno; set => _turno = value; }
        public string placa { get => _placa; set => _placa = value; }
        public string chofer { get => _chofer; set => _chofer = value; }
        public string liquidacion { get => _liquidacion; set => _liquidacion = value; }
        public string estado_pago { get => _estado_pago; set => _estado_pago = value; }
        public string xmlDocumentos { get => _xmlDocumentos; set => _xmlDocumentos = value; }
        public string ruta_documento { get => _ruta_documento; set => _ruta_documento = value; }

        public string usr_ing_archivo { get => _usr_ing_archivo; set => _usr_ing_archivo = value; }
        public string contenedor { get => _contenedor; set => _contenedor = value; }
        public int fila { get => _fila; set => _fila = value; }


        #endregion

        private static void OnInit_N4()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        public Archivos_Zal()
        {
            OnInit_N4();
            
        }

        public Archivos_Zal( Int64 _id_ppzal,string _booking,string _referencia,
         string _fecha_salida,
         string _turno,
         string _placa ,
         string _chofer,
         string _liquidacion,
         string _estado_pago,
         string _xmlDocumentos,
         string _ruta_documento,
         string _usr_ing_archivo)
        {

            this.id_ppzal = _id_ppzal;
            this.booking = _booking;
            this.referencia = _referencia;
            this.fecha_salida = _fecha_salida;
            this.turno = _turno;
            this.placa = _placa;
            this.chofer = _chofer;
            this.liquidacion = _liquidacion;
            this.estado_pago = _estado_pago;
            this.xmlDocumentos = _xmlDocumentos;
            this.ruta_documento = _ruta_documento;
            this.usr_ing_archivo = _usr_ing_archivo;
 
        OnInit_N4();
            
        }

        /*listado de pases a colocar archivo*/
        public static List<Archivos_Zal>Detalle_Pases_Subir_Archivo(Int64 _pid_ppzal, DateTime _Desde, DateTime _Hasta, string _Contenedor, out string OnError)
        {
            OnInit_N4();
            parametros.Clear();
            parametros.Add("ID_PPZAL", _pid_ppzal);
            parametros.Add("Desde", _Desde);
            parametros.Add("Hasta", _Hasta);
            parametros.Add("contenedor", _Contenedor);
            return sql_pointer.ExecuteSelectControl<Archivos_Zal>(v_conexion, 4000, "SP_GET_ID_PASE_ZAL_ARCHIVO", parametros, out OnError);
        }

        /*listado de pases a visualizar archivo*/
        public static List<Archivos_Zal> Detalle_Pases_Visualizar_Archivo(Int64 _pid_ppzal, DateTime _Desde, DateTime _Hasta, string _Ruc,  out string OnError)
        {
            OnInit_N4();
            parametros.Clear();
            parametros.Add("ID_PPZAL", _pid_ppzal);
            parametros.Add("Desde", _Desde);
            parametros.Add("Hasta", _Hasta);
            parametros.Add("ruc", _Ruc);
            return sql_pointer.ExecuteSelectControl<Archivos_Zal>(v_conexion, 4000, "SP_GET_ID_PASE_ZAL_VER_ARCHIVO", parametros, out OnError);
        }

        /*cargar archivo*/
        public bool Grabar_Archivo()
        {
            OnInit_N4();

            parametros.Clear();
            string OnError;
            parametros.Add("id_ppzal", this.id_ppzal);
            parametros.Add("xmlDocumentos", this.ruta_documento);
            parametros.Add("usr_ing_archivo", this.usr_ing_archivo);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 4000, "SP_INSERTA_ARCHIVO_ZAL", parametros, out OnError);
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
