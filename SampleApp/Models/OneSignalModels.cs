using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleApp.Models
{

    public class AppModel
    {
        /// <summary>
        /// Response
        /// </summary>
        public string id { get; set; }
        public string name { get; set; }
        public int players { get; set; }
        public int messagable_players { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public string apns_certificates { get; set; }
        public string safari_apns_cetificate { get; set; }
        public string safari_push_id { get; set; }
        public string basic_auth_key { get; set; }

     


        /// <summary>
        /// Request
        /// </summary>
        public string apns_env { get; set; }
        public string apns_p12 { get; set; }
        public string apns_p12_password { get; set; }
        public string gcm_key { get; set; }
        public string chrome_key { get; set; }
        public string safari_apns_p12 { get; set; }
        public string chrome_web_key { get; set; }
        public string safari_apns_p12_password { get; set; }
        public string site_name { get; set; }
        public string safari_site_origin { get; set; }
        public string safari_icon_16_16 { get; set; }
        public string safari_icon_32_32 { get; set; }
        public string safari_icon_64_64 { get; set; }
        public string safari_icon_128_128 { get; set; }
        public string safari_icon_256_256 { get; set; }
        public string chrome_web_origin { get; set; }
        public string chrome_web_gcm_sender_id { get; set; }
        public string chrome_web_default_notification_icon { get; set; }
        public string chrome_web_sub_domain { get; set; }
    }


    public class AppUserModel
    {
        public string  app_id { get; set; }
        public string identifier { get; set; }
        public string language { get; set; }
        public string timezone { get; set; }
        public string game_version { get; set; }
        public string device_os { get; set; }
        public string device_type { get; set; }
        public string device_model { get; set; }
        public string tags { get; set; }
    }

}