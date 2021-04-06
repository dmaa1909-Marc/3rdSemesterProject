using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public class ServiceToken {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_In { get; set; }
        public string UserName { get; set; }

        [JsonProperty(".issued")]
        public DateTime DateAcquired { get; set; }
        
        [JsonProperty(".expires")]
        public DateTime DateExpireing { get; set; }

        public ServiceToken() {

        }

        public ServiceToken(string access_token, string token_type, int expires_In, string userName, DateTime dateAcquired, DateTime dateExpireing) {
            Access_token = access_token;
            Token_type = token_type;
            Expires_In = expires_In;
            UserName = userName;
            DateAcquired = dateAcquired;
            DateExpireing = dateExpireing;
        }
    }
}
