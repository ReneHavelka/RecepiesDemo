﻿using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Recipes.Commands
{
	public class UpdateRecipe
	{
		IApplicationDbContext _dbContext;

		public UpdateRecipe(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoUpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken = default)
		{
			_dbContext.RecipesDemo.Update(recipe);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
