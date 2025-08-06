using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiraApi.Models.Authentication;
using YiraHealthCampManagerAPI.Models.Account;
using YiraHealthCampManagerAPI.Models.CampRequest;
using YiraHealthCampManagerAPI.Models.Organization;

namespace YiraHealthCampManagerAPI.Context
{
    public sealed class YiraDbContext : IdentityDbContext<ApplicationUser>
    {
        public YiraDbContext(DbContextOptions<YiraDbContext> options) : base(options)
        {

        }

        #region
        public DbSet<GetLogomainRes> GetLogoRes { get; set; }
        public DbSet<OrgUsers> OrgUsers { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserData> UserData { get; set; }

        #endregion


        #region

        public DbSet<HealthCampRequest> HealthCampRequest { get; set; }
        public DbSet<HealthService> HealthService { get; set; }
        public DbSet<RequestedService> RequestedService { get; set; }

        #endregion


        #region
        public DbSet<Organizations> organizations { get; set; }
        public DbSet<OrganizationTypes> OrganizationTypes { get; set; }

        #endregion




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable(tb =>
                {
                    tb.HasTrigger("TR_INS_UHealthScores");
                    tb.HasTrigger("UserFoodRecommendations_trg_recomm_insert");
                });


                b.Property(u => u.UniqueUserName).ValueGeneratedOnAddOrUpdate();
            });

        }

    }
}
