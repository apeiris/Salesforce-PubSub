using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetUtils
{
	public class SalesforceConfig  
		{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string ApiVersion { get; set; }
		public string LoginUrl { get; set; }
		public string GrpcUrl { get; set; }
		public string PubSubEndpoint { get; set; }
		public List<Topic> Topics { get; set; }
		}

	public class Topic
		{
		public string Name { get; set; }
		public List<string> FieldsToFilter { get; set; }
		}

	}
