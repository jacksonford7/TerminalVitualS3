using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Configuraciones;
using AccesoDatos;
using Respuesta;
using System.Reflection;
using System.Text;

namespace Aduanas.Billion
{
    public class SourceGrap: ModuloBase
    {

        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public SourceGrap() : base()
        {
            this.ColorConfigs = new List<SourceGrapCfg>();
        }


        public int id { get; set; } // cab
        public string tipo { get; set; } // cab
        public string label { get; set; } // cab
        public string descripcion { get; set; } //opcional
        public int ejes { get; set; } //opcional
        public string fuente { get; set; } // cab
        public string procedimiento { get; set; } //opcional

        public List<SourceGrapCfg> ColorConfigs { get; set; }
        public string[] labels { get; set; } //det sp
        public string[] data { get; set; }//det sp
        public string[] backgroundColor { get; set; } //set fc
        public string[] borderColor { get; set; } //set cf

        public static Respuesta.ResultadoOperacion<SourceGrap> RecuperarGrafico(  int id, string usRuc, DateTime desde, DateTime hasta)
        {
            try
            {
                var gg = new SourceGrap();
                gg.actualMetodo = MethodBase.GetCurrentMethod().Name;
                //inicializa
                string pv = string.Empty;
                if (!gg.Accesorio.Inicializar(out pv))
                {
                    
                    return Respuesta.ResultadoOperacion<SourceGrap>.CrearFalla(pv);
                }
                var bcon = gg.Accesorio.ObtenerConfiguracion("Billion")?.valor;
                if (string.IsNullOrEmpty(bcon))
                {
                  
                    return Respuesta.ResultadoOperacion<SourceGrap>.CrearFalla("La cadena de conexion Billion no existe");
                }
                var qr = BDOpe.ComandoSelectAEntidad<SourceGrap>(bcon, "bill.listar_configuracion_grafico", new Dictionary<string, object>() { { "id", id } });
                if (!qr.Exitoso)
                {
                   
                    return Respuesta.ResultadoOperacion<SourceGrap>.CrearFalla(qr.MensajeProblema);
                }
                gg.fuente = qr.Resultado.fuente;
                gg.procedimiento = qr.Resultado.procedimiento;
                gg.tipo = qr.Resultado.tipo;
                gg.label = qr.Resultado.label;
                gg.id = qr.Resultado.id;
                gg.descripcion = qr.Resultado.descripcion;
                gg.ejes = qr.Resultado.ejes;
                //no lanzo error.
                if (gg != null && !string.IsNullOrEmpty(gg.fuente) && !string.IsNullOrEmpty(gg.procedimiento))
                {

                    gg.Parametros.Clear();
                    gg.Parametros.Add(nameof(id), id);
                    var rp = BDOpe.ComandoSelectALista<SourceGrapCfg>(bcon, "bill.listar_configuracion_grafico_color", gg.Parametros);
                    if (!rp.Exitoso)
                    {

                        return Respuesta.ResultadoOperacion<SourceGrap>.CrearFalla(rp.MensajeProblema);
                    }
                    var cfgs = rp.Resultado;
                    if (cfgs != null && cfgs.Count > 0)
                    {
                        var nc = cfgs.Where(ci => ci.nombre.Equals(nameof(backgroundColor)));
                        var nb = cfgs.Where(ci => ci.nombre.Equals(nameof(borderColor)));
                        gg.backgroundColor = new string[nc.Count()];
                        gg.borderColor = new string[nb.Count()];
                        var i = 0;
                        foreach (var b in nb)
                        {
                            gg.borderColor[i] = b.valor;
                            i++;
                        }
                        i = 0;
                        foreach (var c in nc)
                        {
                            gg.backgroundColor[i] = c.valor;
                            i++;
                        }
                    }
                    gg.Parametros.Clear();
                    gg.Parametros.Add("desde", desde);
                    gg.Parametros.Add("hasta", hasta);
                    gg.Parametros.Add("cliente", usRuc);
                    var qvalues = BDOpe.ComandoSelectALista<SourceGrapVal>(gg.fuente, gg.procedimiento, gg.Parametros);
                    if (!qvalues.Exitoso)
                    {

                        return Respuesta.ResultadoOperacion<SourceGrap>.CrearFalla(qvalues.MensajeProblema);
                    }

                    var cvalues = qvalues.Resultado;
                    if (cvalues != null && cvalues.Count > 0)
                    {
                        gg.labels = new string[cvalues.Count];
                        gg.data = new string[cvalues.Count];
                        var ci = 0;
                        foreach (var c in cvalues)
                        {
                            gg.labels[ci] = c.serie;
                            var tt = c.valor.ToString();
                            var coma = tt.IndexOf(".");
                            var punto = tt.IndexOf(",");
                            var esdec = (coma > 0 || punto > 0);
                            gg.data[ci] = esdec ? tt : tt.Split(coma > 0 ? ',' : '.')[0];
                            ci++;
                        }
                    }
                }
                return ResultadoOperacion<SourceGrap>.CrearResultadoExitoso(gg);
            }
            catch (Exception e)
            {
                return ResultadoOperacion<SourceGrap>.CrearFalla(e.Message);
            }
        }


 
    }



