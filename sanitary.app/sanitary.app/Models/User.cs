using Realms;

namespace sanitary.app.Models
{
    public class User : RealmObject
    {
		public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Token { get; internal set; }

        public bool IsUserHaveFullAccess { get; set; }
    }
}
