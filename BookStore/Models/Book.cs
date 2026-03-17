using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}