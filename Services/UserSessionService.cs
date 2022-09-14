//using Newtonsoft.Json;
//using WebFinancialHelper.Models;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Security.Policy;

//namespace WebFinancialHelper.Services
//{
//    public class UserSessionService
//    {
        
//        public string GetUserSession(LoginModel request)
//        {
//            var userSession = new UserSessionModel() { Id = 1, Username = request.Username };
//            HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(userSession));
//        }
//    }
//}
