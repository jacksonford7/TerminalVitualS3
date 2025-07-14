using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Aduanas;

namespace CSLSite.services
{
    public class DataChart 
    {
        //idgrap-->id unico, usID usuario solicita, usRuc: ruc usuario, desde, hasta , oth => otros futuro
        public static string GetJSONData(int idGrap, string usRuc, DateTime desde, DateTime hasta)
        {
            try
            {
         
                var rsop = Aduanas.Billion.SourceGrap.RecuperarGrafico(idGrap,  usRuc, desde, hasta);
                if (!rsop.Exitoso)
                {
                    var json = new jsonError();
                    json.error = rsop.MensajeProblema;
                    json.descripcion = rsop.QueHacer;
                    return json.ToString();
                }
                var jso = new jsonGrap();
                jso.id = idGrap;
                jso.label = rsop.Resultado.label;
                jso.labels = rsop.Resultado.labels;
                jso.tipo = rsop.Resultado.tipo;
                jso.borderColor = rsop.Resultado.borderColor;
                jso.backgroundColor = rsop.Resultado.backgroundColor;
                jso.data = rsop.Resultado.data;
                jso.descripcion = rsop.Resultado.descripcion;
                jso.ejes = rsop.Resultado.ejes;
                jso.error = string.Empty;
                var S = jso.ToString();
                return S;
            }
            catch (ApplicationException e)
            {
                var json = new jsonError();
                json.error = e.Message;
                json.descripcion = nameof(ApplicationException);
                return json.ToString();
            }
            catch (Exception e)
            {
                var json = new jsonError();
                json.error = e.Message;
                json.descripcion = nameof(Exception);
                return json.ToString();
            }
        }
        public static string GetJSONDataMulti(int idGrap, string usRuc, DateTime desde, DateTime hasta)
        {
            try
            {
         
                var rsop = Aduanas.Billion.SourceGroup.MultiRecuperarGrafico(idGrap,  usRuc, desde, hasta);
                if (!rsop.Exitoso)
                {
                    var json = new jsonError();
                    json.error = rsop.MensajeProblema;
                    json.descripcion = rsop.QueHacer;
                    return json.ToString();
                }
                var jso = new jsonGroup();
                jso.id = idGrap;
                jso.label = rsop.Resultado.label;
                jso.labels = rsop.Resultado.labels;
                jso.tipo = rsop.Resultado.tipo;
                jso.borderColor = rsop.Resultado.borderColor;
                jso.backgroundColor = rsop.Resultado.backgroundColor;
               
                jso.descripcion = rsop.Resultado.descripcion;
                jso.ejes = rsop.Resultado.ejes;
                jso.error = string.Empty;

                foreach (var z in rsop.Resultado.series)
                {
                    var ji =new jsonGroupItem();
                    ji.grupo_id = z.grupo_id;
                    ji.grupo_valores = new string[z.grupo_valores.Length];
                    
                    var ci = 0;
                    foreach (var ic in z.grupo_valores)
                    {
                        var tt = ic.ToString();
                        var coma = tt.IndexOf(".");
                        var punto = tt.IndexOf(",");
                        var esdec = (coma > 0 || punto > 0);
                        ji.grupo_valores[ci] = esdec ? tt : tt.Split(coma > 0 ? ',' : '.')[0];
                        ci++;
                    }

                    jso.series.Add(ji);
                }

                var S = jso.ToString();
                return S;
            }
            catch (ApplicationException e)
            {
                var json = new jsonError();
                json.error = e.Message;
                json.descripcion = nameof(ApplicationException);
                return json.ToString();
            }
            catch (Exception e)
            {
                var json = new jsonError();
                json.error = e.Message;
                json.descripcion = nameof(Exception);
                return json.ToString();
            }
        }
        public static string GetJSONDataMultiSerie(int idGrap, string usRuc, DateTime desde, DateTime hasta)
        {
            try
            {
                var rsop = Aduanas.Billion.SourceMulti.MultiRecuperarGrafico(idGrap, usRuc, desde, hasta);
                if (!rsop.Exitoso)
                {
                    var json = new jsonError();
                    json.error = rsop.MensajeProblema;
                    json.descripcion = rsop.QueHacer;
                    return json.ToString();
                }
                var jso = new jsonMultiAxis();
                jso.id = idGrap;
                jso.label = rsop.Resultado.label;
                jso.tipo = rsop.Resultado.tipo;
                jso.borderColor = rsop.Resultado.borderColor;
                jso.backgroundColor = rsop.Resultado.backgroundColor;
                jso.descripcion = rsop.Resultado.descripcion;
                jso.ejes = rsop.Resultado.ejes;
                jso.error = string.Empty;
                foreach (var z in rsop.Resultado.series)
                {
                    var ji = new jsonMultItem();
                    ji.serie = z.serie;
                    ji.conteo = z.conteo;
                    ji.total = z.total;
                    jso.series.Add(ji);
                }
                var S = jso.ToString();
                return S;
            }
            catch (ApplicationException e)
            {
                var json = new jsonError();
                json.error = e.Message;
                json.descripcion = nameof(ApplicationException);
                return json.ToString();
            }
            catch (Exception e)
            {
                var json = new jsonError();
                json.error = e.Message;
                json.descripcion = nameof(Exception);
                return json.ToString();
            }
        }

    }

