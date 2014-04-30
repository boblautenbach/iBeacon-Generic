using System;
using MonoTouch.Foundation;

namespace iBeaconTestApp.Models
{
	public class TestData
	{
		BeaconContent[] regions;
		BeaconContent[] beacons ;

		void LoadTestData ()
		{
			regions = new BeaconContent[2] 
			{
				new BeaconContent() {ContentId = "1", ScheduledTime = new TimeSpan(0,0,0), Region = "My Beacon", NotificationTitle = "Bingo!",
					NotificationMessage = "You Entered Try Bingo Region", ViewImage = "", ViewContent = "Entered Bingo Region", RegionEvent = MonoTouch.CoreLocation.CLRegionState.Inside},

				new BeaconContent() {ContentId = "2", ScheduledTime = new TimeSpan(0,0,0), Region = "My Beacon", NotificationTitle = "Exit Bingo!",
					NotificationMessage = "You Exited Try Bingo Region", ViewImage = "", ViewContent = "Exit Bingo Region", RegionEvent =  MonoTouch.CoreLocation.CLRegionState.Outside}

			};


			beacons = new BeaconContent[2] 
			{
				new BeaconContent() {ContentId = "3", ScheduledTime = new TimeSpan(0,0,0), Region = "My Beacon", NotificationTitle = "Near Wine!",
					NotificationMessage = "Try Wine", ViewImage = "", ViewContent = "Near Wine Beacon",
					Major = "49147", Minor = "24250", ProximityUuid  = "DFDFDFDF-9345-9348-5348-CE1238434123"},

				new BeaconContent() {ContentId = "4", ScheduledTime = new TimeSpan(0,0,0), Region = "My Beacon", NotificationTitle = "Near Beer!",
					NotificationMessage = "Try Beer", ViewImage = "", ViewContent = "Near Beer Beacon", 
					Major = "2004", Minor = "2008", ProximityUuid = "DFDFDFDF-9345-9348-5348-CE1238434123"},
			};
		}

		public TestData ()
		{
			LoadTestData ();
		}

		public BeaconContent[] GetRegionContent()
		{
			return regions;
		}

		public BeaconContent[] GetBeaconContent()
		{
			return beacons;
		}



	}
}

