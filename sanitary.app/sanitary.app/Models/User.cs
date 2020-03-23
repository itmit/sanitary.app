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
		}

		/// <summary>
		/// Возвращает почту пользователя
		/// </summary>
		public string Email
		{
			get;
			set;
		}

		/// <summary>
		/// Возвращае пароль пользователя
		/// </summary>
		public string Password
		{
			get;
			set;
		}
        public string Token { get; internal set; }
        #endregion
    }
}
