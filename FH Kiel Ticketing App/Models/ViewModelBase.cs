using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FH_Kiel_Ticketing_App.Models
{
    public class ViewModelBase
    {
        public User user { get; set; }
    }

    public class StudentNotificationViewModel : ViewModelBase
    {
        public List<Notification> readNotifications { get; set; }
        public List<Notification> unreadNotifications { get; set; }
    }
}