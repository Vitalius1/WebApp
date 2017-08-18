using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class User : BaseEntity
    {
// -------------------------------------------------
        public int UserId { get; set; }
// -------------------------------------------------
        public string FirstName { get; set; }
// -------------------------------------------------
        public string LastName { get; set; }
// -------------------------------------------------
        public string Username { get; set; }
// -------------------------------------------------
        public double Wallet { get; set; }
// -------------------------------------------------
        public string Password { get; set; }
// -------------------------------------------------
        public DateTime CreatedAt { get; set; }
// -------------------------------------------------
        public DateTime UpdatedAt { get; set; }
// -------------------------------------------------
        public List<Bid> Auctions { get; set; }
        public User()
        {
            Auctions = new List<Bid>();
        }
// -------------------------------------------------

    }
}
