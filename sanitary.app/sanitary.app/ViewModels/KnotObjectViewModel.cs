using System.Collections.ObjectModel;
using sanitary.app.Models;

namespace sanitary.app.ViewModels
{
	public class KnotObjectViewModel
	{
		#region Fields
		private ObservableCollection<KnotObject> _listKnot;
		#endregion

		public KnotObjectViewModel()
		{
			ListKnot = new ObservableCollection<KnotObject>
			{
				new KnotObject
				{
					Title = "Узел кабинет директора",
					ListObject = new ObservableCollection<Estimate>
					{
						new Estimate
						{
							Description = "Заглушка (110) PPRC Pro Aqua",
							Price = "1000",
							Quantity = "1",
							Total = "1000"
						},
						new Estimate
						{
							Description = "Заглушка (100) PPRC Pro Aqua",
							Price = "1000",
							Quantity = "3",
							Total = "3000"
						}
					}
				},
				new KnotObject
				{
					Title = "Узел кабинет программиста",
					ListObject = new ObservableCollection<Estimate>
					{
						new Estimate
						{
							Description = "Раковина, стекло 1200*900",
							Price = "12000",
							Quantity = "1",
							Total = "12000"
						}
					}
				}
			};
		}

		#region Prop
		public ObservableCollection<KnotObject> ListKnot
		{
			get => _listKnot;
			set => _listKnot = value;
		}
		#endregion
	}
}
