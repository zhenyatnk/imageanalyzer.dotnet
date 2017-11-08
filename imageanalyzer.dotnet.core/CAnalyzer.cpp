#include "interfaces/IAnalyzer.h" 
#include "imageanalyzer/core/Tasks.hpp" 
#include "threadpoolex/core/ITaskEx.hpp" 
#include "threadpoolex/core/ITaskWait.hpp" 

#include <vector>
#include <atomic>
#include <vcclr.h>

namespace imageanalyzer {
namespace dotnet {
namespace core {

using namespace threadpoolex::core;
using namespace imageanalyzer::core;

namespace {

class CObserverCompleted
    :public EmptyObserverTask
{
public:
    CObserverCompleted(std::atomic_int &aCountCompleted)
        :m_CountCompleted(aCountCompleted)
    {}

    virtual void HandleComplete() override
    {
        ++m_CountCompleted;
    }

private:
    std::atomic_int & m_CountCompleted;
};
//------------------------------------------------------
class CObserverProxy
    :public IObserverTask
{
public:
    CObserverProxy(interfaces::IObserverTask^ aObserver)
        :m_Observer(aObserver)
    {}

    virtual void HandleStart() override
    {
        m_Observer->HandleStart();
    }
    
    virtual void HandleComplete() override
    {
        m_Observer->HandleComplete();
    }

    virtual void HandleError(const std::string &aMessage, const int& aErrorCode) override
    {
        m_Observer->HandleError(gcnew String(aMessage.c_str()), aErrorCode);
    }

private:
    gcroot<interfaces::IObserverTask^> m_Observer;
};
//------------------------------------------------------
class CObserverTryExpansion
    :public EmptyObserverTimer
{
public:
    CObserverTryExpansion(IThreadPool::Ptr aThreadPool)
        :m_ThreadPool(aThreadPool)
    {}

    void HandleCheck() override
    {
        m_ThreadPool->TryExpansion();
    }

private:
    IThreadPool::Ptr m_ThreadPool;
};
//------------------------------------------------------
std::wstring MarshalString(String ^ s)
{
    std::wstring os;
    using namespace Runtime::InteropServices;
    const wchar_t* chars =
        (const wchar_t*)(Marshal::StringToHGlobalUni(s)).ToPointer();
    os = chars;
    Marshal::FreeHGlobal(IntPtr((void*)chars));
    return os;
}

}

public ref class CAnalyzer
    :public interfaces::IAnalyzer
{
public:
    CAnalyzer();
    ~CAnalyzer();

    virtual void add_task(String^ aFileName, ICollection<interfaces::IObserverTask^>^ aObserver) override;
    virtual bool complete() override;

private:
    threadpoolex::core::ITimerActive::Ptr* m_timer;
    threadpoolex::core::IThreadPool::Ptr* m_threadpool;
    std::atomic_int* m_CountCompleted;
    std::atomic_int* m_Count;
};

CAnalyzer::CAnalyzer()
    :m_timer(new ITimerActive::Ptr(CreateTimerActive(1000))),
    m_threadpool(new threadpoolex::core::IThreadPool::Ptr(CreateThreadPool(1, CreateExpansionToCPU(threadpoolex::core::CreateSystemInfo(), 80, 1)))),
    m_CountCompleted(new std::atomic_int(0)),
    m_Count(new std::atomic_int(0))
{
    (*m_timer)->AddObserver(std::make_shared<CObserverTryExpansion>(*m_threadpool));
}

CAnalyzer::~CAnalyzer()
{
    delete m_CountCompleted;
    delete m_Count;
    delete m_timer;
    delete m_threadpool;
}

void CAnalyzer::add_task(String^ aFileName, ICollection<interfaces::IObserverTask^>^ aObjservers)
{
    auto task = CreateTaskAnalyzeInFile(MarshalString(aFileName));
    
    for each (interfaces::IObserverTask^ objserver in aObjservers)
        task->AddObserver(IObserverTask::Ptr(new CObserverProxy(objserver)));
    task->AddObserver(IObserverTask::Ptr(new CObserverCompleted(*m_CountCompleted)));

    ++(*m_Count);
    (*m_threadpool)->AddTask(task);
}

bool CAnalyzer::complete()
{
    return *m_CountCompleted == *m_Count;
}

interfaces::IAnalyzer^ IAnalyzerCreate::Create()
{
    return gcnew CAnalyzer();
}

}
}
}