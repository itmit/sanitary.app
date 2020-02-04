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
					Title = "Объект Офис ООО “Итмит”",
				},
				new Directory
				{
					Title = "Объект Офис Пожарные",
				},
				new Directory
				{
					Title = "Объект Офис ООО Первые",
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
