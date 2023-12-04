using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    
        public interface INotificationService
        {
            Task SendNotifiactionAsync(int userId, string title, string body, object content = null);
        }
        public class NotificationUrlObject
        {
            public string url { get; set; }
       }
    
}
