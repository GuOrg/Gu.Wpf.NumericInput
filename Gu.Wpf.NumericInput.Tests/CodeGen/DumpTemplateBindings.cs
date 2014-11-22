namespace Gu.Wpf.NumericInput.Tests.CodeGen
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;

    public class DumpTemplateBindings
    {
        [TestCase(typeof(TextBox)), Explicit]
        public void TemplateBindDepedencyProperties(Type type)
        {
            writeTemplateBindings(type);
            while (type.BaseType != typeof(Object))
            {
                type = type.BaseType;
                writeTemplateBindings(type);
            }
        }

        private void writeTemplateBindings(Type type)
        {
            Console.WriteLine("#####  {0}  #####", type.Name);

            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static)
                     .Where(x => x.FieldType == typeof(DependencyProperty))
                     .ToArray();
            foreach (var fieldInfo in fieldInfos)
            {
                var dp = (DependencyProperty)fieldInfo.GetValue(null);
                var metadata = dp.DefaultMetadata as FrameworkPropertyMetadata;
                //if (metadata != null && !metadata.Inherits)
                //{
                //    Console.WriteLine("{0} = \"{{TemplateBinding {0}}}\"", dp.Name);
                //}
                Console.WriteLine("{0} = \"{{TemplateBinding {0}}}\" <!-- Inherits: {1} -->", dp.Name, metadata != null ? metadata.Inherits.ToString() : "false");
            }
            Console.WriteLine();
        }
    }
}
