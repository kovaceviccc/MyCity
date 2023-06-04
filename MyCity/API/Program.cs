using API.Controllers;
using API.Extensions;
using API.Hubs;
using Entities.DbSet;
using Entities.Domain.Enums;
using Entities.Repository;
using Entities.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//TODO: add auto mapper

//database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


//auth
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]!)),
    ValidateIssuer = false,
    ValidateAudience = false, //OVO JE PRAVILO PROBLEM SISU MU 
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(jwt =>
    {
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = tokenValidationParameters;


        jwt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"].ToString();

                var token = !string.IsNullOrEmpty(accessToken)
                    ? accessToken
                    : string.Empty;

                var path = context.HttpContext.Request.Path;

                // Check if request is for hub
                if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/hubs"))
                {
                    context.HttpContext.Items["Id"] = TokenHelper.GetUserIdClaimFromToken(token);
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };

    });
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddDefaultTokenProviders();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(RolesEnum.Admin.ToString(), policy =>
    {
        policy.AuthenticationSchemes = new List<string>() { JwtBearerDefaults.AuthenticationScheme };
        policy.RequireRole(RolesEnum.Admin.ToString());

    });
    opt.AddPolicy(RolesEnum.User.ToString(), policy =>
    {
        policy.AuthenticationSchemes = new List<string>() { JwtBearerDefaults.AuthenticationScheme };
        policy.RequireRole(RolesEnum.User.ToString());
    });
    opt.AddPolicy(RolesEnum.AuthorizedPersonel.ToString(), policy =>
    {
        policy.AuthenticationSchemes = new List<string>() { JwtBearerDefaults.AuthenticationScheme };
        policy.RequireRole(RolesEnum.AuthorizedPersonel.ToString());
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<EventHub>("/hubs/event");

app.Run();
