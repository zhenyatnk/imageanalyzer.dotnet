#pragma once

#include "threadpoolex\core\ITimerActive.hpp"
#include "IObserverTask.h"

using namespace System;

namespace imageanalyzer {
namespace dotnet {
namespace core {
namespace interfaces {

public interface class IAnalyzer
{
    void add_task(String^ aFileName, interfaces::IObserverTask^ aObserver);
    void wait();
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
