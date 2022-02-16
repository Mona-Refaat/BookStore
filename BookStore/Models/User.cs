using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class User
    {
        public int Id{ get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User name is required.")]
        public string Name { get; set; }
        public ICollection<Borrowing> Borrowings{ get; set; }
    }
}
