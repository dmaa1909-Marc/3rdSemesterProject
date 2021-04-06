using System;
using System.ComponentModel.DataAnnotations;


namespace Model {
    public class UserAccount {

        public UserAccount() { 
        }

        public Guid Id { get; set; }
        public string Username { get;set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int AvatarId { get; set; } = 1;
        public bool PremiumAccount { get; set; } = false;
        public int? PaymentDetailsId { get; set; }
        public bool Banned { get; set; } = false;
        public byte[] RowVersion { get; set; }


        public UserAccount(Guid id, string username, string email, DateTime registrationDate, int avatarId, bool premiumAccount, int? paymentDetailsId, bool banned, byte[] rowVersion) {
            Id = id;
            Username = username;
            Email = email;
            RegistrationDate = registrationDate;
            AvatarId = avatarId;
            PremiumAccount = premiumAccount;
            PaymentDetailsId = paymentDetailsId;
            Banned = banned;
            RowVersion = rowVersion;
        }

        public UserAccount Clone() {
            return new UserAccount(Id, Username, Email, RegistrationDate, AvatarId, PremiumAccount, PaymentDetailsId, Banned, RowVersion);
        }
    }
}