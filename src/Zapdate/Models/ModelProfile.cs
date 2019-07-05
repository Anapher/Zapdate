using AutoMapper;
using System.Linq;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto.Universal;
using Zapdate.Models.Universal;

namespace Zapdate.Models
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<UpdatePackage, UpdatePackageDto>()
                .ForMember(x => x.Version, opts => opts.MapFrom(y => y.VersionInfo.SemVersion))
                .ForMember(x => x.CustomFields, opts => opts.MapFrom(y => y.CustomFields.ToDictionary(e => e.Key, e => e.Value)));

            CreateMap<UpdateChangelog, UpdateChangelogInfo>();
            CreateMap<UpdateChangelogInfo, UpdateChangelog>();

            CreateMap<UpdateFile, UpdateFileDto>();
            CreateMap<UpdateFileDto, UpdateFile>().ConstructUsing(x => new UpdateFile(x.Path, x.Hash, x.Signature ?? ""));

            CreateMap<UpdatePackageDistribution, UpdatePackageDistributionInfo>();
            CreateMap<UpdatePackageDistributionInfo, UpdatePackageDistribution>();
        }
    }
}
