namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;

    /// <summary>
    /// Add increase / decrease buttons to a <see cref="NumericBox{T}"/>.
    /// </summary>
    [TemplatePart(Name = IncreaseButtonName, Type = typeof(RepeatButton))]
    [TemplatePart(Name = DecreaseButtonName, Type = typeof(RepeatButton))]
    [DefaultProperty(nameof(Child))]
    [ContentProperty(nameof(Child))]
    public class SpinnerDecorator : Control
    {
        public const string DecreaseButtonName = "PART_DecreaseButton";
        public const string IncreaseButtonName = "PART_IncreaseButton";

        /// <summary>Identifies the <see cref="SpinUpdateMode"/> dependency property.</summary>
        public static readonly DependencyProperty SpinUpdateModeProperty = NumericBox.SpinUpdateModeProperty.AddOwner(
            typeof(SpinnerDecorator),
            new FrameworkPropertyMetadata(
                SpinUpdateMode.AsBinding,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the <see cref="Child"/> dependency property.</summary>
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register(
            nameof(Child),
            typeof(ISpinnerBox),
            typeof(SpinnerDecorator),
            new PropertyMetadata(
                default(ISpinnerBox),
                (d, e) => ((SpinnerDecorator)d).OnChildChanged((ISpinnerBox)e.OldValue, (ISpinnerBox)e.NewValue)));

        static SpinnerDecorator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinnerDecorator), new FrameworkPropertyMetadata(typeof(SpinnerDecorator)));
        }

        /// <summary>
        /// Gets or sets a value indicating how the IncreaseCommand and DecreaseCommand behaves.
        /// The default is AsBinding meaning the value updates using the UpdateSourceTrigger specified in the binding. Default is LostFocus.
        /// If set to PropertyChanged the binding source will be updated at each click even if the binding has UpdateSourceTrigger = LostFocus.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public SpinUpdateMode SpinUpdateMode
        {
            get => (SpinUpdateMode)this.GetValue(SpinUpdateModeProperty);
            set => this.SetValue(SpinUpdateModeProperty, value);
        }

        /// <summary>
        /// Gets or sets the single child of a <see cref="SpinnerDecorator" />.
        /// </summary>
        public ISpinnerBox Child
        {
            get => (ISpinnerBox)this.GetValue(ChildProperty);
            set => this.SetValue(ChildProperty, value);
        }

        /// <summary>
        /// Gets enumerator to logical children.
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (this.Child is null)
                {
                    return EmptyEnumerator.Instance;
                }

                return new SingleChildEnumerator(this.Child);
            }
        }

        /// <summary>
        /// This method is used by TypeDescriptor to determine if this property should
        /// be serialized.
        /// http://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/ContentControl.cs,164.
        /// </summary>
        /// <returns>True if the value should be serialized.</returns>
        // Lets derived classes control the serialization behavior for Content DP
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool ShouldSerializeContent()
        {
            return this.ReadLocalValue(ChildProperty) != DependencyProperty.UnsetValue;
        }

        /// <summary>This method is invoked when the <see cref="ChildProperty"/> changes.</summary>
        /// <param name="oldChild">The old value of <see cref="ChildProperty"/>.</param>
        /// <param name="newChild">The new value of <see cref="ChildProperty"/>.</param>
        protected virtual void OnChildChanged(ISpinnerBox oldChild, ISpinnerBox newChild)
        {
            this.RemoveLogicalChild(oldChild);

            if (newChild is BaseBox newBox)
            {
                var logicalParent = LogicalTreeHelper.GetParent(newBox);
                if (logicalParent != null)
                {
                    if (this.TemplatedParent != null &&
                        FrameworkObject.IsEffectiveAncestor(logicalParent, this))
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
                        //// LogicalTreeHelper.RemoveLogicalChild(logicalParent, newChild);
                    }
                }
            }

            this.AddLogicalChild(newChild);
        }

        /// <summary>
        /// Creates AutomationPeer (<see cref="UIElement.OnCreateAutomationPeer"/>).
        /// </summary>
        /// <returns>An <see cref="UIElement.OnCreateAutomationPeer"/> for the <see cref="SpinnerDecorator"/>.</returns>
        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return new SpinnerDecoratorAutomationPeer(this);
        }
    }
}
