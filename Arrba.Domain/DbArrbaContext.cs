using System;
using Arrba.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Domain
{
    public class DbArrbaContext : IdentityDbContext<User, Role, long>
    {
        public DbArrbaContext(DbContextOptions options)
            : base(options)
        {
        }  

        public DbSet<Balance> Balances { get; set; }
        public DbSet<BalanceTransaction> BalanceTransactions { get; set; }
        public DbSet<SuperCategory> SuperCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategGroup> CategGroups { set; get; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ItemModel> ItemModels { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyCateg> PropertyCategs { get; set; }
        public DbSet<PropertyGroup> PropertyGroups { get; set; }
        public DbSet<CategBrand> CategBrands { get; set; }
        public DbSet<CategType> CategTypes { get; set; }
        public DbSet<UserPhone> UserPhones { get; set; }
        public DbSet<ServicePrice> ServicePrices { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<SelectOption> SelectOptions { get; set; }
        public DbSet<CheckBoxGroup> CheckBoxGroups { get; set; }
        public DbSet<PropertyCheckBoxGroup> PropertyCheckBoxGroups { get; set; }
        public DbSet<Dealership> Dealerships { get; set; }
        public DbSet<AdVehicle> AdVehicles { get; set; }
        public DbSet<DynamicPropertyAdVehicle> DynamicPropertyAdVehicles { get; set; }
        public DbSet<AdVehicleServiceStore> AdVehicleServiceStores { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity => entity.ToTable("Users"));
            builder.Entity<Role>(entity => entity.ToTable("Roles"));
            builder.Entity<IdentityUserRole<long>>(entity => entity.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<long>>(entity => entity.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<long>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<long>>(entity => entity.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<long>>(entity => entity.ToTable("UserTokens"));

            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "admin@mail.ru",
                NormalizedUserName = "ADMIN@MAIL.RU",
                Email = "admin@mail.ru",
                NormalizedEmail = "ADMIN@MAIL.RU",
                // 123456Ru!
                PasswordHash = "AQAAAAEAACcQAAAAEA7vpzNBUIMLvB4bdfb8xX5IIsMZ86GfG1In4YX3q8BYyZoFQYSGuVOVWB3XfCdZOA==",
                SecurityStamp = "6FPVSOIY6BCPOQH4BZUCNYIKQ5WB4VRM",
                ConcurrencyStamp = "9ac88e11-e377-4a49-bef9-ed7767ff6f9d",
                UserNickName = "admin@mail.ru",
                RegistrationDate = DateTime.Now,
                LastLogin = DateTime.Now,
            });

            builder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Admin",
            });

            builder.Entity<IdentityUserRole<long>>().HasData(new IdentityUserRole<long>
            {
                RoleId = 1,
                UserId = 1
            });

            builder.ApplyConfiguration(new AdVehicleConfiguration());
            builder.ApplyConfiguration(new BrandConfiguration());
            builder.ApplyConfiguration(new CheckBoxGroupConfiguration());
            builder.ApplyConfiguration(new CurrencyConfiguration());
            builder.ApplyConfiguration(new DealershipConfiguration());
            builder.ApplyConfiguration(new ItemTypeConfiguration());
            builder.ApplyConfiguration(new PropertyConfiguration());
            builder.ApplyConfiguration(new SelectOptionConfiguration());
            builder.ApplyConfiguration(new ServicePriceConfiguration());
            builder.ApplyConfiguration(new SuperCategoryConfiguration());
            builder.ApplyConfiguration(new CategBrandConfiguration());
            builder.ApplyConfiguration(new CategTypeConfiguration());
            builder.ApplyConfiguration(new DynamicPropertyAdVehicleConfiguration());
            builder.ApplyConfiguration(new PropertyCategConfiguration());
            builder.ApplyConfiguration(new PropertyCheckBoxGroupConfiguration());
        }
    }
}
