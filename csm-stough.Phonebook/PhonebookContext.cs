using csm_stough.Phonebook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csm_stough.Phonebook
{
    public class PhonebookContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public string DbPath { get; }

        public PhonebookContext()
        {
            DbPath = System.Configuration.ConfigurationManager.AppSettings.Get("DbConnString");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(DbPath);
    }
}
