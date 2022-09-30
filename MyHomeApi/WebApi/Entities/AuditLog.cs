namespace MyHomeApi.Entities
{
	public class AuditLog
	{
		public int Id { get; set; }
		public string Action { get; set; }
		public int HttpResponseCode { get; set; }
		public int UserId { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
