using Application.Common.Interfaces;
using Application.DishTypes.Commands;
using Application.DishTypes.Queries;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BlazorAppDemo.Components.Pages.DishTypes
{
	public partial class DishTypeListing
	{
		int dishTypeIdOnDragStart;

		[SupplyParameterFromForm]
		public IList<DishType> DishTypeListModel { get; set; } = new List<DishType>();

		int? FirstPositionInOrder { get; set; }
		int? LastPositionInOrder { get; set; }

		[SupplyParameterFromForm]
		IDictionary<int, bool> DishTypesToDelete { get; set; }
		[SupplyParameterFromForm]
		private string NewDishTypeName { get; set; }

		private string OrderChangeNotification { get; set; } = String.Empty;
		private string NewDishTypeNameNotification { get; set; } = String.Empty;

		[Inject]
		NavigationManager Navigation { get; set; }

		[Inject]
		ApplicationDbContext DbContext { get; set; }

		protected async override Task OnParametersSetAsync()
		{
			GetDishTypes getDishTypes = new(DbContext);
			DishTypeListModel = await getDishTypes.GetDishTypeListAsync();

			FirstPositionInOrder = DishTypeListModel.Min(x => x.Order);
			LastPositionInOrder = DishTypeListModel.Max(x => x.Order);

			DishTypesToDelete = DishTypeListModel.ToDictionary(x => x.Id, y => false);
		}

		private async Task OnOrderChangeApprovalAsync()
		{
			OrderChangeNotification = "Poradie bude aktualizované.";
			var updateDishType = new UpdateDishType(DbContext);
			await updateDishType.DoUpdateDishTypesAsync(DishTypeListModel);

			await Task.Delay(1000);
			Navigation.Refresh(true);
		}

		private void DishTypePositionUp(int dishTypeId)
		{
			FirstPositionInOrder = DishTypeListModel.Min(x => x.Order);

			var dishType = DishTypeListModel.FirstOrDefault(x => x.Id == dishTypeId);

			if (dishType.Order == FirstPositionInOrder) { return; }

			var maxOfPrecedingDishTypePositions = DishTypeListModel.Where(x => x.Order < dishType.Order).Select(x => x.Order).Max();
			var precedingDishType = DishTypeListModel.FirstOrDefault(x => x.Order == maxOfPrecedingDishTypePositions);
			var positionToChange = dishType.Order;
			dishType.Order = precedingDishType.Order;
			precedingDishType.Order = positionToChange;

			var lastPositionInOrder = DishTypeListModel.Max(x => x.Order);
			DishTypeListModel = DishTypeListModel.OrderBy(x => x.Id == 0 ? lastPositionInOrder + 1 : x.Order).ToList();

			StateHasChanged();
		}

		private void DishTypePositionDown(int dishTypeId)
		{
			var lastPositionInOrder = DishTypeListModel.Max(x => x.Order);

			var dishType = DishTypeListModel.FirstOrDefault(x => x.Id == dishTypeId);

			if (dishType.Order == lastPositionInOrder) { return; }

			var minOfFollowingDishTypePositions = DishTypeListModel.Where(x => x.Order > dishType.Order).Select(x => x.Order).Min();
			var followingDishType = DishTypeListModel.FirstOrDefault(x => x.Order == minOfFollowingDishTypePositions);
			var positionToChange = dishType.Order;
			dishType.Order = followingDishType.Order;
			followingDishType.Order = positionToChange;

			DishTypeListModel = DishTypeListModel.OrderBy(x => x.Id == 0 ? lastPositionInOrder + 1 : x.Order).ToList();

			StateHasChanged();
		}

		private async Task ToDelete()
		{
			var dishTypeIds = DishTypesToDelete.Where(x => x.Value == true).Select(x => x.Key);

			var deleteRecipe = new DeleteDishType(DbContext);
			await deleteRecipe.DoDeleteDishType(dishTypeIds);

			Navigation.Refresh(true);
		}

		private void HandleOnDragStart(int dishTypeIdOnDragStart)
		{
			this.dishTypeIdOnDragStart = dishTypeIdOnDragStart;
		}

		private void HandleOnDragOver(int dishTypeIdOnDragOver)
		{
			if (dishTypeIdOnDragStart == dishTypeIdOnDragOver) { return; }

			var lastPositionInOrder = DishTypeListModel.Max(x => x.Order);

			var dishTypeOnDragStart = DishTypeListModel.FirstOrDefault(x => x.Id == dishTypeIdOnDragStart);
			var onDragStartOrder = dishTypeOnDragStart.Order;
			var dishTypeOnDragOver = DishTypeListModel.FirstOrDefault(x => x.Id == dishTypeIdOnDragOver);

			dishTypeOnDragStart.Order = dishTypeOnDragOver.Order;
			dishTypeOnDragOver.Order = onDragStartOrder;

			DishTypeListModel = DishTypeListModel.OrderBy(x => x.Id == 0 ? lastPositionInOrder + 1 : x.Order).ToList();

			StateHasChanged();
		}

		private async Task OnNewDishTypeAsync()
		{
			if (NewDishTypeName == null || NewDishTypeName.Trim() == String.Empty)
			{
				NewDishTypeNameNotification = "Názov nesmie by prázdny.";
				await Task.Delay(1000);
				NewDishTypeNameNotification = String.Empty;
				return;
			}

			NewDishTypeName = NewDishTypeName.Trim();
			if (NewDishTypeName.Length > 30) { NewDishTypeName = NewDishTypeName.Substring(0, 30); }
			var newDishType = new DishType { Name = NewDishTypeName };
			var lastPositionInOrder = DishTypeListModel.Max(x => x.Order);
			newDishType.Order = lastPositionInOrder + 1;

			NewDishTypeNameNotification = "Nový druh jedla bude pridaný.";
			var updateDishType = new UpdateDishType(DbContext);
			await updateDishType.AddDishTypes(newDishType);

			await Task.Delay(1000);
			Navigation.Refresh(true);
		}
	}
}