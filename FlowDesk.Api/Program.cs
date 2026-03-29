using FlowDesk.Api.Filters;
using FlowDesk.Api.Middleware;
using FlowDesk.Data;
using FlowDesk.Data.UnitOfWork;
using FlowDesk.Services.DTOs;
using FlowDesk.Services.Services;
using FlowDesk.Services.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=FlowDeskDb;Trusted_Connection=true;";
builder.Services.AddDbContext<FlowDeskDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IValidator<CreateTaskRequest>, CreateTaskRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateTaskRequest>, UpdateTaskRequestValidator>();
builder.Services.AddScoped<IValidator<CreateProjectRequest>, CreateProjectRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateProjectRequest>, UpdateProjectRequestValidator>();
builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/flowdesk-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
