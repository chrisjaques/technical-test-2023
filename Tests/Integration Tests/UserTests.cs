using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechnicalTest2023.DbContext;
using TechnicalTest2023.Models;
using TechnicalTest2023.Services.Impl;
using TechnicalTest2023.Services.Interfaces;

namespace Tests.Integration_Tests;

public class UserTests
{
    [Fact]
    public async Task Get_users_returns_200()
    {
        var application = GetWebApplication();
        var client = application.CreateClient();

        await GenerateSomeUsers(application); // Populate database

        var response = await client.GetAsync("/api/Users");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_user_by_valid_id_returns_200()
    {
        var application = GetWebApplication();
        var client = application.CreateClient();

        await GenerateSomeUsers(application); // Populate database

        var response = await client.GetAsync("/api/Users/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_user_by_invalid_id_returns_404()
    {
        var application = GetWebApplication();
        var client = application.CreateClient();

        await GenerateSomeUsers(application); // Populate database

        var response = await client.GetAsync("/api/Users/0");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_user_returns_201()
    {
        var application = GetWebApplication();
        var client = application.CreateClient();

        await GenerateSomeUsers(application); // Populate database

        var user = new
        {
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
            FirstName = "Chris",
            LastName = "J",
            Address = new Address
            {
                City = "Wellington",
                StreetName = "The Terrace",
                StreetNumber = 15 // Changing this so we know it will always succeed, rest of tests use "1" 
            }
        };

        var response = await client.PostAsJsonAsync("/api/Users", user);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_duplicate_user_returns_409()
    {
        var application = GetWebApplication();
        var client = application.CreateClient();

        await GenerateSomeUsers(application); // Populate database

        var response = await client.PostAsJsonAsync("/api/Users", GetCoreUser()); // core user has already been added

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Post_invalid_user_returns_400()
    {
        var application = GetWebApplication();
        var client = application.CreateClient();

        await GenerateSomeUsers(application); // Populate database

        var response = await client.PostAsJsonAsync("/api/Users", GetInvalidUser());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_user_persists_in_db()
    {
        var application = GetWebApplication();
        var client = application.CreateClient();

        // Intentionally not populating the database
        
        var coreUser = GetCoreUser();

        var response = await client.PostAsJsonAsync("/api/Users", coreUser);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var services = application.Services.CreateScope();
        var userCtx = services.ServiceProvider.GetRequiredService<UserContext>();
        var user = await userCtx.Users.Include(x => x.Address).FirstAsync();
        Assert.Equal(coreUser.DateOfBirth, user.DateOfBirth);
        Assert.Equal(coreUser.FirstName, user.FirstName);
        Assert.Equal(coreUser.LastName, user.LastName);

        Assert.Equal(coreUser.Address.StreetNumber, user.Address.StreetNumber);
        Assert.Equal(coreUser.Address.StreetNumberSuffix, user.Address.StreetNumberSuffix);
        Assert.Equal(coreUser.Address.StreetName, user.Address.StreetName);
        Assert.Equal(coreUser.Address.Suburb, user.Address.Suburb);
        Assert.Equal(coreUser.Address.City, user.Address.City);
        Assert.Equal(coreUser.Address.PostCode, user.Address.PostCode);
    }

    // Made a method instead of a field just to move to the bottom and make tests easier to read
    private static User GetCoreUser() => new()
    {
        DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
        FirstName = "Chris",
        LastName = "J",
        Address = new Address
        {
            City = "Wellington",
            StreetName = "The Terrace",
            StreetNumber = 1,
            StreetNumberSuffix = "A",
            Suburb = "Wellington Central",
            PostCode = "6011"
        }
    };

    private static User GetInvalidUser() => new()
    {
        DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
        FirstName = "Chris",
        LastName = "J",
        Address = new Address
        {
            City = "Wellington",
            StreetName = "The Terrace",
            StreetNumber = 1,
            PostCode = "123456789toolong"
        }
    };

    private async Task GenerateSomeUsers(WebApplicationFactory<Program> application)
    {
        using var services = application.Services.CreateScope();
        var ctx = services.ServiceProvider.GetRequiredService<UserContext>();
        await ctx.AddAsync(GetCoreUser());
        for (var i = 0; i < 5; i++)
        {
            var randomStringForUserInfo = Random.Shared.Next(10000).ToString();
            var randomStringForAddressInfo = Random.Shared.Next(10000).ToString();
            await ctx.AddAsync(new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                FirstName = randomStringForUserInfo,
                LastName = randomStringForUserInfo,
                Address = new Address
                {
                    City = randomStringForAddressInfo,
                    StreetName = randomStringForAddressInfo,
                    StreetNumber = 1
                }

            });
        }
        await ctx.SaveChangesAsync();
    }

    private WebApplicationFactory<Program> GetWebApplication()
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                // Configure user db
                var userContextOptions = new DbContextOptionsBuilder<UserContext>()
                    .UseInMemoryDatabase("Test-UserList")
                    .Options;
                var userContext = new UserContext(userContextOptions);
                userContext.Database.EnsureDeleted();
                services.AddSingleton(userContext);

                // Configure address db
                var addressContextOptions = new DbContextOptionsBuilder<AddressContext>()
                    .UseInMemoryDatabase("Test-AddressList")
                    .Options;
                var addressContext = new AddressContext(addressContextOptions);
                addressContext.Database.EnsureDeleted();
                services.AddSingleton(addressContext);

                services.AddSingleton<IUserService, UserService>();
            });
        });
}