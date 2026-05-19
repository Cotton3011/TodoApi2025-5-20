using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TodoController : Controller
	{
		// POST: https://Localhost:7113/Todo
		[HttpPost]
		public IActionResult Create(CreateTodoDto dto)
		{
			return Ok(new { message = "受信成功", title = dto.Title });
		}

	[Authorize]
		[HttpGet("protected")]
		public IActionResult GetProtectedTodo()
		{
			return Ok(new
			{
				message = "認証済みユーザーだけ見られるデータです"
			});
		}
	}
}
