using System;
namespace HostingService.Domain.Abstractions
{
    public class Entity
    {
        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();
        protected Entity()
        {

        }
        protected void NotifyDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            this._domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            this._domainEvents?.Clear();
        }
    }
}

