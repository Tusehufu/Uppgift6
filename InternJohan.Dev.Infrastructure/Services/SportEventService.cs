using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Dapper;
using Microsoft.Data.SqlClient;
using InternJohan.Dev.Infrastructure.ViewModel;


namespace InternJohan.Dev.Infrastructure.Repository
{
    public class SportEventService
    {
        private readonly SportEventRepository _sportEventRepository;
        public SportEventService(SportEventRepository sportEventRepository)
        {
            _sportEventRepository = sportEventRepository;
        }

        public async Task<IEnumerable<SportEvent>> GetAll()
        {
            return await  _sportEventRepository.FindAll();

        }

        public async Task<int> Add(SportEvent sportevent)
        {
            await _sportEventRepository.Insert(sportevent);

            return sportevent.Id;
        }
        public async Task<bool> DeleteEvent(int userId, int eventId)
        {

            // Kontrollera om användaren är ägaren till evenemanget
            //var isOwner = await _sportEventRepository.IsUserEventHost(userId, eventId);

            /*try*/ /*(isOwner)*/
            {
                // Användaren är ägaren, tillåt borttagning
                 await _sportEventRepository.DeleteEvent(userId, eventId);
                return true;
            }
            //catch
            {
                // Användaren är inte ägaren, neka borttagning
                return false;
            }
        }

        public async Task<SportEvent> FindById(int id)
        {
            return await _sportEventRepository.FindById(id);
        }
        public async Task<bool> Update(SportEvent sportevent)
        {
            return await _sportEventRepository.Update(sportevent);
        }
        // Anropar repository för att gå med i evenemanget
        public async Task<bool> JoinEvent(int userId, int eventId)
        {
            return await _sportEventRepository.JoinEvent(userId, eventId);
        }

        public async Task<bool> IsUserParticipant(int userId, int eventId)
        {
            return await _sportEventRepository.IsUserParticipant(userId, eventId);
        }

        public async Task AddParticipant(int userId, int eventId)
        {
            await _sportEventRepository.AddParticipant(userId, eventId);
        }
        public async Task RemoveParticipant(int userId, int eventId)
        {
            await _sportEventRepository.RemoveParticipant(userId, eventId);
        }
        //public async Task LeaveEvent(int userId, int eventId)
        //{
        //    await _sportEventRepository.LeaveEvent(userId, eventId);
        //}
    }
}

