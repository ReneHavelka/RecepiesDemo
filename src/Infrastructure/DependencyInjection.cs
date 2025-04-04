using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string defaultConnection)
		{
			services.AddDbContextFactory<ApplicationDbContext>(options =>
				options.UseSqlServer(defaultConnection), ServiceLifetime.Transient);

			return services;
		}
	}
}
