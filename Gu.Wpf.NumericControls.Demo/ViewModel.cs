namespace Gu.Wpf.NumericControls.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.NumericControls.Demo.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double DoubleValue { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
