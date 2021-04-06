using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using RestSharp.Serializers.NewtonsoftJson;
using System.Diagnostics;
using System.Configuration;

namespace Services {
    public class TokenAdmin {

        private static TokenAdmin instance;
        private static object padLock = new object();


        RestClient restClient;
        private ServiceToken serviceToken;

        private TokenAdmin() {
            restClient = new RestClient($"https://localhost:44328/");
            restClient.UseNewtonsoftJson();
        }

        ~TokenAdmin() {
            SignOutOfService();
        }

        public static TokenAdmin GetInstance {
            get {
                if(instance == null) {
                    lock(padLock) {
                        if(instance == null) {
                            instance = new TokenAdmin();
                        }
                    }
                }
                return instance;
            }
        }

        public ServiceToken ServiceToken {
            get {
                if(serviceToken == null) {
                    SignInToService();
                }
                return serviceToken;
            }
            private set {
                serviceToken = value;
            }
        }

        private void SignInToService() {
            if(serviceToken == null) {

                string userName = ConfigurationManager.AppSettings.Get("ApiUsername");
                string password = ConfigurationManager.AppSettings.Get("ApiPassword");
                RestRequest request = new RestRequest("token");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "password");
                request.AddParameter("username", userName);
                request.AddParameter("password", password);

                var responce = restClient.Post<ServiceToken>(request);

                if(!responce.IsSuccessful) {
                    Debug.WriteLine($"Unable to aquire bearer token for user {userName}");
                }

                serviceToken = responce.Data;
            }
        }

        private void SignOutOfService() {
            if(serviceToken != null) {
                //TODO: sgnout user / serviceToken
            }
        }

        public bool AddServiceTokenToRestRequest(RestRequest restRequest) {
            if(ServiceToken != null) {
                string bearerToken = ServiceToken.Token_type + " " + ServiceToken.Access_token;
                restRequest.AddHeader("Authorization", bearerToken);
                return true;
            }
            else {
                Debug.WriteLine("Unable To add bearer token to request");
                return false;
            }
        }
    }
}
