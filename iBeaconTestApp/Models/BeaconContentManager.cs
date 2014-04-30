using System;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using System.Linq;
using MonoTouch.UIKit;

namespace iBeaconTestApp.Models
{
	public class BeaconContentManager
	{
		private static readonly BeaconContentManager _instance = new BeaconContentManager();

		private static NSMutableDictionary _deliveredContent = new NSMutableDictionary();

		private static double _repeatDeliveredContenterAfterInMinutes;

		private static DateTime _deliveredContentLastCleared = DateTime.Today;


		//Dummy CMS
		private static TestData _testData = new TestData();


		private BeaconContentManager ()	
		{
			/*
			   Call to Service to determine how much time should expire
			   before we can repeat content that has already been delivered.

				We will clear this Dictionary whenever the LastUpdated date rolls forward.

			*/

			//RepeatDeliveredContenterAfterInMinutes = GetDelivedContentExpirationTime ();
			//I set this to 1 munute as default. This should be loaded from a config/service
			//Region enter/exit messages will be re-popped to users after this time expires.
			RepeatDeliveredContenterAfterInMinutes = 1;

		}

		public static BeaconContentManager GetInstance()
		{
			return _instance;
		}


		private DateTime DeliveredContentLastCleared
		{
			get
			{
				return _deliveredContentLastCleared;
			}
			set
			{
				_deliveredContentLastCleared = value;
			}

		}

		private double RepeatDeliveredContenterAfterInMinutes
		{
			get
			{
				return _repeatDeliveredContenterAfterInMinutes;
			}
			set
			{
				_repeatDeliveredContenterAfterInMinutes = value;

			}

		}

		public void ClearDeliveredContent()
		{
			_deliveredContent.Clear ();
			DeliveredContentLastCleared = DateTime.Today;
		}

		//Calls to Services to get content
		public BeaconContent GetEnterRegionContent(CLRegion region)
		{
			/*
		 	 	If you have very few beacons per region and/or your
				content is now overly dynamic (i.e.: changed throughout the
			  	day, I recommend not caching (caveat: questionable connections)

				As an alternative to pulling all beacon data for a given region,
				we could pull both the entry and exit region content upon
				entering a region
			*/

			bool show = false;
			BeaconContent[] regions;

			regions = _testData.GetRegionContent ();

			var reg = (from h in regions
					where h.RegionEvent == CLRegionState.Inside && h.Region == region.Identifier.ToString ()
				select h).FirstOrDefault();

			if (reg != null) 
			{
				show = ShouldShowContent (reg.ContentId);
			}

			return (show) ? reg as BeaconContent : null;
		}


		//Calls to Services to get content
		public BeaconContent GetExitRegionContent(CLRegion region)
		{
			/*
		 	 	If you have very few beacons per region and/or your
				content is now overly dynamic (i.e.: changed throughout the
			  	day, I recommend not caching (caveat: questionable connections)

				As an alternative to pulling all beacon data for a given region,
				we could pull both the entry and exit region content upon
				entering a region
			*/
			bool show = false;
			BeaconContent[] regions;

			regions = _testData.GetRegionContent ();

			var reg = (from h in regions
					where h.RegionEvent == CLRegionState.Outside && h.Region == region.Identifier.ToString ()
				select h).FirstOrDefault();

			if (reg != null) 
			{
				show = ShouldShowContent (reg.ContentId);
			}

			return (show) ? reg as BeaconContent : null;
		}

	

		public BeaconContent GetBeaconContent(CLBeacon beacon, CLBeaconRegion region)
		{

			BeaconContent[] regions;

			regions = _testData.GetBeaconContent ();

			var r = (from h in regions
				where h.Major == beacon.Major.ToString() && h.Minor == beacon.Minor.ToString() && h.ProximityUuid.ToString() == beacon.ProximityUuid.AsString()
				select h).FirstOrDefault();

			//Only needed for testing
			if (r != null) {
				r.Proximity = beacon.Proximity;
				r.Rssi = beacon.Rssi;
				r.Accuracy = beacon.Accuracy;
			}

			return (r != null) ? r as BeaconContent : null;
		}

		/// <summary>
		/// This method is used to help reduce spamming of guests while the app is not Active (foreground)
		/// It tracks which messsages have been shown and expires
		/// them to be reshown based upon RepeatDeliveredContenterAfterInMinutes
		/// If reduces Local notification spam
		/// </summary>
		/// <returns><c>true</c>, if show content was shoulded, <c>false</c> otherwise.</returns>
		/// <param name="contentId">Content identifier.</param>

		public bool ShouldShowContent(string contentId)
		{
			//Don't limit entry and exit content if the app is open and active
			if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active) {
				return true;
			}


			NSObject value;

			NSObject key = NSObject.FromObject (contentId);
			NSDate future = (NSDate)(DateTime.Now.AddMinutes (this.RepeatDeliveredContenterAfterInMinutes));

			//  Clear the _delivered content dictionary every 24 hours
			//  We use to limit spamming along with RepeatDeliveredContenterAfterInMinutes
			//	which will stop notifications from repeating in within the time specificied
	
			if (DateTime.Today > DeliveredContentLastCleared) {
				ClearDeliveredContent ();
			}


			//NSDate use UTC
			if (_deliveredContent.TryGetValue (key, out value)) {
				if (DateTime.UtcNow > (NSDate)value) {
					_deliveredContent.Remove (key);
					_deliveredContent.Add (key, future);
					return true;
				}
			} else {
				_deliveredContent.Add (key, future);
				return true;
			}

			return false;
		}
	}
}

