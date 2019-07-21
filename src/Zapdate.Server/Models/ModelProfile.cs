using AutoMapper;
using System.Linq;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Dto.Universal;
using Zapdate.Server.Models.Universal;

namespace Zapdate.Server.Models
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<UpdatePackage, UpdatePackageDto>()
                .ForMember(x => x.Version, opts => opts.MapFrom(y => y.VersionInfo.SemVersion))
                .ForMember(x => x.CustomFields, opts => opts.MapFrom(y => y.CustomFields.ToDictionary(e => e.Key, e => e.Value)))
                .ForMember(x => x.Changelogs, opts => opts.MapFrom(y => y.Changelogs.OrderBy(x => x.Language)))
                .ForMember(x => x.Files, opts => opts.MapFrom(y => y.Files.OrderBy(x => x.Path)))
                .ForMember(x => x.Distribution, opts => opts.MapFrom(y => y.Distributions.OrderBy(x => x.Name)));

            CreateMap<UpdateChangelog, UpdateChangelogInfo>();
            CreateMap<UpdateChangelogInfo, UpdateChangelog>();

            CreateMap<UpdateFile, UpdateFileDto>();
            CreateMap<UpdateFileDto, UpdateFile>().ConstructUsing(x => new UpdateFile(x.Path, x.Hash, x.Signature ?? ""));

            CreateMap<UpdatePackageDistribution, UpdatePackageDistributionInfo>();
            CreateMap<UpdatePackageDistributionInfo, UpdatePackageDistribution>();
        }
    }
}
