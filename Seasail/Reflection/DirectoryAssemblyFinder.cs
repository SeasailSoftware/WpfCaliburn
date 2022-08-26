using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seasail.Reflection
{
    /// <summary>
    /// 目录程序集查找器
    /// </summary>
    public class DirectoryAssemblyFinder : IAssemblyFinder
    {
        private static readonly ConcurrentDictionary<string, Assembly[]> CacheDict = new ConcurrentDictionary<string, Assembly[]>();
        private readonly string _path;

        /// <summary>
        /// 初始化一个<see cref="DirectoryAssemblyFinder"/>类型的新实例
        /// </summary>
        public DirectoryAssemblyFinder(string path)
        {
            _path = path;
        }

        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        public Assembly[] Find(Func<Assembly, bool> predicate, bool fromCache = false)
        {
            return FindAll(fromCache).Where(predicate).ToArray();
        }

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <returns></returns>
        public Assembly[] FindAll(bool fromCache = false)
        {
            if (fromCache && CacheDict.ContainsKey(_path))
            {
                return CacheDict[_path];
            }
            var files = Directory.GetFiles(_path, "*.dll", SearchOption.TopDirectoryOnly)
                .Concat(Directory.GetFiles(_path, "*.exe", SearchOption.TopDirectoryOnly))
                .ToArray();
            //try
            //{
            //    Assembly[] assemblies = files.Select(Assembly.LoadFile).Distinct().ToArray();
            //    CacheDict[_path] = assemblies;
            //    return assemblies;
            //}
            //catch(Exception ex)
            //{
            //    return null;
            //}
            List<Assembly> assemblies = new List<Assembly>();
            foreach(var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    assemblies.Add(assembly);
                }
                catch
                {

                }
            }
            CacheDict[_path] = assemblies.Distinct().ToArray();
            return CacheDict[_path];
        }

    }
}
