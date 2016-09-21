// ReSharper disable PossibleNullReferenceException
// ReSharper disable UnusedMember.Local
namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using Gu.Wpf.NumericInput.Select;
    using NUnit.Framework;

    public class ResharperPattern
    {
        public void TestMethod()
        {
            var wrapper = new IntBox();
            // ReSharper disable once SuppressSetMutable
            wrapper.Value = 1;
        }

        [Test]
        [Explicit]
        public void DotSettingsHasCorrectPattern()
        {
            var pattern = this.MutablePropertiesRegex<IntBox>();
            var regex = "MutablePropertyName/Properties/=RegEx/@EntryIndexedValue\">";
            var match = File.ReadLines(this.GlobalSlnDotSettingsFile().FullName)
                             .SingleOrDefault(line => line.Contains(regex));
            Assert.NotNull(match);
            if (!match.Contains(pattern))
            {
                int lineNo = 0;
                foreach (var line in File.ReadLines(this.GlobalSlnDotSettingsFile().FullName))
                {
                    lineNo++;
                    if (line.Contains(regex))
                    {
                        Console.WriteLine($"Replace line {lineNo}");
                        Console.WriteLine(line);
                        Console.WriteLine("with:");
                        Console.WriteLine(Regex.Replace(line, @"\([^)]+\)", pattern));
                        Assert.Fail("DotSettings does not contain the correct pattern.");
                    }
                }
            }
            StringAssert.Contains(pattern, match);
        }

        [Test]
        public void DumpPattern()
        {
            Console.Write(this.GetResharperPattern<IntBox>());
            Console.Write(this.GetResharperPattern<LongBox>());
            Console.Write(this.GetResharperPattern<FloatBox>());
            Console.Write(this.GetResharperPattern<DoubleBox>());
            Console.Write(this.GetResharperPattern<DecimalBox>());
        }

        [Test, Explicit]
        public void WriteUserDotSettings()
        {
            var file = this.UserSlnDotSettingsFile();
            if (file == null)
            {
                return;
            }
            File.Delete(file.Name);
            using (var writer = new StreamWriter(File.OpenWrite(file.FullName)))
            {
                writer.WriteLine(@"<wpf:ResourceDictionary xml:space=""preserve"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns:s=""clr-namespace:System;assembly=mscorlib"" xmlns:ss=""urn:shemas-jetbrains-com:settings-storage-xaml"" xmlns:wpf=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">");
                writer.WriteLine(@"		<s:Int64 x:Key=""/Default/Environment/SearchAndNavigation/DefaultOccurrencesGroupingIndex/@EntryValue"">0</s:Int64>");
                writer.Write(this.GetResharperPattern<IntBox>());
                writer.WriteLine("</wpf:ResourceDictionary>");
            }
        }

        private FileInfo GlobalSlnDotSettingsFile()
        {
            var dir = new DirectoryInfo(new Uri(this.GetType().Assembly.CodeBase).LocalPath).Parent.Parent.Parent.Parent;
            var match = dir.EnumerateFiles("Gu.Wpf.Media.sln.DotSettings")
                                    .FirstOrDefault();
            return match;
        }

        private FileInfo UserSlnDotSettingsFile()
        {
            var dir = new DirectoryInfo(new Uri(this.GetType().Assembly.CodeBase).LocalPath).Parent.Parent.Parent.Parent;
            var match = dir.EnumerateFiles("Gu.Wpf.Media.sln.DotSettings.user")
                                    .FirstOrDefault();
            return match;
        }

        private string GetResharperPattern<T>()
             where T : DependencyObject
        {
            var guid = Guid.NewGuid()
               .ToString("N")
               .ToUpperInvariant();
            var lines = Properties.Resources.Pattern.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToArray();
            var pattern = this.MutablePropertiesRegex<T>();
            var builder = new StringBuilder();
            foreach (var line in lines)
            {
                var formatted = line.Replace("{GUID}", guid)
                                    .Replace("{DependencyObjectType}", typeof(T).FullName)
                                    .Replace("{dependencyObject}", this.FirstCharLower(typeof(T).Name))
                                    .Replace("{PropertyRegex}", pattern);

                builder.AppendLine(formatted);
            }

            return builder.ToString();
        }

        private string MutablePropertiesRegex<T>()
    where T : DependencyObject
        {
            var mutableDps =
                typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                    .Except(typeof(TextBox).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
                    .Where(f => f.FieldType == typeof(DependencyProperty))
                    .Select(f => f.GetValue(null))
                    .OfType<DependencyProperty>()
                    .Where(dp => !dp.ReadOnly)
                    .OrderBy(x => x.Name)
                    .ToArray();
            var pattern = $"({string.Join("|", mutableDps.Select(dp => dp.Name))})";
            return pattern;
        }

        private string AllMutablePropertiesRegex<T>()
            where T : DependencyObject
        {
            var mutableDps = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                                      .Where(f => f.FieldType == typeof(DependencyProperty))
                                      .Select(f => f.GetValue(null))
                                      .OfType<DependencyProperty>()
                                      .Where(dp => !dp.ReadOnly)
                                      .OrderBy(x => x.Name)
                                      .ToArray();
            var pattern = $"({string.Join("|", mutableDps.Select(dp => dp.Name))})";
            return pattern;
        }

        private string FirstCharLower(string text)
        {
            return text.Substring(0, 1)
                       .ToLowerInvariant() + text.Substring(1);
        }
    }
}
