using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace character_operations
{


    public partial class LoadCharJson
    {
        [JsonProperty("Characters")]
        public Base_Character[]? Characters { get; set; }

        [JsonProperty("Loose_equiptment")]
        public Equiptment[]? LooseEquiptment { get; set; }
    }
    /*
    public partial class Character
    {
        [JsonProperty("temp_char", NullValueHandling = NullValueHandling.Ignore)]
        public Base_Character TempChar { get; set; }

        [JsonProperty("temp_char2", NullValueHandling = NullValueHandling.Ignore)]
        public Base_Character TempChar2 { get; set; }
    }*/

    public partial class Base_Character
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("hp")]
        public int Hp { get; set; }

        [JsonProperty("atk")]
        public int Atk { get; set; }

        [JsonProperty("def")]
        public int Def { get; set; }

        [JsonProperty("spd")]
        public int Spd { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("light_cone")]
        public string? LightCone { get; set; }

        [JsonProperty("equiptment")]
        public Equiptment[]? Equiptment { get; set; }
    }

    public partial class Equiptment
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("set")]
        public string? Set { get; set; }

        [JsonProperty("stats")]
        public Dictionary<string, double>[]? Stats { get; set; }
    }
}
