using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasail.IO
{
    public interface IFileGenerater:IDisposable
    {
        void Create(string fileName);
        void WriteRow(string[] items);
        void Close();
    }
}
