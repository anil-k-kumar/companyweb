using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using companyweb1.Models;

namespace companyweb1.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        //User registraion
        [HttpGet]
      public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(user u)
        {
        bool Status = false;
            string message = "";
            //verify model
            if (ModelState.IsValid)
            {
                var isExit = IsEmailExit(u.Email);
                if (isExit)
                {
                    ModelState.AddModelError("EmailExit", "Email already exist");
                    return View(u);
                }
                //password hash
                u.Password = Crypto.Hash(u.Password);
                u.Cpassword = Crypto.Hash(u.Cpassword);// to remove conform paassword match issue as password is hashed 
           
                //saving data to database
                using (companyEntities c=new companyEntities())
                {
                    c.users.Add(u);
                    c.SaveChanges();

                    message = "Regestraion is successfully done with user name " + u.UserName + " Using mail id: " + u.Email +"";
                    Status = true;
                }
                
            }
            
            else
            {
                message = "Invalid reqiuest";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(u);
        }
        //to check duplicate email
        [NonAction]
        public bool IsEmailExit(String EmailId)
        {
            using (companyEntities c=new companyEntities())
            {
                var v = c.users.Where(a => a.Email == EmailId).FirstOrDefault();
                return v != null;
            }
        }


        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(userlog us)
        {
            using (companyEntities c = new companyEntities())
            {
                var v = c.users.Where(a => a.UserName == us.UserName).FirstOrDefault();
                if (v != null)
                {
                    if(string.Compare(Crypto.Hash(us.Password),v.Password)==0){
                        int timeout = us.Remember ? 525600 : 10;
                        var ticket = new FormsAuthenticationTicket(us.UserName, us.Remember, timeout);
                        string encrypt = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypt);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        
                        return RedirectToAction("Index","Employee");
                      
                       
                    }
                    else
                    {
                        ModelState.AddModelError("", "UserNaMe and Password Wrong.");

                    }
                    
                }
                else {

                    ModelState.AddModelError("", "UserNaMe and Password Wrong.");
                }
            }
            return View();
        }


        [NonAction]
        public void SendEmail(string emailID,String UserName,string password)
        {
            var fromEmail = new MailAddress("kkumaranil485@gmail.com", "Company Web");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "4MT15cs011"; // Replace with actual password

           
                string subject = "Forgot Password";
                string body = "Request for Your Forgot password is Succesfull." +"Your User Name and Password is as follows<br/>" + "User Name:" + UserName + "Password:" + password;
        

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID,user u)
        {
            string message = "";
            bool status = false;

            using (companyEntities dc = new companyEntities())
            {
                var account = dc.users.Where(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    SendEmail(account.Email,u.UserName,u.Password);
                    message = "Your User Name and Password has been sent to your email id.";
                    status = true;
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View();
        }


        public ActionResult Index()
        {
            return View();
        }
       
        [Authorize]
        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }

}