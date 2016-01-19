namespace Gu.Wpf.NumericInput.Tests.SandBox
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;

    class EventBox
    {
        [Test]
        public void TestName()
        {
            var propertyInfos = typeof (FrameworkPropertyMetadata).GetProperties().Where(x => x.PropertyType == typeof (bool));
            foreach (var propertyInfo in propertyInfos)
            {
                Console.WriteLine($"if (fpm.{propertyInfo.Name})");
                Console.WriteLine("{");
                Console.WriteLine($"    flags |= FrameworkPropertyMetadataOptions.{propertyInfo.Name};");
                Console.WriteLine("}");
            }
        }
    }
}
