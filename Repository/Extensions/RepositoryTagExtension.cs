using Entities.Models;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryTagExtensions
    {
        public static IQueryable<Tag> Search(this IQueryable<Tag> tags, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return tags;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return tags.Where(c => c.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Tag> Sort(this IQueryable<Tag> tags, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return tags.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Tag>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return tags.OrderBy(e => e.Name);

            return tags.OrderBy(orderQuery);
        }


    }
}
