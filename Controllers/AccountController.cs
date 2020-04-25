using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
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
            //[Bind(Exclude = "EmailVarify,AuthCode")]
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
                //genetrate activation token 
                u.AuthCode = Guid.NewGuid();
                //password hash
                u.Password = Crypto.Hash(u.Password);
                u.Cpassword = Crypto.Hash(u.Cpassword);// to remove conform paassword match issue as password is hashed 
                //u.EmailVarify = false;
                //savind data to database
                using (companyEntities c=new companyEntities())
                {
                    c.users.Add(u);
                    c.SaveChanges();
                    //Sendind email verification
                    //sendEmailLink(u.Email, u.AuthCode.ToString());

                    message = "Regestraion is successfully done.Account activation link has been send to your Email Id: " + u.Email;
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
        [NonAction]
        public void sendEmailLink(String Email,String AuthCode)
        {
            var VerUrl = "/user/VerifyAccount/" + AuthCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, VerUrl);

            var fromEmail = new MailAddress("kkumaranil485@gmail.com","Webpage Login");
            var toEmail = new MailAddress(Email);
            var fromPassword = "4MT15cs011";    //sender password
            String sub = "your account creation is  success";
            string body="Your account is succesfully created.Please click on below link to activate" + "<br><a href='"+link+"'>" + link + "</a>";
            var smtp = new SmtpClient
            {
                Host="smtp.gmail.com",
                Port=587,
                EnableSsl=true,
                DeliveryMethod=SmtpDeliveryMethod.Network,
                UseDefaultCredentials=false,
                Credentials=new NetworkCredential(fromEmail.Address,fromPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        public ActionResult login()
        {
            return View();
        }
        public ActionResult login(user us)
        {
            using (companyEntities c = new companyEntities())
            {
                var v = c.users.Where(a => a.UserName == us.UserName && a.Password == us.Password).FirstOrDefault();
                if (v != null)
                {
                    return RedirectToAction("Logout");               
                }
            }
            return View();
        }
        public ActionResult logout()
        {
          
        }
    }

}