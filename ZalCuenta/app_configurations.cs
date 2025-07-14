using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLHandler;

namespace ZalCuenta
{
    class app_configurations
    {
       private static sql_handler pointer = null;

        private void initialize()
        {
            pointer = sql_handler.GetInstance();
            configurations = new List<app_configuration>();
            string error; //hubo una novedad al cargar configuraciones
            configurations = pointer.ExecuteSelectControl<app_configuration>(pointer.basic_con, 2000, "PC_C_App_Configurations", null, out error);
        }

        //Initialize static members
        private static void OnInit()
        {

            pointer = (pointer == null) ? SQLHandler.sql_handler.GetInstance() : pointer;
            configurations = new List<app_configuration>();
            string error; //hubo una novedad al cargar configuraciones
            configurations = pointer.ExecuteSelectControl<app_configuration>(pointer.basic_con, 2000, "PC_C_App_Configurations", null, out error);
        }

        /// <summary>
        /// List of all app congfigurarions by instance.
        /// </summary>
        private static List<app_configuration> configurations { get; set; }
       
        #region "Singleton"
        private static app_configurations instance = null;
        public static app_configurations GetInstance()
        {
            if (instance == null)
                instance = new app_configurations();
            return instance;
        }
        private app_configurations()
        {
            initialize();
        }
        #endregion

        /// <summary>
        /// Get a config for app
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static app_configuration get_configuration(string name)
        {
            OnInit();
            return configurations?.Where(s => s.name.Equals(name)).FirstOrDefault();
            //si no hay esta nulo
    
        }


        public static string ObjectIsNull(object objeto, string ObjectName) 
        {
            return objeto == null ? string.Format("El campo {0} no acepta valores nulos",ObjectName) : null;
        }
        public static string CheckString(string cadena, int min, int max, string campo)
        {
            if (string.IsNullOrEmpty(cadena)) { return string.Format("El valor del campo {0} no puede estar vacío",campo); }
            if (cadena.Length < min || cadena.Length > max) { return string.Format("El valor del campo {0} debe tener entre {1} y {2} caracteres", campo, min, max); }
            return string.Empty;
        }
    }
}
