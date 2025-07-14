using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace ClsOrdenesP2D
{
    public class meta : ToStringBase
    {
        [JsonProperty] public string imo_clasification { get; set; }
    }
}
