using System.Collections.Generic;
using System.Collections.Immutable;
using Zapdate.Core;
using Zapdate.Server.Core.Dto.UseCaseResponses;
using Zapdate.Server.Core.Interfaces;

namespace Zapdate.Server.Core.Dto.UseCaseRequests
{
    public class SearchUpdateRequest : IUseCaseRequest<SearchUpdateResponse>
    {
        public SearchUpdateRequest(int projectId, SemVersion version, string channel, IList<string> versionFilter)
        {
            ProjectId = projectId;
            Version = version;
            Channel = channel;
            VersionFilter = versionFilter;
        }

        public int ProjectId { get; }
        public SemVersion Version { get; }
        public string Channel { get; }
        public IList<string> VersionFilter { get; }
    }
}
