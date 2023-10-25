// See https://aka.ms/new-console-template for more information
using character_operations;
using System;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Run
{
    static void Main(string[] args)
    {
        Console.WriteLine("hello world");
        string file_name = Path.Combine(Directory.GetCurrentDirectory(), "load_char.json");
        string json = File.ReadAllText(file_name);
        Console.WriteLine(json);

        //more of a general unpacker at this point
        LoadCharJson data = JsonConvert.DeserializeObject<LoadCharJson>(json);
        //should make sure it doesn't all need to be in the same json. 

        Base_Character test = data.Characters[0]; //effectively set pointer to character[0], in variable test
                                                  //Base_Character special = new Special_Character();

        Console.WriteLine("\nChanging name...:");
        test.Name = "sadman"; //trying to change name
        test.hello(); //getting name of copy
        data.Characters[0].hello(); //original has been changed
        Console.WriteLine("alls good part 1\n");

        Console.WriteLine("printing all characters");
        foreach (Base_Character character in data.Characters)
        {
            Console.WriteLine(character.Name);
        }

        Console.WriteLine("\ntesting special characters");
        Special_Character spec = new Special_Character(test);
        spec.iam();
        spec.hello();
    }
}

namespace character_operations
{ 
    // Frame of special characters set up here
    public class Special_Character : Base_Character
    {
        public Special_Character(Base_Character dude) 
        {
            Hp = dude.Hp;
            Name = dude.Name;
        }
        public void iam()
        {
            Console.Write("Hello, I am special, I am: ");
            Console.WriteLine(Name);
        }
    }
    public partial class Base_Character
    {
        //maybe dont need this section (?)
        private int flat_hp = 0;
        private int flat_atk = 0;
        private int flat_def = 0;
        private int flat_spd = 0;
        private int percent_hp = 1;
        private int percent_atk = 1;
        private int percent_def = 1;
        private int percent_spd = 1;
        //maybe dont need this section ^

        private float crt_rate = 0.05f;
        private float crt_dmg = 1.5f;
        private float effect_hit_rate = 1;
        private float energy_regen_rate = 1;
        private float break_effect = 1;
        private float[] type_dmg = new float[7];
        private float[] type_res = new float[7]; //both types 7 long
        public void hello()
        {
            Console.Write("Hello World, I am ");
            Console.WriteLine(Name);
        }
    }
}
