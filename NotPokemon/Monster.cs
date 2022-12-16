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

        public char Type { get; set; }

        public int HP { get; set; }

        public int MP { get; set; }

        public int Attack { get; set; }

        public bool CanBePlayer { get; set; }

        public bool CanBeEnemy { get; set; }
        
    }
}
