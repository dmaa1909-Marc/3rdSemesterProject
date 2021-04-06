using Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services {
    public class AvatarsRepository {
        private RestClient restClient = new RestClient("https://localhost:44328/api/Avatars/");
        private TokenAdmin tokenAdmin = TokenAdmin.GetInstance;

        public IEnumerable<Avatar> GetAllUserAvatars() {
            var request = new RestRequest(null, DataFormat.Json);

            tokenAdmin.AddServiceTokenToRestRequest(request);

            var restResponse = restClient.Get<IEnumerable<Avatar>>(request);

            if(!restResponse.IsSuccessful) {
                Debug.WriteLine($"Unable to get all Avatars: {restResponse.StatusCode}\n" +
                    $"\t{restResponse.ErrorMessage}");
            }

            return restResponse.Data;
        }

        public Avatar GetAvatarById(int Id) {
            var request = new RestRequest(Id.ToString(), DataFormat.Json);

            tokenAdmin.AddServiceTokenToRestRequest(request);

            var restResponse = restClient.Get<Avatar>(request);

            if(!restResponse.IsSuccessful) {
                Debug.WriteLine($"Unable to get Avatar by Id: {restResponse.StatusCode}\n" +
                    $"\t{restResponse.ErrorMessage}");
            }

            return restResponse.Data;
        }
    }
}
