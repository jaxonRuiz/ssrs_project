// See https://aka.ms/new-console-template for more information
using CharacterOperations;
using EffectComponents;
using System;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

/* Scuffy Star Rail Calculator
 * Author: Jaxon Ruiz
 * this is a scuffy calculator i'm hoping to make for the video game Honkai: Star Rail
 * in its final version, it should be able to load characters and their equiptment (done)
 * simulate character actions (WIP)
 * simulate enemy interactions (WIP)
 * simulate turn orders and incorperate speed values (TODO)
 * 
 * This is mostly a small personal project to try to get me to make an actual, functional
 * piece of software. Hello anyone that happens to be looking :D
 */

//just using this class to immediately run tests 
class Run
{
    static void Main(string[] args)
    {/*
        string file_name = Path.Combine(Directory.GetCurrentDirectory(), "load_char.json");
        string json = File.ReadAllText(file_name);
        Console.WriteLine(json);

        //more of a general unpacker at this point
        LoadCharJson data = JsonConvert.DeserializeObject<LoadCharJson>(json);
        //should make sure it doesn't all need to be in the same json. */

        Tester tester = new Tester();
        tester.start();

        tester.test_names01();
        tester.print_characters();
        tester.test_special_character_frame();

        tester.test_setup();

    }
}
class Tester
{
    public LoadCharJson data;
    public Base_Character character;
    public void start()
    {
        string file_name = Path.Combine(Directory.GetCurrentDirectory(), "load_char.json");
        string json = File.ReadAllText(file_name);
        Console.WriteLine(json);
        data = JsonConvert.DeserializeObject<LoadCharJson>(json);
        
        
        character = data.Characters[0]; 
        //effectively set pointer to character[0], in variable test
        //Base_Character special = new Special_Character();
    }
    public void test_names01()
    {
        

        Console.WriteLine("\nChanging name...:");
        character.Name = "sadman"; //trying to change name
        character.hello(); //getting name of copy
        data.Characters[0].hello(); //original has been changed
        Console.WriteLine("alls good part 1\n");
    }
    public void print_characters()
    {
        Console.WriteLine("printing all characters");
        foreach (Base_Character character in data.Characters)
        {
            Console.WriteLine(character.Name);
        }
    }
    public void test_special_character_frame()
    {
        Console.WriteLine("\ntesting special characters");
        Special_Character spec = new Special_Character(character);
        spec.Name = "bobby the special one";
        spec.iam(); //own sp functions works
        spec.basic_attack(scaler:2);
        spec.hello(); //inhitance works
        data.Characters[0].hello();


        //check that child class can be held in list of parents
        Base_Character[] tempList = new Base_Character[] { spec, character };
        foreach (Base_Character c in tempList)
        {
            c.hello();
        }
    }
    public void test_setup()
    {
        character.setup();
        Console.WriteLine(character.current_atk());
    }
}
//for all operations pertaining to characters
namespace CharacterOperations 
{
    public struct DmgObj
    {
        public int base_dmg { get; set; }
        public string type { get; set; }
        public double type_pen { get; set; }

    }
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
        { return (int)((int)Math.Round(base_atk * multiplier_stats["atk"]) + flat_stats["atk"]); }
        
        /// <summary>
        /// gets max health
        /// </summary>
        /// <returns>max health as int</returns>
        public int max_hp()
        { return (int)((int)Math.Round(base_hp * multiplier_stats["hp"]) + flat_stats["hp"]); }

        public int Current_hp()
        { return live_hp; }

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
            {"energy_regen_rate", 1f},
            {"break_effect", 1f}
        };


        public int live_hp = 0;
        public bool alive = false;
        private double[] type_dmg = new double[7]; //based on enumeration order above 
        private double[] type_res = new double[7]; 
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
                    else
                    {
                        Console.Error.WriteLine("Unexpected stat prefix in equiptment: {0}\nOf character: {1}", label, this.Name);
                    }
                }
            }
        }
        public void setup()
        {
            // WORK OUT TYPES LATER DRUNK JAXON GOING TO PLAY GAMES NOW
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
            extra_stats["crt_dmg"] = 1f;
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
        public int apply_crit(int dmg, bool true_crit=false)
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
        public virtual DmgObj basic_attack(double scaler=1)
        {
            int base_damage = (int)scaler * current_atk() + flat_stats["atk"];
            DmgObj hit = new DmgObj();
            hit.type = Type;
            hit.base_dmg = base_damage;
            hit.type_pen = type_dmg[Types.IndexOf(Type)];
            return hit;
        }
        public int get_hit(int dmg, string type)
        {
            return 0;
            //make sure to remove/deconstruct DmgObj after use
        }
    }
}
