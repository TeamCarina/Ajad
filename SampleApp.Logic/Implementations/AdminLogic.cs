using SampleApp.Comm.Contracts;
using SampleApp.Logic.Contracts;
using SampleApp.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleApp.Implementations.Logic
{
    public class AdminLogic : IAdminLogic
    {
        private readonly ISampleAppRepo<Account> account;
        private readonly ISampleAppRepo<AspNetUsers> users;
        private readonly ISampleAppRepo<FailedEmail> failedEmail;
        private readonly ISampleAppRepo<Transection> transectionObj;
        private readonly ISampleAppRepo<Status> status;
        private readonly ISampleAppRepo<TransectionMode> transectionMode;
        private readonly ISampleAppRepo<TransectionType> transectionType;
        private readonly ISampleAppRepo<Campaign> campaign;

        public AdminLogic(ISampleAppRepo<Account> _account, ISampleAppRepo<AspNetUsers> _users, ISampleAppRepo<FailedEmail> _failedEmail, ISampleAppRepo<Transection> _transection, ISampleAppRepo<Status> _status, ISampleAppRepo<TransectionMode> _transectionMode, ISampleAppRepo<TransectionType> _transectionType, ISampleAppRepo<Campaign> _campaign)
        {
            account = _account;
            transectionObj = _transection;
            status = _status;
            transectionMode = _transectionMode;
            transectionType = _transectionType;
            users = _users;
            failedEmail = _failedEmail;
            campaign = _campaign;

            //context.Configuration.LazyLoadingEnabled = false;
        }

        public AspNetUsers GetUserDetailsById(string UserId)
        {
            var usersObj = new AspNetUsers();
            try
            {
                usersObj = users.Table.Where(x => x.Id.Equals(UserId)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return usersObj;
        }

        public bool EditProfile(string UserId, string FirstName, string LastName, string Password)
        {
            bool result = false;
            try
            {
                var item = users.Table.Where(x => x.Id.Equals(UserId)).FirstOrDefault();
                if (item != null)
                {
                    item.FirstName = FirstName;
                    item.LastName = LastName;
                    item.PasswordHash = Password;
                    users.Update(item);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool UpdateAccountSetting(string AppId, string CompanyName)
        {
            bool result = false;
            try
            {
                var item = users.Table.Where(x => x.AppId.Equals(AppId)).FirstOrDefault();
                if (item != null)
                {
                    item.SubDomain = CompanyName;

                    users.Update(item);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool CreateCampaign(string UserId, string AppId, string Title, string URL, DateTime CreateDate)
        {
            bool result = false;
            try
            {
                Campaign addobj = new Campaign();

                addobj.UserId = UserId;
                addobj.Title = Title;
                addobj.AppId = AppId;
                addobj.URL = URL;
                addobj.CreateDate = CreateDate;
                campaign.Insert(addobj);
                result = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;

        }
        public List<Campaign> GetAllCampaignbyuserId(string UserId)
        {
            var Campaign = new List<Campaign>();
            try
            {
                Campaign = campaign.Table.Where(x => x.UserId.Equals(UserId)).ToList<Campaign>();
            }
            catch (Exception)
            {
                throw;
            }
            return Campaign;
        }

        public Campaign GetCampaignDetailsById(string APPId)
        {
            var CamObj = new Campaign();
            try
            {
                CamObj = campaign.Table.Where(x => x.AppId.Equals(APPId)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return CamObj;

        }


        public AspNetUsers GetuserIdbyemail(string Email)
        {
            var userObj = new AspNetUsers();
            try
            {
                userObj = users.Table.Where(x => x.Email.Equals(Email)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return userObj;
        }

        public bool DeleteCampaignbyId(string AppId)
        {

            bool result = false;
            try
            {
                var CamObj = new Campaign();
                CamObj = campaign.Table.Where(x => x.AppId.Equals(AppId)).FirstOrDefault();



                campaign.Delete(CamObj);
                result = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;

        }
    }
}