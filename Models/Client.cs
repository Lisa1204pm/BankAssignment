using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BankAssignment.Models
{
    public class Client
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Client ID")]
        public int ClientID { get; set; }
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        [MaxLength(50, ErrorMessage = "Must be less than 50 chars")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        [MaxLength(50, ErrorMessage = "Must be less than 50 chars")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Email Format please.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }


    }
}
