using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogX.Core.Entities
{
    public abstract class EntityBase
    {
        public virtual int Id { get; protected set; }

        public DateTime CreatedTime { get; protected set; } = DateTime.Now;

        public DateTime UpdatedTime { get; protected set; } = DateTime.Now;
    }
}
