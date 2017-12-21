#pragma once

#include "baseex\core\ITimerActive.hpp"
#include "IObserverTask.h"

using namespace System;
using namespace System::Collections::Generic;

namespace imageanalyzer {
namespace dotnet {
namespace core {
namespace interfaces {

public interface class IComparator
    :public IDisposable
{
    double compare(String^ aFileName);
};

}

public ref class IComparatorCreate
{
public:
    static interfaces::IComparator^ Create(String^ aFileFind);
};

}
}
}
