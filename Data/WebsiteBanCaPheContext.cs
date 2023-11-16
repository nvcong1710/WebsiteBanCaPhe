using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteBanCaPhe.Models;

namespace WebsiteBanCaPhe.Data
{
    public class WebsiteBanCaPheContext : DbContext
    {
        public WebsiteBanCaPheContext (DbContextOptions<WebsiteBanCaPheContext> options)
            : base(options)
        {
        }

        public DbSet<WebsiteBanCaPhe.Models.Account> Account { get; set; } = default!;

        public DbSet<WebsiteBanCaPhe.Models.Cart>? Cart { get; set; }

        public DbSet<WebsiteBanCaPhe.Models.CartDetail>? CartDetail { get; set; }

        public DbSet<WebsiteBanCaPhe.Models.Category>? Category { get; set; }

        public DbSet<WebsiteBanCaPhe.Models.UserOrder>? UserOrder { get; set; }

        public DbSet<WebsiteBanCaPhe.Models.OrderDetail>? OrderDetail { get; set; }

        public DbSet<WebsiteBanCaPhe.Models.Product>? Product { get; set; }

        public DbSet<WebsiteBanCaPhe.Models.Feedback>? Feedback { get; set; }
    }
}
