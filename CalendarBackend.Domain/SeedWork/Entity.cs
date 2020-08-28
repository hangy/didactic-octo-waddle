/*
Copyright(c) .NET Foundation and Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
namespace CalendarBackend.Domain.SeedWork
{
    using System.Collections.Generic;
    using CalendarBackend.Domain.Events;
    using System;

    public abstract class Entity
    {
        private List<IDomainEvent>? domainEvents;

        private int? requestedHashCode;

        public List<IDomainEvent> DomainEvents => this.domainEvents ??= new List<IDomainEvent>();

        public virtual Guid Id { get; set; }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !(left == right);
        }

        public static bool operator ==(Entity? left, Entity? right)
        {
            if (object.Equals(left, null))
                return object.Equals(right, null);
            else
                return left.Equals(right);
        }

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            if (eventItem is null)
            {
                throw new ArgumentNullException(nameof(eventItem));
            }

            this.DomainEvents.Add(eventItem);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            var item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!requestedHashCode.HasValue)
                    requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public bool IsTransient()
        {
            return this.Id == default;
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            if (domainEvents is null) return;
            domainEvents.Remove(eventItem);
        }
    }
}
