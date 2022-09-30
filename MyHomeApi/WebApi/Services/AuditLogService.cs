using MyHomeApi.Entities;
using MyHomeApi.Helpers;

namespace MyHomeApi.Services
{
	public interface IAuditLogService
	{
		void AddAuditLog(AuditLog auditLog);

		IEnumerable<AuditLog> GetAuditLogs(int pageNumber, int pageSize);
	}

	public class AuditLogService : IAuditLogService
	{
		private DataContext _context;

		public AuditLogService(DataContext context)
		{
			_context = context;
		}

		public void AddAuditLog(AuditLog auditLog)
		{
			_context.AuditLogs.Add(auditLog);
			_context.SaveChanges();
		}

		public IEnumerable<AuditLog> GetAuditLogs(int pageNumber, int pageSize)
		{
			return _context.AuditLogs
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize);
		}
	}
}
