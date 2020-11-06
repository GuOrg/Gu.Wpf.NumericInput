namespace Gu.Wpf.NumericInput
{
    using System.Windows.Automation.Peers;

    /// <summary>
    /// An <see cref="AutomationPeer"/> for <see cref="SpinnerDecorator"/>.
    /// </summary>
    public class SpinnerDecoratorAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnerDecoratorAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="SpinnerDecorator"/>.</param>
        public SpinnerDecoratorAutomationPeer(SpinnerDecorator owner)
            : base(owner)
        {
        }

        /// <inheritdoc/>
        protected override string GetClassNameCore()
        {
            return nameof(SpinnerDecorator);
        }

        /// <inheritdoc/>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }
    }
}
