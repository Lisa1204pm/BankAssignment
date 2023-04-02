using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAssignment.Models
{
    public class BankAccount
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display (Name="Account Number")]
        public int AccountNum { get; set; }
        [Display(Name = "Balance")]
        public double Balance { get; set; }
        [Display(Name = "Account Type")]
        [Required(ErrorMessage = "Please select a type")]
        public string AccountType { get; set; }
        public virtual BankAccountType BankAccountType { get; set; }

        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }

    }
}
