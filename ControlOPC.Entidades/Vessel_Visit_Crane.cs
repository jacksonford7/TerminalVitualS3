using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    class Vessel_Visit_Crane
    {
        public Vessel_Visit_Crane()
        {

        }

        public DateTime? startDate { get; set; }

        public DateTime? endDate { get; set; }

        public int? id { get; set; }

        public Vessel_Visit_Crane(DateTime? _sdate, DateTime? _edate, int? _id)
        {
            this.endDate = _edate;
            this.startDate = _sdate;
            this.id = _id;
        }

        /// <summary>
        /// Return null if no controled exception
        /// </summary>
        /// <returns></returns>
        public int? Save()
        {
            //save to database for trasnsaction scope.

            return null;
        }
    }
}
