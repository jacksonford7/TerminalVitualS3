using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Configuraciones;
using Respuesta;

namespace N4.Entidades
{
   public class PLH_Log:ModuloBase
    {
        public string usuario { get; set; }
        public DateTime? register_date { get; set; }
        public int? registros { get; set; }
        public int? correctos { get; set; }
        public int? incorrectos { get; set; }
        public Int64 uuid { get; set; } //campo relacional

        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public void GrabarLog(DataTable tblog)
        {
            this.actualMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
#if DEBUG
                this.LogEvent(usuario, this.actualMetodo, "No se inicializa");
#endif
                return;
            }
            if (string.IsNullOrEmpty(usuario))
            {
#if DEBUG
                this.LogEvent(usuario, this.actualMetodo, "Sin usuario");
#endif
                return;
            }
            bool hasDet = tblog != null && tblog.Rows.Count > 0;
            this.Parametros.Clear();
            this.Parametros.Add(nameof(usuario), usuario);

            if (hasDet)
            {
                this.registros = tblog.Rows.Count;
                var correctos = tblog.AsEnumerable().Where(f => f.Field<bool>("valid")).Count();
                this.correctos = correctos;
                this.incorrectos = tblog.Rows.Count - correctos;
            }
            else
            {
#if DEBUG
                this.LogEvent(usuario, this.actualMetodo, "SIN TABLA");
#endif
            }



            this.Parametros.Add(nameof(registros), registros);
            this.Parametros.Add(nameof(correctos), correctos);
            this.Parametros.Add(nameof(incorrectos), incorrectos);
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO   

            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_PLH]", this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
#if DEBUG
                this.LogEvent(usuario, this.actualMetodo, result.MensajeProblema);
#endif
                return;
            }
            this.uuid = result.Resultado.HasValue ? result.Resultado.Value : -1;
            foreach (DataRow dr in tblog.Rows)
            {
                dr["PWLogUID"] = result.Resultado;
            }
            var ores = BDOpe.ComandoInsertMasivo(bcon, "dbo.PLH_Detail", tblog);
            if (!ores.Exitoso)
            {
#if DEBUG
                this.LogEvent(usuario, this.actualMetodo, ores.MensajeProblema);
#endif
                return;
            }
        }


    }



}
