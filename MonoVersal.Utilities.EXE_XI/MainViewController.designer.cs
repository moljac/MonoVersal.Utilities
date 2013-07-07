// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace MonoVersal.Utilities.EXE_XI
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextView textBoxDeviceInformation { get; set; }

		[Action ("showInfo:")]
		partial void showInfo (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (textBoxDeviceInformation != null) {
				textBoxDeviceInformation.Dispose ();
				textBoxDeviceInformation = null;
			}

			if (textBoxDeviceInformation != null) {
				textBoxDeviceInformation.Dispose ();
				textBoxDeviceInformation = null;
			}
		}
	}
}
