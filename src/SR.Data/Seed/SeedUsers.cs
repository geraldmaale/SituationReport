using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SR.Shared.Entities;
using SR.Shared.Identity;

namespace SR.Data.Seed;

public static class SeedUsers
{
	public static void Users(IApplicationBuilder builder)
	{
		// seed the database.  Best practice = in Main, using service scope
		using var scope = builder.ApplicationServices.CreateScope();

		// var services = scope.ServiceProvider;
		try
		{
			using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			// User manager
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			// Admin
			var getAdmin = userManager.FindByNameAsync("support@sdesk.org").Result;
			if (getAdmin == null)
			{
				// var userRole = roleManager.FindByNameAsync(UserRoles.Administrator).Result;

				var officer = new Officer()
				{
					FirstName = "Gerald",
					LastName = "Maale",
					Gender = GenderEnum.Male,
					Rank = RankEnum.Inspector,
					PhoneNumber = "0200000000",
					FullName = "Gerald Maale",
					Username = "support@sdesk.org",
					Email = "support@sdesk.org"
				};

				var newOfficer = context.Officers.Add(officer);

				var admin = new ApplicationUser()
				{
					UserName = officer.Username,
					FullName = $"{officer.FirstName} {officer.LastName}",
					EmailConfirmed = true,
					Email = officer.Email,
					PhoneNumber = officer.PhoneNumber,
					OfficerId = newOfficer.Entity.OfficerId
				};

				var result = userManager.CreateAsync(admin, "Pa$$word1").Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				// Add role
				result = userManager.AddToRoleAsync(admin, UserRoles.Administrator).Result;
				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				result = userManager.AddClaimsAsync(admin, new Claim[] {
					new Claim(JwtClaimTypes.Name, $"{admin.FullName}"),
					new Claim(JwtClaimTypes.Id, admin.Id),
					new Claim(JwtClaimTypes.Email, admin.Email),
					new Claim("OfficerId", officer.OfficerId.ToString()),
				}).Result;

				if (!result.Succeeded)
				{
					throw new Exception(result.Errors.First().Description);
				}

				context.SaveChanges();
			}

		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}
}