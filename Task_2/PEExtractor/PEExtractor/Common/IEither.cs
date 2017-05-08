using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Common
{
    
    /// https://siliconcoding.wordpress.com/2012/10/26/either_in_csharp/ 
    public interface IEither<out L, out R>
    {
        T Case<T>(Func<L, T> ofLeft, Func<R, T> ofRight);
        void Case(Action<L> ofLeft, Action<R> ofRight);
    }
}
