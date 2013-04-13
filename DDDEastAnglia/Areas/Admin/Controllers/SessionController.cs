using System.Data;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    [RequireHttps]
    public partial class SessionController : Controller
    {
        private DDDEAContext db = new DDDEAContext();

        //
        // GET: /Admin/Session/

        public virtual ActionResult Index()
        {
            return View(db.Sessions.OrderByDescending(s => s.Votes).ToList());
        }

        //
        // GET: /Admin/Session/Details/5

        public virtual ActionResult Details(int id = 0)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        //
        // GET: /Admin/Session/Edit/5

        public virtual ActionResult Edit(int id = 0)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        //
        // POST: /Admin/Session/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        //
        // GET: /Admin/Session/Delete/5

        public virtual ActionResult Delete(int id = 0)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        //
        // POST: /Admin/Session/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}