using MarcketDataLayer.Entities.Accuont.RolePermission;
using MarcketDataLayer.Entities.Articles;
using MarcketDataLayer.Entities.ContactUs;
using MarcketDataLayer.Entities.ProductOrder;
using MarcketDataLayer.Entities.Products;
using MarcketDataLayer.Entities.Site;
using MarcketDataLayer.Entities.Slider;
using MarcketDataLayer.Entities.Store;
using MarcketDataLayer.Entities.Wallet;
using MarketPlace.DataLayer.Entities.Accuont.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Context
{
   public class MarketDBContext:DbContext
    {
        public MarketDBContext(DbContextOptions<MarketDBContext> options) : base(options) { }
        #region Accuont
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        #endregion

        #region site

        public DbSet<SiteSetting> SiteSettings { get; set; }

        #endregion
        #region contacts

        public DbSet<ContactUs> ContactUses { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketMessage> TicketMessages { get; set; }

        #endregion
        #region Slider

        public DbSet<Slider> Slider { get; set; }

        #endregion

        #region site
        public DbSet<SiteBanner> SiteBanners { get; set; }

        #endregion

        #region store

        public DbSet<Seller> Sellers { get; set; }

       
      


        

        #endregion

        #region products

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public DbSet<ProductColor> ProductColor { get; set; }

        public DbSet<ProductGallery> ProductGalleries { get; set; }

        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public DbSet<ProductCommet> ProductCommet { get; set; }

        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<UserCompare> UserCompares { get; set; }



        #endregion

        #region order

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderIsPaidDetials> OrderIsPaidDetials { get; set; }

        #endregion

        #region wallet

        public DbSet<SellerWallet> SellerWallets { get; set; }

        #endregion

        #region propduct discount

        public DbSet<ProductDiscount> ProductDiscounts { get; set; }

        public DbSet<ProductDiscountUse> ProductDiscountUses { get; set; }

        #endregion
        #region Articles
        public DbSet<ArticlesGroups> ArticlesGroups { get; set; }


        public DbSet<Articles> Articles { get; set; }

        

        public DbSet<ArticlesComment> ArticlesComment { get; set; }





        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }



          //  modelBuilder.Entity<ArticlesComments>()

          //.HasOne<Articles>(f => f.Articlesid)

          //.WithMany(g => g.ArticlesComments);

            


            base.OnModelCreating(modelBuilder);


        }


    }
}
