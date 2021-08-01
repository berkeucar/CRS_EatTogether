using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EatTogether.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public List<Session_ApplicationUser> SessionUserTable{ get; set; }
        public int ScoredTimes { get; set; }
        public double Score { get; set; }
        public List<RatedUserModel> RatedUsersList { get; set; }
        public List<RatedUserModel> RatedUsersList2 { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            modelBuilder.Entity<Session_ApplicationUser>()
                .HasRequired(x => x.Session)
                .WithMany(y => y.SessionUserTable)
                .HasForeignKey(z => z.SessionId);

            modelBuilder.Entity<Session_ApplicationUser>()
                .HasRequired(x => x.User)
                .WithMany(y => y.SessionUserTable)
                .HasForeignKey(z => z.ApplicationUserId);
            
            modelBuilder.Entity<RatedUserModel>()
               .HasRequired(x => x.UserRated)
               .WithMany(y => y.RatedUsersList)
               .HasForeignKey(z => z.UserRatedId).WillCascadeOnDelete(false); ;
           
            modelBuilder.Entity<RatedUserModel>()
                .HasRequired(x => x.UserRates)
                .WithMany(y => y.RatedUsersList2)
                .HasForeignKey(z => z.UserRatesId).WillCascadeOnDelete(false); ;
        }

        public DbSet<SessionModel> Session { get; set; }
        public DbSet<PlaceModel> Places { get; set; }
        public DbSet<Session_ApplicationUser> SessionUsers { get; set; }
        public DbSet<RatedUserModel> RatedUsers { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}