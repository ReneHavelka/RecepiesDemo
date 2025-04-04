using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
	public interface IApplicationDbContext
	{
		public DbSet<DishType> DishTypesDemo { get; set; }
		public DbSet<Recipe> RecipesDemo { get; set; }

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
