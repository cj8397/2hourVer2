using _2Hours_Ver2;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace shoppingCart.BusinessLogic
{
    public class Session
    {
        public const string SESSION_START   = "Session_Start";
        public const string SESSION_END     = "Session_End";
        private mergedEntities db     = new mergedEntities();

        // Get data stored under the current session.
        // This data is stored on the server in a collection.
        public DateTime End
        {
            get
            {
                try
                {
                    return (DateTime)HttpContext.Current.Session[SESSION_END];
                }
                catch
                {
                    Initialize();
                }
                return (DateTime)HttpContext.Current.Session[SESSION_END];
            }
        }


        // Return value from session cookie manually if the session does not exist.
        public string SessionID
        {
            get
            {
                if (HttpContext.Current.Session.SessionID != null)
                    return HttpContext.Current.Session.SessionID;
                return null;
            }
        }
        public void Initialize()
        {
            HttpContext.Current.Session[SESSION_START] = DateTime.Now;
            HttpContext.Current.Session[SESSION_END] = DateTime.Now.AddMinutes(1);
        }
        public void Store(string sessionID)
        {
            var session = new Visit();
            session.sessionID = sessionID;
            session.started = DateTime.Now;

            db.Visits.Add(session);
            db.SaveChanges();
        }

        public void Update()
        {
            if (SessionID == null)
                Initialize();
            HttpContext.Current.Session[SESSION_END] = DateTime.Now.AddMinutes(1);
        }
        public void Clear()
        {
            if (SessionID != null)
            {
                HttpContext.Current.Session.Clear(); // remove stored items
                HttpContext.Current.Session.Abandon();
            }
        }

        //check for duplicate sessions in the Visits table and delete the old one
        public void ValidateSession(string sessionID)
        {
            var visit = db.Visits.Find(sessionID);
            if (visit != null) RemoveItems(visit.sessionID);
        }


        //populates sessions in the visits table
        public void CastSession(string sessionID)
        {
            db.Visits.Add(new Visit
            {
                sessionID = sessionID,
                started = DateTime.Now
            });
            db.SaveChanges();

        }

        //remove all items from cart
        public void RemoveItems(string sessionID)
        {
            var orderProducts = db.OrderProducts
                .Where(v => v.sessionID == sessionID);
            RemoveOrderProducts(orderProducts.ToList());
        }

        public void RemoveOrderProducts(List<OrderProduct> orderProducts)
        {
            if (orderProducts.Any())
            {
                orderProducts.ForEach(op => db.OrderProducts.Remove(op));
                db.SaveChanges();
            }
        }

        //removes expired sessions from Visits table
        private void TerminateSession(string sessionID, IEnumerable<DateTime?> expiredSession)
        {
            if (expiredSession.Any())
            {
                foreach (var item in expiredSession)
                {
                    var visitExp = db.Visits.FirstOrDefault(v => v.started == item);
                    if (visitExp != null)
                    {
                        bool isUpdated = db.OrderProducts
                            .Where(x => x.sessionID == visitExp.sessionID && End < x.updatedSession)
                            .Any();
                        if (!isUpdated)
                        {
                            db.Visits.Remove(visitExp);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        //removes all expired sessions from ProductVisits table when app starts
        public void RemoveExpired(string sessionID)
        {
            if (SessionID != null)
            {
                var expSession = db.OrderProducts.Where(e => End > e.updatedSession);

                RemoveOrderProducts(expSession.ToList());

                var lapsedSessions = db.Visits
                    .Where(x => End > x.started)
                    .Select(x => x.started);

                TerminateSession(sessionID, lapsedSessions);
            }
        }

    }//end class
}