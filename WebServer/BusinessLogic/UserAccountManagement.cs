using Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServer.Models;

namespace WebServer.BusinessLogic {
    public class UserAccountManagement {

        private UserAccountsRepository userAccountRepository = new UserAccountsRepository();
        private AvatarsRepository avatarsRepository = new AvatarsRepository();

        public bool RegisterUserAccountForApplicationUser(ApplicationUser newApplicationUser) {
            var newUserAccount = new UserAccount {
                Id = new Guid(newApplicationUser.Id),
                Username = newApplicationUser.Email,
                Email = newApplicationUser.Email,
                RegistrationDate = DateTime.Now,
                AvatarId = 1,
                PremiumAccount = false,
                PaymentDetailsId = null,
                Banned = false
            
                
            };

            return userAccountRepository.AddUserAccount(newUserAccount);
        }

        public Avatar GetAvatarForUserByUserId(string userId) {
            UserAccount userAccount = userAccountRepository.GetUserAccountById(userId);
            if(userAccount != null) {
                return avatarsRepository.GetAvatarById(userAccount.AvatarId);
            } else {
                return null;
            }
        }
    }
}