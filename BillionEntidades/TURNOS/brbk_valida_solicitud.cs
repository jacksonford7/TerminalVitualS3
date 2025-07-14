using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_valida_solicitud : Cls_Bil_Base
    {
        #region "Variables"

        private static Int64? lm = -3;
     
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
       
        private string _RUC = string.Empty;
        private string _MENSAJE = string.Empty;

        private int _IDPRODUCTO;
        #endregion

        #region "Propiedades"


        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }

       
        public string RUC { get => _RUC; set => _RUC = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }

        public int IDPRODUCTO { get => _IDPRODUCTO; set => _IDPRODUCTO = value; }
        #endregion





        public brbk_valida_solicitud()
        {
            init();

          

        }

        #region "Valida si existe turnos aprobados y expirados"
        public bool Valida_Existe_Turnos_Expirados(out string OnError)
        {

            parametros.Clear();
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("RUC", this.RUC);

            var t = sql_puntero.ExecuteSelectOnly<brbk_valida_solicitud>(sql_puntero.Conexion_Local, 6000, "[Brbk].[VALIDA_SOLICITUD_RECHAZAR]", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }
            else
            {
                this.MENSAJE = t.MENSAJE;

                OnError = string.Format("{0}", this.MENSAJE);
                    

                return true;
            }


        }
        #endregion

        #region "Valida si existe pase de puerta expirados y no facturados"
        public bool Valida_Existe_PasesPuerta_Expirados(out string OnError)
        {

            parametros.Clear();
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("RUC", this.RUC);

            var t = sql_puntero.ExecuteSelectOnly<brbk_valida_solicitud>(sql_puntero.Conexion_Local, 6000, "[Brbk].[VALIDA_PASES_EXPIRADOS]", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }
            else
            {
                this.MENSAJE = t.MENSAJE;

                OnError = string.Format("{0}", this.MENSAJE);


                return true;
            }


        }
        #endregion

        #region "Valida tipo de carga"
        public bool Valida_Tipo_Carga(out string OnError)
        {

            parametros.Clear();
            //parametros.Add("IDPRODUCTO", this.IDPRODUCTO);
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("RUC", this.RUC);
            var t = sql_puntero.ExecuteSelectOnly<brbk_valida_solicitud>(sql_puntero.Conexion_Local, 6000, "[Brbk].[VALIDA_TIPO_CARGA]", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }
            else
            {
                this.MENSAJE = t.MENSAJE;

                OnError = string.Format("{0}", this.MENSAJE);


                return true;
            }


        }
        #endregion

        #region "Valida si el numero de carga ya fue excluido"
        public bool Valida_Carga_Excluida(out string OnError)
        {

            parametros.Clear();
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("RUC", this.RUC);

            var t = sql_puntero.ExecuteSelectOnly<brbk_valida_solicitud>(sql_puntero.Conexion_Local, 6000, "[Brbk].[VALIDA_EXCLUIR_NUMERO_CARGA]", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }
            else
            {
                this.MENSAJE = t.MENSAJE;

                OnError = string.Format("{0}", this.MENSAJE);


                return true;
            }


        }
        #endregion
    }
}
