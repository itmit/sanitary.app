namespace sanitary.app.Models
{
	public class Estimate
	{
		#region Prop
		/// <summary>
		/// Возвращает наименование сметы
		/// </summary>
		public string Description
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает количество
		/// </summary>
		public string Quantity
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает цену
		/// </summary>
		public string Price
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает итого
		/// </summary>
		public string Total
		{
			get;
			set;
		}
		#endregion
	}
}
