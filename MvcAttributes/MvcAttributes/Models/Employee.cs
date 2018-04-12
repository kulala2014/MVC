using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MvcAttributes.Models
{
   // [Bind(Exclude="Address")]
    public class Employee
    {
        [HiddenInput(DisplayValue=false)]
        public string Name { get; set; }

        [Remote("CheckEmail","Employee",ErrorMessage="Email is already exist")]
        public string Email { get; set; }
        public string Address { get; set; }
     
        [Editable(false)]
        public string PhoneNo { get; set; }
    }
}