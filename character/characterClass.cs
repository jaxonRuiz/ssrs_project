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


//for all operations pertaining to characters
namespace CharacterOperations
{

    /*
    internal enum Types
    {
        physical,
        fire,
        ice,
        lightning,
        wind,
        quantum,
        imaginary
    }*/

    // Frame of special characters set up here 
    public class Special_Character : Base_Character
    {
        public Special_Character(Base_Character dude)
        {
            base_hp = dude.base_hp;
            Name = dude.Name;
        }
        public void iam()
        {
            Console.Write("Hello, I am special, I am: ");
            Console.WriteLine(Name);
        }
        public override DmgObj basic_attack(double scaler = 1)
        {
            Console.WriteLine("overrode basic attack!");
            return new DmgObj();
        }
    }

    //base character model, which characters will be built off of
    //should establish some rules to make sure special characters dont get too difficult
    //according ot base character model, all characters will only have :
    //  basic attack, skill attack, ult, calculate_turn_order, extra_action, 
    //  as actions (plus equiptment stuffs)
    //  all fancy things should be done INTERNALLY
    public partial class Base_Character
    {
        List<string> Types = new List<string>() {
        "physical",
        "fire",
        "ice",
        "lightning",
        "wind",
        "quantum",
        "imaginary" };
        readonly Random rng = new Random();
        public int current_atk()
        { return (int)Math.Round(base_atk * multiplier_stats["atk"]) + flat_stats["atk"]; }
        public int current_def()
        { return (int)Math.Round(base_def * multiplier_stats["def"]) + flat_stats["def"]; }
        public int current_spd()
        { return (int)Math.Round(base_spd * multiplier_stats["spd"]) + flat_stats["spd"]; }

        /// <summary>
        /// gets max health
        /// </summary>
        /// <returns>max health as int</returns>
        public int max_hp()
        { return (int)Math.Round(base_hp * multiplier_stats["hp"]) + flat_stats["hp"]; }

        public int current_hp()
        { return live_hp; }

