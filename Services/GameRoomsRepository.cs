using Model;
using RestSharp;
using System;

namespace Services {
    public class GameRoomsRepository {

        private RestClient restClient = new RestClient("https://localhost:44328/api/Games/");
        private TokenAdmin tokenAdmin = TokenAdmin.GetInstance;

        public string AddNewGame(PostNewGame postNewGame) {
            var request = new RestRequest(null, DataFormat.Json);

            tokenAdmin.AddServiceTokenToRestRequest(request);

            request.AddJsonBody(postNewGame);
            var restResponse = restClient.Post<string>(request);

            string gamePin;

            if(restResponse.IsSuccessful) {
                gamePin = restResponse.Data;
            }
            else {
                gamePin = null;
            }

            return gamePin;
        }
    }
}