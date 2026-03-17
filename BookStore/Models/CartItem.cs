using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }   // Link to User

        public int BookId { get; set; }   // Link to Book

        public int Quantity { get; set; }

        // Navigation Properties
        public User User { get; set; }

        public Book Book { get; set; }
    }
}