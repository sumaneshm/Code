using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage ="Name is a must to identify a Product")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Please describe your product")]
        public string Description { get; set; }

        [Range(0.1,double.MaxValue,ErrorMessage ="Please enter a positive value as Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "People would like to filter products based on category and please do supply one for this")]
        public string Category { get; set; }
    }
}
