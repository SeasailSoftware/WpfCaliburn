using Seasail.Dependency;
using Seasail.Finder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seasail.Reflection
{
    /// <summary>
    /// 定义类型查找行为
    /// </summary>
    [IgnoreDependency]
    public interface ITypeFinder : IFinder<Type>
    { }
}
