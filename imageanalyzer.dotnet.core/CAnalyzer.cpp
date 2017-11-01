#include "CAnalyzer.h" 
#include "imageanalyzer/core/Tasks.hpp" 

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

void CAnalyzer::AddTask(String^ aFileName, interfaces::IObserverTask^ aObjserver)
{
    auto task = CreateTaskAnalyzeInFile(MarshalString(aFileName));
    task->AddObserver(IObserverTask::Ptr(new CObserverProxy(aObjserver)));
    ThreadPoolGlobal::GetInstance()()->AddTask(task);
}

}
}
}