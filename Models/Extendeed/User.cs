using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace companyweb1.Models
{
    [MetadataType(typeof(metadate))]
    public partial class user
    {
        public string Cpassword { get; set; }
    }
    public class metadate
    {
        [Display(Name = "Full Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name is required ")]
        public string FName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required ")]
        public string LName { get; set; }

        [Display(Name = "Email Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Id is required ")]
        
        public string Email { get; set; }

        [Display(Name = "User Name")] 
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name is required ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Minimum 6 Characters")]
        public string Password { get; set; }

        [Display(Name ="Confirm passworrd")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password and password is not matcing")]
        public string Cpassword { get; set; }
    }
}