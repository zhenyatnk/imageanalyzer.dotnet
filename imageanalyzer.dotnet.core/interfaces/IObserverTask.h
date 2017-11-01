#pragma once

#include "IObserverError.h"

using namespace System;

namespace imageanalyzer {
namespace dotnet {
namespace core {
namespace interfaces {

public interface class IObserverTask
    :public IObserverError
{
    void HandleStart();
    void HandleComplete();
};

}
}
}
}
