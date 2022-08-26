using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seasail.Data;

namespace Seasail.Extensions
{
    public static partial class TNHExtension
    {


        /// <summary>
        /// 如果条件成立，添加项
        /// </summary>
        public static void AddIf<T>(this ICollection<T> collection, T value, Func<bool> func)
        {
            Check.NotNull(collection, nameof(collection));
            if (func())
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// 如果不存在,添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="value"></param>
        /// <param name="existFunc"></param>
        public static void AddIfNotExist<T>(this ICollection<T> collection, T value, Func<T, bool> existFunc = null)
        {
            Check.NotNull(collection, nameof(collection));
            bool exists = existFunc == null ? collection.Contains(value) : existFunc(value);
            if (!exists)
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// 如果不为空，添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="value"></param>
        public static void AddIfNotNull<T>(this ICollection<T> collection, T value) where T : class
        {
            Check.NotNull(collection, nameof(collection));
            if (value != null)
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// 判断集合是否为null或空集合
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// 判断两个集合是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="collectionOther"></param>
        /// <returns></returns>
        public static bool Equals<T>(this ICollection<T> collection, ICollection<T> collectionOther)
        {
            return Enumerable.SequenceEqual(collection, collectionOther);
        }

        /// <summary>
        /// 获取对象，不存在对使用委托添加对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="selector"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static T GetIfNullAdd<T>(this ICollection<T> collection, Func<T, bool> selector, Func<T> factory)
        {
            Check.NotNull(collection, nameof(collection));
            T item = collection.FirstOrDefault(selector);
            if (item == null)
            {
                item = factory();
                collection.Add(item);
            }
            return item;
        }
    }
}
