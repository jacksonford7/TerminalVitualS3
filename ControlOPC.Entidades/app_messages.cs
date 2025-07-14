using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    class app_messages :Base
    {
        /// <summary>
        /// List of all app messages for screen.
        /// </summary>
        private static List<app_message> messages { get; set; }
        #region "Singleton"
        private static app_messages instance = null;
        public static app_messages GetInstance()
        {
            if (instance == null)
                instance = new app_messages();
            return instance;
        }
        private app_messages()
        {
            initialize();
        }
        
        //FOR STATIC METHODS
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            messages = new List<app_message>();
            string error; //hubo una novedad al cargar configuraciones
            messages = sql_pointer.ExecuteSelectControl<app_message>(sql_pointer.basic_con, 2000, "PC_C_Tb_Msg_Error", null, out error);
        }

        #endregion
        //FOR INSTANCE METHODS
        private void initialize()
        {
            base.init();
            messages = new List<app_message>();
            string error; //hubo una novedad al cargar configuraciones
            messages = sql_pointer.ExecuteSelectControl<app_message>(sql_pointer.basic_con, 2000, "PC_C_Tb_Msg_Error", null, out error);
        }
        /// <summary>
        /// Get a print message for keyword
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static app_message get_message(string name)
        {
            OnInit();
            return messages?.Where(s => s.keyword.Equals(name)).FirstOrDefault();
            //si no hay esta nulo
        }
    }
}
