### Project Description
A set of controls, extensions and helper classes for [Windows Runtime XAML](http://en.wikipedia.org/wiki/Windows_Runtime_XAML_Framework) applications.

### Project Status
The development of the project has kind of slowed down in 2013. I'm still hoping to speed it up, but I can't promise anything. I have a notebook with tons of ideas and a backlog of bugs and questions to fix. At this stage I would try cross-referencing questions here with [Stack Overflow](http://stackoverflow.com/questions/tagged/winrt-xaml-toolkit) to increase the change of getting an answer. If you see a bug that needs fixed - you should try fixing it yourself. I'll happily take a pull request to include it here and publish a fixed version to NuGet.

### Disclaimer
This project is not managed by Microsoft. Its coordinator is currently employed by Microsoft, but the project has mostly been developed earlier and continues to be developed outside of work hours.
It is not a full port of the [Silverlight Toolkit](http://silverlight.codeplex.com/), though it includes ported versions of some of its controls. This does not diminish the value of the code that is part of it and that you are free to use and modify. Attribution is welcomed, but not required.

### How do I use it?
Clone the full [source code](https://github.com/xyzzer/WinRTXamlToolkit.git) or just the snippet you find useful. Be sure to try the samples! Ask questions on [Stack Overflow](http://stackoverflow.com/questions/tagged/winrt-xaml-toolkit).

For current version for UWP (Windows 10 and Windows Phone 10) use NuGet
* [WinRT XAML Toolkit for Windows 10](https://www.nuget.org/packages/winrtxamltoolkit)
* [WinRT XAML Toolkit - Calendar Control for Windows 10](https://www.nuget.org/packages/WinRTXamlToolkit.Controls.Calendar)
* [WinRT XAML Toolkit - Data Visualization Controls for Windows 10](https://www.nuget.org/packages/winrtxamltoolkit.Controls.DataVisualization)
* [WinRT XAML Toolkit - Gauge Control for Windows 10](https://www.nuget.org/packages/WinRTXamlToolkit.Controls.Gauge)
* [WinRT XAML Toolkit - Debugging Tools for Windows 10](https://www.nuget.org/packages/WinRTXamlToolkit.Debugging)

For older version for Windows 8.1 and Windows Phone 8.1 use NuGet
* [WinRT XAML Toolkit for Windows 8.1](http://www.nuget.org/packages/winrtxamltoolkit.windows)
* [WinRT XAML Toolkit for Windows Phone 8.1](http://www.nuget.org/packages/winrtxamltoolkit.windowsphone)
* [WinRT XAML Toolkit - Calendar Control for Windows 8.1](http://www.nuget.org/packages/winrtxamltoolkit.Controls.Calendar.Windows)
* [WinRT XAML Toolkit - Calendar Control for Windows Phone 8.1](http://www.nuget.org/packages/winrtxamltoolkit.Controls.Calendar.WindowsPhone)
* [WinRT XAML Toolkit - Data Visualization Controls for Windows 8.1](http://www.nuget.org/packages/winrtxamltoolkit.Controls.DataVisualization.Windows)
* [WinRT XAML Toolkit - Data Visualization Controls for Windows Phone 8.1](http://www.nuget.org/packages/winrtxamltoolkit.Controls.DataVisualization.WindowsPhone)
* [WinRT XAML Toolkit - Gauge Control for Windows 8.1](http://www.nuget.org/packages/WinRTXamlToolkit.Controls.Gauge.Windows)
* [WinRT XAML Toolkit - Gauge Control for Windows Phone 8.1](http://www.nuget.org/packages/WinRTXamlToolkit.Controls.Gauge.WindowsPhone)
* [WinRT XAML Toolkit - Debugging Tools for Windows 8.1](http://www.nuget.org/packages/WinRTXamlToolkit.Debugging.Windows)
* [WinRT XAML Toolkit - Debugging Tools for Windows Phone 8.1](http://www.nuget.org/packages/WinRTXamlToolkit.Debugging.WindowsPhone)

For even older versions supporting Windows 8.0 and newer
* [WinRT XAML Toolkit - Core library](http://nuget.org/packages/winrtxamltoolkit/1.6.1.3)
* [WinRT XAML Toolkit - Composition](http://www.nuget.org/packages/WinRTXamlToolkit.Composition/1.6.1.3)
* [WinRT XAML Toolkit - Debugging Tools](http://nuget.org/packages/winrtxamltoolkit.debugging/1.6.1.3)
* [WinRT XAML Toolkit - Calendar Control](http://www.nuget.org/packages/winrtxamltoolkit.Controls.Calendar/1.6.1.3)
* [WinRT XAML Toolkit - Data Visualization Controls](http://www.nuget.org/packages/winrtxamltoolkit.Controls.DataVisualization/1.6.1.3)
* [WinRT XAML Toolkit - Gauge Control](http://www.nuget.org/packages/WinRTXamlToolkit.Controls.Gauge/1.6.1.3)

### Features
* Integrated extensions from the **[AsyncUI library](http://asyncui.codeplex.com/)** - a set of extension methods for UI classes that add support for async/await to wait for events such as:
  * Wait for a BitmapImage to load
  * Wait for a Button or one of a list of buttons to be clicked
  * Wait for a FrameworkElement to load, unload or become non-zero-sized
  * Wait for all images in a FrameworkElement's visual tree to load
  * Wait for a MediaElement to change state - eg. to start or finish playback
  * Wait for a Selector (e.g. ListBox) to change selected item
  * Wait for a Storyboard to complete
  * Wait for a VisualState transition to complete
  * Wait for a WebView to complete navigation
  * Wait for a WriteableBitmap to load (uses polling due to lack of an event)

* **Controls**
  * **AlternativeFrame**, **AlternativePage** - support asynchronous page transitions and preloading pages so when navigation is initiated - all content might already be loaded. Includes 4 built-in transitions: dissolve, flip, push, wipe. You can add new ones yourself.
  * **AnimatingContainer** - a container control that will animate its contents rotating or zooming in/out, eg. to make them feel more alive.
  * **AutoCompleteTextBox** - a TextBox with dictionary-based autocompletion support by [fex](http://www.codeplex.com/site/users/view/fex) with keyboard support added by yours truly.
  * **CameraCaptureControl** - supports displaying camera preview, capturing photos and videos, cycling between existing video capture devices, setting preference to Front/Back panel camera, etc.
  * **CascadingTextBlock** - a TextBlock replacement that animates the individual letters in a cascade - fading in while falling down into position, then optionally fading out while falling down from the standard position.
  * **Chart** - Silverlight Toolkit's Chart control ported by [Mahmoud Moussa](http://twitter.com/MahmoudMoussa) ([ZeeMoussa](http://www.codeplex.com/site/users/view/ZeeMoussa) on CodePlex) and merged from his [Windows 8 Toolkit - Charts and More](http://modernuitoolkit.codeplex.com/team/view) project. Supports pie charts, bar charts, scatter charts, etc.
  * **CountdownControl** - a movie-style control that animates a ring-slice shape while counting down seconds - e.g. to take a picture with a camera after a given number of seconds (supports async/await).
  * **CustomAppBar** - a custom implementation of the AppBar that automatically handles the three gestures to switch IsOpen (WinKey+Z, Right-Click, EdgeGesture), adds a CanOpen property, so you can prevent it from opening and opens/hides with a sliding animation when placed anywhere in the app, so you can layer content on top of it. Also features CanDismiss property to force the app bar to stay open in some situations, CanOpenInSnappedView which allows to block the app bar from showing up when the app is in the snapped view.
  * **CustomGridSplitter** - a custom implementation of a GridSplitter as a templated control. 
  * **DelayedLoadControl** - given a content/DataTemplate - loads the contents after a given amount of time - e.g. to allow for staged loading of contents on screen.
  * **ImageButton** - a custom Button control that takes one to three images to be used to represent different states of the button (normal/hover/pressed/disabled) as well as ways for the button to render all 4 states with just one or two images.
  * **ImageToggleButton** - custom ToggleButton control, that like ImageButton - helps create buttons based on button state images using from 1 to 8 different state images and generating other state images with some simple image processing.
  * **InputDialog** - a custom/templated dialog control that takes text input.
  * **ListItemButton** - a simple button control with Click event and Command property to be used inside of list controls (the standard button steals pointer capture from the List/Grid~Items so they can't be selected.
  * **NumericUpDown** - allows to display and manipulate a number using text input, +/- buttons or Blend-like swipe-manipulations
  * **PieSlice** - a pie slice path/shape given StartAngle, EndAngle and Radius.
  * **RingSlice** - a pie slice path/shape given StartAngle, EndAngle, Radius and InnerRadius.
  * **TreeView** - traditional tree view control ported from Silverlight Toolkit. Has separate touch and mouse themes.
  * **ToolWindow** - a window content control that snaps/docks to edges if the screen or parent control.
  * **WatermarkTextBox** - TextBox control with a watermark. Set WatermarkText to change the watermark prompt, change WatermarkStyle to change the style of the watermark TextBlock.
  * **WebBrowser** - a templated control with a WebView + address bar, title bar, backstack navigation, favicon. _work in progress (visual states are a bit messed up), but might be helpful as a starting point_
  * **WrapPanel** (ported from Silverlight Toolkit) - used for layout of child items in wrapping rows or columns - similar to the way text wraps on a page. Different than VariableSizedWrapGrid since it supports items of varying size and auto-sized rows or columns, but it is not a grid and so it does not explicitly support items spanning multiple cells without the use of negative margins.

* **Controls.Extensions**
  * **AnimationHelper** - two attached properties - Storyboard and IsPlaying. Allows to easily control Storyboard playback from a view model (note limitation - a single storyboard per control).
  * **AppBarExtensions.HideWhenSnapped** - allows to make the AppBar automatically hide when the app goes to the snapped view. 
  * **ContentControlExtensions.FadeTransitioningContentTemplate** - allows to change content template with a fade out/fade in transition.
  * **ControlExtensions.Cursor** - enables setting a mouse cursor to show when hovering over a control.
  * **FrameworkElementExtensions.ClipToBounds** - automatically updates the Clip property to clip the contents of the element to its bounds.
  * **ImageExtensions.FadeInOnLoaded/.Source** - allows to specify an image source such that the image fades in smoothly when the image source is loaded.
  * **ListBoxExtensions./ListViewExtensions**
    * **BindableSelection** - allows a two-way binding of the SelectedItems collection on the Selector/list controls.
    * **ItemToBringIntoView** - allows to control which item should be visible through a view model binding without changing the selected item itself.
  * **ManipulationInertiaStartingRoutedEventArgsExtensions** - adds extensions to the arguments of the ManipulationInertiaStarting event that calculate flick ballistics - help determine and control where and when an inertial manipulation will end so you can make your flicks always end where you want them to! It's the basis of the ToolWindow behavior where a flicked window always quickly and smoothly snaps to the side of the screen.
  * **RichTextBlockExtensions**
    * **PlainText** - attached property that allows to easily single-way-bind plain text to a RichTextBlock (not really that useful other than for visualizing RichTextBlock styles in the sample app provided).
    * **LinkedHtmlFragment** - attached property that allows to easily single-way-bind plain text with HTML links (anchor tags) to a RichTextBlock to automatically generate links. Extension methods like SetLinkedHtmlFragment() and AppendLink() are also available.
  * **ScrollViewerExtensions.ScrollToHorizontalOffsetWithAnimation(), .ScrollToVerticalOffsetWithAnimation()** - provide a way to scroll a ScrollViewer to specified offset with an animation.
  * **TextBlockExtensions/GetCharacterRect()** - an extension method that returns a rectangle that holds a character at a given index in the TextBlock.
  * **TextBoxValidationExtensions** - extensions that allow to specify the Format of the requested Text input as well as brushes to use to highlight a TextBox with valid or invalid Text.
  * **ViewboxExtensions.GetChildScaleX()/GetChildScaleY()** - return the effective scale of the Viewbox Child.
  * **VisualTreeHelperExtensions** - provides a set of extension methods that enumerate visual tree ascendants/descendants of a given control - making it easy to do these operations with LINQ as well as simple ways to list controls of a given type or find the first control of a given type searching up or down the visual tree.
  * **WebViewExtensions** - extensions to get currently loaded page address, title, favicon, head tag's inner HTML.

* **Converters**.
  * **BindingDebugConverter** - helps debug bindings by allowing to trace or break whenever a binding gets updated.
  * **BooleanToDataTemplateConverter** - given two DataTemplates (TrueTemplate and FalseTemplate) - converts the input value to the given template. A different take on DataTemplateSelector.
  * **BooleanToVisibilityConverter** - the mother of all converters
  * **ColorToBrushConverter** - converts a Color to a Brush
  * **DoubleToIntConverter**
  * **NullableBoolToBoolConverter**
  * **NullableBoolToVisibilityConverter**
  * **SecondsToTimeSpanStringConverter** - converts the number of seconds (a double type) to a TimeSpan - useful for configuring some animations

* **Debugging** helpers
  * **VisualTreeDebugger** - provides a trace of the visual tree structure when a control loads, its layout updates or it gets tapped as well as allowing the application to break in the debugger if one of these events occurs
  * **Debug/DebugConsole/DebugConsoleOverlay/DC.Trace()** - enables tracing and displaying traced information right in the application on a collapsible panel
  * **DC.ShowVisualTree()** - enables visualizing and manipulating visual tree of the application using a TreeView control and custom property editors

* **Imaging Extensions**
  * **BitmapImageLoadExtensions** - extensions to simplify loading BitmapImages based on StorageFile or file name
  * **ColorExtensions** - Conversions between pixels and pixel buffer types of byte, int and Color
  * **IBufferExtensions** - Adds a GetPixels() extension method to the PixelBuffer property of a WriteableBitmap that reads in the buffer to a byte array and exposes an indexer compatible to the one of the Pixels property in Silverlight's WriteableBitmap
  * **WriteableBitmap~** - a set of extension methods for a WriteableBitmap
    * **WriteableBitmapSaveExtensions** - support for loading and saving the bitmap to/from files
    * **WriteableBitmapBlitBlockExtensions** - support for quick blitting of a full-width section of a bitmap to another bitmap of same width
    * **WriteableBitmapCopyExtensions** - support creating a copy of a WriteableBitmap
    * **WriteableBitmapCropExtensions** - support for creating a cropped version of a WriteableBitmap
    * **WriteableBitmapDarkenExtension** - performs image processing to darken the pixels of the WriteableBitmap.
    * **WriteableBitmapFloodFillExtensions** - support for flood-filling a region of a WriteableBitmap - either limited by an outline color or by replacing a given color - usually a color at the starting position or colors similar to it
    * **WriteableBitmapFromBitmapImageExtension** - allows to create a WriteableBitmap from a BitmapImage assuming the BitmapImage is installed with the application.
    * **WriteableBitmapGrayscaleExtension** - performs image processing to make the pixels of the WriteableBitmap (more) grayscale.
    * **WriteableBitmapLightenExtension** - performs image processing to lighten the pixels of the WriteableBitmap.

* **IO** helpers
  * **ScaledImageFile.Get()** - Used to retrieve a StorageFile that uses qualifiers in the naming convention.
  * **StorageFileExtensions.GetSize()/.GetSizeString()** - allow to get the size of a file and its string representation (automatically converting from bytes to kB, MB, GB, TB)
  * **StorageFolderExtensions**
    * **.ContainsFile()** - returns a value that states whether a file with specific name exists in the folder
    * **.CreateTempFile()** - creates a temporary file
    * **.CreateTempFileName()** - returns an unused, unique file name for a temporary file
  * **StringIOExtensions** - allows to easily read or write a string from/to file in a single call
  * **Serialization**
    * **JsonSerialization** - allows to serialize a properly DataContract-annotated object to a JSON string or file or deserialize an existing one.
    * **XmlSerialization** - allows to serialize a properly DataContract- or XmlSerializer-annotated object to a XML string or file or deserialize an existing one.

* **Net.WebFile.SaveAsync()**- downloads a file from a given URL, automatically figuring out the recommended file name and saving it to a given or default folder

* **Tools**
  * **BackgroundTimer** - a timer class similar to DispatcherTimer in its interface, but somewhat more precise and running on a background thread
  * **EnumExtensions.GetValues<T>()** - allows to get an array of strongly typed values of enum type T
  * **TryCatchRetry** - allows to run a specific Task or Action, catching exceptions and retrying for a specified number of retries, with optional delays in between (mostly a debugging tool or a means of temporary workarounds)


### Work in Progress
The libraries are just a set of helper methods and controls that I found useful to fill the gaps in Windows Runtime XAML framework and continues to evolve.

### Wanted Additions
* **Cancellation support** to AsyncUI extensions and other awaitable methods
* **AutoComplete** control or attached property/behavior
* **ColorPicker** control (already in development, but incomplete)
* **Date/Time/DateTimePicker** control
* **InkCanvas** control
* **MultiScaleImage** control for Deep Zoom support - could be based on the [WPF implementation](http://www.codeproject.com/Articles/128695/Deep-Zoom-for-WPF) from CodeProjet
* **Pivot** control - analogous to a Pivot control on Windows Phone or a TabControl in WPF.
* **WeatherAppTabControl** - a Pivot/TabControl like control that mimics the one in the Weather App.
* **CropControl** - a control for cropping images. There is one someone has already made though that might be good enough.
* **Project and item templates** - with or without AlternativePage/Frame, with a recommended AppShell control, settings panels, app bar, mvvm basics etc.
* **ScatterView**, **ScatterViewItem** - implementations of the **ItemsControl** classes of the original Microsoft Surface that allow to freely drag the items on the screen.
* **Extended visual debugging and integration support tools** such as sample data generation or capture from live runtime data, visual tree debugging from within Visual Studio, storyboard and resource dictionary debugging etc.
* **TextBox input patterns** - to be able to display watermarked patterns and allow only certain keys - e.g. for phone numbers, SSNs etc.

### Feedback
Please share your experience with the toolkit either in the [DISCUSSIONS](http://winrtxamltoolkit.codeplex.com/discussions) page or directly on [twitter](https://twitter.com/#!/xyzzer/). What do you use? How does it work for you? What are you missing? What would you change? Feedback is crucial and drives further development.

### Related Projects
* [Tim Heuer's](https://twitter.com/#!/timheuer) and [Morten Nielsen's](http://twitter.com/dotmorten) [Callisto](https://github.com/timheuer/callisto/tree/master/src/Callisto) - contains a host of controls (DateTimePicker, Flyout, LiveTile, Menu, SettingsFlyout), converters, Tilt Effect, OAuth helper, SQLite connection helper, etc.
* [Joost van Schaik's](https://twitter.com/LocalJoost) [Win8nl utilities](http://win8nl.codeplex.com/) - a helper library for Jupiter, featuring some excellent attached behaviors. Based on [WinRT Behaviors](http://winrtbehaviors.codeplex.com/) and [MVVM Light](mvvmlight.codeplex.com).
* [Joost van Schaik's](https://twitter.com/LocalJoost) [WinRT Behaviors](http://winrtbehaviors.codeplex.com/) - modeled after the original Blend Behaviors - these make creating attached behaviors easier and are configurable with Blend.
* [Johaan Laanstra's](http://www.johanlaanstra.nl/) [Windows.UI.Interactivity](https://github.com/jlaanstra/Windows.UI.Interactivity) - a port of System.Windows.Interactivity to the Windows Runtime that includes both behaviors and triggers
* [WinRT XAML Calendar](http://winrtxamlcalendar.codeplex.com/) - a version of the Silverlight Toolkit calendar ported to WinRT.
* [AsyncUI](http://asyncui.codeplex.com) - a separate version of the AsyncUI library with support for WPF, Silverlight and Windows Phone 7 with Async CTP3 (it might not yet have all the features that WinRT XAML Toolkit has in the AsyncUI namespace, but has the most useful ones)
* [Michiel Post's](https://twitter.com/michielpostnl) [Q42.WinRT library](https://github.com/Q42/Q42.WinRT) for data driven Windows 8 C# / XAML WinRT projects
* [Mahmoud Moussa](http://twitter.com/MahmoudMoussa) [Windows 8 Toolkit - Charts and More](http://modernuitoolkit.codeplex.com/team/view) - the original location of the Silverlight Toolkit's chart controls ported to Jupiter that are now also part of this project.
* [XAML Crop Control](https://xamlcropcontrol.codeplex.com/) - A XAML control for cropping images, implemented in C#.  WinRT only.

### Commercial component libraries
This library gives you some helpful controls and extensions, but it is an open source project developed in free time. If you need controls not available here and otherwise well tested controls - consider the commercial solutions.
* [Actipro WinRT XAML Controls](http://www.actiprosoftware.com/products/controls/winrt)
* [ComponentOne Studio WinRT Edition](http://www.componentone.com/Studio/Platform/WinRT)
* [DevExpress Windows 8 XAML Controls](http://www.devexpress.com/Products/NET/Controls/WinRT/)
* [Infragistics Windows 8 Controls](http://www.infragistics.com/products/windows-ui)
* [Mindscape Metro Elements](http://www.mindscapehq.com/products/metroelements)
* [OhZee WinRT XAML Controls](http://ohzeecreative.com/all-portfolio-list/xaml-controls/)
* [Perpetuum UI Controls for Windows 8](http://www.perpetuumsoft.com/Windows8-UI-Controls.aspx)
* [Syncfusion Essential Studio for WinRT](https://www.syncfusion.com/products/winrt?src=winrtxamltoolkit)
* Telerik [UI for Windows Universal](http://www.telerik.com/windows-universal-ui) and [UI for Universal Windows Platform](http://www.telerik.com/universal-windows-platform-ui)
