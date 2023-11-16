using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiscOperations
{
    public struct DmgObj
    {
        public int base_dmg { get; set; }
        public string type { get; set; }
        public double type_pen { get; set; }
        public double armor_pen { get; set; }
        public int attacker_level { get; set; }
        public DmgObj(int damage, string type, double pen, int level)
        {
            base_dmg = damage;
            this.type = type;
            type_pen = pen;
            armor_pen = 0;
            attacker_level = level;
        }

        //damage will work as a projectile (sort of)
        //create obj in attack, return ->
        //-> read obj in target 
        // calculate res/def, apply hp damage
        //deconstruct (i think its done automatically?)
    }
}
