using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite.app_start
{
    public class unit_depot
    {
        public string cntr { get; set; }
        public string marca { get; set; }
        public DateTime? cas { get; set; }
        public string categoria { get; set; }
        public Int64 gkey { get; set; }
        public unit_depot() { }

        public unit_depot(string _cntr, string _marca    , string _categoria, DateTime? _cas, Int64 _gkey) {
            this.cntr = _cntr;
            this.categoria = _categoria;
            this.gkey = _gkey;
            this.marca = _marca;
            this.cas = _cas;

        }

    }
}