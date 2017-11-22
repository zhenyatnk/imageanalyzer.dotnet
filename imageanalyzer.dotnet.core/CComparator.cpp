#include "interfaces/IComparator.h" 
#include <imageanalyzer.native/core/Tasks.hpp>
#include <threadpoolex/core/ITaskEx.hpp> 
#include <threadpoolex/core/ITaskWait.hpp>

#include <imageanalyzer.native/core/TMetaImage.hpp>
#include <imageanalyzer.native/core/IMetaComparator.hpp>

#include <fstream>
#include <vector>
#include <atomic>
#include <vcclr.h>

namespace imageanalyzer {
namespace dotnet {
namespace core {

using namespace threadpoolex::core;
using namespace imageanalyzer::native::core;

namespace {
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

public ref class CComparator
    :public interfaces::IComparator
{
public:
    CComparator(String^ aFileName);
    ~CComparator();

    virtual double compare(String^ aFileName) override;

private:
    TMetaImage CreateTMetaImage(String^ aFileName);

private:
    imageanalyzer::native::core::TMetaImage *meta_find;
};

CComparator::CComparator(String^ aFileName)
{
    meta_find = new imageanalyzer::native::core::TMetaImage(CreateTMetaImage(aFileName));
}

CComparator::~CComparator()
{
    delete meta_find;
}

double CComparator::compare(String^ aFileName)
{
    return CreateEuclideanDistance()->GePercentEqual(*meta_find, CreateTMetaImage(aFileName));
}

TMetaImage CComparator::CreateTMetaImage(String^ aFileName)
{
    return CreateTMetaImageFromFile(MarshalString(aFileName));
}

interfaces::IComparator^ IComparatorCreate::Create(String^ aFileName)
{
    return gcnew CComparator(aFileName);
}

}
}
}