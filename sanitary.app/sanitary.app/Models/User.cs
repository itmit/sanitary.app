using PropertyChanged;
using Realms;

namespace sanitary.app.Models
{
    public class User : RealmObject
    {
		#region MyRegion
		/// <summary>
		/// Возвращает имя пользователя
		/// </summary>
		public string Name
		{
			get;
			set;
		} = "Иван Иванов";

		/// <summary>
		/// Возвращает почту пользователя
		/// </summary>
		public string Email
		{
			get;
			set;
		} = "IvanIvanov@gmail.com";

		/// <summary>
		/// Возвращае пароль пользователя
		/// </summary>
		public string Password
		{
			get;
			set;
		}
		#endregion
	}
}
