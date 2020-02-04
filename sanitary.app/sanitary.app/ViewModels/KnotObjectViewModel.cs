using System.Collections.ObjectModel;
using sanitary.app.Models;

namespace sanitary.app.ViewModels
{
	public class KnotObjectViewModel
	{
		#region Fields
		private ObservableCollection<Estimate> _listObject;
		private ObservableCollection<Directory> _listKnot;
		#endregion

		public KnotObjectViewModel()
		{
			ListKnot = new ObservableCollection<Directory>
			{
				new Directory
				{
					Title = "Узел кабинет директора"
				},
				new Directory
				{
					Title = "Узел кабинет программиста"
				}
			};

			ListObject = new ObservableCollection<Estimate>
			{
				new Estimate
				{
					Price = "1000",
					Quantity = "1",
					Total = "1000"
				},
				new Estimate
				{
					Price = "1000",
					Quantity = "3",
					Total = "3000"
				},
			};
		}

		#region Prop
		public ObservableCollection<Estimate> ListObject
		{
			get => _listObject;
			set => _listObject = value;
		}

		public ObservableCollection<Directory> ListKnot
		{
			get => _listKnot;
			set => _listKnot = value;
		}
		#endregion
	}
}
