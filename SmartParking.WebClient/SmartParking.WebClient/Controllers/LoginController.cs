using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartParkingSystemAPI.Models;
using System.Configuration;

namespace SmartParking.WebClient.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            //return RedirectToAction("Index", "UserManagement"); 
            string token = GetToken(model);
            Session["OAuthToken"] = token;
            if (token != "")
            {
                Session["UserName"] = model.UserName;
                return RedirectToAction("Index", "UserManagement");
            }
            else
                return RedirectToAction("Index"); 
        }
        private string GetToken(LoginModel model)
        {
            //string URI = "http://localhost:59822//oauth/token";
            string URI = ConfigurationManager.AppSettings["as:TokenIssuer"].ToString();
            string myParameters = "Username=" + model .UserName ;
            myParameters += "&Password=" + model.Password  ;
            myParameters += "&grant_type=password";
            myParameters += "&client_id=414e1927a3884f68abc79f7283837fd1";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                try
                {
                    string HtmlResult = wc.UploadString(URI, myParameters);
                    Token token = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Token>(HtmlResult);
                    return token.access_token;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }
        class Token
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
        }
    }
}
