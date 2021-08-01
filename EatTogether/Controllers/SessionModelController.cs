using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using EatTogether.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Eat_Together.Controllers
{
    public class SessionModelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SessionModel
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            if (currentUserId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View();
        }

        //RATING USERS
        [HttpPost]
        public ActionResult UpdateUserScore()
        {
            string Username = Request["User"];
            ApplicationUser UserApp = db.Users.FirstOrDefault(x=> x.UserName == Username);
            double ScoreTotal = UserApp.ScoredTimes * UserApp.Score;
            string tmp = Request["Score"];
            int CurrentPoint = Int32.Parse(tmp);
            double add = (CurrentPoint + ScoreTotal) / (UserApp.ScoredTimes + 1);

            // adding the rate relation to the current user's rate list
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            RatedUserModel Rated = new RatedUserModel();
            Rated.UserRated = UserApp;
            Rated.UserRates = currentUser;
            db.RatedUsers.Add(Rated);

            UserApp.Score = add;
            UserApp.ScoredTimes++; 
            db.Entry(UserApp).State = EntityState.Modified;
            db.SaveChanges();
            tmp = Request["InheritedId"];
            
            return RedirectToAction(tmp, "SessionModel/Rate");
        }

        private IEnumerable<Session_ApplicationUser> GetUsersOfTheSession(int? SessionId)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.SessionUsers.ToList().Where(x=> x.SessionId == SessionId && x.ApplicationUserId != currentUserId && (db.RatedUsers.FirstOrDefault(y => y.UserRatedId == x.ApplicationUserId && y.UserRatesId == currentUserId) == null));
        }

        public ActionResult Rate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            SessionModel sessionModel = db.Session.Find(id);

            if (sessionModel == null || currentUser == null)
            {
                return HttpNotFound();
            }

            IEnumerable<Session_ApplicationUser> SessionUsersList = GetUsersOfTheSession(id);
            List<String> Users = new List<String>();
            foreach ( var item in SessionUsersList)
            {
                ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == item.ApplicationUserId);
                if (user != null)
                {
                    Users.Add(user.UserName);
                }
            }
           
            if (sessionModel.User != currentUser && db.RatedUsers.FirstOrDefault(y => y.UserRatedId == sessionModel.User.Id && y.UserRatesId == currentUserId) == null)
            {
                Users.Add(sessionModel.User.UserName);
            }

            ViewBag.userList = new SelectList(Users);
            ViewBag.sessionId = id;
            ViewBag.empty = false;
            if (Users.Count() == 0)
            {
                ViewBag.empty = true;
            }
            return View();
        }

        // JOIN AND DISJOIN
        public ActionResult Disjoin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            SessionModel sessionModel = db.Session.Find(id);

            if (sessionModel == null || currentUser == null)
            {
                return HttpNotFound();
            }

            Session_ApplicationUser sessionUser = db.SessionUsers.FirstOrDefault(x => x.ApplicationUserId == currentUserId && x.SessionId == sessionModel.Id); 

            if (sessionUser == null)
            {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            sessionModel.SpaceRemaining++;
            db.SessionUsers.Remove(sessionUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult JoinSessions()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(GetOtherSessionModels()) ;
        }

        public ActionResult Join(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            SessionModel sessionModel = db.Session.Find(id);

            Session_ApplicationUser sessionUser = new Session_ApplicationUser();
            sessionUser.ApplicationUserId = currentUserId;
            sessionUser.SessionId = sessionModel.Id;

            if (sessionModel == null || currentUser==null )
            {
                return HttpNotFound();
            }

            if (db.SessionUsers.FirstOrDefault(x => x.ApplicationUserId == currentUserId && x.SessionId == sessionModel.Id) != null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // adding current user to the participants
            db.SessionUsers.Add(sessionUser);
            sessionModel.SpaceRemaining--;
            ViewBag.joined = true;
            
            // saving the changes upon the database: User, Session and Sesson_User
            db.SaveChanges();
            return RedirectToAction("JoinSessions");
        }

        private IEnumerable<SessionModel> GetUserSessionModels()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.Session.ToList().Where(x => x.User == currentUser && x.Date > DateTime.Now);
        }

        private IEnumerable<SessionModel> GetOtherSessionModels()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.Session.ToList().Where(x => x.User != currentUser && x.SpaceRemaining > 0 && x.Date > DateTime.Now &&
            db.SessionUsers.FirstOrDefault(y => y.ApplicationUserId == currentUserId && y.SessionId == x.Id) == null);
        }

        private IEnumerable<SessionModel> GetJoinedSessionModels()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.Session.ToList().Where(x => db.SessionUsers.FirstOrDefault(y => y.ApplicationUserId == currentUserId && y.SessionId == x.Id) != null && x.Date > DateTime.Now);
        }

        private IEnumerable<SessionModel> GetExpiredSessionModels()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.Session.ToList().Where(x => (db.SessionUsers.FirstOrDefault(y => y.ApplicationUserId == currentUserId && y.SessionId == x.Id) != null || x.User == currentUser) && x.Date < DateTime.Now);
        }

        // GET: SessionModel
        public ActionResult GenerateSessionModelTable()
        {
            ViewBag.whole = false;
            ViewBag.joined = false;
            
            return PartialView("_SessionTable", GetUserSessionModels());
        }

        public ActionResult GenerateAllSessionModelTable()
        {
            ViewBag.whole = true;
            ViewBag.joined = true;            
            return PartialView("_SessionTable", GetOtherSessionModels()) ; 
        }

        public ActionResult GenerateJoinedSessionsTable()
        {
            ViewBag.whole = false;
            ViewBag.joined = true;
            
            return PartialView("_SessionTable", GetJoinedSessionModels());
        }

        public ActionResult GenerateExpiredSessionsTable()
        {
            ViewBag.whole = true;
            ViewBag.joined = false;
            return PartialView("_SessionTable", GetExpiredSessionModels());
        }

        //DETAILS
        // GET: SessionModel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            SessionModel sessionModel = db.Session.Find(id);
            if (sessionModel == null)
            {
                return HttpNotFound();
            }

            if (sessionModel.User.Id != User.Identity.GetUserId())
            {
                ViewBag.newwhole = false;
            }
            else
            {
                ViewBag.newwhole = true;
            }

            string currentUserId = User.Identity.GetUserId();
            if (db.SessionUsers.FirstOrDefault(y => y.ApplicationUserId == currentUserId && y.SessionId == sessionModel.Id) != null)
            {
                ViewBag.turnIndex = true;
            }
            else
            {
                ViewBag.turnIndex = false;
            }

            return View(sessionModel);
        }

        // GET: SessionModel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SessionModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserCount,Date,Description,City,PlaceName")] SessionModel sessionModel)
        {
            if (ModelState.IsValid)
            {
                // additional logic to bind the session opening user to the session
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                sessionModel.User = currentUser;
                sessionModel.SpaceRemaining = sessionModel.UserCount-1;
                sessionModel.SessionUserTable = new List<Session_ApplicationUser>();
                db.Session.Add(sessionModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sessionModel);
        }

        //AJAX FUNCTION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,UserCount,Date,Description,City")] SessionModel sessionModel)
        {
            string currentUserId = User.Identity.GetUserId();
            if (currentUserId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            if (ModelState.IsValid)
            {
                sessionModel.Place = new PlaceModel();
                sessionModel.Place.Name = Request["Place.Name"].ToString();
                sessionModel.Place.Type= Request["Place.Type"].ToString();
                // additional logic to bind the session opening user to the session
                
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                sessionModel.User = currentUser;
                sessionModel.SpaceRemaining = sessionModel.UserCount-1;
                sessionModel.SessionUserTable = new List<Session_ApplicationUser>();

                db.Session.Add(sessionModel);
                db.SaveChanges();
                
            }
            ViewBag.joined = false;
            ViewBag.whole = false;
            
            return PartialView("_SessionTable", GetUserSessionModels());
        }

        // GET: SessionModel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessionModel sessionModel = db.Session.Find(id);
            if (sessionModel == null)
            {
                return HttpNotFound();
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            
            if (sessionModel.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(sessionModel);
        }

        // POST: SessionModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserCount,Date,Description,City")] SessionModel sessionModel)
        {
            if (ModelState.IsValid)
            {
                int count = db.SessionUsers.ToList().Count(x => x.SessionId == sessionModel.Id);
                if (sessionModel.UserCount - count-1 < 0)
                {
                    sessionModel.SpaceRemaining = 0;
                    sessionModel.UserCount = count;
                }
                else
                { 
                    sessionModel.SpaceRemaining = sessionModel.UserCount - count-1;
                }

                var SessionId = Request["Id"];
                int ID = Convert.ToInt32(SessionId);
                SessionModel currentSession = db.Session.AsNoTracking().FirstOrDefault(x => x.Id == ID);
                int PlaceId = currentSession.Place.Id;
                PlaceModel Place = db.Places.Find(PlaceId);
                Place.Name = Request["PlaceName"];
                db.Entry(Place).State = EntityState.Modified;
                sessionModel.Place = Place;
                db.Entry(sessionModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sessionModel);
        }

        // GET: SessionModel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessionModel sessionModel = db.Session.Find(id);
            if (sessionModel == null)
            {
                return HttpNotFound();
            }
            return View(sessionModel);
        }

        // POST: SessionModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SessionModel sessionModel = db.Session.Find(id);
            PlaceModel place = db.Places.Find(sessionModel.Place.Id);
           
            db.Places.Remove(place);
            db.Session.Remove(sessionModel); 
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
