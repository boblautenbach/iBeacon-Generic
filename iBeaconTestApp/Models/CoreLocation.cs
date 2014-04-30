using System;
using MonoTouch.CoreLocation;
using MonoTouch.CoreBluetooth;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace iBeaconTestApp.Models
{
	public class CoreLocation : CLLocationManagerDelegate
	{
		NSObject _beacon;

		public enum CoreLocationPropertyName
		{
			CustomContent,
		}

		BeaconContentManager _beaconManager = BeaconContentManager.GetInstance();

		[Export ("CustomContent")]
		public BeaconContent CustomContent {
			get;
			set;
		}

		public CoreLocation(){}

		private void SetContent(BeaconContent content)
		{
			if (content != null){
			

				if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active) {
			
					//fire off observer notification
					this.SetValueForKey((BeaconContent)content, new NSString (CoreLocationPropertyName.CustomContent.ToString ()));
				
				}else{
				
					if (content.NotificationMessage != string.Empty) {
						//Create and schedule a Local Notification
						NotificationHelper.SendLocalAlertNotification (content);
					}

				}
			}
		}

		public override void RegionEntered (CLLocationManager manager, CLRegion region)
		{
			SetContent(_beaconManager.GetEnterRegionContent (region));
		}

		public override void RegionLeft (CLLocationManager manager, CLRegion region)
		{
			SetContent(_beaconManager.GetExitRegionContent (region));
		}

		public override void DidRangeBeacons (CLLocationManager manager, CLBeacon[] beacons, CLBeaconRegion region)
		{

			if (beacons.Length > 0) {
				CLBeacon selectedBeacon = (CLBeacon)beacons.GetValue (0);

				if (selectedBeacon.Proximity == CLProximity.Near || selectedBeacon.Proximity == CLProximity.Immediate) 
				{
					SetContent (_beaconManager.GetBeaconContent (selectedBeacon, region));
				}
			}
		}

		public override void DidDetermineState (CLLocationManager manager, CLRegionState state, CLRegion region)
		{
			//get state on initial call.  App user may already be inide
			//a region upon opening the app.
			if (state == CLRegionState.Inside) {
				SetContent(_beaconManager.GetEnterRegionContent (region));			
			}
		}
	}
}

