using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

//JWT署名・検証用クラス
using Microsoft.IdentityModel.Tokens;
//JWT生成用クラス
using System.IdentityModel.Tokens.Jwt;
//ユーザー情報を保持
using System.Security.Claims;
//文字列→バイト変換用
using System.Text;
using TodoApi.Dtos;

namespace TodoApi.Controllers
{

	[ApiController]
	//https://localhost:7113/Auth
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		public AuthController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpPost("login")]
		public IActionResult Login(LoginDto dto)
		{
			if (dto.Username != "test" || dto.Password != "pass")
			{
				//認証失敗　401を返す
				return Unauthorized(new
				{
					message = "ログイン失敗"
				});
			}

			//JWTへ入れる情報
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, dto.Username),
				new Claim(ClaimTypes.Role, "User")
			};
			//JWT署名キー作成
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
			//署名方式設定
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			//JWT本体作成
			var token = new JwtSecurityToken(
				claims: claims,						//ユーザ情報
				expires: DateTime.Now.AddHours(1),	//有効期限
				signingCredentials: credentials		//改竄防止署名
				);

			//JWT文字列化
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return Ok(new
			{
				token = jwt,
				message = "ログイン成功",
			});
		}
	}
}
