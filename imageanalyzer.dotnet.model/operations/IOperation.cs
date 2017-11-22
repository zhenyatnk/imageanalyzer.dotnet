using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imageanalyzer.dotnet.model.operations
{
    public interface IOperation
        : IObservableOperation
    {
        void Execute();
    }
    public interface IOperationCompare
    : IObservableOperationCompare
    {
        void Execute();
    }
}
