namespace Cube.Identity.API.Application
{
	public interface IIdentityAppService
	{
		bool Validate(string user, string password);
	}
}