﻿namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    [DefaultProperty(nameof(Child))]
    [ContentProperty(nameof(Child))]
    public class SpinnerDecorator : Control
    {
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register(
            "Child",
            typeof(BaseBox),
            typeof(SpinnerDecorator),
            new PropertyMetadata(default(BaseBox), OnChildChanged));

        static SpinnerDecorator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinnerDecorator), new FrameworkPropertyMetadata(typeof(SpinnerDecorator)));
        }

        public BaseBox Child
        {
            get { return (BaseBox)this.GetValue(ChildProperty); }
            set { this.SetValue(ChildProperty, value); }
        }

        /// <summary>
        /// Gets enumerator to logical children.
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (this.Child == null)
                {
                    return EmptyEnumerator.Instance;
                }

                return new SingleChildEnumerator(this.Child);
            }
        }

        /// <summary>
        /// This method is used by TypeDescriptor to determine if this property should
        /// be serialized.
        /// http://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/ContentControl.cs,164
        /// </summary>
        // Lets derived classes control the serialization behavior for Content DP
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool ShouldSerializeContent()
        {
            return this.ReadLocalValue(ChildProperty) != DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// This method is invoked when the Child property changes.
        /// http://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/ContentControl.cs,262
        /// </summary>
        /// <param name="oldChild">The old value of the Child property.</param>
        /// <param name="newChild">The new value of the Child property.</param>
        protected virtual void OnChildChanged(BaseBox oldChild, BaseBox newChild)
        {
            this.RemoveLogicalChild(oldChild);

            if (newChild != null)
            {
                DependencyObject logicalParent = LogicalTreeHelper.GetParent(newChild);
                if (logicalParent != null)
                {
                    if (this.TemplatedParent != null && FrameworkObject.IsEffectiveAncestor(logicalParent, this))
                    {
                        // In the case that this SpinnerDecorator belongs in a parent template
                        // and represents the content of a parent, we do not wish to change
                        // the logical ancestry of the content.
                        return;
                    }
                    else
                    {
                        // If the new content was previously hooked up to the logical
                        // tree then we sever it from the old parent.
                        var message = "Cannot add child that already belongs to a parent.\r\n" +
                                      "Fixing this requires more source diving than I feel like right now.\r\n" +
                                      "Waiting to see if it becomes a problem";
                        throw new NotSupportedException(message);
                        //LogicalTreeHelper.RemoveLogicalChild(logicalParent, newChild);
                    }
                }
            }

            // Add the new content child
            this.AddLogicalChild(newChild);
        }

        /// <summary>
        /// Creates AutomationPeer (<see cref="UIElement.OnCreateAutomationPeer"/>)
        /// </summary>
        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return new SpinnerDecoratorAutomationPeer(this);
        }

        private static void OnChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SpinnerDecorator)d).OnChildChanged((BaseBox)e.OldValue, (BaseBox)e.NewValue);
        }
    }
}
