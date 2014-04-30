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


			//Used to revieve willenterforeground/background notifications from app delegate
			_notificationCenter = NSNotificationCenter.DefaultCenter.AddObserver (
				(NSString)Globals.NotificationConstants.AppStatus.ToString(),
				CheckApplicationStatus); 

			//usig KVO to tracking content change updates for the view
			_locationDelegate.AddObserver (this, (NSString)CoreLocation.CoreLocationPropertyName.CustomContent.ToString(), NSKeyValueObservingOptions.New, IntPtr.Zero);

			_locationManager = new CLLocationManager ();

			var beaconId = new NSUuid(Globals.BroadCastUUid);
			_beaconRegion = new CLBeaconRegion (beaconId, Globals.BeaconRegion);
		
			_beaconRegion.NotifyEntryStateOnDisplay = true;
		
			_locationManager.Delegate = _locationDelegate;
		
			_locationManager.StartMonitoring(_beaconRegion);

			//Only rangebeacons if in the forground.
			//I have tested this and this viewdidload event fires momentarily IF :
			//1) the app is shutdown
			//2) you are actively monitoring regions
			//3) you enter a region being monitorings
			//I needed to ensure I don't start ranging if the app fires
			//up from background to handle region events
			RangeBeacons (!(UIApplication.SharedApplication.ApplicationState == UIApplicationState.Background));
		}

		/// <summary>
		/// Handles Ranging on/off
		/// </summary>
		/// <param name="shouldRange">If set to <c>true</c> should range.</param>
		void RangeBeacons(bool shouldRange)
		{
			if (shouldRange)
			{
				imgAdd.Image = UIImage.FromBundle (Globals.DefaultImage);
				lblAddtext.Text = Globals.DefaultText;
				_locationManager.StartRangingBeacons (_beaconRegion);
				_locationManager.RequestState(_beaconRegion);
			}else{
				_locationManager.StopRangingBeacons (_beaconRegion);
			}
		}
		/// <summary>
		/// Delegate from NSNotification Center foreground/background evets
		/// </summary>
		/// <param name="notification">Notification.</param>
		private void CheckApplicationStatus(NSNotification notification)
		{
			NSNumber appStatus = (NSNumber)notification.UserInfo.ValueForKey((NSString)Globals.NotificationConstants.isInForeground.ToString());
			RangeBeacons ((bool)appStatus);
		}

		/// <Docs>Key-path to use to perform the value lookup. The keypath consists of a series of lowercase ASCII-strings with no
		/// spaces in them separated by dot characters.</Docs>
		/// <param name="change">To be added.</param>
		/// <summary>
		/// KVO obseving handler
		/// </summary>
		/// <param name="keyPath">Key path.</param>
		/// <param name="ofObject">Of object.</param>
		/// <param name="context">Context.</param>
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

