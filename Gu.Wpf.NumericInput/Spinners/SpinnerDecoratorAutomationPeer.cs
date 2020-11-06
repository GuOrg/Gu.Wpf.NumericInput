namespace Gu.Wpf.NumericInput
{
    using System.Windows.Automation.Peers;

    public class SpinnerDecoratorAutomationPeer : FrameworkElementAutomationPeer
    {
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