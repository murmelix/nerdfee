using Microsoft.EntityFrameworkCore;
using Nerdfee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdfee.Data
{
    public class NerdfeeContext : DbContext
    {
        public NerdfeeContext(DbContextOptions<NerdfeeContext> options) : base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleImage> ArticleImages { get; set; }
    }
}
