using CountryProject.Models;
using HoildaysProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoildaysProject.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {

        }

        public DbSet<Holiday> holidays { get; set; }
        public DbSet<Country> countries { get; set; }
    }
}
