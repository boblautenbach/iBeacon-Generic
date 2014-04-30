// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace iBeaconTestApp
{
	[Register ("iBeaconTestAppViewController")]
	partial class iBeaconTestAppViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView imgAdd { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblAddtext { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imgAdd != null) {
				imgAdd.Dispose ();
				imgAdd = null;
			}

			if (lblAddtext != null) {
				lblAddtext.Dispose ();
				lblAddtext = null;
			}
		}
	}
}
