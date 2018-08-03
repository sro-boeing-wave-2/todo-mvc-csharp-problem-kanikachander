using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;

namespace NotosAPI.Models
{
    public class NotosAPIContext : DbContext
    {
        public NotosAPIContext (DbContextOptions<NotosAPIContext> options)
            : base(options)
        {
        }

        public DbSet<NotesAPI.Models.Notes> Notes { get; set; }
        //public DbSet<NotesAPI.Models.CheckedList> CheckedList { get; set; }
        //public DbSet<NotesAPI.Models.Labels> Labels { get; set; }
    }
}
