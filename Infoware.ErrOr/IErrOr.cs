using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoware.ErrOr
{
    public interface IErrOr
    {
        bool IsFaulted { get; }
        bool IsSuccess { get; }
        Exception Exception { get; }
    }
}
