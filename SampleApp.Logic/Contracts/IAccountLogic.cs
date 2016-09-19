using SampleApp.Model.Models;
using System;
using System.Collections.Generic;

namespace SampleApp.Logic.Contracts
{
    public interface IAccountLogic
    {
        bool OpenAccount(string userId);

        Account GetAccountDetails(string userId);

        Account GetAccountDetailsByAccountNumber(string accountNumber);

        List<Transection> GetTransections(string userId);

        bool TransferAmount(TransferModel model);

        List<FailedEmail> GetFailedEmails();

        bool DeteteFailedEmail(Guid failId);

        bool UpdateATMPin(string accountNumber, int currentPin, int newPin);

        bool ATMWIthdrawl(string accountNumber, decimal amount);

        //AspNetUsers GetSession(string Email);
     

      
    }
}