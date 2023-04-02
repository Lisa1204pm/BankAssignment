using System.ComponentModel.DataAnnotations;

namespace BankAssignment.ViewModels
{
    public class ClientAccountVM
    {
        [Key]
        [Display(Name = "Client ID")]
        public int ClientID { get; set; }
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        [MaxLength(50,ErrorMessage = "Must be less than 50 chars")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        [MaxLength(50, ErrorMessage = "Must be less than 50 chars")]
        public string LastName { get; set; }
        [Display(Name = "Account Number")]
        public int AccountNum { get; set; }
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "Balance")]
        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        [RegularExpression("^[0-9]*(\\.[0-9]{1,2})?$", ErrorMessage = "only two decimal allowed")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double Balance { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
        public string? Message { get; set; }

    }
}
