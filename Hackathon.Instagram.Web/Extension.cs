using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Web
{
    public static class MapExtension
    {
        public static TTo Map<TFrom, TTo>(this TFrom model)
        {
            CreateMapIfNonExistant<TFrom, TTo>();

            return Mapper.Map<TTo>(model);
        }

        private static void CreateMapIfNonExistant<TFrom, TTo>()
        {
            var mapper = Mapper.FindTypeMapFor<TFrom, TTo>();

            if (mapper == null) Mapper.CreateMap<TFrom, TTo>();
        }
    }
}
