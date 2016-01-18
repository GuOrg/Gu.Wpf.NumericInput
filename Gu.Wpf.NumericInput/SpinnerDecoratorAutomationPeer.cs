namespace Gu.Wpf.NumericInput
{
    using System.Windows.Automation.Peers;

    public class SpinnerDecoratorAutomationPeer : FrameworkElementAutomationPeer
    {
        public SpinnerDecoratorAutomationPeer(SpinnerDecorator owner)
            : base(owner)
        {
        }

        protected override string GetClassNameCore()
        {
            return nameof(SpinnerDecorator);
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }
    }
}