    public class jsonGrap
    {
        public int id { get; set; } // cab
        public string tipo { get; set; } // cab
        public string label { get; set; } // cab
        public string descripcion { get; set; } //opcional
        public int ejes { get; set; } //opcional
        public string error { get; set; }

        public string[] labels { get; set; } //det sp
        public string[] data { get; set; }//det sp
        public string[] backgroundColor { get; set; } //set fc
        public string[] borderColor { get; set; } //set cf


        public override string ToString()
        {
            StringBuilder js = new StringBuilder();
            js.Append("{");
            js.AppendFormat("\"{0}\":{1},", nameof(id), id);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(tipo), tipo);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(label), label);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(descripcion), descripcion);
            js.AppendFormat("\"{0}\":{1},", nameof(ejes), ejes);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(error), error);
            if (this.labels != null && this.labels.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(labels));
                var tot = labels.Length;
                var act = 1;
                foreach (var n in labels)
                {
                    if (tot != act)
                    { js.AppendFormat("\"{0}\",", n); }
                    else
                    { js.AppendFormat("\"{0}\"", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("],");
            }
            else
            {
                js.AppendFormat("\"{0}\":[],", nameof(labels));
            }


            if (this.backgroundColor != null && this.backgroundColor.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(backgroundColor));
                var tot = backgroundColor.Length;
                var act = 1;
                foreach (var n in backgroundColor)
                {
                    if (tot != act)
                    { js.AppendFormat("\"{0}\",", n); }
                    else
                    { js.AppendFormat("\"{0}\"", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("],");
            }
            else
            {
                js.AppendFormat("\"{0}\":[],", nameof(backgroundColor));
            }

            if (this.borderColor != null && this.borderColor.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(borderColor));
                var tot = borderColor.Length;
                var act = 1;
                foreach (var n in borderColor)
                {
                    if (tot != act)
                    { js.AppendFormat("\"{0}\",", n); }
                    else
                    { js.AppendFormat("\"{0}\"", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("],");
            }
            else
            {
                js.AppendFormat("\"{0}\":[],", nameof(borderColor));
            }


            if (this.data != null && this.data.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(data));
                var tot = data.Length;
                var act = 1;
                foreach (var n in data)
                {
                    if (tot != act)
                    { js.AppendFormat("{0},", n); }
                    else
                    { js.AppendFormat("{0}", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("]");
            }
            else
            {
                js.AppendFormat("\"{0}\":[]", nameof(data));
            }

            js.Append("}");
            return js.ToString();
        }

    }

    public class jsonError
    {
        public int id { get; set; } // cab

        public string descripcion { get; set; } //opcional
        public string error { get; set; }




        public override string ToString()
        {
            StringBuilder js = new StringBuilder();
            js.Append("{");
            js.AppendFormat("\"{0}\":{1},", nameof(id), id);

            js.AppendFormat("\"{0}\":\"{1}\",", nameof(descripcion), descripcion);

            js.AppendFormat("\"{0}\":\"{1}\"", nameof(error), error);


            js.Append("}");
            return js.ToString();
        }

    }

    public class jsonGroup
    {

        public jsonGroup() { this.series = new List<jsonGroupItem>(); }
        public int id { get; set; } // cab
        public string tipo { get; set; } // cab
        public string label { get; set; } // cab
        public string descripcion { get; set; } //opcional
        public int ejes { get; set; } //opcional
        public string error { get; set; }

        public string[] labels { get; set; } //det sp
        public List<jsonGroupItem> series { get; set; }//det sp
        public string[] backgroundColor { get; set; } //set fc
        public string[] borderColor { get; set; } //set cf


        public override string ToString()
        {
            StringBuilder js = new StringBuilder();
            js.Append("{");
            js.AppendFormat("\"{0}\":{1},", nameof(id), id);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(tipo), tipo);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(label), label);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(descripcion), descripcion);
            js.AppendFormat("\"{0}\":{1},", nameof(ejes), ejes);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(error), error);
            if (this.labels != null && this.labels.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(labels));
                var tot = labels.Length;
                var act = 1;
                foreach (var n in labels)
                {
                    if (tot != act)
                    { js.AppendFormat("\"{0}\",", n); }
                    else
                    { js.AppendFormat("\"{0}\"", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("],");
            }
            else
            {
                js.AppendFormat("\"{0}\":[],", nameof(labels));
            }


            if (this.backgroundColor != null && this.backgroundColor.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(backgroundColor));
                var tot = backgroundColor.Length;
                var act = 1;
                foreach (var n in backgroundColor)
                {
                    if (tot != act)
                    { js.AppendFormat("\"{0}\",", n); }
                    else
                    { js.AppendFormat("\"{0}\"", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("],");
            }
            else
            {
                js.AppendFormat("\"{0}\":[],", nameof(backgroundColor));
            }

            if (this.borderColor != null && this.borderColor.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(borderColor));
                var tot = borderColor.Length;
                var act = 1;
                foreach (var n in borderColor)
                {
                    if (tot != act)
                    { js.AppendFormat("\"{0}\",", n); }
                    else
                    { js.AppendFormat("\"{0}\"", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("],");
            }
            else
            {
                js.AppendFormat("\"{0}\":[],", nameof(borderColor));
            }


            //nuevo serie multiple
            if (this.series != null && this.series.Count > 0)
            {
                var tot = series.Count;
                var act = 1;
                js.AppendFormat("\"{0}\":[", nameof(series));
                foreach (var it in this.series)
                {
                    js.Append("{");
                    js.AppendFormat("\"{0}\":\"{1}\",", nameof(it.grupo_id), it.grupo_id);
                    var inc = 1;
                    var tto = it.grupo_valores.Length;
                    if (tto > 0)
                    {
                        js.AppendFormat("\"{0}\":[", nameof(it.grupo_valores));
                        foreach (var vz in it.grupo_valores)
                        {
                            js.AppendFormat("{0}", vz);
                            if (tto != inc)
                                js.Append(",");

                            inc++;
                        }
                        js.Append("]");
                    }
                    else
                    {
                        js.AppendFormat("\"{0}\":[]", nameof(it.grupo_valores));
                    }
                    js.Append("}");
                    //coma del final
                    if (tot != act)
                        js.Append(",");

                    act++;
                }
                js.Append("]");
            }
            else
            {
                js.AppendFormat("\"{0}\":[]", nameof(series));
            }

            js.Append("}");
            return js.ToString();
        }

    }

    public class jsonGroupItem
    {

        public string grupo_id { get; set; }
        public string[] grupo_valores { get; set; }
    }

    public class jsonMultiAxis
    {

        public jsonMultiAxis() { this.series = new List<jsonMultItem>(); }
        public int id { get; set; } // cab
        public string tipo { get; set; } // cab
        public string label { get; set; } // cab
        public string descripcion { get; set; } //opcional
        public int ejes { get; set; } //opcional
        public string error { get; set; }


        public List<jsonMultItem> series { get; set; }//det sp
        public string[] backgroundColor { get; set; } //set fc
        public string[] borderColor { get; set; } //set cf


        public override string ToString()
        {
            StringBuilder js = new StringBuilder();
            js.Append("{");
            js.AppendFormat("\"{0}\":{1},", nameof(id), id);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(tipo), tipo);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(label), label);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(descripcion), descripcion);
            js.AppendFormat("\"{0}\":{1},", nameof(ejes), ejes);
            js.AppendFormat("\"{0}\":\"{1}\",", nameof(error), error);

             if (this.backgroundColor != null && this.backgroundColor.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(backgroundColor));
                var tot = backgroundColor.Length;
                var act = 1;
                foreach (var n in backgroundColor)
                {
                    if (tot != act)
                    { js.AppendFormat("\"{0}\",", n); }
                    else
                    { js.AppendFormat("\"{0}\"", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("],");
            }
            else
            {
                js.AppendFormat("\"{0}\":[],", nameof(backgroundColor));
            }

            if (this.borderColor != null && this.borderColor.Length > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(borderColor));
                var tot = borderColor.Length;
                var act = 1;
                foreach (var n in borderColor)
                {
                    if (tot != act)
                    { js.AppendFormat("\"{0}\",", n); }
                    else
                    { js.AppendFormat("\"{0}\"", n); }
                    act++;
                }
                //remover la ultima coma
                js.Append("],");
            }
            else
            {
                js.AppendFormat("\"{0}\":[],", nameof(borderColor));
            }

           
            if (this.series != null && this.series.Count > 0)
            {
                js.AppendFormat("\"{0}\":[", nameof(series));
                //el nuevo objeto
                var i = 1;
                var f = this.series.Count;
                foreach (var to in this.series)
                {
                    js.Append("{");
                    js.AppendFormat("\"{0}\":\"{1}\",", nameof(to.serie), to.serie);
                    js.AppendFormat("\"{0}\":{1},", nameof(to.conteo), to.conteo);
                    js.AppendFormat("\"{0}\":{1}", nameof(to.total), to.total);
                    js.Append("}");
                    if (i != f) { js.Append(","); }
                    i++;
                }
               
                js.Append("]");
            }
            else
            {
                js.AppendFormat("\"{0}\":[]", nameof(series));
            }
            js.Append("}");
            return js.ToString();
        }

    }

    public class jsonMultItem
    {

        public string serie { get; set; }
        public int conteo { get; set; }
        public decimal total { get; set; }

    }




}