using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, PagableDTO<T> pageable) where T : class
        {
            if (!string.IsNullOrWhiteSpace(pageable.SearchTerm))
                query = typeof(T) == typeof(UserDTO) ? query.ApplyUserSearch(pageable.SearchTerm) : query.ApplySearch(pageable.SearchTerm);
            

            if (!string.IsNullOrWhiteSpace(pageable.OrderBy))
                query = query.ApplyOrdering(pageable.OrderBy, pageable.IsAscending);
            

            return query.Skip((pageable.Page - 1) * pageable.Length)
                        .Take(pageable.Length);
        }

        //private static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, string searchTerm)
        //{
        //    if (string.IsNullOrEmpty(searchTerm))
        //    {
        //        return query; // Skip filtering if searchTerm is empty or null
        //    }

        //    var parameter = Expression.Parameter(typeof(T), "x");
        //    var searchProperties = typeof(T).GetProperties()
        //        .Where(p => p.PropertyType == typeof(string));

        //    Expression orExpression = null;

        //    foreach (var property in searchProperties)
        //    {
        //        var propertyAccess = Expression.Property(parameter, property);
        //        var searchExpression = Expression.Call(
        //            propertyAccess,
        //            typeof(string).GetMethod("Contains", new[] { typeof(string) }),
        //            Expression.Constant(searchTerm, typeof(string))
        //        );

        //        orExpression = orExpression == null
        //            ? searchExpression
        //            : Expression.OrElse(orExpression, searchExpression);
        //    }

        //    if (orExpression == null) return query;

        //    var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);
        //    return query.Where(lambda);
        //}
        private static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query; // Skip filtering if searchTerm is empty or null
            }

            var parameter = Expression.Parameter(typeof(T), "x");

            // Define properties to exclude from search
            var excludedProperties = new HashSet<string>
            {
                "CreatedAt",
                "CreatedDate",
                "LastUpdatedAt",
                "DeletedAt",
                "CreatedBy_Id",
                "UpdatedBy_Id",
                "DeletedBy_Id"
            };

            // Get properties of type T to include in the search, excluding the unwanted ones
            var searchProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && !excludedProperties.Contains(p.Name));

            Expression orExpression = null;

            foreach (var property in searchProperties)
            {
                var propertyAccess = Expression.Property(parameter, property);

                // Create a "Contains" expression for string properties
                var searchExpression = Expression.Call(
                    propertyAccess,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                    Expression.Constant(searchTerm, typeof(string))
                );

                orExpression = orExpression == null
                    ? searchExpression
                    : Expression.OrElse(orExpression, searchExpression);
            }

            // If no properties to search, return the query without modifications
            if (orExpression == null) return query;

            var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);
            return query.Where(lambda);
        }



        private static IQueryable<T> ApplyUserSearch<T>(this IQueryable<T> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query; // Skip filtering if searchTerm is empty or null
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var searchProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string)); // Only search string properties

            Expression orExpression = null;

            foreach (var property in searchProperties)
            {
                if (property.Name != "Password") // Skip password field
                {
                    // Handle "Phone" property renaming to "PhoneNumber"
                    bool changePhone = false;
                    if (property.Name == "Phone")
                    {
                        changePhone = true;
                    }

                    var propertyAccess = Expression.Property(parameter, changePhone ? typeof(T).GetProperty("PhoneNumber") : property );
                    var searchExpression = Expression.Call(
                        propertyAccess,
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        Expression.Constant(searchTerm, typeof(string))
                    );

                    // Combine each search condition using OR logic
                    orExpression = orExpression == null
                        ? searchExpression
                        : Expression.OrElse(orExpression, searchExpression);
                }
            }

            if (orExpression == null) return query; // No valid search expressions

            var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);
            return query.Where(lambda);
        }



        private static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string orderBy, bool isAscending)
        {
            var entityType = typeof(T);
            var property = entityType.GetProperty(orderBy);

            if (property == null)
                throw new ArgumentException($"Property '{orderBy}' does not exist on type '{entityType.Name}'");

            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(parameter, property);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var methodName = isAscending ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType, property.PropertyType);

            return (IQueryable<T>)method.Invoke(null, new object[] { query, orderExpression });
        }
    }

}
