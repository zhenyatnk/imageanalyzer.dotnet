#pragma once

using namespace System;

namespace imageanalyzer {
namespace dotnet {
namespace core {
namespace interfaces {

public interface class IObserverError
{
    void HandleError(String ^aMessage, int aErrorCode);
};

}
}
}
}
