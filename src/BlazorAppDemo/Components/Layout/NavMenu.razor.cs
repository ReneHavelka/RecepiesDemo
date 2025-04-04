using Application.DishTypes.Queries;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BlazorAppDemo.Components.Layout
{
	public partial class NavMenu
	{
		[Inject]
		ApplicationDbContext DbContext { get; set; }

		public IList<DishType> dishTypeList = new List<DishType>();

		protected async override Task OnInitializedAsync()
		{
			var getDishTypes = new GetDishTypes(DbContext);
			dishTypeList = await getDishTypes.GetDishTypeListAsync();
		}
	}
}
