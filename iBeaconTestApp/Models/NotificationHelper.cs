using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;

namespace iBeaconTestApp.Models
{
	public static class NotificationHelper
	{
		public static void SendLocalAlertNotification (BeaconContent message)
		{
			// create the notification
			var notification = new UILocalNotification ();
			var keyName = message.NotificationTitle;

			//The Name can be used to cancel a Local notifications
			var keys = new object[] { keyName };
			var objects = new object[] { keyName};
			var userInfo = NSDictionary.FromObjectsAndKeys (objects, keys);


			// set the fire date/time
			notification.FireDate = DateTime.Now.AddSeconds (message.TimeToDisplayInSeconds);
			// configure the alert stuff
			notification.AlertAction = message.NotificationTitle;
			notification.AlertBody = message.NotificationMessage.ToString();
			notification.UserInfo = userInfo;

			// modify the badge
			notification.ApplicationIconBadgeNumber += 1;


			// set the sound to be the default sound
			notification.SoundName = UILocalNotification.DefaultSoundName;

			// schedule
			UIApplication.SharedApplication.ScheduleLocalNotification (notification);
		}

		public static void CancelLocalNotification(UILocalNotification n)
		{
			n.ApplicationIconBadgeNumber -= 1;
			UIApplication.SharedApplication.CancelLocalNotification(n);
		}

		public static void CancelLocalNotificationByName(NSString name)
		{
			UILocalNotification[] nots = UIApplication.SharedApplication.ScheduledLocalNotifications;
			nots.ToList ().ForEach (x => {
			NSObject n;
			if(x.UserInfo.TryGetValue((NSObject)name, out n))
				{	
					CancelLocalNotification(x);
				}
			});
		}
	}
}