    public class SourceGrapCfg
    {
        public string nombre { get; set; }
        public string valor { get; set; }
    }

    public class SourceGrapVal
    {
        public string serie { get; set; }
        public decimal valor { get; set; }
    }


    public class SourceGroup : ModuloBase
    {


        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public SourceGroup() : base()
        {
            this.ColorConfigs = new List<SourceGrapCfg>();
            this.series = new List<SourceGroupItem>();
        }

        
        public int id { get; set; } // cab
        public string tipo { get; set; } // cab
        public string label { get; set; } // cab
        public string descripcion { get; set; } //opcional
        public int ejes { get; set; } //opcional
        public string error { get; set; }

        public string[] labels { get; set; } //det sp
        public List<SourceGroupItem> series { get; set; }//det sp
        public string[] backgroundColor { get; set; } //set fc
        public string[] borderColor { get; set; } //set cf


        public string fuente { get; set; } // cab
        public string procedimiento { get; set; } //opcional

        public List<SourceGrapCfg> ColorConfigs { get; set; }


        //nuevo metodo juega con el id ejes debe servir para amabos
        public static Respuesta.ResultadoOperacion<SourceGroup> MultiRecuperarGrafico(int id, string usRuc, DateTime desde, DateTime hasta)
        {
            try
            {
                var gg = new SourceGroup();
                gg.actualMetodo = MethodBase.GetCurrentMethod().Name;
                //inicializa
                string pv = string.Empty;
                if (!gg.Accesorio.Inicializar(out pv))
                {

                    return Respuesta.ResultadoOperacion<SourceGroup>.CrearFalla(pv);
                }
                var bcon = gg.Accesorio.ObtenerConfiguracion("Billion")?.valor;
                if (string.IsNullOrEmpty(bcon))
                {

                    return Respuesta.ResultadoOperacion<SourceGroup>.CrearFalla("La cadena de conexion Billion no existe");
                }
                var qr = BDOpe.ComandoSelectAEntidad<SourceGroup>(bcon, "bill.listar_configuracion_grafico", new Dictionary<string, object>() { { "id", id } });
                if (!qr.Exitoso)
                {

                    return Respuesta.ResultadoOperacion<SourceGroup>.CrearFalla(qr.MensajeProblema);
                }
                gg.fuente = qr.Resultado.fuente;
                gg.procedimiento = qr.Resultado.procedimiento;
                gg.tipo = qr.Resultado.tipo;
                gg.label = qr.Resultado.label;
                gg.id = qr.Resultado.id;
                gg.descripcion = qr.Resultado.descripcion;
                gg.ejes = qr.Resultado.ejes;
                //no lanzo error.
                if (gg != null && !string.IsNullOrEmpty(gg.fuente) && !string.IsNullOrEmpty(gg.procedimiento))
                {
                    gg.Parametros.Clear();
                    gg.Parametros.Add(nameof(id), id);
                    var rp = BDOpe.ComandoSelectALista<SourceGrapCfg>(bcon, "bill.listar_configuracion_grafico_color", gg.Parametros);
                    if (!rp.Exitoso)
                    {

                        return Respuesta.ResultadoOperacion<SourceGroup>.CrearFalla(rp.MensajeProblema);
                    }
                    var cfgs = rp.Resultado;
                    if (cfgs != null && cfgs.Count > 0)
                    {
                        var nc = cfgs.Where(ci => ci.nombre.Equals(nameof(backgroundColor)));
                        var nb = cfgs.Where(ci => ci.nombre.Equals(nameof(borderColor)));
                        gg.backgroundColor = new string[nc.Count()];
                        gg.borderColor = new string[nb.Count()];
                        var i = 0;
                        foreach (var b in nb)
                        {
                            gg.borderColor[i] = b.valor;
                            i++;
                        }
                        i = 0;
                        foreach (var c in nc)
                        {
                            gg.backgroundColor[i] = c.valor;
                            i++;
                        }
                    }
                    gg.Parametros.Clear();
                    gg.Parametros.Add("desde", desde);
                    gg.Parametros.Add("hasta", hasta);
                    gg.Parametros.Add("cliente", usRuc);

                    //de retornar 3 columnas maximo
                    var qvalues = BDOpe.ComandoSelectALista<SourceGroupVal>(gg.fuente, gg.procedimiento, gg.Parametros);
                    if (!qvalues.Exitoso)
                    {
                        return Respuesta.ResultadoOperacion<SourceGroup>.CrearFalla(qvalues.MensajeProblema);
                    }
                    //tengo la lista de los distintos meses o periodos
                    var serieLista = qvalues.Resultado.Select(s => s.serie).Distinct();
                    if (serieLista == null)
                    {
                        return Respuesta.ResultadoOperacion<SourceGroup>.CrearFalla("Serie no contiene valores");
                    }
                    //la serie para todos --> ENERO/FEBRERO/MARZO ABRIL ETC...
                    gg.labels = serieLista.ToArray();
                     //tengo la lista de los distintos clientes
                    var serieCliente = qvalues.Resultado.Select(d => d.grupo).Distinct();
                    foreach (var c in serieCliente)
                    {
                        var si = new SourceGroupItem();
                        si.grupo_id = c;
                        //los valores de cada serie
                        var m = 0;
                        si.grupo_valores = new decimal[ gg.labels.Length];
                        foreach (var u in serieLista)
                        {
                            var val = qvalues.Resultado.Where(n => n.serie.Equals(u) && n.grupo.Equals(c)).FirstOrDefault();
                            if (val != null)
                            {
                                si.grupo_valores[m] = val.valor;
                            }
                            else
                            {
                                si.grupo_valores[m] = 0;
                            }
                            m++;
                        }
                        gg.series.Add(si);
                    }
                }
                return ResultadoOperacion<SourceGroup>.CrearResultadoExitoso(gg);
            }
            catch (Exception e)
            {
                return ResultadoOperacion<SourceGroup>.CrearFalla(e.Message);
            }
        }
   }

