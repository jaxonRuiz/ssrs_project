using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterOperations;
using MiscOperations;

namespace environment
{
    class interface_v01
    {
        private struct TurnValue
        {
            public int turn_value { get; set; }
            public Base_Character character { get; set; }
            public TurnValue(Base_Character character, int turn_value)
            {
                this.character = character; //!!make sure takes pointer to base_character, not a new character
                this.turn_value = turn_value;
            }
        }
        public Team team_1;
        public Team team_2;
        public interface_v01(Team teamA, Team teamB)
        {
            team_1 = teamA;
            team_2 = teamB;
        }
        public int Run()
        {
            List<TurnValue> queue = calculate_turn_order();

            while (team_1.is_alive() && team_2.is_alive())
            {
                //queue[0]. DO STUFF
                //remove top off queue
                //calculate turn order of character, insert to list

            }
            return 0;
        }
        private List<TurnValue> calculate_turn_order()
        {
            List<TurnValue> order = new List<TurnValue>();
            for (int i = 0; i < team_1.Length() + team_2.Length(); i++)
            {
                //scuffy way of doing both lists
                if (i < team_1.Length())
                {
                    order[i] = new TurnValue(team_1.roster[i], 0); //replace 0 with turn value num
                    //probably do insertion here 
                    //iterate through current List to find appropriate spot for value here
                } 
                else
                {
                    order[i] = new TurnValue(team_1.roster[i - team_1.Length()], 0); //replace 0 with turn value num
                    //and insertion here
                }
                //need to make sure indexing is right here ^ (i think it is?)
            }
            
            return order;

        }

        

    }

    class Team
    {
        public Base_Character[]? roster;

        public bool is_alive()
        {
            foreach (Base_Character character in roster)
            {
                if (!character.alive) return false;
            }
            return true;
        }
        public int Length()
        {
            return roster.Length;
        }
    }
}
