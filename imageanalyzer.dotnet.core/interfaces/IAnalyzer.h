#pragma once

#include "threadpoolex\core\ITimerActive.hpp"
#include "IObserverTask.h"

using namespace System;
using namespace System::Collections::Generic;

namespace imageanalyzer {
namespace dotnet {
namespace core {
namespace interfaces {

public interface class IAnalyzer
    :public IDisposable
{
    void add_task(String^ aFileName,  ICollection<interfaces::IObserverTask^>^ aObservers);
    bool complete();
};

}

public ref class IAnalyzerCreate
{
public:
    static interfaces::IAnalyzer^ Create();
};

}
}
}
