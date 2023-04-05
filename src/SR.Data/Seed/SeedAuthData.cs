using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SR.Shared.Entities;
using SR.Shared.Identity;

namespace SR.Data.Seed
{
	public static class SeedAuthData
	{
		public static void Seed(IApplicationBuilder builder)
		{
			// seed the database.  Best practice = in Main, using service scope
			using var scope = builder.ApplicationServices.CreateScope();

			// var services = scope.ServiceProvider;
			try
			{
				using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				context.Database.Migrate();

				// Role Manager
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				// Seed roles      
				if (!context.Roles.Any())
				{
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.Administrator)).Result;
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.CrewRole)).Result;
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.OperationAshaiman)).Result;
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.OperationKpone)).Result;
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.OperationMain)).Result;
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.PassportRole)).Result;
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.PermitRole)).Result;
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.ReadOnly)).Result;
					_ = roleManager.CreateAsync(new IdentityRole(UserRoles.RevenueRole)).Result;
				}

				// Seed Operation Offices
				if (!context.OperationOffices.Any())
				{
					var offices = new List<OperationOffice>()
					{
						new() {OfficeName = "Operation Office - Main", Location = "Tema", Role=UserRoles.OperationMain},
						new() {OfficeName = "Kpone Operation Office", Location = "Kpone", Role=UserRoles.OperationKpone},
						new() {OfficeName = "Ashaiman Operation Office", Location = "Ashaiman", Role=UserRoles.OperationAshaiman},
					};
					context.OperationOffices.AddRange(offices);
					context.SaveChanges();
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}