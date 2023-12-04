using Boilerplate.Application.DTOs;
using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    public interface IPushTokenService
    {
        public Task<PushTokenDto> CreatePushToken(PushTokenDto pushToken);
        public Task<PaginatedList<NotificationDto>> GetAllNotifications();
        public Task<bool> DeletePushToken();
    }

    public class NotificationDto
    {
        public string Title { get; set; }
        public string MessageBody { get; set; }
        public string Url { get; set; }
    }
}



public class PushTokenDto
{
    [Required]
    public string PushToken { get; set; }
}