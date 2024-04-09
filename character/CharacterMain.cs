// See https://aka.ms/new-console-template for more information
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
using System.Reflection;

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
        //tester.test_names01();
        tester.print_characters();
        //tester.test_special_character_frame();
        //tester.test_setup();

        tester.test_details();

        Console.WriteLine();
        tester.test_attack();

    }
}
class Tester
{
    public LoadCharJson data;
    public Base_Character character;
    public void start()
    {
        // opening saved character files (scuffy rn)
        string workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        string file_name = Path.Combine(projectDirectory, "//saved_jsons//just_yanqing.json");
        string temppathfuckmylife = "C:\\Users\\bluea\\Documents\\Coding\\RealStuff\\FullProjects\\ssrs_project\\character\\saved_jsons\\just_yanqing.json";
        Console.WriteLine(temppathfuckmylife);
        string json = File.ReadAllText(temppathfuckmylife);
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
    public void test_details()
    {
        character.show_details();
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
    public void test_attack()
    {
        DmgObj damage = character.basic_attack();
        Console.WriteLine("{0} did {1} damage of {2} type!", character.Name, damage.base_dmg, character.Type);
        Console.WriteLine("{0} missed and hit themselves!", character.Name);
        character.get_hit(damage);
        Console.WriteLine("{0} now has {1} health",character.Name, character.current_hp());
    }
}