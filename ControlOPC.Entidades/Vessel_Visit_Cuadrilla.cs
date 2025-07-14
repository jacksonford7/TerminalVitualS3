using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    class Vessel_Visit_Cuadrilla
    {
        public Vessel_Visit_Cuadrilla()
        {

        }

        public Vessel_Visit_Cuadrilla(DateTime? _sdate, DateTime? _edate, int? _id)
        {
            this.endDate = _edate;
            this.startDate = _sdate;
            this.id = _id;

        }

        public DateTime? startDate { get; set; }

        public DateTime? endDate { get; set; }

        public int? id { get; set; }

        public int? Save()
        {


            return null;
        }




    }
    
}
