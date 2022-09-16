﻿using Microsoft.Extensions.Options;
using BCrypt.Net;
using MyHomeApi.Authorization;
using MyHomeApi.Entities;
using MyHomeApi.Helpers;
using MyHomeApi.Infrastructure.Models.Users;

namespace MyHomeApi.Services
{
	public interface IUserService
	{
		AuthenticateResponse Authenticate(AuthenticateRequest model);
		IEnumerable<User> GetAll();
		User GetById(int id);
	}

	public class UserService : IUserService
	{
		private DataContext _context;
		private IJwtUtils _jwtUtils;
		private readonly AppSettings _appSettings;

		public UserService(
			DataContext context,
			IJwtUtils jwtUtils,
			IOptions<AppSettings> appSettings)
		{
			_context = context;
			_jwtUtils = jwtUtils;
			_appSettings = appSettings.Value;
		}


		public AuthenticateResponse Authenticate(AuthenticateRequest model)
		{
			var user = _context.Users.SingleOrDefault(x => x.Email == model.Username);

			// validate
			if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
				throw new AppException("Username or password is incorrect");

			// authentication successful so generate jwt token
			var jwtToken = _jwtUtils.GenerateJwtToken(user);

			return new AuthenticateResponse(user, jwtToken);
		}

		public IEnumerable<User> GetAll()
		{
			return _context.Users;
		}

		public User GetById(int id)
		{
			var user = _context.Users.Find(id);
			if (user == null) throw new KeyNotFoundException("User not found");
			return user;
		}
	}
}
