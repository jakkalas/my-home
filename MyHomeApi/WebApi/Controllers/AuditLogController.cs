using Microsoft.AspNetCore.Mvc;
using MyHomeApi.Entities;
using MyHomeApi.Services;

namespace MyHomeApi.Controllers
{
	public class AuditLogController : Controller
	{
		private IAuditLogService _auditLogService;

		public AuditLogController(IAuditLogService auditLogService)
		{
			_auditLogService = auditLogService;
		}

		[HttpGet]
		public IActionResult GetAuditLog(int pageSize, int pageNumber)
		{
			var auditLogs = _auditLogService.GetAuditLogs(pageSize, pageNumber);
			return Ok(auditLogs);
		}
	}
}
