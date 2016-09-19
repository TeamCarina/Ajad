using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using SampleApp.Logic;
using SampleApp.Logic.Contracts;
using SampleApp.Model.Models;
using SampleApp.Models;
using SampleApp.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace SampleApp.Helper
{
    public class JobScheduler
    {

        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<SendEmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                      //s.WithIntervalInHours(1)
                      //.OnEveryDay()
                      //.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                      //.WithIntervalInHours(1)
                    s.WithIntervalInMinutes(10)
                //s.WithIntervalInSeconds(10)
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public class SendEmailJob : IJob
        {
            public readonly IAccountLogic accountLogic;
            public SendEmailJob(IAccountLogic _accountLogic)
            {
                accountLogic = _accountLogic;
            }
            public void Execute(IJobExecutionContext context)
            {
                List<FailedEmail> failedEmails = accountLogic.GetFailedEmails();
                foreach (var val in failedEmails)
                {
                    if (!string.IsNullOrEmpty(val.Email) && EmailLogic.SendEmail(val.Email, val.Body, val.Subject))
                    {
                        accountLogic.DeteteFailedEmail(val.FailEmailID);
                        
                    }
                }
            }
        }
    }
}