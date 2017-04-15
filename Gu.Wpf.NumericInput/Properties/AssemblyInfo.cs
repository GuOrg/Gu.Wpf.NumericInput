using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("Gu.Wpf.NumericInput")]
[assembly: AssemblyDescription("WPF textboxes for numeric input")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Johan Larsson")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: AssemblyVersion("0.5.1.4")]
[assembly: AssemblyFileVersion("0.5.1.4")]
[assembly: NeutralResourcesLanguage("en")]
[assembly: Guid("2026010E-5005-413E-BA3F-18CF88F28B0F")]

[assembly: InternalsVisibleTo("Gu.Wpf.NumericInput.Tests", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo("Gu.Wpf.NumericInput.UITests", AllInternalsVisible = true)]

[assembly: XmlnsDefinition("http://gu.se/NumericInput", "Gu.Wpf.NumericInput")]
[assembly: XmlnsPrefix("http://gu.se/NumericInput", "numeric")]

[assembly: XmlnsDefinition("http://gu.se/Select", "Gu.Wpf.NumericInput.Select")]
[assembly: XmlnsPrefix("http://gu.se/Select", "select")]

[assembly: XmlnsDefinition("http://gu.se/Touch", "Gu.Wpf.NumericInput.Touch")]
[assembly: XmlnsPrefix("http://gu.se/Touch", "touch")]