using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiscOperations
{
    //quick temp class for stat objects, instead of moving tuples around
    //may want to reorganize later with other structs
    /// <summary>
    /// structure to hold a stat's (string)identifier
    /// and its value
    /// </summary>
    public struct StatObj
    {
        public string stat { get; set; }
        public double value { get; set; }
        public StatObj(string stat, int value)
        {
            this.stat = stat;
            this.value = value;
        }
    }
    public class Base_Effect
    {
        /*intended to constitute an object so that all 
         * effects can be held in a list on characters,
         * applying each one according to conditions.
         * will return a grouping of stat and amount 
         */
        public StatObj stat;
        public bool condition = false;
        public Base_Effect(string stat, double value)
        {
            this.stat.stat = stat; //haha im so good at naming convenctions 
            this.stat.value = value;
        }
        public void end_effect()
        {
            condition = false;
        }
        public virtual StatObj apply()
        {
            //effect is active
            if (condition)
            {
                return stat;
            } else //if condition is not met
            {
                return new StatObj("skip",0);
            }
        }
    }
    public class Timed_Effect: Base_Effect
    {
        private int duration;
        public Timed_Effect(string stat, double value,int duration) : base(stat,value)
        {
            this.duration = duration;
        }
        public override StatObj apply()
        {
            if (duration > 0)
            {
                return stat;
            }
            else //if condition is not met
            {
                return new StatObj("skip", 0);
            } 
        }
        //ticks timer. ONLY USE AT END OF TURN, AS EXTRA ACTIONS DONT COUNT AS A NEW TURN
        public void tick_duration()
        {
            if (duration > 0) duration -= 1;
        }
    }
}
