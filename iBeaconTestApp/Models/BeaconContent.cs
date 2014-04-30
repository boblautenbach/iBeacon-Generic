using System;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;

namespace iBeaconTestApp.Models
{

	public class BeaconContent : NSObject
	{


		public string NotificationTitle {
			get;
			set;
		}

		public string ViewImage {
			get;
			set;
		}

		public string ViewContent {
			get;
			set;
		}

		public string NotificationMessage {
			get;
			set;
		}

		public double TimeToDisplayInSeconds
		{
			get
			{
				if(ScheduledTime == TimeSpan.Zero )
				{	
					//set to fire in 1 second if no value specified in CMS
					return 1;
				}else{
					TimeSpan today = DateTime.Today.TimeOfDay;

					return (ScheduledTime - today).TotalSeconds;

				}
			}
		}

		public TimeSpan ScheduledTime{
			get;
			set;
		}
			
		public string ContentId {
			get;
			set;
		}

		public string Region{
			get;
			set;
		}

		public string Major{
			get;
			set;
		}

		public string Minor{
			get;
			set;
		}

		public string ProximityUuid{
			get;
			set;
		}

		public CLProximity Proximity{
			get;
			set;
		}

		public double Accuracy{
			get;
			set;
		}

		public int Rssi{
			get;
			set;
		}


		public CLRegionState RegionEvent {
			get;
			set;
		} 
		public BeaconContent ()
		{
		}
	}
}