    public class SourceGroupItem
    {

        public string grupo_id { get; set; }
        public decimal[] grupo_valores { get; set; }
    }

    public class SourceGroupVal
    {
        public string serie { get; set; }
        public string grupo { get; set; }
        public decimal valor { get; set; }
    }


    public class SourceMulti : ModuloBase
    {


        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public SourceMulti() : base()
        {
            this.ColorConfigs = new List<SourceGrapCfg>();
            this.series = new List<SourceMultiItem>();
        }


        public int id { get; set; } // cab
        public string tipo { get; set; } // cab
        public string label { get; set; } // cab
        public string descripcion { get; set; } //opcional
        public int ejes { get; set; } //opcional
        public string error { get; set; }


        public List<SourceMultiItem> series { get; set; }//det sp
        public string[] backgroundColor { get; set; } //set fc
        public string[] borderColor { get; set; } //set cf


        public string fuente { get; set; } // cab
        public string procedimiento { get; set; } //opcional

        public List<SourceGrapCfg> ColorConfigs { get; set; }


        //nuevo metodo juega con el id ejes debe servir para amabos
        public static Respuesta.ResultadoOperacion<SourceMulti> MultiRecuperarGrafico(int id, string usRuc, DateTime desde, DateTime hasta)
        {
            try
            {
                var gg = new SourceMulti();
                gg.actualMetodo = MethodBase.GetCurrentMethod().Name;
                //inicializa
                string pv = string.Empty;
                if (!gg.Accesorio.Inicializar(out pv))
                {

                    return Respuesta.ResultadoOperacion<SourceMulti>.CrearFalla(pv);
                }
                var bcon = gg.Accesorio.ObtenerConfiguracion("Billion")?.valor;
                if (string.IsNullOrEmpty(bcon))
                {

                    return Respuesta.ResultadoOperacion<SourceMulti>.CrearFalla("La cadena de conexion Billion no existe");
                }
                var qr = BDOpe.ComandoSelectAEntidad<SourceGroup>(bcon, "bill.listar_configuracion_grafico", new Dictionary<string, object>() { { "id", id } });
                if (!qr.Exitoso)
                {

                    return Respuesta.ResultadoOperacion<SourceMulti>.CrearFalla(qr.MensajeProblema);
                }
                gg.fuente = qr.Resultado.fuente;
                gg.procedimiento = qr.Resultado.procedimiento;
                gg.tipo = qr.Resultado.tipo;
                gg.label = qr.Resultado.label;
                gg.id = qr.Resultado.id;
                gg.descripcion = qr.Resultado.descripcion;
                gg.ejes = qr.Resultado.ejes;
                //no lanzo error.
                if (gg != null && !string.IsNullOrEmpty(gg.fuente) && !string.IsNullOrEmpty(gg.procedimiento))
                {
                    gg.Parametros.Clear();
                    gg.Parametros.Add(nameof(id), id);
                    var rp = BDOpe.ComandoSelectALista<SourceGrapCfg>(bcon, "bill.listar_configuracion_grafico_color", gg.Parametros);
                    if (!rp.Exitoso)
                    {

                        return Respuesta.ResultadoOperacion<SourceMulti>.CrearFalla(rp.MensajeProblema);
                    }
                    var cfgs = rp.Resultado;
                    if (cfgs != null && cfgs.Count > 0)
                    {
                        var nc = cfgs.Where(ci => ci.nombre.Equals(nameof(backgroundColor)));
                        var nb = cfgs.Where(ci => ci.nombre.Equals(nameof(borderColor)));
                        gg.backgroundColor = new string[nc.Count()];
                        gg.borderColor = new string[nb.Count()];
                        var i = 0;
                        foreach (var b in nb)
                        {
                            gg.borderColor[i] = b.valor;
                            i++;
                        }
                        i = 0;
                        foreach (var c in nc)
                        {
                            gg.backgroundColor[i] = c.valor;
                            i++;
                        }
                    }
                    gg.Parametros.Clear();
                    gg.Parametros.Add("desde", desde);
                    gg.Parametros.Add("hasta", hasta);
                    gg.Parametros.Add("cliente", usRuc);

                    //de retornar 3 columnas maximo
                    var qvalues = BDOpe.ComandoSelectALista<SourceMultiItem>(gg.fuente, gg.procedimiento, gg.Parametros);
                    if (!qvalues.Exitoso)
                    {
                        return Respuesta.ResultadoOperacion<SourceMulti>.CrearFalla(qvalues.MensajeProblema);
                    }
                    //tengo la lista de los distintos meses o periodos
                    var serieLista = qvalues.Resultado.Select(s => s.serie).Distinct();
                    if (serieLista == null)
                    {
                        return Respuesta.ResultadoOperacion<SourceMulti>.CrearFalla("Serie no contiene valores");
                    }
                    gg.series = qvalues.Resultado;
                }
                return ResultadoOperacion<SourceMulti>.CrearResultadoExitoso(gg);
            }
            catch (Exception e)
            {
                return ResultadoOperacion<SourceMulti>.CrearFalla(e.Message);
            }
        }
    }

    public class SourceMultiItem
    {

        public string serie { get; set; }
        public int conteo { get; set; }
        public decimal total { get; set; }
    }




}