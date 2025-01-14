using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public abstract class Entity
    {
        protected Entity(Guid id) => Id = id;

        protected Entity()
        {

        }
        [Key] 
        public Guid Id { get;  set; }
    }
}
