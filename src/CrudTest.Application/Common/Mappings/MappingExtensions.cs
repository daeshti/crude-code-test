using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CrudTest.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudTest.Application.Common.Mappings 
{

    /***
     * This class provides extension methods to IQueryable to provide a more fluent api
     * (making chaining the dot(.) operator possible.
     */
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}