using Microsoft.AspNetCore.Components;

namespace BlazorAppDemo.Components.Pages.Recipes
{
	public partial class PdfViewer
	{

		[Parameter]
		[SupplyParameterFromQuery]
		public int RecipeId { get; set; }
		private string pdfUrl;

		protected override void OnInitialized()
		{
			pdfUrl = $"api/pdf/{RecipeId}";
		}
	}
}
