Solution structure


1. Universal class library projects

The toolkit is made of universal class library projects that enable sharing the code
in class libraries that target both Windows and Windows Phone apps.
This is more complicated, but was chosen instead of portable class libraries to enable
slight differences between different target platform flavors, such as template differences
and accounting for minor differences between WinRT on phone and PC.

Both target platform versions of each library share dll name and namespace.
One of the reasons is to enable sharing XAML files that include paths to resources, such as
Source="ms-appx:///WinRTXamlToolkit/Controls/AlternativeFrame/AlternativeFrame.xaml"
Universal class library project is not a template in VS, but is manually created based on universal app project


2. NuGet packages

Windows and Windows Phone versions of each library live in separate packages. This was done to reduce package sizes.
For Universal apps you need to add a reference to the correct package for each platform-specific universal app project.
