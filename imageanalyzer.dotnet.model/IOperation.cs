using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imageanalyzer.dotnet.ui.operations
{
    public interface IOperation
        : IObservableOperation
    {
        void Execute();
    }
}
