using MessengerApi.Core.Enums;

namespace MessengerApi.Core.Repositories
{
    public interface IEventTracerRepository
    {
        int GetEventCount(EventType eventType);
        void AddEvent(EventType eventType);
    }
}