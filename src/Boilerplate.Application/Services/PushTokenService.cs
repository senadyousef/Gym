using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Services
{
    public class PushTokenService : IPushTokenService
    {
        private IPushTokenRepository _pushTokenRepository;
        private ICurrentUserService _currentUserService;
        private INotificationTicketRepository _notificationTicketRepository;

        public PushTokenService(IPushTokenRepository pushTokenRepository, ICurrentUserService currentUserService,INotificationTicketRepository notificationTicketRepository)
        {
            _pushTokenRepository = pushTokenRepository ?? throw new ArgumentNullException(nameof(pushTokenRepository));

            _currentUserService = currentUserService;
            _notificationTicketRepository = notificationTicketRepository;
        }

        public async Task<PushTokenDto> CreatePushToken(PushTokenDto pushToken)
        {
            var token = _pushTokenRepository.GetAll().FirstOrDefault(p => p.UserId == int.Parse(_currentUserService.UserId));
            if (token == null )
            {
                var createPushToken = new PushToken
                {
                    UserId = int.Parse(_currentUserService.UserId),
                    Token = pushToken.PushToken,
                    Valid = true
                };
                var created = _pushTokenRepository.Create(createPushToken);
            }
            else
            {
                token.Token = pushToken.PushToken;
                _pushTokenRepository.Update(token);

            }

            await _pushTokenRepository.SaveChangesAsync();

            return pushToken;
        }

        public async Task<bool> DeletePushToken()
        {
            var pushToken = _pushTokenRepository.GetAll().FirstOrDefault(p => p.UserId ==int.Parse( _currentUserService.UserId));
            await _pushTokenRepository.Delete(pushToken.Id);
            return await _pushTokenRepository.SaveChangesAsync() > 0;
        }

        public async Task<PaginatedList<NotificationDto>> GetAllNotifications()
        {
            var notifications =await  _notificationTicketRepository.GetAll().Where(x=>x.UserId ==int.Parse( _currentUserService.UserId)).Select(x => new NotificationDto
            {
                MessageBody = x.MessageBody,
                Title = x.Title,
                Url = x.Url,

            }).ToPaginatedListAsync(1,10);
            return notifications;
        }
    }
}
