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

			//Dummy data that would be part of a CMS system
			//The Scheduled Time can be a string ("0:0:0") or a time field (like SQl server.
			//I would normally return JSON and convert to my local objects.
			regions = new BeaconContent[2] 
			{
				new BeaconContent() {ContentId = "1", ScheduledTime = new TimeSpan(0,0,0), Region = "My Beacon", NotificationTitle = "Fashion Place!",
					NotificationMessage = @"Stop in and see our 40%% off sale.", ViewImage = "Images/ibeaconEnter", ViewContent = "Welcome to Fashion Place!", RegionEvent = MonoTouch.CoreLocation.CLRegionState.Inside},

				new BeaconContent() {ContentId = "2", ScheduledTime = new TimeSpan(0,0,0), Region = "My Beacon", NotificationTitle = "Fashion Place!",
					NotificationMessage = @"Come back soon!", ViewImage = "Images/ibeaconExit", ViewContent = "Fashion Place will miss you!", RegionEvent =  MonoTouch.CoreLocation.CLRegionState.Outside}

			};


			beacons = new BeaconContent[2] 
			{
				new BeaconContent() {ContentId = "3", ScheduledTime = new TimeSpan(0,0,0), Region = "My Beacon", NotificationTitle = "Fashion Place",
					NotificationMessage = @"Amazing Deal on Jackets.", ViewImage = "Images/ibeaconAtBeacon", ViewContent = "OMG!  Great Deal on Jacket!",
					Major = "49147", Minor = "24250", ProximityUuid  = "DFDFDFDF-9345-9348-5348-CE1238434123"},

				new BeaconContent() {ContentId = "4", ScheduledTime = new TimeSpan(0,0,0), Region = "My Beacon", NotificationTitle = "Fashion Outlet",
					NotificationMessage = @"Another Amazing Deal.", ViewImage = "Images/ibeaconAtBeacon", ViewContent = "This will look great on you!", 
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

