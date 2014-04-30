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
		MonoTouch.UIKit.UILabel lblProxDesc { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblProximity { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblProxState { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblProximity != null) {
				lblProximity.Dispose ();
				lblProximity = null;
			}

			if (lblProxDesc != null) {
				lblProxDesc.Dispose ();
				lblProxDesc = null;
			}

			if (lblProxState != null) {
				lblProxState.Dispose ();
				lblProxState = null;
			}
		}
	}
}