        public void show_details()
        {
            Console.WriteLine();
            Console.WriteLine("==Total Stats==");
            Console.Write("hp:");
            Console.Write(current_hp());
            Console.Write(" / ");
            Console.WriteLine(max_hp());
            Console.Write("atk:");
            Console.WriteLine(current_atk());
            Console.Write("def:");
            Console.WriteLine(current_def());
            Console.Write("spd:");
            Console.WriteLine(current_spd());
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("==Base Stats==");
            Console.Write("hp:");
            Console.WriteLine(base_hp);
            Console.Write("atk:");
            Console.WriteLine(base_atk);
            Console.Write("def:");
            Console.WriteLine(base_def);
            Console.Write("spd:");
            Console.WriteLine(base_spd);
            Console.WriteLine();

            Console.WriteLine("==flat stats==");
            foreach (string stat in flat_stats.Keys)
            {
                Console.Write(stat + ": ");
                Console.Write(flat_stats[stat]);
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("==multiplier stats==");
            foreach (string stat in multiplier_stats.Keys)
            {
                Console.Write(stat + ": ");
                Console.Write(multiplier_stats[stat] * 100 - 100);
                Console.WriteLine("%");
            }
            Console.WriteLine();
            Console.WriteLine("==extra stats==");
            foreach (string stat in extra_stats.Keys)
            {
                if (stat.Equals("crt_rate"))
                {
                    Console.Write(stat + ": ");
                    Console.Write(extra_stats[stat] * 100);
                    Console.WriteLine("%");
                }
                else
                {
                    Console.Write(stat + ": ");
                    Console.Write(extra_stats[stat] * 100 - 100);
                    Console.WriteLine("%");
                }
            }
        }

        public Base_Effect[]? effects;
        //flat stat increases, mostly from equiptment or buffs
        private Dictionary<string, int> flat_stats = new Dictionary<string, int>()
        { //probably dont need to keep as ints to save bytes of space right?
            {"hp", 0},
            {"atk", 0},
            {"def", 0},
            {"spd", 0}
        };
        //multipliers stats, mostly from equpiptment or buffs
        private Dictionary<string, double> multiplier_stats = new Dictionary<string, double>()
        {
            {"hp", 1f},
            {"atk", 1f},
            {"def", 1f},
            {"spd", 1f}
        };
        //all misc stats from equiptment
        private Dictionary<string, double> extra_stats = new Dictionary<string, double>()
        {
            {"crt_rate", 0.05f},
            {"crt_dmg", 1f},
            {"effect_hit_rate", 1f},
            {"effect_resistance", 1f },
            {"energy_regen_rate", 1f},
            {"break_effect", 1f}
        };


        public int live_hp = 0;
        public bool alive = false;
        private double[] type_dmg = { 1f, 1f, 1f, 1f, 1f, 1f, 1f}; //based on enumeration order above 
        private double[] type_res = { 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f };
        private double get_type_dmg(string t)
        {
            
            return type_dmg[Types.IndexOf(t)];
        }
        private double get_type_res(string t)
        {
            return type_res[Types.IndexOf(t)];
        }
        public void hello()
        {
            Console.Write("Hello World, I am ");
            Console.WriteLine(Name);
        }

        //initializing character to:
        //  load all equiptment stats into own stats
        private void LoadEquiptment()
        {
            foreach (Equiptment item in this.Equiptment)
            {
                //taking each set of stats in equiptment (weird thing here around loading character stats as an array of single dictionaries

                //taking each stat in the above set, and putting into internal dictionaries, based on flags
                foreach (String label in item.Stats.Keys)
                {
                    if (label[0] == '_') //flat stat flag
                    {
                        flat_stats[label.Substring(1)] += (int)item.Stats[label];
                    }
                    else if (label[0] == '%') //percent multiplier stat flag
                    {
                        multiplier_stats[label.Substring(1)] += item.Stats[label];
                    }
                    else if (label[0] == '~') //extra stats flag
                    {
                        extra_stats[label.Substring(1)] += item.Stats[label];
                    }
                    else if (label[0] == '!')//element damage boost (fml) 
                    {
                        type_dmg[Types.IndexOf(Type)] += item.Stats[label];
                    }
                    else
                    {
                        Console.Error.WriteLine("Unexpected stat prefix in equiptment: {0}\nOf character: {1}", label, this.Name);
                    }
                }
            }
        }
        public void setup()
        {
            
            //resetting all initial stats, in case of resetting character
            foreach (string key in flat_stats.Keys)
            {
                flat_stats[key] = 0;
            }
            foreach (string key in multiplier_stats.Keys)
            {
                multiplier_stats[key] = 1f;
            }
            extra_stats["crt_rate"] = 0.05f;
            extra_stats["crt_dmg"] = 1.5f;
            extra_stats["effect_hit_rate"] = 1f;
            extra_stats["energy_regen_rate"] = 1f;
            extra_stats["break_effect"] = 1f;


            //loading all equiptment stats
            //taking each equiptment on character
            LoadEquiptment();

            //resetting character health and status.
            alive = true;
            live_hp = max_hp();
        }
        //prob use to apply crit to an attack after, with a way to toggle true_crit for testing purposes
        /// <summary>
        /// use to apply crit damage to an attack. 
        /// </summary>
        /// <param name="true_crit">possible for misses or not</param>
        /// <returns></returns>
        public int apply_crit(int dmg, bool true_crit = true)
        {
            //true true crit has chance to mise
            if (true_crit)
            {
                int n = rng.Next(0, 100);
                int r = (int)extra_stats["crt_rate"] * 100;

                if (n > r) return dmg; //crit missed
                //crit!
                return (int)(dmg * extra_stats["crt_dmg"]);
            }
            //false crit just give a multiplier
            else return (int)(dmg * (extra_stats["crt_rate"] * extra_stats["crt_dmg"] + 1)); //idk if this is right for calculations...
        }

        public void read_effects()
        {
            //apply all effects as needed.
        }
        /// <summary>
        /// basic attack action
        /// </summary>
        /// <param name="scaler">percentage attack scales by. assume %100 by default</param>
        /// <returns></returns>
        public virtual DmgObj basic_attack(double scaler = 1)
        {
            int base_damage = (int)scaler * current_atk() + flat_stats["atk"];
            DmgObj hit = new DmgObj();
            hit.type = Type;
            hit.base_dmg = base_damage;
            hit.type_pen = get_type_dmg(Type);
            hit.attacker_level = Level;
            return hit;
        }

        private double def_calculation(DmgObj hit)
        {
            double DEF = base_def * (1 + multiplier_stats["def"] - hit.armor_pen) + flat_stats["def"];
            if (DEF < 0) DEF = 0;
            return 1 - (DEF / (DEF + 200 + (10 * hit.attacker_level))); //LEVEL STUFF IS WRONG HERE FIX LATER
        }
        private double res_calculation(DmgObj hit)
        {
            double RES = 1 - (get_type_res(hit.type) - hit.type_pen);
            if (RES > 2) RES = 2;
            if (RES < 0.1) RES = 0.1;
            return RES;
        }

        /// <summary>
        /// for when a character is hit by an attack.
        /// returns health after attack
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        public int get_hit(DmgObj hit)
        {
            double incoming_damage = hit.base_dmg;
            double def_mult = def_calculation(hit);
            double res_mult = res_calculation(hit);
            int dmg = (int)Math.Round(incoming_damage * def_mult * res_mult);
            live_hp -= dmg;
            if (live_hp < 0)
            {
                live_hp = 0;
                die();
            }
            return dmg;
            //make sure to remove/deconstruct DmgObj after use
        }
    
        public void die()
        {
            alive = false;
            foreach (Base_Effect effect in effects)
            {
                effect.end_effect();
            }
            Console.WriteLine("Character is dead now.");
        }
    }
}

