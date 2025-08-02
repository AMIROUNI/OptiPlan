using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using OptiPlanBackend.Data;
using OptiPlanBackend.Repositories.Implementations;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Implementations;
using OptiPlanBackend.Services.Interfaces;
using OptiPlanBackend.Settings;
using Scalar.AspNetCore;
using System;
using System.Text;
using System.Text.Json.Serialization;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDatabase")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});




// Load .env file
DotNetEnv.Env.Load();

builder.Services.Configure<SmtpSettings>(options =>
{
    options.Email = Environment.GetEnvironmentVariable("SMTP_EMAIL");
    options.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
    options.Host = Environment.GetEnvironmentVariable("SMTP_HOST");
    options.Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
});



// Add JSON options for handling circular references and enum serialization

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //  Handle circular references (you already have this)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        //  Enable string-based enum deserialization
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });



//-------------------------------------------------------------------
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IProjectRepository), typeof(ProjectRepository));
builder.Services.AddScoped(typeof(IWorkItemRepository), typeof(WorkItemRepository));
builder.Services.AddScoped(typeof(ISprintRepository), typeof(SprintRepository));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(ITeamMembershipRepository), typeof(TeamMembershipRepository));
builder.Services.AddScoped(typeof(ITeamRepository), typeof(TeamRepository));
builder.Services.AddScoped(typeof(IInvitationRepository),typeof( InvitationRepository));
builder.Services.AddScoped(typeof(ICommentRepository),typeof( CommentRepository));
builder.Services.AddScoped(typeof(IAttachmentRepository), typeof(AttachmentRepository));


//-------------------------------------------------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IWorkItemService, WorkItemService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped(typeof(ISprintService), typeof(SprintService));
builder.Services.AddScoped(typeof(ITeamMembershipService), typeof(TeamMembershipService));
builder.Services.AddScoped(typeof(ITeamService), typeof(TeamService));
builder.Services.AddScoped(typeof(ICommentService), typeof(CommentService));
builder.Services.AddScoped(typeof(IAttachmentService), typeof(AttachmentService));








builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
