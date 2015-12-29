namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Markup;
    using NUnit.Framework;

    public class NamespacesTests
    {
        private Assembly assembly;
        private const string Uri = @"http://gu.se/NumericInput";

        [SetUp]
        public void SetUp()
        {
            this.assembly = typeof(DoubleBox).Assembly;
        }

        [Test]
        public void XmlnsDefinitions()
        {
            string[] skip = { ".Annotations", ".Properties", "XamlGeneratedNamespace", ".Internals" };

            var strings = this.assembly.GetTypes()
                                  .Select(x => x.Namespace)
                                  .Where(x=> x != null)
                                  .Distinct()
                                  .Where(x => !skip.Any(s => x.EndsWith(s)))
                                  .OrderBy(x => x)
                                  .ToArray();
            var attributes = this.assembly.CustomAttributes.Where(x => x.AttributeType == typeof(XmlnsDefinitionAttribute))
                                     .ToArray();
            var actuals = attributes.Select(a => a.ConstructorArguments[1].Value)
                                                             .OrderBy(x => x);
            foreach (var s in strings)
            {
                Console.WriteLine(@"[assembly: XmlnsDefinition(""{0}"", ""{1}"")]", Uri, s);
            }
            CollectionAssert.AreEqual(strings, actuals);
            foreach (var attribute in attributes)
            {
                Assert.AreEqual(Uri, attribute.ConstructorArguments[0].Value);
            }
        }

        [Test]
        public void XmlnsPrefix()
        {
            var prefixAttribute = this.assembly.CustomAttributes.Single(x => x.AttributeType == typeof(XmlnsPrefixAttribute));
            Assert.AreEqual(Uri, prefixAttribute.ConstructorArguments[0].Value);
        }
    }
}
