using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Shared.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Picture { get; set; }                
        public List<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();        
        [NotMapped]
        public string Position { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Person p2)
            {
                return Id == p2.Id;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
