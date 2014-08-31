//
// SampleDataSource.cpp
// Implementation of the SampleDataSource, SampleDataGroup, SampleDataItem, and SampleDataCommon classes
//

#include "pch.h"

using namespace App1::Data;

using namespace Platform;
using namespace Platform::Collections;
using namespace concurrency;
using namespace Windows::ApplicationModel::Resources::Core;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Interop;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Media::Imaging;
using namespace Windows::Storage;
using namespace Windows::Data::Json;

//
// SampleDataItem
//

SampleDataItem::SampleDataItem(String^ uniqueId, String^ title, String^ subtitle, String^ imagePath, String^ description,
	String^ content) :
	_uniqueId(uniqueId),
	_title(title),
	_subtitle(subtitle),
	_description(description),
	_imagePath(imagePath),
	_content(content)
	{
	}

	String^ SampleDataItem::UniqueId::get()
	{
		return _uniqueId;
	}

	String^ SampleDataItem::Title::get()
	{
		return _title;
	}

	String^ SampleDataItem::Subtitle::get()
	{
		return _subtitle;
	}

	String^ SampleDataItem::Description::get()
	{
		return _description;
	}

	String^ SampleDataItem::Content::get()
	{
		return _content;
	}

	String^ SampleDataItem::ImagePath::get()
	{
		return _imagePath;
	}

	Windows::UI::Xaml::Data::ICustomProperty^ SampleDataItem::GetCustomProperty(Platform::String^ name)
	{
		return nullptr;
	}

	Windows::UI::Xaml::Data::ICustomProperty^ SampleDataItem::GetIndexedProperty(Platform::String^ name, Windows::UI::Xaml::Interop::TypeName type)
	{
		return nullptr;
	}

	Platform::String^ SampleDataItem::GetStringRepresentation()
	{
		return Title;
	}

	Windows::UI::Xaml::Interop::TypeName SampleDataItem::Type::get()
	{
		return this->GetType();
	}

	//
	// SampleDataGroup
	//

	SampleDataGroup::SampleDataGroup(String^ uniqueId, String^ title, String^ subtitle, String^ imagePath, String^ description) :
		_uniqueId(uniqueId),
		_title(title),
		_subtitle(subtitle),
		_description(description),
		_imagePath(imagePath),
		_items(ref new Vector<SampleDataItem^>())
		{
		}

		String^ SampleDataGroup::UniqueId::get()
		{
			return _uniqueId;
		}

		String^ SampleDataGroup::Title::get()
		{
			return _title;
		}

		String^ SampleDataGroup::Subtitle::get()
		{
			return _subtitle;
		}

		String^ SampleDataGroup::Description::get()
		{
			return _description;
		}

		IObservableVector<SampleDataItem^>^ SampleDataGroup::Items::get()
		{
			return _items;
		}

		String^ SampleDataGroup::ImagePath::get()
		{
			return _imagePath;
		}

		Windows::UI::Xaml::Data::ICustomProperty^ SampleDataGroup::GetCustomProperty(Platform::String^ name)
		{
			return nullptr;
		}

		Windows::UI::Xaml::Data::ICustomProperty^ SampleDataGroup::GetIndexedProperty(Platform::String^ name, Windows::UI::Xaml::Interop::TypeName type)
		{
			return nullptr;
		}

		Platform::String^ SampleDataGroup::GetStringRepresentation()
		{
			return Title;
		}

		Windows::UI::Xaml::Interop::TypeName SampleDataGroup::Type::get()
		{
			return this->GetType();
		}

		//
		// SampleDataSource
		//

		SampleDataSource::SampleDataSource()
		{
			_groups = ref new Vector<SampleDataGroup^>();

			Uri^ uri = ref new Uri("ms-appx:///DataModel/SampleData.json");
			create_task(StorageFile::GetFileFromApplicationUriAsync(uri))
				.then([](StorageFile^ storageFile)
			{
				return FileIO::ReadTextAsync(storageFile);
			})
				.then([this](String^ jsonText)
			{
				JsonObject^ jsonObject = JsonObject::Parse(jsonText);
				auto jsonVector = jsonObject->GetNamedArray("Groups")->GetView();

				for (const auto &jsonGroupValue : jsonVector)
				{
					JsonObject^ groupObject = jsonGroupValue->GetObject();
					SampleDataGroup^ group = ref new SampleDataGroup(groupObject->GetNamedString("UniqueId"),
						groupObject->GetNamedString("Title"),
						groupObject->GetNamedString("Subtitle"),
						groupObject->GetNamedString("ImagePath"),
						groupObject->GetNamedString("Description"));

					auto jsonItemVector = groupObject->GetNamedArray("Items")->GetView();
					for (const auto &jsonItemValue : jsonItemVector)
					{
						JsonObject^ itemObject = jsonItemValue->GetObject();

						SampleDataItem^ item = ref new SampleDataItem(itemObject->GetNamedString("UniqueId"),
							itemObject->GetNamedString("Title"),
							itemObject->GetNamedString("Subtitle"),
							itemObject->GetNamedString("ImagePath"),
							itemObject->GetNamedString("Description"),
							itemObject->GetNamedString("Content"));

						group->Items->Append(item);
					};

					_groups->Append(group);
				};
			})
				.then([this](task<void> t)
			{
				try
				{
					t.get();
				}
				catch (Platform::COMException^ e)
				{
					OutputDebugString(e->Message->Data());
					// TODO: If App can recover from exception,
					// remove throw; below and add recovery code.
					throw;
				}
				// Signal load completion event
				_loadCompletionEvent.set();
			});
		}

		IObservableVector<SampleDataGroup^>^ SampleDataSource::Groups::get()
		{
			return _groups;
		}

		SampleDataSource^ SampleDataSource::_sampleDataSource = nullptr;

		task<void> SampleDataSource::Init()
		{
			if (_sampleDataSource == nullptr)
			{
				_sampleDataSource = ref new SampleDataSource();
			}
			return create_task(_sampleDataSource->_loadCompletionEvent);
		}

		task<IIterable<SampleDataGroup^>^> SampleDataSource::GetGroups()
		{
			return Init()
				.then([]() -> IIterable<SampleDataGroup^> ^
			{
				return _sampleDataSource->Groups;
			});
		}

		task<SampleDataGroup^> SampleDataSource::GetGroup(String^ uniqueId)
		{
			return Init()
				.then([uniqueId]() -> SampleDataGroup ^
			{
				// Simple linear search is acceptable for small data sets
				for (const auto& group : _sampleDataSource->Groups)
				{
					if (group->UniqueId == uniqueId)
					{
						return group;
					}
				}
				return nullptr;
			});
		}

		task<SampleDataItem^> SampleDataSource::GetItem(String^ uniqueId)
		{
			return Init()
				.then([uniqueId]() -> SampleDataItem ^
			{
				// Simple linear search is acceptable for small data sets
				for (const auto& group : _sampleDataSource->Groups)
				{
					for (const auto& item : group->Items)
					{
						if (item->UniqueId == uniqueId)
						{
							return item;
						}
					}
				}
				return nullptr;
			});
		}
