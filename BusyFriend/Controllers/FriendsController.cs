using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BusyFriend.Models;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace BusyFriend.Controllers
{
    public class FriendsController : TwilioController
    {
        private readonly BusyFriendContext _db = new BusyFriendContext();

        #region All CRUD Stuff

        // GET: Friends
        public ActionResult Index()
        {
            return View(_db.Friends.ToList());
        }

        // GET: Friends/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var friend = _db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // GET: Friends/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FriendId,Name,Telephone,Status,Message")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                _db.Friends.Add(friend);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(friend);
        }

        // GET: Friends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var friend = _db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FriendId,Name,Telephone,Status,Message")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(friend).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(friend);
        }

        // GET: Friends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var friend = _db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var friend = _db.Friends.Find(id);
            _db.Friends.Remove(friend);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        // POST: ReceiveCall
        [HttpPost]
        public ActionResult ReceiveCall(string From, string To)
        {
            var twiml = new TwilioResponse();

            // Check if it's friend
            var friend = (from f in _db.Friends
                          where f.Telephone.Equals(From)
                          select f).ToList().SingleOrDefault();
            if (friend == null)
            {
                return TwiML(twiml.Say("You've probably got the wrong number or aren't my bro!"));
            }

            // I will answer this bro's call
            if (friend.Status)
            {
                return TwiML(twiml.Say("You've reached Marcos Placona. Hang on!").Dial("+447590566866"));
            }
            return TwiML(twiml.Say(friend.Message));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
