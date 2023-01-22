namespace Authentication
{
    public class AuthenticationConstants
    {
       //   public static readonly string SOCKETURL = "https://7bb2-45-127-232-58.in.ngrok.io/";//"ws://localhost:5000/";
        //  public static readonly string URL = "https://7bb2-45-127-232-58.in.ngrok.io";//"localhost:5000";

      public static readonly string SOCKETURL = "ws://localhost:5000/";
    public static readonly string URL = "localhost:5000";

     // public static readonly string SOCKETURL = "http://54.173.83.60:5000/";
    //   public static readonly string URL = "http://54.173.83.60:5000";
       

        public static readonly string REGISTER = URL + "/users/register";
        public static readonly string UPDATENAME  = URL + "/users/update";
        public static readonly string TUTORIALSTEP = URL + "/users/tutorialStep";
        public static readonly string ADDCOINS = URL + "/users/addcoins";

        public static readonly string UPDATEUSER = URL + "/users/update";
        public static readonly string SIGNUP = URL + "/users/signup";
        public static readonly string GETUSER = URL + "/users/data";
        public static readonly string GETUSERMINIMUMDATA = URL + "/users/minimumData";
        public static readonly string CURRENTSCREEN = URL + "/users/screen";
        public static readonly string WATCHADS = URL + "/users/watchads";
        public static readonly string DAILYREWARD = URL +"/users/dailyreward";
        public static readonly string FORGETPASSWORD = URL + "/users/forgot-password";
        public static readonly string UPDATECOINS = URL + "/users/updateCoins";
        public static readonly string ANALYTICS = URL + "/analytics";
      
        public static readonly string VERIFYOTP = URL + "verify-otp";
        public static readonly string RESETPASSWORD = URL + "reset-password";
        public static readonly string CHANGEPASSWORD = URL + "change-password";
        public static readonly string GETLEADERBOARD = URL + "/users/leaderboard";
        public static readonly string USERPROFILE = URL + "/users/get-profile";
        public static readonly string LIST = URL + "/users/search-user";
        public static readonly string PURCHASE = URL + "/service/transaction";
        public static readonly string VERSION = URL + "/service/version";
        public static readonly string RoomController = "RoomController";
        public static readonly string CHANGEPROFILEPICTURE = "/users/update-profile";


        #region GROUPS

        public static readonly string CREATEGROUP = URL + "/groups/create";
        public static readonly string INVITEUSER = URL + "/groups/add-member";
        public static readonly string GROUPLIST = URL + "/groups/group-list";
        public static readonly string MEMBERS = URL + "/groups/member-list";
        public static readonly string REQUESTEDLIST = URL + "/groups/request-list";
        public static readonly string REQUESTRESPOND = URL + "/groups/accept-or-reject";
        public static readonly string DELETEMEMBER = URL + "/groups/remove-member";
        public static readonly string DELETEGROUP = URL + "/groups/delete";

        public static readonly string GETMESSAGELIST = URL + "/message/get-message-list";
        public static readonly string SENDMESSAGE = URL + "/message/send-message";
        public static readonly string GETHISTORYGAMES = URL + "/history/game-list";
        public static readonly string HISTORYGAMEDDATA = URL + "/history/get-history";

        #endregion

        public static readonly string GETLIVEGAMES = URL + "/users/game-list";
    }

    public class AnalyticsEvents
    {
        public static readonly string SCREENOPEN = "screenOpen";

    }

    public class PlayerPrefsData
    {
        public static readonly string GAMEID = "gameId";
        public static readonly string ID = "id";
        public static readonly string ROLE = "role";
        public static readonly string TOKEN = "authToken";

        public static readonly string OPPONENTID = "opponentId";
        public static readonly string OPPONENTNAME = "opponentName";
        public static readonly string OPPONENTURL = "opponentUrl";
        public static readonly string MYURL = "myUrl";

        public static readonly string FIRSTTIME = "firstTIme";
        public static readonly string PURCHASE = "purchase";

     
        public static readonly string FIRSTNAME = "firstname";
       
        public static readonly string SOCIALID = "social_id";
        public static readonly string LASTNAME = "lastname";
        public static readonly string EMAIL = "email";
       
        public static readonly string PASSWORD = "password";
        public static readonly string ALREADYLOGIN = "alreadylogin";

        public static readonly string TASKSDONE = "TASKSDONE";
    }
}