using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaFarHub.Models
{
    public class FetchedTaggedNotifications
    {
		private List<int> taggingID;
		private List<int> postID;
		private List<string> friendUserName;
		private List<string> friendUserNameWhoIsTagged;
		private List<bool> notificationSeen;
		private List<bool> clickedAndSeen;
		private List<DateTime> taggedDateTime;

		public List<DateTime> TaggedDateTime
		{
			get { return taggedDateTime; }
			set { taggedDateTime = value; }
		}


		public List<bool> ClickedAndSeen
        {
			get { return clickedAndSeen; }
			set { clickedAndSeen = value; }
		}


		public List<bool> NotificationSeen
        {
			get { return notificationSeen; }
			set { notificationSeen = value; }
		}


		public List<string> FriendUserNameWhoIsTagged

		{
            get { return friendUserNameWhoIsTagged; }
			set { friendUserNameWhoIsTagged = value; }
		}


		public List<string> FriendUserName
		{
			get { return friendUserName; }
			set { friendUserName = value; }
		}


		public List<int> PostID
		{
			get { return postID; }
			set { postID = value; }
		}


		public List<int> TaggingID
		{
			get { return taggingID; }
			set { taggingID = value; }
		}


		public FetchedTaggedNotifications()
		{
			TaggingID = new List<int>();
			PostID = new List<int>();
			friendUserName = new List<string>();
			friendUserNameWhoIsTagged = new List<string>();
			notificationSeen = new List<bool>();
			clickedAndSeen = new List<bool>();
			taggedDateTime = new List<DateTime>();
		}

	}
}