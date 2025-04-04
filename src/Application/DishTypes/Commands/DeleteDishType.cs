using Application.Common.Interfaces;
using Application.Recipes.Queries;

namespace Application.DishTypes.Commands
{
	public class DeleteDishType
	{
		IApplicationDbContext _dbContext;

		public DeleteDishType(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoDeleteDishType(IEnumerable<int> dishTypeIds, CancellationToken cancellationToken = default)
		{
			var entitiesToDelete = _dbContext.DishTypesDemo.Where(x => dishTypeIds.Contains(x.Id));

			var getRecipes = new GetRecipes(_dbContext);
			var recipesSelected = await getRecipes.GetCompleteRecipes(dishTypeIds);

			foreach (var recipe in recipesSelected)
			{

				recipe.DishTypeId = 0;
			}

			_dbContext.RecipesDemo.UpdateRange(recipesSelected);
			await _dbContext.SaveChangesAsync(cancellationToken);

			_dbContext.DishTypesDemo.RemoveRange(entitiesToDelete);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
