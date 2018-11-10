using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using kw.Models;

namespace kw.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<kw.Models.ThemeModel> ThemeModel { get; set; }
        public DbSet<kw.Models.Comment> Comment { get; set; }
    }
}
