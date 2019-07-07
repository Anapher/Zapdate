﻿using Zapdate.Core.Shared;

namespace Zapdate.Core.Domain.Entities
{
    // aggregate root
    public class Project : BaseEntity
    {
        public Project(string name, AsymmetricKey asymmetricKey)
        {
            Name = name;
            AsymmetricKey = asymmetricKey;
        }

#pragma warning disable CS8618 // Constructor for mapping
        private Project()
        {
        }
#pragma warning restore CS8618

        public string Name { get; private set; }
        public AsymmetricKey AsymmetricKey { get; private set; }
    }
}