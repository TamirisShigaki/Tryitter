using System.Security.Claims;
using System.Text;
using tryitter.Constants;
using tryitter.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using tryitter.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<TryitterContext>();
builder.Services.AddScoped<StudentRepository>();
<<<<<<< HEAD
// builder.Services.AddScoped<PostRepository>();
=======
builder.Services.AddScoped<PostRepository>();
>>>>>>> 09207d47446b3c226ff021d6ba94c215c21c3c3c
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenConstants.Secret))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StudentName", policy =>
    policy.RequireClaim(ClaimTypes.Name));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(doc =>
    {
        doc.SwaggerEndpoint("/Swagger/v1/Swagger.json", "Tryitter v1");
        doc.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program { }