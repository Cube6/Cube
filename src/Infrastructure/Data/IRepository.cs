namespace Data
{
	public interface IRepository
	{
		void ExcuteSql(string sql);
	}

	public class Repository : IRepository
	{
		public virtual void ExcuteSql(string sql)
		{
			throw new System.NotImplementedException();
		}
	}
}
