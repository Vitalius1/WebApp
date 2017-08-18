using System;
using System.ComponentModel.DataAnnotations;
 
namespace WebApp.Models
{
    public class AuctionViewModel : BaseEntity
    {
//------------------------------------------------------------------------------------
        [Required]
        [MinLength(3, ErrorMessage = "Must be at least 3 letters.")]
        public string ProductName {get; set;}
//------------------------------------------------------------------------------------
        [Required]
        [MinLength(10, ErrorMessage = "Must be at least 10 letters.")]
        public string Description {get; set;}
//------------------------------------------------------------------------------------
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "No negative numbers or 0")]
        public double StartingBid {get; set;}
//------------------------------------------------------------------------------------
      
        [Required]
        [MyDate (ErrorMessage = " End Date can not be in the past.")]
        [DataType(DataType.Date)]
        public string EndDate {get; set;}
//------------------------------------------------------------------------------------
        public class MyDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                DateTime theirDate = Convert.ToDateTime(value);
                return theirDate >= DateTime.Now;
            }
        }
//-----------------------------------------------------------------------------------

    }
}