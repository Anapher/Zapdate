using System.Collections.Generic;

namespace Zapdate.Models.Response
{
    public class ProjectDto
    {
        public int Id { get; }
        public string Name { get; }

        public ProjectDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
