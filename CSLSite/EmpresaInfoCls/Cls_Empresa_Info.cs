using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CSLSite.EmpresaInfoCls
{
    [Serializable]
    public class Cls_Empresa_Info : Cls_Bil_Base
    {
        public string TipoCliente { get; set; }              // txttipcli
        public string RazonSocial { get; set; }              // txtrazonsocial
        public string RucCiPasaporte { get; set; }           // txtruccipas
        public string ActividadComercial { get; set; }       // txtactividadcomercial
        public string DireccionOficina { get; set; }         // txtdireccion
        public string TelefonoOficina { get; set; }          // txttelofi
        public string PersonaContacto { get; set; }          // txtcontacto
        public string CelularContacto { get; set; }          // txttelcelcon
        public string EmailContacto { get; set; }            // txtmailinfocli
        public string EmailEBilling { get; set; }            // txtmailebilling
        public string Certificaciones { get; set; }          // txtcertificaciones
        public string SitioWeb { get; set; }                 // turl
        public string AfiGremios { get; set; }               // txtafigremios
        public string ReferenciaComercial { get; set; }      // txtrefcom
        public string RepresentanteLegal { get; set; }       // txtreplegal
        public string TelefonoDomicilio { get; set; }        // txttelreplegal
        public string DireccionDomiciliaria { get; set; }    // txtdirdomreplegal
        public string CedulaRepresentanteLegal { get; set; } // txtci
        public string EmailRepresentanteLegal { get; set; }  // tmailRepLegal


        public Cls_Empresa_Info() : base()
        {
            init();
        }


        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }
        public static List<Cls_Empresa_Info> ObtenerEmpresaInfo(string id)
        {
            try
            {

                OnInit("Portal_Sca");
                parametros.Clear();
                parametros.Add("RUCCIPAS", id);


                string controlError;

                var EmpresaInfo = sql_puntero.ExecuteSelectControl<Cls_Empresa_Info>(
                    nueva_conexion,
                    4000,
                    "SCA_CONSULTA_SOLICITUDES_EMPRESA_NEW",
                    parametros,
                    out controlError
                );
                return EmpresaInfo;

            }


            catch (SqlException sqlEx)
            {
                throw new Exception("Ocurrió un error al consultar los sellos en la base de datos.", sqlEx);
            }
            catch (TimeoutException timeoutEx)
            {
                throw new Exception("El tiempo de espera para la consulta de sellos ha expirado.", timeoutEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al consultar los sellos.", ex);
            }

        }


        public static bool ProcesarCambios(Cls_Empresa_Info dato)
        {
            try
            {
                OnInit("Portal_Sca");
                parametros.Clear();

                parametros.Add("TipoCliente", dato.TipoCliente);
                parametros.Add("RazonSocial", dato.RazonSocial);
                parametros.Add("RucCiPasaporte", dato.RucCiPasaporte);
                parametros.Add("ActividadComercial", dato.ActividadComercial);
                parametros.Add("DireccionOficina", dato.DireccionOficina);
                parametros.Add("TelefonoOficina", dato.TelefonoOficina);
                parametros.Add("PersonaContacto", dato.PersonaContacto);
                parametros.Add("CelularContacto", dato.CelularContacto);
                parametros.Add("EmailContacto", dato.EmailContacto);
                parametros.Add("EmailEBilling", dato.EmailEBilling);
                parametros.Add("Certificaciones", dato.Certificaciones);
                parametros.Add("SitioWeb", dato.SitioWeb);
                parametros.Add("AfiGremios", dato.AfiGremios);
                parametros.Add("ReferenciaComercial", dato.ReferenciaComercial);
                parametros.Add("RepresentanteLegal", dato.RepresentanteLegal);
                parametros.Add("TelefonoDomicilio", dato.TelefonoDomicilio);
                parametros.Add("DireccionDomiciliaria", dato.DireccionDomiciliaria);
                parametros.Add("CedulaRepresentanteLegal", dato.CedulaRepresentanteLegal);
                parametros.Add("EmailRepresentanteLegal", dato.EmailRepresentanteLegal);

                string controlError;

                int? resultado = sql_puntero.ExecuteInsertUpdateDelete(
                    nueva_conexion,
                    4000,
                    "SCA_ACTUALIZA_EMPRESA_INFO",
                    parametros,
                    out controlError
                );

                if (resultado.HasValue && resultado > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al procesar cambios en la empresa", ex);
            }
        }
    }

}


