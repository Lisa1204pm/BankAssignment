using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Xml.Linq;

namespace BankAssignment.Models
{
    public class ClientAccount
    {
        [Key, Column(Order = 0)]
        [Display(Name = "Client ID")]
        public int ClientID { get; set; }
        [Key, Column(Order = 1)]
        [Display(Name = "Account Number")]
        public int AccountNum { get; set; }
        public virtual Client Client { get; set; }
        public virtual BankAccount BankAccount { get; set; }   

    }
}
