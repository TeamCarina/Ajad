using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Model.Models
{
    public class TransferModel
    {
        public Guid AccountId { get; set; }
        public string SourceAccountNumber { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public string Amount { get; set; }
        public string Remarks { get; set; }
        public string DestinationAccountNumber { get; set; }
    }

    public class AccountCheckModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AccountNumber { get; set; }
    }

    public class CurrencyModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateAppMdoel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
    }
}
