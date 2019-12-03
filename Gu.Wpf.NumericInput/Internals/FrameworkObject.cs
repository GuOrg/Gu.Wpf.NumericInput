// ReSharper disable All
namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// http://referencesource.microsoft.com/#PresentationFramework/src/Framework/MS/Internal/FrameworkObject.cs,301f142557ad0322.
    /// </summary>
    internal struct FrameworkObject
    {
        private static readonly DependencyObjectType FrameworkElementDType = DependencyObjectType.FromSystemType(typeof(FrameworkElement));
        private static readonly DependencyObjectType FrameworkContentElementDType = DependencyObjectType.FromSystemType(typeof(FrameworkContentElement));

        internal FrameworkObject(DependencyObject d)
        {
            // [code should be identical to Reset(d)]
            this.DependencyObject = d;
            if (FrameworkElementDType.IsInstanceOfType(d))
            {
                this.FrameworkElement = (FrameworkElement)d;
                this.FrameworkContentElement = null;
            }
            else if (FrameworkContentElementDType.IsInstanceOfType(d))
            {
                this.FrameworkElement = null;
                this.FrameworkContentElement = (FrameworkContentElement)d;
            }
            else
            {
                this.FrameworkElement = null;
                this.FrameworkContentElement = null;
            }
        }

        internal FrameworkElement FrameworkElement { get; private set; }

        internal FrameworkContentElement FrameworkContentElement { get; private set; }

        internal DependencyObject DependencyObject { get; private set; }

        internal bool IsFE => this.FrameworkElement != null;

        internal bool IsFCE => this.FrameworkContentElement != null;

        // returns the effective parent, whether visual, logical,
        // inheritance context, etc.
        internal DependencyObject EffectiveParent
        {
            get
            {
                DependencyObject parent;

                if (this.FrameworkElement is { } fe)
                {
                    parent = VisualTreeHelper.GetParent(fe);
                }
                else if (this.FrameworkContentElement is { } fce)
                {
                    parent = fce.Parent;
                }
                else
                {
                    parent = this.DependencyObject switch
                    {
                        Visual visual => VisualTreeHelper.GetParent(visual),
                        ContentElement ce => ContentOperations.GetParent(ce),
                        Visual3D visual3D => VisualTreeHelper.GetParent(visual3D),
                        _ => null,
                    };
                }

                if (parent == null && this.DependencyObject != null)
                {
                    throw new NotSupportedException("Parent is null.");

                    // parent = this._do.InheritanceContext;
                }

                return parent;
            }
        }

        internal static bool IsEffectiveAncestor(DependencyObject d1, DependencyObject d2)
        {
            for (var fo = new FrameworkObject(d2);
                fo.DependencyObject != null;
                fo.Reset(fo.EffectiveParent))
            {
                if (ReferenceEquals(fo.DependencyObject, d1))
                {
                    return true;
                }
            }

            return false;
        }

        internal void Reset(DependencyObject d)
        {
            this.DependencyObject = d;

            switch (d)
            {
                case FrameworkElement fe:
                    this.FrameworkElement = fe;
                    this.FrameworkContentElement = null;
                    break;
                case FrameworkContentElement fce:
                    this.FrameworkElement = null;
                    this.FrameworkContentElement = fce;
                    break;
                default:
                    this.FrameworkElement = null;
                    this.FrameworkContentElement = null;
                    break;
            }
        }

        public override string ToString() => this.DependencyObject?.ToString() ?? "null";
    }
}
