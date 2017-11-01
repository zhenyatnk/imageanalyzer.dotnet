#pragma once

#include "interfaces\IObserverTask.h"

using namespace System;

namespace imageanalyzer {
namespace dotnet {
namespace core {

public ref class CAnalyzer
{
public:
    void AddTask(String^ aFileName, interfaces::IObserverTask^ aObserver);

};

}
}
}
