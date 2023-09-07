using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TechnicalTest2023.DbContext;
using TechnicalTest2023.Logging;
using TechnicalTest2023.Services.Impl;
using TechnicalTest2023.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseInMemoryDatabase("UserList"));
builder.Services.AddDbContext<AddressContext>(opt =>
    opt.UseInMemoryDatabase("AddressList"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<HttpContextEnricher>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        var error = context.HttpContext.Features.Get<EnrichedErrors>();
        if (error == null) return;

        var type = error.EnrichedError switch
        {
            EnrichedErrorType.InvalidUserInputError =>
                "Invalid input provided.",
            _ => "User already exists.",
        };

        context.ProblemDetails.Type = type;
        context.ProblemDetails.Detail = error.ErrorDetails;
    };
});

builder.Host.UseSerilog((_, serviceProvider, loggerConfiguration) =>
{
    var enricher = serviceProvider.GetRequiredService<HttpContextEnricher>();

    loggerConfiguration
        .Enrich.FromLogContext()
        .Enrich.With(enricher)
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] {HttpContext} {Message:lj}{NewLine}{Exception}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
