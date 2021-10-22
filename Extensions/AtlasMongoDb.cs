namespace Catalog.Extensions
{
	public class AtlasMongoDb
	{
		public string Host { get; set; }
		
		public int Port { get; set; }
		public string User { get; set; }
		
		public string Password { get; set; }
	

		public string ConnectionString
		{
			get
			{
				return $"mongodb+srv://{User}:{Password}@{Host}";
			}
		}
	}
}