using System.Collections.ObjectModel;
using sanitary.app.Models;

namespace sanitary.app.ViewModels
{
	public class ObjectViewModel
	{
		#region Fields
		private ObservableCollection<ListObject> _listObject;
		#endregion

		public ObjectViewModel()
		{
			ListObject = new ObservableCollection<ListObject>
			{
				new ListObject
				{
					NumberObject = "01",
					Date = "20.01.2020",
					Knot = "3"
				},
				new ListObject
				{
					NumberObject = "02",
					Date = "21.02.2020",
					Knot = "3"
				}
			};
		}

		#region Prop
		public ObservableCollection<ListObject> ListObject
		{
			get => _listObject;
			set => _listObject = value;
		}
		#endregion
	}
}
