using System.Collections.ObjectModel;
using sanitary.app.Models;

namespace sanitary.app.ViewModels
{
	public class DownloadEstimatesViewModel
	{
		private ObservableCollection<Estimate> _estimatesList;

		#region Fields
		
		#endregion
		public DownloadEstimatesViewModel()
		{
			EstimatesList = new ObservableCollection<Estimate>
			{
				new Estimate
				{
					Description = "Заглушка, (110) PPRC Pro Aqua",
					Price = 1000,
					Quantity = 3,
					Total = 3000
				},
				new Estimate
				{
					Description = "Заглушка, (110) PPRC Pro Aqua",
					Price = 1000,
					Quantity = 1,
					Total = 1000
				}
			};
		}

		#region MyRegion
		public ObservableCollection<Estimate> EstimatesList
		{
			get => _estimatesList;
			set => _estimatesList = value;
		}
		#endregion
	}
}
