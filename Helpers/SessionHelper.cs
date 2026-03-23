using System.Web;

namespace UniManage.Helpers
{
    public static class SessionHelper
    {
        public static void SetUserSession(HttpSessionStateBase session, int userId, string fullName, string role)
        {
            session["UserId"] = userId;
            session["FullName"] = fullName;
            session["Role"] = role;
        }

        public static void ClearSession(HttpSessionStateBase session)
        {
            session.Clear();
            session.Abandon();
        }
    }
}
