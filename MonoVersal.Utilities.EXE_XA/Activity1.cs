using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MonoVersal.Utilities.EXE
{
	[Activity(Label = "MonoVersal.Utilities", MainLauncher = true, Icon = "@drawable/icon")]
	public class Activity1 : Activity
	{
		DeviceInformation device_data = null;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			device_data = new DeviceInformation();

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			EditText editTextDeviceData = FindViewById<EditText>(Resource.Id.editTextDeviceData);

			editTextDeviceData.Text += "PhoneNumber = ";
			editTextDeviceData.Text += device_data.PhoneNumber;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "Serial = ";
			editTextDeviceData.Text += device_data.Serial;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "UDID = ";
			editTextDeviceData.Text += device_data.UDID;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "DeviceId = ";
			editTextDeviceData.Text += device_data.DeviceId;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "DeviceIdIMEI = ";
			editTextDeviceData.Text += device_data.DeviceIdIMEI;
			editTextDeviceData.Text += System.Environment.NewLine;


	
			editTextDeviceData.Text += "Manufacturer = ";
			editTextDeviceData.Text += device_data.Manufacturer;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "Brand = ";
			editTextDeviceData.Text += device_data.Brand;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "Product = ";
			editTextDeviceData.Text += device_data.Product;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "Model = ";
			editTextDeviceData.Text += device_data.Model;
			editTextDeviceData.Text += System.Environment.NewLine;



			editTextDeviceData.Text += "MAC_Address = ";
			editTextDeviceData.Text += device_data.MAC_Address;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "MAC_WiFi = ";
			editTextDeviceData.Text += device_data.MAC_WiFi;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "MAC_Bluetooth = ";
			editTextDeviceData.Text += device_data.MAC_Bluetooth;
			editTextDeviceData.Text += System.Environment.NewLine;

			editTextDeviceData.Text += "BluetoothName = ";
			editTextDeviceData.Text += device_data.BluetoothName;
			editTextDeviceData.Text += System.Environment.NewLine;


			
			return;
		}
	}
}

