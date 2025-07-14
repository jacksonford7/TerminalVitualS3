using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Proforma_Cabecera : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _PF_ID;
        private string _PF_GLOSA = string.Empty;
        private DateTime? _PF_FECHA = null;
        private string _PF_TIPO_CARGA = string.Empty;
        private string _PF_CODIGO_AGENTE = string.Empty;
        private string _PF_ID_AGENTE = string.Empty;
        private string _PF_DESC_AGENTE = string.Empty;
        private string _PF_ID_CLIENTE = string.Empty;
        private string _PF_DESC_CLIENTE = string.Empty;
        private string _PF_ID_FACTURADO = string.Empty;
        private string _PF_DESC_FACTURADO = string.Empty;
        private decimal _PF_SUBTOTAL = 0;
        private decimal _PF_IVA = 0;
        private decimal _PF_TOTAL = 0;
        private string _PF_NUMERO_CARGA = string.Empty;
        private string _PF_CONTENEDORES= string.Empty;
        private DateTime? _PF_FECHA_HASTA = null;
        private string _PF_SESION;
        private string _PF_HORA_HASTA = null;
        private string _PF_IP = string.Empty;
        private decimal _PF_TOTAL_BULTOS = 0;

        #endregion

        #region "Propiedades"

        public Int64 PF_ID { get => _PF_ID; set => _PF_ID = value; }
        public string PF_GLOSA { get => _PF_GLOSA; set => _PF_GLOSA = value; }
        public DateTime? PF_FECHA { get => _PF_FECHA; set => _PF_FECHA = value; }
        public DateTime? PF_FECHA_HASTA { get => _PF_FECHA_HASTA; set => _PF_FECHA_HASTA = value; }
        public string PF_TIPO_CARGA { get => _PF_TIPO_CARGA; set => _PF_TIPO_CARGA = value; }
        public string PF_CODIGO_AGENTE { get => _PF_CODIGO_AGENTE; set => _PF_CODIGO_AGENTE = value; }
        public string PF_ID_AGENTE { get => _PF_ID_AGENTE; set => _PF_ID_AGENTE = value; }
        public string PF_DESC_AGENTE { get => _PF_DESC_AGENTE; set => _PF_DESC_AGENTE = value; }
        public string PF_ID_CLIENTE { get => _PF_ID_CLIENTE; set => _PF_ID_CLIENTE = value; }
        public string PF_DESC_CLIENTE { get => _PF_DESC_CLIENTE; set => _PF_DESC_CLIENTE = value; }
        public string PF_ID_FACTURADO { get => _PF_ID_FACTURADO; set => _PF_ID_FACTURADO = value; }
        public string PF_DESC_FACTURADO { get => _PF_DESC_FACTURADO; set => _PF_DESC_FACTURADO = value; }
        public string PF_NUMERO_CARGA { get => _PF_NUMERO_CARGA; set => _PF_NUMERO_CARGA = value; }
        public string PF_CONTENEDORES { get => _PF_CONTENEDORES; set => _PF_CONTENEDORES = value; }

        public decimal PF_SUBTOTAL { get => _PF_SUBTOTAL; set => _PF_SUBTOTAL = value; }
        public decimal PF_IVA { get => _PF_IVA; set => _PF_IVA = value; }
        public decimal PF_TOTAL { get => _PF_TOTAL; set => _PF_TOTAL = value; }
        public string  PF_SESION { get => _PF_SESION; set => _PF_SESION = value; }
        public string  PF_HORA_HASTA { get => _PF_HORA_HASTA; set => _PF_HORA_HASTA = value; }

        public string PF_IP { get => _PF_IP; set => _PF_IP = value; }

        private static String v_mensaje = string.Empty;

        public decimal PF_TOTAL_BULTOS { get => _PF_TOTAL_BULTOS; set => _PF_TOTAL_BULTOS = value; }
        #endregion



        public List<Cls_Bil_Proforma_Detalle> Detalle { get; set; }
        public List<Cls_Bil_Proforma_Servicios> DetalleServicios { get; set; }

        public Cls_Bil_Proforma_Cabecera()
        {
            init();

            this.Detalle = new List<Cls_Bil_Proforma_Detalle>();
            this.DetalleServicios = new List<Cls_Bil_Proforma_Servicios>();

        }

        public Cls_Bil_Proforma_Cabecera(Int64 _PF_ID, string _PF_GLOSA, DateTime? PF_FECHA, string _PF_TIPO_CARGA, string _PF_ID_AGENTE, string _PF_DESC_AGENTE,
                                        string _PF_ID_CLIENTE, string _PF_DESC_CLIENTE, string _PF_ID_FACTURADO, string _PF_DESC_FACTURADO,
                                        decimal _PF_SUBTOTAL, decimal _PF_IVA, decimal _PF_TOTAL,
                                        string _PF_USUARIO_CREA, DateTime? _PF_FECHA_CREA, string _PF_NUMERO_CARGA, string _PF_CONTENEDORES,
                                        DateTime? _PF_FECHA_HASTA, string _PF_CODIGO_AGENTE, string _PF_SESION, string _PF_HORA_HASTA, string _PF_IP)

        {
            this.PF_ID = _PF_ID;
            this.PF_GLOSA = _PF_GLOSA;
            this.PF_FECHA = _PF_FECHA;
            this.PF_TIPO_CARGA = _PF_TIPO_CARGA;
            this.PF_ID_AGENTE = _PF_ID_AGENTE;
            this.PF_DESC_AGENTE = _PF_DESC_AGENTE;
            this.PF_ID_CLIENTE = _PF_ID_CLIENTE;
            this.PF_DESC_CLIENTE = _PF_DESC_CLIENTE;
            this.PF_ID_FACTURADO = _PF_ID_FACTURADO;
            this.PF_DESC_FACTURADO = _PF_DESC_FACTURADO;
            this.PF_SUBTOTAL = _PF_SUBTOTAL;
            this.PF_IVA = _PF_IVA;
            this.PF_TOTAL = _PF_TOTAL;
            this.IV_USUARIO_CREA = _PF_USUARIO_CREA;
            this.IV_FECHA_CREA = _PF_FECHA_CREA;
            this.PF_NUMERO_CARGA = _PF_NUMERO_CARGA;
            this.PF_CONTENEDORES = _PF_CONTENEDORES;
            this.PF_FECHA_HASTA = _PF_FECHA_HASTA;
            this.PF_CODIGO_AGENTE = _PF_CODIGO_AGENTE;
            this.PF_SESION = _PF_SESION;
            this.PF_HORA_HASTA = _PF_HORA_HASTA;
            this.PF_IP = _PF_IP;

            this.Detalle = new List<Cls_Bil_Proforma_Detalle>();
            this.DetalleServicios = new List<Cls_Bil_Proforma_Servicios>();

        }


        private int? PreValidationsTransaction(out string msg)
        {

            if (!this.PF_FECHA.HasValue)
            {

                msg = "La fecha de la transacción no es valida";
                return 0;
            }

            if (string.IsNullOrEmpty(this.PF_TIPO_CARGA))
            {
                msg = "Debe especificar el tipo de carga (CONTENEDOR, CFS, BRBK) ";
                return 0;

            }

            if (string.IsNullOrEmpty(this.PF_CODIGO_AGENTE))
            {
                msg = "Debe especificar el ID del agente de aduana ";
                return 0;

            }

            //if (string.IsNullOrEmpty(this.PF_ID_AGENTE))
            //{
            //    msg = "Debe especificar el ID del agente de aduana ";
            //    return 0;

            //}
            //if (string.IsNullOrEmpty(this.PF_DESC_AGENTE))
            //{
            //    msg = "Debe especificar la descripción del agente de aduana ";
            //    return 0;

            //}
            if (string.IsNullOrEmpty(this.PF_ID_CLIENTE))
            {
                msg = "Debe especificar el ID del cliente ";
                return 0;

            }
            if (string.IsNullOrEmpty(this.PF_DESC_CLIENTE))
            {
                msg = "Debe especificar la descripción del cliente ";
                return 0;

            }
            if (this.PF_TOTAL <= 0)
            {
                msg = "Especifique los totales de la transacción";
                return 0;
            }

            if (string.IsNullOrEmpty(this.PF_SESION))
            {
                msg = "Error en identificador de sesión, no se ha generado la sesión";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_USUARIO_CREA))
            {
                msg = "Debe especificar el usuario que crea la transacción";
                return 0;
            }


            //cuenta solo los activos
            var nRegistros = this.Detalle.Where(d => d.PF_GKEY != 0).Count();

            if (this.Detalle == null || nRegistros <= 0)
            {
                msg = "No se puede agregar el detalle de la transaccion, seleccione las cargas a facturar";
                return 0;
            }


            nRegistros = this.DetalleServicios.Where(d => d.PF_SUBTOTAL != 0).Count();
            if (this.DetalleServicios == null || nRegistros <= 0)
            {
                msg = "No se puede agregar el detalle de servicios, no existen rubros a facturar";
                return 0;
            }


            msg = string.Empty;
            return 1;
        }


        private Int64? Save(out string OnError)
        {

            parametros.Clear();
            parametros.Add("PF_ID", this.PF_ID);
            parametros.Add("PF_GLOSA", this.PF_GLOSA);
            parametros.Add("PF_FECHA", this.PF_FECHA);
            parametros.Add("PF_TIPO_CARGA", this.PF_TIPO_CARGA);
            parametros.Add("PF_CODIGO_AGENTE", this.PF_CODIGO_AGENTE);
            parametros.Add("PF_ID_AGENTE", (string.IsNullOrEmpty(this.PF_ID_AGENTE) ? "CGSA": this.PF_ID_AGENTE));
            parametros.Add("PF_DESC_AGENTE", (string.IsNullOrEmpty(this.PF_DESC_AGENTE) ? "CGSA" : this.PF_DESC_AGENTE));
            parametros.Add("PF_ID_CLIENTE", this.PF_ID_CLIENTE);
            parametros.Add("PF_DESC_CLIENTE", this.PF_DESC_CLIENTE);
            parametros.Add("PF_ID_FACTURADO", this.PF_ID_FACTURADO);
            parametros.Add("PF_DESC_FACTURADO", this.PF_DESC_FACTURADO);
            parametros.Add("PF_SUBTOTAL", this.PF_SUBTOTAL);
            parametros.Add("PF_IVA", this.PF_IVA);//REVISAR
            parametros.Add("PF_TOTAL", this.PF_TOTAL);
            parametros.Add("PF_NUMERO_CARGA", this.PF_NUMERO_CARGA);
            parametros.Add("PF_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("PF_FECHA_CREA", this.IV_FECHA_CREA);
            parametros.Add("PF_FECHA_HASTA", this.PF_FECHA_HASTA);
            parametros.Add("PF_SESION", this.PF_SESION);
            parametros.Add("PF_HORA_HASTA", this.PF_HORA_HASTA);
            parametros.Add("PF_IP", this.PF_IP);
            parametros.Add("PF_TOTAL_BULTOS", this.PF_TOTAL_BULTOS);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_Bil_inserta_proforma_cab", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


        public Int64? SaveTransaction(out string OnError)
        {

            string resultado_otros = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }

                    this.PF_ID = id.Value;
                    var nContador = 1;
                    //si no falla la cabecera entonces añada los items
                    foreach (var i in this.Detalle)
                    {
                        i.PF_ID = id.Value;
                        i.IV_USUARIO_CREA = this.IV_USUARIO_CREA;
                        i.IV_FECHA_CREA = this.IV_FECHA_CREA;

                        var IdRetorno = i.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            return 0;
                        }

                        i.PF_ID = IdRetorno.Value;
                        nContador = nContador + 1;
                    }

                    nContador = 1;
                    //si no falla la cabecera entonces añada los items
                    foreach (var dn in this.DetalleServicios)
                    {
                        dn.PF_ID = id.Value;
                        dn.PF_LINEA = nContador;
                        dn.IV_USUARIO_CREA = this.IV_USUARIO_CREA;
                        dn.IV_FECHA_CREA = this.IV_FECHA_CREA;

                        var IdRetorno = dn.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            if (OnError == string.Empty)
                            {
                                OnError = "*** Error: No existen servicios disponibles ****";
                            }
                            return 0;
                        }

                        dn.PF_ID = IdRetorno.Value;
                        nContador = nContador + 1;
                    }

                    //fin de la transaccion
                    scope.Complete();



                    return this.PF_ID;
                }
            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);
                return null;
            }
        }



    }
}
