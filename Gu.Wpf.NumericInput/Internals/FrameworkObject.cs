// ReSharper disable All
namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Markup;
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

        internal FrameworkObject(FrameworkElement frameworkElement, FrameworkContentElement frameworkContentElement)
        {
            this.FrameworkElement = frameworkElement;
            this.FrameworkContentElement = frameworkContentElement;

            if (frameworkElement != null)
            {
                this.DependencyObject = frameworkElement;
            }
            else
            {
                this.DependencyObject = frameworkContentElement;
            }
        }

        internal FrameworkElement FrameworkElement { get; private set; }

        internal FrameworkContentElement FrameworkContentElement { get; private set; }

        internal DependencyObject DependencyObject { get; private set; }

        internal bool IsFE => this.FrameworkElement != null;

        internal bool IsFCE => this.FrameworkContentElement != null;

        internal bool IsValid => this.FrameworkElement != null || this.FrameworkContentElement != null;

        //// logical parent
        internal DependencyObject Parent
        {
            get
            {
                if (this.IsFE)
                {
                    return this.FrameworkElement.Parent;
                }
                else if (this.IsFCE)
                {
                    return this.FrameworkContentElement.Parent;
                }
                else
                {
                    return null;
                }
            }
        }

        internal DependencyObject TemplatedParent
        {
            get
            {
                if (this.IsFE)
                {
                    return this.FrameworkElement.TemplatedParent;
                }
                else if (this.IsFCE)
                {
                    return this.FrameworkContentElement.TemplatedParent;
                }
                else
                {
                    return null;
                }
            }
        }

        //// internal Style ThemeStyle
        //// {
        ////    get
        ////    {
        ////        if (this.IsFE)
        ////        {
        ////            return this._fe.ThemeStyle;
        ////        }
        ////        else if (this.IsFCE)
        ////        {
        ////            return this._fce.ThemeStyle;
        ////        }
        ////        else
        ////        {
        ////            return null;
        ////        }
        ////    }
        //// }

        internal XmlLanguage Language
        {
            get
            {
                if (this.IsFE)
                {
                    return this.FrameworkElement.Language;
                }
                else if (this.IsFCE)
                {
                    return this.FrameworkContentElement.Language;
                }
                else
                {
                    return null;
                }
            }
        }

        internal Style Style
        {
            get
            {
                if (this.IsFE)
                {
                    return this.FrameworkElement.Style;
                }
                else if (this.IsFCE)
                {
                    return this.FrameworkContentElement.Style;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.IsFE)
                {
                    this.FrameworkElement.SetCurrentValue(FrameworkElement.StyleProperty, value);
                }
                else if (this.IsFCE)
                {
                    this.FrameworkContentElement.SetCurrentValue(FrameworkContentElement.StyleProperty, value);
                }
            }
        }

        // returns the effective parent, whether visual, logical,
        // inheritance context, etc.
        internal DependencyObject EffectiveParent
        {
            get
            {
                DependencyObject parent;

                if (this.IsFE)
                {
                    parent = VisualTreeHelper.GetParent(this.FrameworkElement);
                }
                else if (this.IsFCE)
                {
                    parent = this.FrameworkContentElement.Parent;
                }
                else
                {
                    switch (this.DependencyObject)
                    {
                        case Visual visual:
                            parent = VisualTreeHelper.GetParent(visual);
                            break;
                        case ContentElement ce:
                            parent = ContentOperations.GetParent(ce);
                            break;
                        case Visual3D visual3D:
                            parent = VisualTreeHelper.GetParent(visual3D);
                            break;
                        default:
                            parent = null;
                            break;
                    }
                }

                if (parent == null && this.DependencyObject != null)
                {
                    throw new NotSupportedException("Parent is null.");

                    // parent = this._do.InheritanceContext;
                }

                return parent;
            }
        }

        internal bool IsInitialized
        {
            get
            {
                if (this.IsFE)
                {
                    return this.FrameworkElement.IsInitialized;
                }
                else if (this.IsFCE)
                {
                    return this.FrameworkContentElement.IsInitialized;
                }
                else
                {
                    return true;
                }
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

        internal static FrameworkObject GetContainingFrameworkElement(DependencyObject current)
        {
            var fo = new FrameworkObject(current);

            while (!fo.IsValid && fo.DependencyObject != null)
            {
                // The current object is neither a FrameworkElement nor a
                // FrameworkContentElement.  We will now walk the "core"
                // tree looking for one.

                if (fo.DependencyObject is Visual visual)
                {
                    fo.Reset(VisualTreeHelper.GetParent(visual));
                }
                else if (fo.DependencyObject is ContentElement ce)
                {
                    fo.Reset(ContentOperations.GetParent(ce));
                }
                else if (fo.DependencyObject is Visual3D visual3D)
                {
                    fo.Reset(VisualTreeHelper.GetParent(visual3D));
                }
                else
                {
                    // The parent could be an application.
                    fo.Reset(null);
                }
            }

            return fo;
        }

        internal void Reset(DependencyObject d)
        {
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

        internal void RaiseEvent(RoutedEventArgs args)
        {
            if (this.IsFE)
            {
                this.FrameworkElement.RaiseEvent(args);
            }
            else if (this.IsFCE)
            {
                this.FrameworkContentElement.RaiseEvent(args);
            }
        }

        public override string ToString()
        {
            if (this.IsFE)
            {
                return this.FrameworkElement.ToString();
            }
            else if (this.IsFCE)
            {
                return this.FrameworkContentElement.ToString();
            }

            return "Null";
        }
    }
}
