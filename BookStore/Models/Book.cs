using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        [DisplayName("Book Name")]
        [Required(ErrorMessage = "Book name is required.")]
        public string Name{ get; set; }
        [DisplayName("Book Code")]
        [Required(ErrorMessage = "Book code is required.")]
        public string Code{ get; set; }

        [DisplayName("Copies Count")]
        [Required(ErrorMessage = "This Field is required.")]
        public int Copies{ get; set; }

        public ICollection<Borrowing>Borrowings{ get; set; }
    }
}
