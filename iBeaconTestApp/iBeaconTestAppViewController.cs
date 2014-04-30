using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreBluetooth;
using MonoTouch.CoreLocation;
using iBeaconTestApp.Models;

namespace iBeaconTestApp
{
	public partial class iBeaconTestAppViewController : UIViewController
	{

		private CLLocationManager _locationManager;
		private CoreLocation _locationDelegate = new CoreLocation ();
		private NSObject _notificationCenter;
		private CLBeaconRegion _beaconRegion;

		public iBeaconTestAppViewController (IntPtr handle) : base (handle)
		{

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
			

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_notificationCenter = NSNotificationCenter.DefaultCenter.AddObserver (
				(NSString)Globals.NotificationConstants.AppStatus.ToString(),
				CheckApplicationStatus); 

			_locationDelegate.AddObserver (this, (NSString)CoreLocation.CoreLocationPropertyName.CustomContent.ToString(), NSKeyValueObservingOptions.New, IntPtr.Zero);

			_locationManager = new CLLocationManager ();

			var beaconId = new NSUuid(Globals.BroadCastUUid);
			_beaconRegion = new CLBeaconRegion (beaconId, Globals.BeaconRegion);
		
			_beaconRegion.NotifyOnExit = true;
			_beaconRegion.NotifyOnEntry = true;
			_beaconRegion.NotifyEntryStateOnDisplay = true;
		
			_locationManager.Delegate = _locationDelegate;
		
			_locationManager.StartMonitoring(_beaconRegion);
			RangeBeacons (true);
		}

		void RangeBeacons(bool shouldRange)
		{
			if (shouldRange)
			{
				_locationManager.StartRangingBeacons (_beaconRegion);
				imgAdd.Image = UIImage.FromBundle ("ibeaconHome");
				_locationManager.RequestState(_beaconRegion);
			}else{
				_locationManager.StopRangingBeacons (_beaconRegion);
			}
		}

		private void CheckApplicationStatus(NSNotification notification)
		{
			NSNumber appStatus = (NSNumber)notification.UserInfo.ValueForKey((NSString)Globals.NotificationConstants.isInForeground.ToString());
			RangeBeacons ((bool)appStatus);

		}

		public override void ObserveValue (NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
		{
			BeaconContent beaconContent;
			if (keyPath == CoreLocation.CoreLocationPropertyName.CustomContent.ToString()) {

				beaconContent = (BeaconContent)change.ValueForKey (new NSString (Globals.NotificationConstants.New.ToString().ToLower()));

				InvokeOnMainThread (delegate {  

					if (beaconContent.ViewContent != string.Empty) {
						lblAddtext.Text = beaconContent.ViewContent;
					}

					if (beaconContent.ViewImage != string.Empty) {
						//Load it
						imgAdd.Image = UIImage.FromBundle(beaconContent.ViewImage);
					}

					if(beaconContent.ProximityUuid != null)
					{
						string labelTextStr = String.Format ("Beacon: {0}\nMajor: {1}\nMinor: {2}\nRSSI: {3}\nRegion: {4}\nProximity:{5} ", 
							                     beaconContent.ProximityUuid,
							                     beaconContent.Major,
							                     beaconContent.Minor,
							                     beaconContent.Rssi.ToString (),
							                     beaconContent.Region.ToString (),
							                     beaconContent.Proximity.ToString ());
						//lblProxDesc.Text = labelTextStr;
					}
				});
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			_locationDelegate.Dispose ();
			_notificationCenter.Dispose ();

		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

