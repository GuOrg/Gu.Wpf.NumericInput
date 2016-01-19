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
        private const string NumericUri = @"http://gu.se/NumericInput";
        private const string SelectUri = @"http://gu.se/Select";

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
                                  .Where(x => x != null)
                                  .Distinct()
                                  .Where(x => !skip.Any(x.EndsWith))
                                  .OrderBy(x => x)
                                  .ToArray();
            var attributes = this.assembly.CustomAttributes.Where(x => x.AttributeType == typeof(XmlnsDefinitionAttribute))
                                     .ToArray();
            var actuals = attributes.Select(a => (string)a.ConstructorArguments[1].Value)
                                    .OrderBy(x => x)
                                    .ToArray();
            foreach (var s in strings)
            {
                Console.WriteLine(@"[assembly: XmlnsDefinition(""{0}"", ""{1}"")]", NumericUri, s);
            }
            CollectionAssert.AreEqual(strings, actuals);
            foreach (var attribute in attributes)
            {
                var actual = attribute.ConstructorArguments[0].Value;
                CollectionAssert.Contains(new[] { NumericUri, SelectUri }, actual);
            }
        }

        [Test]
        public void XmlnsPrefix()
        {
            var prefixAttributes = this.assembly.CustomAttributes.Where(x => x.AttributeType == typeof(XmlnsPrefixAttribute)).ToArray();
            Assert.AreEqual(2, prefixAttributes.Length);
            var prefixes = prefixAttributes.Select(x => (string)x.ConstructorArguments[0].Value).ToArray();
            CollectionAssert.AreEquivalent(new[] { NumericUri, SelectUri }, prefixes);
        }
    }
}
