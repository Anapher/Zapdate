using Zapdate.Core.Shared;

namespace Zapdate.Core.Domain.Entities
{
    public class Project : BaseEntity
    {
        public Project(string name, RsaKey rsaKey)
        {
            Name = name;
            RsaKey = rsaKey;
            RsaKeyId = rsaKey.Id;
        }

#pragma warning disable CS8618 // Constructor for mapping
        public Project()
        {
        }
#pragma warning restore CS8618

        public string Name { get; private set; }
        public RsaKey RsaKey { get; private set; }

        public int RsaKeyId { get; private set; }
    }
}
