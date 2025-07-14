using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSLSite.services;

namespace CSLSite
{
    public class CslHelper
    {
       //Return meses
        public static HashSet< Tuple< string, string>> getMonth()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            xlista.Add(Tuple.Create("01", "Ene"));
            xlista.Add(Tuple.Create("02", "Feb"));
            xlista.Add(Tuple.Create("03", "Mar"));
            xlista.Add(Tuple.Create("04", "Abr"));
            xlista.Add(Tuple.Create("05", "May"));
            xlista.Add(Tuple.Create("06", "Jun"));
            xlista.Add(Tuple.Create("07", "Jul"));
            xlista.Add(Tuple.Create("08", "Ago"));
            xlista.Add(Tuple.Create("09", "Sep"));
            xlista.Add(Tuple.Create("10", "Oct"));
            xlista.Add(Tuple.Create("11", "Nov"));
            xlista.Add(Tuple.Create("12", "Dic"));
            return xlista;
        }
        //Return días
        public static HashSet<Tuple<string, string>> getDays()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (int i = 1; i <= 31; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(),i.ToString("00"))); 
            }
            return xlista;
        }
        //Return años
        public static HashSet<Tuple<string, string>> getYears()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            xlista.Add(Tuple.Create((DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year - 1).ToString()));
            xlista.Add(Tuple.Create((DateTime.Now.Year).ToString(), (DateTime.Now.Year).ToString()));
            xlista.Add(Tuple.Create((DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 1).ToString()));
            return xlista;
        }
        //Return hours
        public static HashSet<Tuple<string, string>> getHours()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (int i = 0; i <= 23; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), i.ToString("00")));
            }
            return xlista;
        }
        //Return minutos
        public static HashSet<Tuple<string, string>> getMinutes()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (int i = 1; i <= 60; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), i.ToString("00")));
            }
            return xlista;
        }
        //Return provincias
        public static HashSet<Tuple<string, string>> getProvincias()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnProvincias())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("00","* Elija la Provincia *"));
            return xlista;
        }
        //Return cantones
        public static HashSet<Tuple<string, string>> getCantones(string provincia)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnCantones(provincia))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            return xlista;
        }
        //return instituciones
        public static HashSet<Tuple<string, string>> getInstitucion()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnInstitucion())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0","*Seleccione*"));
            return xlista;
        }
        //return reglas
        public static HashSet<Tuple<string, string>> getReglas(string institucion)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnReglas(institucion))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "*Regla*"));
            return xlista;
        }
        //return bancos
        public static HashSet<Tuple<string, string>> getBancos()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnBancos())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0","* Escoja entidad *"));
            return xlista;
        }
        //return refrigerado
        public static HashSet<Tuple<string, string>> getRefrigeracion()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnRefrigeracion())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", " * No refrigerado *"));
            return xlista;
        }
        //return ventilación
        public static HashSet<Tuple<string, string>> getVentilacion()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (var i=1 ; i<=100;i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), string.Format("{0}%",i)));
            }
            xlista.Add(Tuple.Create("0", " * Sin ventilación (0%) *"));
            return xlista;
        }
        //retorna humedad
        public static HashSet<Tuple<string, string>> getHumedad()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (var i = 1; i <= 100; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), string.Format("{0}", i)));
            }
            xlista.Add(Tuple.Create("0", " * Sin humedad (0) *"));
            return xlista;
        }
        //return iMOS
        public static HashSet<Tuple<string, string>> getImos()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnImos())
            {
                xlista.Add(Tuple.Create(i.Item1, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(i.Item2.ToLower())));
            }
            return xlista;
        }
        //return EMBALAJES
        public static HashSet<Tuple<string, string>> getEmbalajes()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnEmbalajes())
            {
                xlista.Add(Tuple.Create(i.Item1,System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(i.Item2.ToLower())));
            }
            xlista.Add(Tuple.Create("0", "* Escoja tipo de embalaje *"));
            return xlista;
        }
        //return Depositos
        public static HashSet<Tuple<string, string>> getDepositos()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnDepositos())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Escoja del depósito *"));
            return xlista;
        }

        //return Tipo Carga
        public static HashSet<Tuple<string, string>> getTipoCarga()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnTipoCarga())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Escoja el tipo de carga *"));
            return xlista;
        }

        //Mensaje personalizado en string
        public static string JsonNewResponse(bool result,bool fluir,string data, string mensaje)
        {
             return string.Format("{{\"mensaje\": \"{0}\",  \"resultado\": {1},\"data\": \"{2}\",\"fluir\": \"{3}\"  }}", mensaje.Trim(), result.ToString().ToLower(),data.Trim(),fluir.ToString().ToLower());
        }
        //valida que la unidad y booking no se crucen
        public static bool Unitproceed(string unit, string booking, out string message)
        {
            var t = dataServiceHelper.ExistUnit(unit, booking);
            if (t == null)
            {
                message = "No se pudo comprobar la unidad hubo un problema de permisos";
                return false;
            }
            if (t.Value)
            {
                message = string.Format("El contenedor {0}, ya está registrado en el booking {1}. \nSi aún desea usar el contenedor primero proceda a anular el AISV",unit, booking);
                return false;
            }
            message = string.Empty;
            return !t.Value;
        }
        //esto es para cerrar form.
        public static string ExitForm(string mensaje)
        {
            return string.Format(@"<script type='text/javascript'>  alert('{0}');   window.returnValue = true; window.close();</script>",mensaje);
        }
        //obtener el shiper name
        public static string getShiperName(string shiperID)
        {
            return dataServiceHelper.getShipName(shiperID);
        }
        //obtener la lista de hzr
        public static HashSet<Tuple<string,string>> GetHzList(int  hazid)
        {
            return dataServiceHelper.GetHazards(hazid);
        }

        //return Impuestos
        public static List<Tuple<string, string,decimal>> getImpuestos()
        {
            var xlista = new List<Tuple<string, string, decimal>>();
            foreach (var i in services.dataServiceHelper.RetencionImpuestos())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2,i.Item3));
            }
           
            return xlista;
        }
        
    }

    public class bkitem
    {
        public string number { get; set; }
        public string linea { get; set; }
        public string referencia { get; set; }
        public string gkey { get; set; }
        public string pod { get; set; }
        public string pod1 { get; set; }
        public string shiperID { get; set; }
        public string temp { get; set; }
        public string fkind { get; set; }
        public string imo { get; set; }
        public string refer { get; set; }
        public string dispone { get; set; }
        public string iso { get; set; }
        public string cutOff { get; set; }
        public string hume { get; set; }
        public string vent_pc { get; set; }
        public string ventU { get; set; }

    }
}