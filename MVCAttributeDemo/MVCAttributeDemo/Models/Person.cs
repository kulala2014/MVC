using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCAttributeDemo.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Range(1,100)]
        public int Age { get; set; }
        [DataType(DataType.DateTime)]
        public string RegisterDate { get; set; }
        public string Address { get; set; }
    }
}