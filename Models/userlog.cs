using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace companyweb1.Models
{
    public class userlog
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool Remember { get; set; }
    }
}