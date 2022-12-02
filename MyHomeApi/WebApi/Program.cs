using MyHomeApi.Authorization;
using MyHomeApi.Entities;
using MyHomeApi.Helpers;
using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink;
using MyHomeApi.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
	var services = builder.Services;
	var env = builder.Environment;

	services.AddDbContext<DataContext>();
	services.AddCors();
	services.AddControllers().AddJsonOptions(x =>
	{
		// serialize enums as strings in api responses (e.g. Role)
		x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});
	services.AddEndpointsApiExplorer();
	services.AddSwaggerGen();
	services.AddHttpClient<EweLinkService>();

	// configure strongly typed settings object
	services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

	// configure DI for application services
	services.AddScoped<IJwtUtils, JwtUtils>();
	services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// configure HTTP request pipeline
{
	// global cors policy
	app.UseCors(x => x
		.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader());

	// global error handler
	app.UseMiddleware<ErrorHandlerMiddleware>();

	// custom jwt auth middleware
	app.UseMiddleware<JwtMiddleware>();
	app.UseHttpsRedirection();

	app.MapControllers();
}

// create hardcoded test users in db on startup
{
	using var scope = app.Services.CreateScope();
	var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
	if (!dataContext.Users.Any())
	{
		var testUsers = new List<User>
		{
			new User { Id = 1, FirstName = "Admin", LastName = "User", Email = builder.Configuration["UserSettings:AdminEmail"], PasswordHash = BCrypt.Net.BCrypt.HashPassword(builder.Configuration["UserSettings:AdminPassword"]), Role = Role.Admin },
			new User { Id = 2, FirstName = "Normal", LastName = "User", Email = builder.Configuration["UserSettings:NonAdminEmail"], PasswordHash = BCrypt.Net.BCrypt.HashPassword(builder.Configuration["UserSettings:NonAdminPassword"]), Role = Role.User }
		};
		dataContext.Users.AddRange(testUsers);
		dataContext.SaveChanges();
	}
}

app.Run();
