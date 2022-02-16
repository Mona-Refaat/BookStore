using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Borrowing
    {
        public int  Id{ get; set; }

        [Required(ErrorMessage = "Book is required.")]
        public int BookId { get; set; }
        [Required(ErrorMessage = "User is required.")]
        public int UserId { get; set; }

        public DateTime BorrowFrom { get; set; }
        public DateTime BorrowTo { get; set; }
        public DateTime? Returned { get; set; }

        public Book Book{ get; set; }
        public User User{ get; set; }


    }
}
