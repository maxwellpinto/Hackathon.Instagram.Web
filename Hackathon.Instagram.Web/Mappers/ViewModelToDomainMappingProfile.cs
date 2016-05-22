using AutoMapper;
using Hackathon.Instagram.Core.Models.Response;
using Hackathon.Instagram.Domain.EntityFramework.DataEntities;

namespace Hackathon.Instagram.Web.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OAuthResponse, DataAccessInstagram>()
                .ForMember(x => x.AccessToken, o => o.MapFrom(x => x.AccessToken))
                .ForMember(x => x.FullName, o => o.MapFrom(x => x.User.FullName))
                .ForMember(x => x.IdUser, o => o.MapFrom(x => x.User.Id))
                .ForMember(x => x.ProfilePicture, o => o.MapFrom(x => x.User.ProfilePicture))
                .ForMember(x => x.UserName, o => o.MapFrom(x => x.User.Username));
        }
    }
}