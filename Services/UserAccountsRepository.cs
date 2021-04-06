using Model;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services {
    public class UserAccountsRepository {
        private RestClient restClient = new RestClient("https://localhost:44328/api/UserAccounts/");
        private TokenAdmin tokenAdmin = TokenAdmin.GetInstance;

        public UserAccountsRepository() {
            //Using NewtonsoftJson as RestSharps default deserializer can't deserialize byte arrays...
            restClient.UseNewtonsoftJson();
        }


        public bool AddUserAccount(UserAccount newUserAccount) {
            var request = new RestRequest(null, DataFormat.Json);
            

            tokenAdmin.AddServiceTokenToRestRequest(request);

            request.AddJsonBody(newUserAccount);
            var restResponse = restClient.Post(request);

            return restResponse.IsSuccessful;
        }

        public IEnumerable<UserAccount> GetAllUserAccounts() {
            var request = new RestRequest(null, DataFormat.Json);

            tokenAdmin.AddServiceTokenToRestRequest(request);

            var restResponse = restClient.Get<IEnumerable<UserAccount>>(request);

            if(!restResponse.IsSuccessful) {
                Debug.WriteLine($"Unable to get all UserAccounts{restResponse.StatusCode}\n" +
                    $"\t{restResponse.ErrorMessage}");
            }

            return restResponse.Data;
        }

        public UserAccount GetUserAccountById(string userId) {
            var request = new RestRequest(userId, DataFormat.Json);

            tokenAdmin.AddServiceTokenToRestRequest(request);

            var restResponse = restClient.Get<UserAccount>(request);

            if(!restResponse.IsSuccessful) {
                Debug.WriteLine($"Unable to get UserAccount by Id: {restResponse.StatusCode}\n" +
                    $"\t{restResponse.ErrorMessage}");
            }

            return restResponse.Data;
        }

        public bool DeleteUserAccount(Guid id) {
            var request = new RestRequest(id.ToString(), DataFormat.Json);

            tokenAdmin.AddServiceTokenToRestRequest(request);

            var restResponse = restClient.Delete(request);

            if(!restResponse.IsSuccessful) {
                Debug.WriteLine($"Unable to delete UserAccount {id}: {restResponse.StatusCode}\n" +
                    $"\t{restResponse.ErrorMessage}");
            }

            return restResponse.IsSuccessful;
        }

        public IEnumerable<UserAccount> FindUserAccountByUsernameOrEMail(string searchString) {
            var request = new RestRequest(null, DataFormat.Json);

            tokenAdmin.AddServiceTokenToRestRequest(request);

            request.AddParameter("searchString", searchString);

            var restResponse = restClient.Get<IEnumerable<UserAccount>>(request);

            if(!restResponse.IsSuccessful && 
                restResponse.StatusCode != System.Net.HttpStatusCode.NotFound) {
                Debug.WriteLine($"Unable to search for UserAccount by Email: {restResponse.StatusCode}\n" +
                    $"\t{restResponse.ErrorMessage}");
            }

            return restResponse.Data;
        }



        public bool UpdateUserAccount(UserAccount userAccount) {

            var request = new RestRequest(userAccount.Id.ToString() , DataFormat.Json);
            tokenAdmin.AddServiceTokenToRestRequest(request);

            request.AddJsonBody(userAccount);

            var restResponse = restClient.Put<UserAccount>(request);
            int statusCode = (int)restResponse.StatusCode;

            return restResponse.IsSuccessful;
        }

    }
}
