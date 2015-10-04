Solution structure


1. Universal class library projects

The toolkit is made of C# class library projects targeting UWP that enable sharing the code
in class libraries that target both Windows and Windows Phone apps for Windows 10.

Both target platform versions of each library share dll name and namespace.
One of the reasons is to enable sharing XAML files that include paths to resources, such as
Source="ms-appx:///WinRTXamlToolkit/Controls/AlternativeFrame/AlternativeFrame.xaml"
Universal class library project is not a template in VS, but is manually created based on universal app project


2. NuGet packages

...
