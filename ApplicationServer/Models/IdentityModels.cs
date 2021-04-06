using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationServer.Models {
    //You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
   
    public class ApplicationUser : IdentityUser {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder) {

        //    base.OnModelCreating(modelBuilder);

        //    var user = modelBuilder.Entity<IdentityUser>().HasKey(u => u.Id).ToTable("UserAccount", "Decks"); //Specify our our own table names instead of the defaults

        //    user.Property(iu => iu.Id).HasColumnName("Id");
        //    user.Property(iu => iu.UserName).HasColumnName("Username");
        //    user.Property(iu => iu.Email).HasColumnName("Email").HasMaxLength(254).IsRequired();
        //    user.Property(iu => iu.PasswordHash).HasColumnName("PasswordHash");

        //}

    }
  
}