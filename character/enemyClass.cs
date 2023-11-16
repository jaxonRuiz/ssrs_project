using CharacterOperations;
using MiscOperations;
using System;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

namespace enemyOperations
{
    public partial class Base_Enemy
    {
        public string? name;
        public int maxHealth;
        public int live_health;
        public int def;
        public int atk;
        public int spd;

    }
}
