using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageanalyzer.dotnet.model.operations
{
    public interface IObserverOperationCompare
        : IObserverOperation
    {
        void HandleCompareComplete(double percent, string filename);
    }

    public interface IObservableOperationCompare
    {
        void AddObserver(IObserverOperationCompare aHandler);
        void RemoveObserver(IObserverOperationCompare aHandler);
    }

    public interface INotifierCompareComplete
    {
        void NotifyCompareComplete(double percent, string filename);
    }
    
    public class CObservableOperationCompare
        : IObservableOperationCompare,
        INotifierStart,
        INotifierProgress,
        INotifierComplete,
        INotifierCompareComplete
    {
        public CObservableOperationCompare()
        {
            m_Handlers = new List<IObserverOperationCompare>();
        }

        public void AddObserver(IObserverOperationCompare aHandler)
        {
            m_Handlers.Add(aHandler);
        }

        public void RemoveObserver(IObserverOperationCompare aHandler)
        {
            m_Handlers.Remove(aHandler);
        }

        public void NotifyStart()
        {
            foreach (var lObserver in m_Handlers)
                lObserver.HandleStart();
        }

        public void NotifyChangeProgress(double percents)
        {
            foreach (var lObserver in m_Handlers)
                lObserver.HandleChangeProgress(percents);
        }

        public void NotifyComplete()
        {
            foreach (var lObserver in m_Handlers)
                lObserver.HandleComplete();
        }
        public void NotifyCompareComplete(double percent, string filename)
        {
            foreach (var lObserver in m_Handlers)
                lObserver.HandleCompareComplete(percent, filename);
        }

        private List<IObserverOperationCompare> m_Handlers;
    }

    public class CBaseObservableOperationCompare
        : IObservableOperationCompare
    {
        public CBaseObservableOperationCompare()
        {
            m_Observable = new CObservableOperationCompare();
        }

        public void AddObserver(IObserverOperationCompare aHandler)
        {
            m_Observable.AddObserver(aHandler);
        }

        public void RemoveObserver(IObserverOperationCompare aHandler)
        {
            m_Observable.RemoveObserver(aHandler);
        }

        protected CObservableOperationCompare GetObserver()
        {
            return m_Observable;
        }

        private CObservableOperationCompare m_Observable;
    }
}