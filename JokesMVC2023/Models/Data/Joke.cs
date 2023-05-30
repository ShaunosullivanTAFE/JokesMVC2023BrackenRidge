using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JokesMVC2023.Models.Data
{
    public class Joke
    {
        public int Id { get; set; }

        [Display(Name = "Joke Question")]
        [Required]
        [StringLength(50)]
        public string JokeQuestion { get; set; }

        [Display(Name = "Joke Answer")]
        public string JokeAnswer { get; set; }

        // Nav Property
        public ICollection<FavouriteListItem> FavouriteListItems { get; set; }

    }

    public class AppUser
    {
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Nav Property
        public ICollection<FavouriteList> FavoriteLists { get; set;}
    }

    public class FavouriteList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        // FK
        public int UserId { get; set; }

        // Nav Property
        public AppUser User { get; set; }
        public ICollection<FavouriteListItem> FavouriteListItems { get; set; }
    }

    public class FavouriteListItem
    {
        public int Id { get; set; }

        // FK
        public int JokeId { get; set; }
        public int FavouriteListId { get; set; }

        // Nav Property
        public FavouriteList FavouriteList { get; set; }
        public Joke Joke { get; set; }


    }
}
