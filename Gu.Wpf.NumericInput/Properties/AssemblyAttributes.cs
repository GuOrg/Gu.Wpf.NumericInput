using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;

[assembly: CLSCompliant(false)]

[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]
[assembly: XmlnsDefinition("http://gu.se/NumericInput", "Gu.Wpf.NumericInput")]
[assembly: XmlnsDefinition("http://gu.se/NumericInput", "Gu.Wpf.NumericInput.Properties")]
[assembly: XmlnsPrefix("http://gu.se/NumericInput", "numeric")]

[assembly: XmlnsDefinition("http://gu.se/Select", "Gu.Wpf.NumericInput.Select")]
[assembly: XmlnsPrefix("http://gu.se/Select", "select")]

[assembly: XmlnsDefinition("http://gu.se/Touch", "Gu.Wpf.NumericInput.Touch")]
[assembly: XmlnsPrefix("http://gu.se/Touch", "touch")]

[assembly: InternalsVisibleTo("Gu.Wpf.NumericInput.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004962EFBEDF32E591E12D98026DE0EC2762B77C24E0FB41E3A262213A7A0DEBEADDECDB31D33CBA1D5F09F083EDD6912469B1C256E85DBE2E571483C034F156600D0481C7385778CA380D86CEEDE8EFC5945C2499DE1853A5E7DD7FF0C88D35E07EB613921391EC7B5F8701935E13E74F5D886D6FE477B041937EC949A8FF83D7", AllInternalsVisible = true)]

#if NET45
#pragma warning disable SA1402, SA1502, SA1600, SA1649, GU0073
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class DisallowNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class MaybeNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class NotNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        public MaybeNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;

        public bool ReturnValue { get; }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;

        public bool ReturnValue { get; }
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        public NotNullIfNotNullAttribute(string parameterName) => this.ParameterName = parameterName;

        public string ParameterName { get; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class DoesNotReturnIfAttribute : Attribute
    {
        public DoesNotReturnIfAttribute(bool parameterValue) => this.ParameterValue = parameterValue;

        public bool ParameterValue { get; }
    }
}
#endif
