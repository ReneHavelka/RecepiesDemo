using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.DishTypes.Commands
{
	public class UpdateDishType
	{
		IApplicationDbContext _dbContext;
		public UpdateDishType(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddDishTypes(DishType newDishType)
		{
			await _dbContext.DishTypesDemo.AddAsync(newDishType);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DoUpdateDishTypesAsync(IEnumerable<DishType> dishTypes, CancellationToken cancellationToken = default)
		{
			_dbContext.DishTypesDemo.UpdateRange(dishTypes);
			await _dbContext.SaveChangesAsync();
		}
	}
}
