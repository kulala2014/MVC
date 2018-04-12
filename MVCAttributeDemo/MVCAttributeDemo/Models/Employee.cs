using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MVCAttributeDemo.Models
{
    //[Bind(Exclude ="Address")]
    public class Employee
    {
        /*******************Custom server side validations****************************/
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }

        public string PhoneNo { get; set; }
        [Salary]
        public int Salary { get; set; }

        /**************Remote validation*****************/
        //public string Name { get; set; }
        //public int Age { get; set; }

        //[Remote("CheckEmail", "Employee")]
        //public string Email { get; set; }

        //public string Address { get; set; }

        //public string PhoneNo { get; set; }

        //public int Salary { get; set; }


        /*******************Data Annotation validation**************/
        //[Required(ErrorMessage ="Name cannot be empty")]
        //public string Name { get; set; }

        //[Required(ErrorMessage = "Age cannot be empty")]
        //[Range(1,100,ErrorMessage ="Please enter age between 1 and 100")]
        //public int Age { get; set; }

        //[DataType(DataType.EmailAddress)]
        //[RegularExpression(@"^\w+([-+.']\w)*@\w+([-.]\w)*\.\w+([-.]\w+)*$",ErrorMessage ="Please enter vaild email id.")]
        ////[Remote("CheckEmail", "Employee")]
        //public string Email { get; set; }
        //[Required(ErrorMessage ="Address cannot be empty")]
        //public string Address { get; set; }
        //[DataType(DataType.PhoneNumber)]
        //public string PhoneNo { get; set; }
    }

    public class SalaryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value !=null)
            {
                int salary = Convert.ToInt32(value);
                if (salary <50000)
                {
                    return new ValidationResult("Salary of the user must be greater than 5000");
                }
            }
            return ValidationResult.Success;

        }
    }
}