using System.Collections.ObjectModel;

namespace sanitary.app.Models
{
	public class KnotObject
	{
		#region Prop
		/// <summary>
		/// Возвращает заголовок
		/// </summary>
		public string Title
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает список объектов
		/// </summary>
		public ObservableCollection<Estimate> ListObject
		{
			get;
			set;
		}
		#endregion
	}
}
