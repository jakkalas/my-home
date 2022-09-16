using System.ComponentModel.DataAnnotations;

namespace MyHomeApi.Infrastructure.Models.Users
{
	public class AuthenticateRequest
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
