using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageanalyzer.dotnet.ui.operations
{
    public interface IObserverOperation
    {
        void HandleStart();
        void HandleChangeProgress(double percents);
        void HandleComplete();
    }

    public interface IObservableOperation
    {
        void AddObserver(IObserverOperation aHandler);
        void RemoveObserver(IObserverOperation aHandler);
    }

    public interface INotifierStart
    {
        void NotifyStart();
    }

    public interface INotifierProgress
    {
        void NotifyChangeProgress(double percents);
    }

    public interface INotifierComplete
    {
        void NotifyComplete();
    }

    public class CObservableOperation
        : IObservableOperation,
        INotifierStart,
        INotifierProgress,
        INotifierComplete
    {
        public CObservableOperation()
        {
            m_Handlers = new List<IObserverOperation>();
        }

        public void AddObserver(IObserverOperation aHandler)
        {
            m_Handlers.Add(aHandler);
        }

        public void RemoveObserver(IObserverOperation aHandler)
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

        private List<IObserverOperation> m_Handlers;
    }

    public class CBaseObservableOperation
        : IObservableOperation
    {
        public CBaseObservableOperation()
        {
            m_Observable = new CObservableOperation();
        }

        public void AddObserver(IObserverOperation aHandler)
        {
            m_Observable.AddObserver(aHandler);
        }

        public void RemoveObserver(IObserverOperation aHandler)
        {
            m_Observable.RemoveObserver(aHandler);
        }

        protected CObservableOperation GetObserver()
        {
            return m_Observable;
        }

        private CObservableOperation m_Observable;
    }
}