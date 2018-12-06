namespace Demo.Contract.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        //[Required]
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double StarRating { get; set; }
        public string ImageUrl { get; set; }
    }
}
