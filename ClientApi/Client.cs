using System;
namespace ClientApi
{
	public class Client
	{
        public Guid Id { get; set; }
        public string? Name { get; set; }
		public bool IsActive { get; set; }

		public Client()
		{
		}
	}
}

