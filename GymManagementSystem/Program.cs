using System.Text;
using Gym_Management_System.Data;
using Gym_Management_System.Services;
using GymManagementSystem.Config;
using GymManagementSystem.Controllers;
using GymManagementSystem.Middleware;
using GymManagementSystem.Models;
using GymManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container .
builder.Services.Configure<JwtOption>(
    builder.Configuration.GetSection(JwtOption.SectionName)
);

builder.Services.AddIdentity<AccountModel, IdentityRole>(options => options.User.RequireUniqueEmail = true)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtConfig = builder.Configuration.GetSection(JwtOption.SectionName).Get<JwtOption>();
    var secret = jwtConfig.Secret;
    var issuer = jwtConfig.ValidIssuer;
    var audience = jwtConfig.ValidationAudiences;

    if (secret == null || issuer == null || audience == null) throw new ApplicationException("JWTConfig is missing");

    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<RegisterController>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<AuthController>();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection(EmailConfig.SectionName));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync(AppRoles.Admin))
        await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));

    if (!await roleManager.RoleExistsAsync(AppRoles.Employee))
        await roleManager.CreateAsync(new IdentityRole(AppRoles.Employee));

    if (!await roleManager.RoleExistsAsync(AppRoles.Client))
        await roleManager.CreateAsync(new IdentityRole(AppRoles.Client));
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();