using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.ComponentModel.DataAnnotations;

namespace NotPokemon
{
    public class Monster
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //3 types: fire, water, plant
        //works like rock paper scissor
        public char Type { get; set; }

        //Health Points
        public int HP { get; set; }

        //Magic Points
        public int MP { get; set; }

        //How strong is a normal attack?
        public int Attack { get; set; }

        //Can this monster be the player's monster?
        public bool CanBePlayer { get; set; }

        //Can this monster be the enemy's monster?
        public bool CanBeEnemy { get; set; }
        
    }
}
