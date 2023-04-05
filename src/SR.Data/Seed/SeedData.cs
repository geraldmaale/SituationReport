using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SR.Shared.Entities;

namespace SR.Data.Seed
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder builder)
        {
            // seed the database.  Best practice = in Main, using service scope
            using var scope = builder.ApplicationServices.CreateScope();

            // var services = scope.ServiceProvider;
            try
            {
	            Random random = new ();
	            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                #region HQ-Office

                if (!context.Office.Any())
                {
	                var newOffice = new Office()
	                {
		                Name = "Tema Headquaters",
		                Address = "PMB Tema",
		                City = "Tema",
		                Region = "Greeter Accra",
		                Email = "info@ceps.gov.gh",
		                GpsCode = "GP-00-00-00"
	                };
	                context.Office.Add(newOffice);
	                context.SaveChanges();
                }

                #endregion

                /*#region Officers

                if (context.Officers.Count()<2)
                {
	                var officers = new List<Officer>();
	                for (int i = 1; i <= 3; i++)
	                {
		                var names = random.Next(Preprocess.Names.Count);
		                var surnames = random.Next(Preprocess.Surnames.Count);
		                var firstName = Preprocess.Names[names];
		                var lastName = textInfo.ToTitleCase(Preprocess.Surnames[surnames].ToLower());
		                var entity = new Officer()
		                {
			                FirstName = firstName,
			                LastName = lastName,
			                Gender = (GenderEnum)random.Next(0, 2),
			                Rank = (RankEnum)random.Next(2, 12),
			                PhoneNumber = "0214145789",
			                FullName = $"{firstName} {lastName}"
		                };

		                officers.Add(entity);
	                }
	                context.Officers.AddRange(officers);
                context.SaveChanges();
                }
                #endregion*/

                #region PermitTypes

                if (!context.PermitTypes.Any())
                {
	                var permitType = new PermitType(){Name = "Stay Permit"};
	                context.PermitTypes.Add(permitType);
	                context.SaveChanges();
                }

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}