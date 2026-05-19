using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false, // 発行元の検証を無効化
			ValidateAudience = false, // 対象の検証を無効化
			ValidateLifetime = false, // 有効期限の検証を無効化
			ValidateIssuerSigningKey = false, // 署名キーの検証を無効化
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
		};
	});


//別サイトからの通信を許可する設定
//今回はindex.html→fetch()→https://Localhost:7113へアクセスしている
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		policy =>
		{
			policy
				.AllowAnyOrigin() // どのURLからでもアクセス可能
				.AllowAnyHeader() // どんなHTTP Headerでも許可
				.AllowAnyMethod(); // Get/POST/PUT/DELETEすべてのHTTPメソッド許可
		});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Addcorsで設定登録したのを、UseCorsで初めて実行される
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
