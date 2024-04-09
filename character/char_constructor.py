import json
import os

def makeCharacter():
    character = {}
    character['name'] = str(input("name: "))
    if character['name'][-1] == "\r":
        character['name'] = character["name"][:-1]
    character['hp'] = int(input("hp: "))
    character['atk'] = int(input("atk: "))
    character['def'] = int(input("def: "))
    character['spd'] = int(input("spd: "))
    character['max-energy'] = int(input("energy: "))
    character['type'] = str(input("type: "))
    if character['type'][-1] == "\r":
        character['type'] = character["type"][:-1]
    character['light_cone'] = input("light cone: ")
    if character['light_cone'][-1] == "\r":
        character['light_cone'] = character["light_cone"][:-1]
    character['equiptment'] = []
    return character

def makeEquiptment():
    equiptment = {}
    e_stats = {}

    # valid inputs
    stat_type = ["flat", "percent", "misc", "elemental"]
    stat_set = ["hp", "atk", "def", "spd"]
    misc_stats = ["crt_rate", "crt_dmg", "effect_hit_rate", "effect_resistance", "energy_regen_rate", "break_effect"]
    valid_types = ['helmet', 'boots', 'chest', 'hands', 'rope', 'orb']

    # setting equiptment basics
    while True:
        response = str(input("type: "))
        if response in valid_types:
            equiptment["type"] = response
            break
        else:
            print("invalid equiptment type")
            print(valid_types)
    equiptment["set"] = str(input("set: "))
    
    
    #oh my god i think i hate python now lol
    # scuffy looped stat input
    # running step is going to be acting as a 3 way boolean
    #   0 asks for flat, percent, etc
    #   1 enters specified stat
    #   2 checks for continue
    #   -1 is end condition
    print("=====stats=====")
    running_step = 0
    while(running_step >= 0): 
        # begin of cycle
        for i in stat_type:
            print(f"{stat_type.index(i)}: [{i}]")
        response = input("stat type: ")

        # setting flat, percent, misc, elemental type
        if running_step == 0:
            if response in stat_type:
                response = stat_type.index(response)
            if int(response) in range(len(stat_type)):
                running_step = 1
            else:
                print("incorrect input")
            # else goes to ask response again

        # filter into flat/percent stats, or misc/elemental
        if running_step == 1:
            if int(response) == 0: # flat stat input
                # printing stat options
                print()
                for i in stat_set:
                    print(f"{stat_set.index(i)}: [{i}]")

                # accounting for stirng inputs
                base_stat = input("stat: ")
                if base_stat in stat_set:
                    base_stat = stat_set.index(base_stat)
                base_stat = int(base_stat)

                # reading stat type and entering value
                if int(base_stat) in range(len(stat_set)):
                    e_stats["_" + stat_set[base_stat]] = int(input("value: "))
                    running_step = 2
                elif base_stat in stat_set:
                    e_stats["%" + base_stat] = int(input("value: "))
                    running_step = 2
                else:
                    print("wrong stat input")
            elif int(response) == 1: # percent stat input
                # printing stat options
                print()
                for i in stat_set:
                    print(f"{stat_set.index(i)}: [{i}]")

                # accounting for string input
                base_stat = input("stat: ")
                if base_stat in misc_stats:
                    base_stat = misc_stats.index(base_stat)
                base_stat = int(base_stat)

                #reading stat type and entering value
                if int(base_stat) in range(len(stat_set)):
                    print("\nenter value as a percent, without the percent sign")
                    e_stats["%" + stat_set[base_stat]] = float(input("value: ")) / 100
                    running_step = 2
                elif base_stat in stat_set:
                    e_stats["%" + base_stat] = float(input("value: ")) / 100
                    running_step = 2
                else:
                    print("wrong stat input")

            elif int(response) == 2: # misc stat input
                # printing stat options
                print()
                for i in misc_stats:
                    print(f"{misc_stats.index(i)}: [{i}]")

                # accounting for string inputs
                base_stat = input("stat: ")
                if base_stat in misc_stats:
                    base_stat = misc_stats.index(base_stat)
                base_stat = int(base_stat)

                # reading (misc) stat type and entering value
                if int(base_stat) in range(len(misc_stats)):
                    print("\nenter value as a percent, without the percent sign")
                    e_stats["~" + misc_stats[base_stat]] = float(input("value: ")) / 100
                    running_step = 2
                else:
                    print("wrong stat input")

            elif int(response) == 3: # elemental damage stat 
                types = ["physical", "fire", "ice", "lightning", "wind", "quantum", "imaginary"]
                print(types)

                # reading type and entering value
                type_mult = input("type: ")
                if type_mult in types:
                    print("\nenter value as a percent, without the percent sign")
                    e_stats["!" + type_mult + "_dmg"] = float(input("value: ")) / 100

            else: # error flagger
                Exception("Error in response reading")
        
        # asking to continue loop
        if running_step == 2:
            cont = input("Continue? [y/n] ")
            if cont.lower() == "y" or cont.lower() == "yes":
                running_step = 0
            else:
                # ending loop
                print("ending here")
                running_step = -1

    # adding stats to equiptment and returning
    equiptment["stats"] = e_stats
    return equiptment
    
def createCharFile(filename:str = "NONE"):
    output = {"Characters": [], "Loose_equiptment": []}

    print("making characters..")
    while True:
        char = makeCharacter()
        print("\ncharacter created!")

        print("adding equiptment...")
        while True:
            if input("continue making equiptment? [y/n]").lower() != "y":
                print("No more equiptment")
                break
            if len(char["equiptment"]) > 6:
                break
            char["equiptment"].append(makeEquiptment())
        output["Characters"].append(char)
        if input("continue creating characters? [y/n]").lower() != "y":
            print("no more characters")
            break


    print("making loose equiptment..")
    while True:
        if input("create loose equiptment? [y/n]").lower() != "y":
            print("no loose equiptment")
            break
        output["Loose_equiptment"].append(makeEquiptment())
        print("\nequiptment created!")
    
    json_object = json.dumps(output, indent=4)
    # saving to file if name given
    if filename != "NONE":
        fullpath = os.getcwd() + "/saved_jsons/" + filename + ".json"
        with open(fullpath, "w") as outfile:
            outfile.write(json_object)

    return json_object

# print(raw["Characters"][0])
if __name__ == "__main__":
    '''path = "saved_jsons\\emptycharacter.json"
    print(path)
    with open(path, "w") as f:
        raw = json.load(f)

    print(type(raw))
    print(raw)
'''

    # run this for shortcut
    # python3 char_constructor.py < shortcut.txt
    file = createCharFile("MainTeam")
    print(file)