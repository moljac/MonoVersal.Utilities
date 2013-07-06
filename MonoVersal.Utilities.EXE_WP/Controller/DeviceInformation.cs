using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoVersal.Utilities.EXE 
{
	public partial class DeviceInformation
	{
		private MonoVersal.Utilities.DeviceInformation device_data = null;

		public string PhoneNumber	= null;
		public string Serial		= null;
		public string UDID			= null;
		public string DeviceId		= null;
		public string DeviceIdIMEI	= null;
			   
		public string Manufacturer	= null;
		public string Brand			= null;
		public string Product		= null;
		public string Model			= null;
			   
		public string MAC_Address	= null;
		public string MAC_WiFi		= null;
		public string MAC_Bluetooth	= null;
		public string BluetoothName	= null;

		public DeviceInformation()
		{
			device_data = new MonoVersal.Utilities.DeviceInformation();

			PhoneNumber			= device_data.PhoneNumber();
			Serial				= device_data.Serial();
			UDID				= device_data.UDID();
			DeviceId			= device_data.Device();
			DeviceIdIMEI		= device_data.DeviceIdIMEI();

			Manufacturer		= device_data.Manufacturer();
			Brand				= device_data.Brand();
			Product				= device_data.Product();
			Model				= device_data.Model();

			MAC_Address			= device_data.MAC_Address();
			MAC_WiFi			= device_data.MAC_WiFi();
			MAC_Bluetooth		= device_data.MAC_Bluetooth();
			BluetoothName		= device_data.LocalBluetoothName();

			return;
		}
	}
}
