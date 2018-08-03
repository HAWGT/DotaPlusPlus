﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dota2GSI.Nodes
{
    /// <summary>
    /// Class representing hero abilities
    /// </summary>
    public class Abilities : Node, IEnumerable<Ability>
    {
        private List<Ability> abilities = new List<Ability>();

        /// <summary>
        /// The attributes a hero has to spend on abilities
        /// </summary>
        public readonly Attributes Attributes;

        private string json;

        /// <summary>
        /// The number of abilities
        /// </summary>
        public int Count { get { return abilities.Count; } }

        internal Abilities(string json_data) : base(json_data)
        {
            json = json_data;

            List<string> abilities = _ParsedData.Properties().Select(p => p.Name).ToList();
            foreach (string ability_slot in abilities)
            {
                if (ability_slot.Equals("attributes"))
                    Attributes = new Attributes(_ParsedData[ability_slot].ToString());
                else
                    this.abilities.Add(new Ability(_ParsedData[ability_slot].ToString()));
            }
        }

        /// <summary>
        /// Gets the ability at a specified index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns></returns>
        public Ability this[int index]
        {
            get
            {
                if (index > abilities.Count - 1)
                    return new Ability("");

                return abilities[index];
            }
        }

        /// <summary>
        /// Gets the IEnumerable of Abilities
        /// </summary>
        public IEnumerator<Ability> GetEnumerator()
        {
            return abilities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return abilities.GetEnumerator();
        }

        /// <summary>
        /// Returns the json string that generated this Abilities instance
        /// </summary>
        /// <returns>Json string</returns>
        public override string ToString()
        {
            return json;
        }
    }
}
