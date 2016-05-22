using System;
using AutoMapper;
using Hackathon.Instagram.Core.Models.Response;
using Hackathon.Instagram.Domain.EntityFramework.DataEntities;
using Hackathon.Instagram.Core.Models;

namespace Hackathon.Instagram.Web.Mappers
{
    public  class DomainToViewModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DataAccessInstagram, UserInfo>()
                .ForMember(x => x.FullName, o => o.MapFrom(x => x.FullName))
                .ForMember(x => x.ProfilePicture, o => o.MapFrom(x => x.ProfilePicture))
                .ForMember(x => x.Username, o => o.MapFrom(x => x.UserName))
                .ForMember(x => x.Id, o => o.MapFrom(x => x.IdUser));

            Mapper.CreateMap<DataAccessInstagram, OAuthResponse>()
                .ForMember(x => x.AccessToken, o => o.MapFrom(x => x.AccessToken))
                .ForMember(x => x.User, o => o.MapFrom(x => x.Map<DataAccessInstagram, UserInfo>() ));         


        }        
    }
}