using Data;

namespace User.Domain
{
	public class User : DataModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string AvatarUrl { get; set; }
	}
}
