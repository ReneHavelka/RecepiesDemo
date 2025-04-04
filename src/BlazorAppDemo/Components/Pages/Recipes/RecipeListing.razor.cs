using Application.Common.Interfaces;
using Application.Recipes.EventHandlers;
using Application.Recipes.Queries;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;

namespace BlazorAppDemo.Components.Pages.Recipes
{
	public partial class RecipeListing
	{
		[Parameter]
		[SupplyParameterFromQuery]
		public int? Id { get; set; }
		[Parameter]
		[SupplyParameterFromQuery]
		public string DishTypeName { get; set; }

		IList<Recipe> RecipeList { get; set; } = new List<Recipe>();
		IDictionary<int, bool> RecipesToDelete { get; set; }

		[Inject]
		private protected NavigationManager Navigation { get; set; }

		[Inject]
		ApplicationDbContext DbContext { get; set; }

		protected async override Task OnParametersSetAsync()
		{
			var getRecepies = new GetRecipes(DbContext);

			if (Id == null) return;

			RecipeList = await getRecepies.GetRecipeIdsDishTypesIdsNamesListAsync((int)Id);
			if (Id == -1) { DishTypeName = "Všetky recepty"; }

			RecipesToDelete = RecipeList.Select(x => x.Id).ToDictionary(x => x, y => false);
		}

		private async Task ToDelete()
		{
			var handleDeleteRecipes = new HandleDeleteRecipes(DbContext);
			await handleDeleteRecipes.DoDeleteRecipes(RecipesToDelete);

			Navigation.Refresh(true);
		}
	}
}