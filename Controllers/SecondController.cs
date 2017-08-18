using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace WebApp.Controllers
{
    public class SecondController : Controller
    {
        private WebappContext _context;

        public SecondController(WebappContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("auction")]
        public IActionResult Auction()
        {
            return View("Auction");
        }
// ============================================================================================================

        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            // Checking if user is logged in to access the page.
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["NiceTry"] = "You need to be logged in to view account info!";
                return RedirectToAction("LoginPage", "Home");
            }

            User LoggedUser = _context.Users.Single(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            List<Auction> AllAuctions = _context.Auctions.Include(a => a.User).Include(a =>a.Biders).ToList();
            foreach(var a in AllAuctions)
            {
                if(a.EndDate <= DateTime.Now)
                {
                    a.User.Wallet += a.Biders[0].TopBid;
                    _context.Auctions.Remove(a);
                    _context.SaveChanges();
                }
            }
            ViewBag.LoggedUser = LoggedUser;
            return View("Index", AllAuctions);
        }

// ============================================================================================================

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AuctionViewModel NewAc)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["NiceTry"] = "You need to be logged in to view account info!";
                return RedirectToAction("LoginPage", "Home");
            }
            if(ModelState.IsValid)
            {
                Auction AtoAdd = new Auction
                {
                    ProductName = NewAc.ProductName,
                    Description = NewAc.Description,
                    EndDate = Convert.ToDateTime(NewAc.EndDate),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = (int)HttpContext.Session.GetInt32("UserId")
                };
                _context.Auctions.Add(AtoAdd);
                _context.SaveChanges();
                 
                 Bid BtoAdd = new Bid
                 {
                    AuctionId = AtoAdd.AuctionId,
                    UserId = (int)HttpContext.Session.GetInt32("UserId"),
                    TopBid = NewAc.StartingBid,                    
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                 };
                 _context.Bids.Add(BtoAdd);
                 _context.SaveChanges();
                return RedirectToAction("Index", "Second");
            }
            return View("Auction");
        }
// ============================================================================================================

        [HttpGet]
        [Route("auctionitem/{prodId}")]
        public IActionResult ShowAuction(int prodId)
        {
            ViewBag.Wrong = TempData["Wrong"];
            Auction TheAuction = _context.Auctions.Include(a => a.User).Include(a => a.Biders).ThenInclude(b => b.User).Single(a => a.AuctionId == prodId);
            return View("ShowAuction", TheAuction);
        }
// ============================================================================================================

        [HttpPost]
        [Route("placeBid")]
        public IActionResult PlaceBid(double myBid, int AuctionId, int BidId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["NiceTry"] = "You need to be logged in to view account info!";
                return RedirectToAction("LoginPage", "Home");
            }
            Bid CurrentBid = _context.Bids.Single(b => b.BidId == BidId);
            User CurrentUser = _context.Users.Single(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            if(myBid <= CurrentBid.TopBid)
            {
                TempData["wrong"] = "You can't place anything less than the current highest bid of " + CurrentBid.TopBid;
                return RedirectToAction("ShowAuction", new { prodId = AuctionId });
            }
            if(myBid > CurrentUser.Wallet)
            {
                TempData["wrong"] = "You can't place a bid if you don't have enough money. Your available balance is $" + CurrentUser.Wallet;
                return RedirectToAction("ShowAuction", new { prodId = AuctionId });
            }
            CurrentBid.UserId = (int)HttpContext.Session.GetInt32("UserId");
            CurrentBid.TopBid = myBid;
            CurrentUser.Wallet -= myBid;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
// ============================================================================================================


        [HttpGet]
        [Route("delete/{aucId}")]
        public IActionResult Delete(int aucId)
        {
            Auction toDelete = _context.Auctions.Single(a => a.AuctionId == aucId);
            _context.Auctions.Remove(toDelete);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
