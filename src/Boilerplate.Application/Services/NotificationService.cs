using Boilerplate.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expo.Server.Client;
using Expo.Server.Models;
using Boilerplate.Domain.Repositories;
using Boilerplate.Domain.Entities;
using Newtonsoft.Json;

namespace Boilerplate.Application.Services
{
    public class NotificationService : INotificationService
    {
        private IPushTokenRepository _pushTokenRepository;
        private ICurrentUserService _currentUserService;
        private PushApiClient expoSDKClient;
        private IPushTicketRepository _pushTicketRepository;
        private INotificationTicketRepository _notificationTicketRepository;

        public NotificationService(IPushTokenRepository pushTokenRepository, ICurrentUserService currentUserService,INotificationTicketRepository notificationTicketRepository, IPushTicketRepository pushTicketRepository)
        {
            this._pushTokenRepository = pushTokenRepository;
            this._currentUserService = currentUserService;
            this.expoSDKClient = new PushApiClient();
            this._pushTicketRepository = pushTicketRepository;
            this._notificationTicketRepository = notificationTicketRepository;

        }
        public async Task SendNotifiactionAsync(int userId, string title, string body, object content = null)
        {
            var pushToken = _pushTokenRepository.GetAll().FirstOrDefault(p => p.UserId == int.Parse(_currentUserService.UserId));

            if (pushToken == null)
                return;

            var pushTicketReq = new NotificationRequest()
            {
                PushTo = new List<string>() { pushToken.Token },
                PushBadgeCount = 1,
                PushBody = body,
                PushTitle = title,
                PushSound = "default",
                PushData = content,
                UserId = userId.ToString(),

            };
            await SendAsync(pushTicketReq,userId );
        }

        private async Task SendAsync(NotificationRequest pushTicketReq, int userId)
        {
            try
            {
                var result = await expoSDKClient.PushSendAsync(pushTicketReq);

                if (result?.PushTicketErrors?.Count > 0)
                {
                    foreach (var error in result.PushTicketErrors)
                    {
                        Console.WriteLine($"Error: {error.ErrorCode} - {error.ErrorMessage}");
                    }
                }

                if (result?.PushTicketStatuses?.Count > 0)
                {
                    for (int i = 0; i < result.PushTicketStatuses.Count; i++)
                    {
                        PushTicketStatus ticket = result.PushTicketStatuses[i];
                        
                         _pushTicketRepository.Create(new PushTicket
                        {
                            ReceiptId = ticket.TicketId,
                            UserId = userId,

                        });

                         _notificationTicketRepository.Create(new NotificationTicket
                        {
                            ReceiptId = ticket.TicketId,
                            UserId = userId,
                            Url = JsonConvert.SerializeObject(pushTicketReq.PushData),
                            Title = pushTicketReq.PushTitle,
                            MessageBody = pushTicketReq.PushBody
                        });


                        await _pushTicketRepository.SaveChangesAsync();
                        await _notificationTicketRepository.SaveChangesAsync();

                    }
                }
            }
            catch (Exception e)
            {
                //logger.LogError($"Error: ${e.Message}");
            }

        }
    }
}


public class NotificationRequest : PushTicketRequest
{
    public string UserId { get; set; }
}