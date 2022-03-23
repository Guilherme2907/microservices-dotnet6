using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly SqlServerContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(SqlServerContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public async Task Initialize()
        {
            if (await _role.FindByNameAsync(IdentityConfiguration.ADMIN) != null) return;

            await _role.CreateAsync(new IdentityRole(IdentityConfiguration.ADMIN));
            await _role.CreateAsync(new IdentityRole(IdentityConfiguration.CLIENT));


            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "guilherme-admin",
                Email = "guilherme-admin@geekshopping.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (99) 99999-9999",
                FirstName = "Guilherme",
                LastName = "Admin"
            };

            await _user.CreateAsync(admin, "Guiadm123@");
            await _user.AddToRoleAsync(admin, IdentityConfiguration.ADMIN);

            ApplicationUser client = new ApplicationUser()
            {
                UserName = "guilherme-client",
                Email = "guilherme-client@geekshopping.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (99) 99999-8888",
                FirstName = "Guilherme",
                LastName = "Client"
            };

            await _user.CreateAsync(client, "Guiclient123@");
            await _user.AddToRoleAsync(client, IdentityConfiguration.CLIENT);

            await _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.ADMIN)
            });

            await _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.CLIENT)
            });
        }
    }
}
