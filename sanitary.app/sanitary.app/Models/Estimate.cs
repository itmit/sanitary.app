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
		public int Quantity
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает цену
		/// </summary>
		public int Price
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращает итого
		/// </summary>
		public int Total
		{
			get;
			set;
		}
		#endregion
	}
}
