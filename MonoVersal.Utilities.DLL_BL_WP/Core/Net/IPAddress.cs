using System;
using System.Net;

namespace MonoVersal.Utilities.WP.Core.Net
{
	public partial class IPAddress
	{
		public System.Net.IPAddress[] IPAddresses()
		{
			string host_name = System.Net.Dns.GetHostName() + ".local";

			IPHostEntry host_entry = Dns.GetHostEntry(host_name);
			
			
			return host_entry.AddressList;
		}

		// In iOS5 it was necessary to add the .local to the hostname in order for
		// GetHostEntry() to work properly. This would give an address on the same
		// subnet and a windows prorgam was able to communicate with the httplistener. 
		// 
		// In iOS6 I have to remove the +".local" part otherwise I get an error "No
		// such host is known".
		// 
		// When I remove it, the code works but I actually get an IP address of
		// 199.101.28.130 which is on a separate subnet. As a result my Windows app
		// running on 192.168.0.2 cannot communicate with it.

	}
}
