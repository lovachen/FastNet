using AutoMapper;
using System;

namespace AiBao.Mapping
{
    //public class MappingProfile : Profile
    //{
    //    public MappingProfile()
    //    {

    //    }
    //}

    public static class MappingExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TDestination>(source);
        }



    }
}
