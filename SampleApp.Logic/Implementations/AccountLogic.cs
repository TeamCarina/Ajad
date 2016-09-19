using SampleApp.Comm.Contracts;
using SampleApp.Logic;
using SampleApp.Logic.Contracts;
using SampleApp.Model.Models;

using SampleApp.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace SampleApp.Implementations.Logic
{
    public class AccountLogic : IAccountLogic
    {
        private readonly ISampleAppRepo<Account> account;
        private readonly ISampleAppRepo<AspNetUsers> users;
        private readonly ISampleAppRepo<FailedEmail> failedEmail;
        private readonly ISampleAppRepo<Transection> transectionObj;
        private readonly ISampleAppRepo<Status> status;
        private readonly ISampleAppRepo<TransectionMode> transectionMode;
        private readonly ISampleAppRepo<TransectionType> transectionType;
      

     

        public AccountLogic(ISampleAppRepo<Account> _account, ISampleAppRepo<AspNetUsers> _users, ISampleAppRepo<FailedEmail> _failedEmail, ISampleAppRepo<Transection> _transection, ISampleAppRepo<Status> _status, ISampleAppRepo<TransectionMode> _transectionMode, ISampleAppRepo<TransectionType> _transectionType)
        {
            account = _account;
            transectionObj = _transection;
            status = _status;
            transectionMode = _transectionMode;
            transectionType = _transectionType;
            users = _users;
            failedEmail = _failedEmail;
          
         
            //context.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// This method will open an account and deposit 5000 in users account which is minimum account balance as I asumed
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool OpenAccount(string userId)
        {
            bool result = false;
            try
            {
                Account newAccount = new Account();
                newAccount.AccountId = Guid.NewGuid();
                newAccount.UserId = userId;
                newAccount.CreatedDate = DateTime.Now;
                newAccount.CurrentBalance = 5000;
                newAccount.AccountNumber = string.Format("CA000{0}", account.Table.Count() + 1);
                newAccount.PIN = 1234;
                account.Insert(newAccount);
                //Transection table reflection
                Transection transection = new Transection();
                transection.TransectionId = Guid.NewGuid();
                transection.AccountId = newAccount.AccountId;
                transection.Amount = newAccount.CurrentBalance;
                transection.Remarks = "New account initial deposit";
                transection.TransectionDate = DateTime.Now;
                transection.StatusId = status.Table.Where(x => x.StatusKey.Equals("Success")).Select(x => x.StatusID).FirstOrDefault();
                transection.TransectionModeId = transectionMode.Table.Where(x => x.Mode.Equals("CashDeposit")).Select(x => x.TransectionModeId).FirstOrDefault();
                transection.TransectionTypeId = transectionType.Table.Where(x => x.Type.Equals("Credit")).Select(x => x.TransectionTypeId).FirstOrDefault();
                transectionObj.Insert(transection);
                var user = users.Table.Where(x => x.Id.Equals(userId)).FirstOrDefault();
                try
                {
                    //trying to send email to user
                    EmailLogic.SendEmail(user.Email, "Your account is opened with 5000.", "Alert from Bank.");
                }
                catch (Exception)
                {
                    //if email send failed, stored it in database. Job Scheduler will try to resend it and delete it from database if email is send successfully
                    FailedEmail failed = new FailedEmail();
                    failed.FailEmailID = Guid.NewGuid();
                    failed.Email = user.Email;
                    failed.Body = "Your account is opened with 5000.";
                    failed.Subject = "Alerts from bank";
                    failedEmail.Insert(failed);
                }

                result = true;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        /// <summary>
        /// this will get users account details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Account GetAccountDetails(string userId)
        {
            var accountObj = new Account();
            try
            {
                accountObj = account.Table.Include(x => x.Transection).Include(x => x.AspNetUsers).Where(x => x.UserId.Equals(userId)).FirstOrDefault();
                
            }
            catch (Exception)
            {
                throw;
            }
            return accountObj;
        }

        //public AspNetUsers GetSession(string Email)
        //{
        //    var accountObj = new AspNetUsers();
        //    try
        //    {
        //        accountObj = users.Table.Where(x => x.Email.Equals(Email) ).FirstOrDefault();

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return accountObj;
        //}

        /// <summary>
        /// this will also get account details by account number of user
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public Account GetAccountDetailsByAccountNumber(string accountNumber)
        {
            var accountObj = new Account();
            try
            {
                accountObj = account.Table.Include(x => x.Transection).Include(x => x.AspNetUsers).Where(x => x.AccountNumber.Equals(accountNumber)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return accountObj;
        }

        /// <summary>
        /// this will return list of transections by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Transection> GetTransections(string userId)
        {
            var transections = new List<Transection>();
            try
            {
                var accountObj = account.Table.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
                transections = transectionObj.Table.Include(x => x.TransectionMode).Include(x => x.TransectionType).Where(x => x.AccountId.Value.Equals(accountObj.AccountId)).OrderByDescending(x => x.TransectionDate).ToList<Transection>();
            }
            catch (Exception)
            {
                throw;
            }
            return transections;
        }

        /// <summary>
        /// this method will transfer amount form one account to another account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool TransferAmount(TransferModel model)
        {
            bool result = false;
            try
            {
                var destinationAccount = GetAccountDetailsByAccountNumber(model.DestinationAccountNumber);
                var sourceAccount = GetAccountDetailsByAccountNumber(model.SourceAccountNumber);
                var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
                var destinationAccountRegion = regions.FirstOrDefault(region => region.EnglishName.Contains(destinationAccount.AspNetUsers.Country));
                var sourceAccountRegion = regions.FirstOrDefault(region => region.EnglishName.Contains(sourceAccount.AspNetUsers.Country));
                CurrencyModel currencyModel = new CurrencyModel();
                currencyModel.Amount = Decimal.Parse(model.Amount);
                currencyModel.From = sourceAccountRegion.ISOCurrencySymbol;
                currencyModel.To = destinationAccountRegion.ISOCurrencySymbol;
                var convertedAmount = CurrencyConvertor.ConvertCurrency(currencyModel);
                //create transection for destination user
                Transection transection = new Transection();
                transection.TransectionId = Guid.NewGuid();
                transection.AccountId = destinationAccount.AccountId;
                transection.Amount = convertedAmount;
                transection.Remarks = string.Format("{0} , has be credited from your account.", convertedAmount);
                transection.TransectionDate = DateTime.Now;
                transection.StatusId = status.Table.Where(x => x.StatusKey.Equals("Success")).Select(x => x.StatusID).FirstOrDefault();
                transection.TransectionModeId = transectionMode.Table.Where(x => x.Mode.Equals("OnlineTransfer")).Select(x => x.TransectionModeId).FirstOrDefault();
                transection.TransectionTypeId = transectionType.Table.Where(x => x.Type.Equals("Credit")).Select(x => x.TransectionTypeId).FirstOrDefault();
                transectionObj.Insert(transection);
                destinationAccount.CurrentBalance += convertedAmount;
                try
                {
                    //trying to send email to destination account user
                    EmailLogic.SendEmail(destinationAccount.AspNetUsers.Email, string.Format("{0} , has been credited to your account.", convertedAmount), "Alerts from bank");
                }
                catch (Exception ex)
                {
                    //if email send failed, stored it in database. Job Scheduler will try to resend it and delete it from database if email is send successfully
                    FailedEmail failed = new FailedEmail();
                    failed.FailEmailID = Guid.NewGuid();
                    failed.Email = destinationAccount.AspNetUsers.Email;
                    failed.Body = string.Format("{0} , has been credited to your account.", convertedAmount);
                    failed.Subject = "Alerts from bank";
                    failedEmail.Insert(failed);
                }
                //create transection for source account
                Transection transectionSource = new Transection();
                transectionSource.TransectionId = Guid.NewGuid();
                transectionSource.AccountId = sourceAccount.AccountId;
                if (sourceAccount.AspNetUsers.Country != destinationAccount.AspNetUsers.Country)
                    transectionSource.Amount = Decimal.Parse(model.Amount) + Math.Round((Decimal)(2 * Decimal.Parse(model.Amount)) / 100);
                else
                    transectionSource.Amount = Decimal.Parse(model.Amount);
                transectionSource.Remarks = model.Remarks;
                transectionSource.TransectionDate = DateTime.Now;
                transectionSource.StatusId = status.Table.Where(x => x.StatusKey.Equals("Success")).Select(x => x.StatusID).FirstOrDefault();
                transectionSource.TransectionModeId = transectionMode.Table.Where(x => x.Mode.Equals("OnlineTransfer")).Select(x => x.TransectionModeId).FirstOrDefault();
                transectionSource.TransectionTypeId = transectionType.Table.Where(x => x.Type.Equals("Debit")).Select(x => x.TransectionTypeId).FirstOrDefault();
                transectionObj.Insert(transectionSource);
                sourceAccount.CurrentBalance -= transectionSource.Amount;
                try
                {
                    //trying to send email to source account user
                    EmailLogic.SendEmail(sourceAccount.AspNetUsers.Email, string.Format("{0} , has been debited from your account.", transectionSource.Amount), "Alerts from bank");
                }
                catch (Exception ex)
                {
                    //if email send failed, stored it in database. Job Scheduler will try to resend it and delete it from database if email is send successfully
                    FailedEmail failed = new FailedEmail();
                    failed.FailEmailID = Guid.NewGuid();
                    failed.Email = sourceAccount.AspNetUsers.Email;
                    failed.Body = string.Format("{0} , has been debited from your account.", transectionSource.Amount);
                    failed.Subject = "Alerts from bank";
                    failedEmail.Insert(failed);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// this method will return list of failed email list to job schedullar
        /// </summary>
        /// <returns></returns>
        public List<FailedEmail> GetFailedEmails()
        {
            return failedEmail.Table.ToList<FailedEmail>();
        }

        /// <summary>
        /// this method will delete failed emails from database after successfull send from job schedullar
        /// </summary>
        /// <param name="failId"></param>
        /// <returns></returns>
        public bool DeteteFailedEmail(Guid failId)
        {
            bool result = false;
            try
            {
                var item = failedEmail.Table.Where(x => x.FailEmailID.Equals(failId)).FirstOrDefault();
                if (item != null)
                {
                    failedEmail.Delete(item);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// This method is used for change of atm pin
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="oldPin"></param>
        /// <param name="newPin"></param>
        /// <returns></returns>
        public bool UpdateATMPin(string accountNumber, int oldPin, int newPin)
        {
            bool result = false;
            try
            {
                var item = account.Table.Where(x => x.AccountNumber.Equals(accountNumber)).FirstOrDefault();
                if (item != null)
                {
                    if (item.PIN.Equals(oldPin))
                    {
                        item.PIN = newPin;
                        account.Update(item);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// this method is used to withdrawl amount from atm
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ATMWIthdrawl(string accountNumber, decimal amount)
        {
            bool result = false;
            try
            {
                var item = account.Table.Where(x => x.AccountNumber.Equals(accountNumber)).FirstOrDefault();
                if (item != null)
                {
                    item.CurrentBalance -= amount;
                    account.Update(item);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 
       
       
       

     
       
      
    }
}