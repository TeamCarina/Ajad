using SampleApp.Model.Models;
using System;
using System.Collections.Generic;

namespace SampleApp.Logic.Contracts
{
    public interface IAdminLogic
    {
        AspNetUsers GetUserDetailsById(string UserId);
        AspNetUsers GetuserIdbyemail(string Email);
        bool EditProfile(string UserId, string FirstName, string LastName, string Password);
        bool UpdateAccountSetting(string AppId, string CompanyName);
        bool CreateCampaign(string UserId, string AppId, string Title, string URL, DateTime CreateDate);
       List<Campaign> GetAllCampaignbyuserId(string UserId);
       Campaign GetCampaignDetailsById(string APPId);
       bool DeleteCampaignbyId(string AppId);

    }
}
