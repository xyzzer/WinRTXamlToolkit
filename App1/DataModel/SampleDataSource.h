//
// SampleDataSource.h
// Declaration of the SampleDataSource, SampleDataGroup, SampleDataItem, and SampleDataCommon classes
//

#pragma once

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

namespace App1
{
	namespace Data
	{
		ref class SampleDataGroup; // Resolve circular relationship between SampleDataItem and SampleDataGroup

		/// <summary>
		/// Generic item data model.
		/// </summary>
		[Windows::UI::Xaml::Data::Bindable]
		public ref class SampleDataItem sealed : public Windows::UI::Xaml::Data::ICustomPropertyProvider
		{
		public:
			property Platform::String^ UniqueId { Platform::String^ get(); }
			property Platform::String^ Title { Platform::String^ get(); }
			property Platform::String^ Subtitle { Platform::String^ get(); }
			property Platform::String^ Description { Platform::String^ get(); }
			property Platform::String^ Content { Platform::String^ get(); }
			property Platform::String^ ImagePath { Platform::String^ get(); }

			// Implementation of ICustomPropertyProvider provides a string representation for the object to be used as automation name for accessibility
			virtual Windows::UI::Xaml::Data::ICustomProperty^ GetCustomProperty(Platform::String^ name);
			virtual Windows::UI::Xaml::Data::ICustomProperty^ GetIndexedProperty(Platform::String^ name, Windows::UI::Xaml::Interop::TypeName type);
			virtual Platform::String^ GetStringRepresentation();
			property Windows::UI::Xaml::Interop::TypeName Type { virtual Windows::UI::Xaml::Interop::TypeName get(); }

		internal:
			SampleDataItem(Platform::String^ uniqueId, Platform::String^ title, Platform::String^ subtitle, Platform::String^ imagePath,
				Platform::String^ description, Platform::String^ content);

		private:
			Platform::String^ _uniqueId;
			Platform::String^ _title;
			Platform::String^ _subtitle;
			Platform::String^ _imagePath;
			Platform::String^ _description;
			Platform::String^ _content;
		};

		/// <summary>
		/// Generic group data model.
		/// </summary>
		[Windows::UI::Xaml::Data::Bindable]
		public ref class SampleDataGroup sealed : public Windows::UI::Xaml::Data::ICustomPropertyProvider
		{
		public:
			property Platform::String^ UniqueId { Platform::String^ get(); }
			property Platform::String^ Title { Platform::String^ get(); }
			property Platform::String^ Subtitle { Platform::String^ get(); }
			property Platform::String^ Description { Platform::String^ get(); }
			property Windows::Foundation::Collections::IObservableVector<SampleDataItem^>^ Items
			{
				Windows::Foundation::Collections::IObservableVector<SampleDataItem^>^ get();
			}
			property Platform::String^ ImagePath { Platform::String^ get(); }

			// Implementation of ICustomPropertyProvider provides a string representation for the object to be used as automation name for accessibility
			virtual Windows::UI::Xaml::Data::ICustomProperty^ GetCustomProperty(Platform::String^ name);
			virtual Windows::UI::Xaml::Data::ICustomProperty^ GetIndexedProperty(Platform::String^ name, Windows::UI::Xaml::Interop::TypeName type);
			virtual Platform::String^ GetStringRepresentation();
			property Windows::UI::Xaml::Interop::TypeName Type { virtual Windows::UI::Xaml::Interop::TypeName get(); }

		internal:
			SampleDataGroup(Platform::String^ uniqueId, Platform::String^ title, Platform::String^ subtitle, Platform::String^ imagePath,
				Platform::String^ description);

		private:
			Platform::String^ _uniqueId;
			Platform::String^ _title;
			Platform::String^ _subtitle;
			Platform::String^ _imagePath;
			Platform::String^ _description;
			Platform::Collections::Vector<SampleDataItem^>^ _items;
		};

		/// <summary>
		/// Creates a collection of groups and items with content read from a static json file.
		/// 
		/// SampleDataSource initializes with data read from a static json file included in the 
		/// project.  This provides sample data at both design-time and run-time.
		/// </summary>
		[Windows::UI::Xaml::Data::Bindable]
		public ref class SampleDataSource sealed
		{
		public:
			property Windows::Foundation::Collections::IObservableVector<SampleDataGroup^>^ Groups
			{
				Windows::Foundation::Collections::IObservableVector<SampleDataGroup^>^ get();
			}

		internal:
			SampleDataSource();
			static concurrency::task<Windows::Foundation::Collections::IIterable<SampleDataGroup^>^> GetGroups();
			static concurrency::task<SampleDataGroup^> GetGroup(Platform::String^ uniqueId);
			static concurrency::task<SampleDataItem^> GetItem(Platform::String^ uniqueId);

		private:
			concurrency::task_completion_event<void> _loadCompletionEvent;
			Platform::Collections::Vector<SampleDataGroup^>^ _groups;
			static concurrency::task<void> Init();

			static SampleDataSource^ _sampleDataSource;
		};
	}
}
