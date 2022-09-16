using Microsoft.EntityFrameworkCore;
using MyHomeApi.Entities;
using System.Collections.Generic;

namespace MyHomeApi.Helpers
{
	public class DataContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		private readonly IConfiguration Configuration;

		public DataContext(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
#if DEBUG
			// in memory database
			options.UseInMemoryDatabase("TestDb");
#else
			options.UseSqlite(Configuration.GetConnectionString("MyHomeDatabase"));
#endif
		}
	}
}
