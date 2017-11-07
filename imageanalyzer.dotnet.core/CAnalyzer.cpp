#include "interfaces/IAnalyzer.h" 
#include "imageanalyzer/core/Tasks.hpp" 
#include "threadpoolex/core/ITaskWait.hpp" 

#include <vector>
#include <vcclr.h>

namespace imageanalyzer {
namespace dotnet {
namespace core {

using namespace threadpoolex::core;
using namespace imageanalyzer::core;

namespace {

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
    virtual void wait() override;

private:
    threadpoolex::core::ITimerActive::Ptr* m_timer;
    std::vector<IWait::Ptr> *m_Waiters;
};

CAnalyzer::CAnalyzer()
    :m_timer(new ITimerActive::Ptr(CreateTimerActive(1000)))
{
    (*m_timer)->AddObserver(std::make_shared<CObserverTryExpansion>(ThreadPoolGlobal::GetInstance()()));
    m_Waiters = new std::vector<IWait::Ptr>();
}

CAnalyzer::~CAnalyzer()
{
    delete m_timer;
    delete m_Waiters;
}

void CAnalyzer::add_task(String^ aFileName, ICollection<interfaces::IObserverTask^>^ aObjservers)
{
    IWait::Ptr lwaiter;
    auto task = CreateTaskWait(CreateTaskAnalyzeInFile(MarshalString(aFileName)), lwaiter);
    
    for each (interfaces::IObserverTask^ objserver in aObjservers)
        task->AddObserver(IObserverTask::Ptr(new CObserverProxy(objserver)));

    ThreadPoolGlobal::GetInstance()()->AddTask(task);
    m_Waiters->push_back(lwaiter);
}

void CAnalyzer::wait()
{
    for (auto wait : *m_Waiters)
        wait->Wait();

    delete m_Waiters;
    m_Waiters = new std::vector<IWait::Ptr>();
}

interfaces::IAnalyzer^ IAnalyzerCreate::Create()
{
    return gcnew CAnalyzer();
}

}
}
}