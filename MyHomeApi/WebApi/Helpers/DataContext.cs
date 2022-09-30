using Microsoft.EntityFrameworkCore;
using MyHomeApi.Entities;
using System.Collections.Generic;

namespace MyHomeApi.Helpers
{
	public class DataContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<AuditLog> AuditLogs { get; set; }

		private readonly IConfiguration Configuration;

		public DataContext(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseSqlite(Configuration.GetConnectionString("MyHomeDatabase"));
		}
	}
}
