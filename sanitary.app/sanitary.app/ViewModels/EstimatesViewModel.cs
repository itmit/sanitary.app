using System.Collections.ObjectModel;
using sanitary.app.Models;

namespace sanitary.app.ViewModels
{
	public class EstimatesViewModel
	{
		#region Fields
		private ObservableCollection<Directory> _estimatesList;
		#endregion

		public EstimatesViewModel()
		{
			EstimatesList = new ObservableCollection<Directory>
			{
				new Directory
				{
					Title = "Название А",
				},
				new Directory
				{
					Title = "Название Б",
				},
				new Directory
				{
					Title = "Название В",
				}
			};
		}

		#region Prop
		public ObservableCollection<Directory> EstimatesList
		{
			get => _estimatesList;
			set => _estimatesList = value;
		}
		#endregion
	}
}
