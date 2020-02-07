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
					NameObject = "Название А"
				},
				new ListObject
				{ 
					NameObject = "Название Б"
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
