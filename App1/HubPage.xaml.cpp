//
// HubPage.xaml.cpp
// Implementation of the HubPage class
//

#include "pch.h"
#include "HubPage.xaml.h"
#include "SectionPage.xaml.h"
#include "ItemPage.xaml.h"

using namespace App1;

using namespace Platform;
using namespace Platform::Collections;
using namespace concurrency;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Interop;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The HubPage item template is documented at http://go.microsoft.com/fwlink/?LinkID=321224

HubPage::HubPage()
{
	InitializeComponent();
	SetValue(_defaultViewModelProperty, ref new Map<String^,Object^>(std::less<String^>()));
	auto navigationHelper = ref new Common::NavigationHelper(this);
	SetValue(_navigationHelperProperty, navigationHelper);
	navigationHelper->LoadState += ref new Common::LoadStateEventHandler(this, &HubPage::LoadState);
}

DependencyProperty^ HubPage::_defaultViewModelProperty =
	DependencyProperty::Register("DefaultViewModel",
		TypeName(IObservableMap<String^,Object^>::typeid), TypeName(HubPage::typeid), nullptr);

/// <summary>
/// used as a trivial view model.
/// </summary>
IObservableMap<String^, Object^>^ HubPage::DefaultViewModel::get()
{
	return safe_cast<IObservableMap<String^, Object^>^>(GetValue(_defaultViewModelProperty));
}

DependencyProperty^ HubPage::_navigationHelperProperty =
	DependencyProperty::Register("NavigationHelper",
		TypeName(Common::NavigationHelper::typeid), TypeName(HubPage::typeid), nullptr);

/// <summary>
/// Gets an implementation of <see cref="NavigationHelper"/> designed to be
/// used as a trivial view model.
/// </summary>
Common::NavigationHelper^ HubPage::NavigationHelper::get()
{
	return safe_cast<Common::NavigationHelper^>(GetValue(_navigationHelperProperty));
}

/// The methods provided in this section are simply used to allow
/// NavigationHelper to respond to the page's navigation methods.
/// 
/// Page specific logic should be placed in event handlers for the  
/// <see cref="NavigationHelper::LoadState"/>
/// and <see cref="NavigationHelper::SaveState"/>.
/// The navigation parameter is available in the LoadState method 
/// in addition to page state preserved during an earlier session.

void HubPage::OnNavigatedTo(NavigationEventArgs^ e)
{
	NavigationHelper->OnNavigatedTo(e);
}

void HubPage::OnNavigatedFrom(NavigationEventArgs^ e)
{
	NavigationHelper->OnNavigatedFrom(e);
}

/// <summary>
/// Populates the page with content passed during navigation.  Any saved state is also
/// provided when recreating a page from a prior session.
/// </summary>
/// <param name="sender">
/// The source of the event; typically <see cref="NavigationHelper"/>
/// </param>
/// <see cref="Frame::Navigate(Type, Object)"/> when this page was initially requested and
/// a dictionary of state preserved by this page during an earlier
/// session.  The state will be null the first time a page is visited.</param>
void HubPage::LoadState(Object^ sender, Common::LoadStateEventArgs^ e)
{
	(void) sender;	// Unused parameter
	(void) e;	// Unused parameter

	// TODO: Create an appropriate data model for your problem domain to replace the sample data
	Data::SampleDataSource::GetGroup("Group-4")
		.then([this](Data::SampleDataGroup^ Section3Items)
	{
		DefaultViewModel->Insert("Section3Items", Section3Items);
	}, task_continuation_context::use_current());
}

/// <summary>
/// Invoked when a HubSection header is clicked.
/// </summary>
/// <param name="sender">The Hub that contains the HubSection whose header was clicked.</param>
/// <param name="e">Event data that describes how the click was initiated.</param>
void HubPage::Hub_SectionHeaderClick(Object^ sender, HubSectionHeaderClickEventArgs^ e)
{
	HubSection^ section = e->Section;
	auto group = section->DataContext;

	// Navigate to the appropriate destination page, configuring the new page
	// by passing required information as a navigation parameter
	Frame->Navigate(TypeName(SectionPage::typeid), safe_cast<Data::SampleDataGroup^>(group)->UniqueId);
}

/// <summary>
/// Invoked when an item within a section is clicked.
/// </summary>
/// <param name="sender">The GridView or ListView
/// displaying the item clicked.</param>
/// <param name="e">Event data that describes the item clicked.</param>
void HubPage::ItemView_ItemClick(Object^ sender, ItemClickEventArgs^ e)
{
    WinRTXamlToolkit::Debugging::WinRTProxy::DC::ShowVisualTree();
	(void) sender;	// Unused parameter

	// Navigate to the appropriate destination page, configuring the new page
	// by passing required information as a navigation parameter
	auto itemId = safe_cast<Data::SampleDataItem^>(e->ClickedItem)->UniqueId;
	Frame->Navigate(TypeName(ItemPage::typeid), itemId);
}
