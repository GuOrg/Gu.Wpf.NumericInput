namespace Gu.Wpf.NumericInput.Demo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.NumericInput.Demo.Annotations;

    public class MainVm : INotifyPropertyChanged
    {
        private readonly ObservableCollection<IBoxVm> _vms = new ObservableCollection<IBoxVm>();

        public MainVm()
        {
            _vms = new ObservableCollection<IBoxVm>
                       {
                           new BoxVm<double>(typeof(DoubleBox)),
                           new BoxVm<double>(typeof(DoubleBox), -10, 10, 1),
                           new BoxVm<int>(typeof(IntBox)),
                           new BoxVm<int>(typeof(IntBox), -10, 10, 1)
                       };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<IBoxVm> Vms
        {
            get
            {
                return _vms;
            }
        }

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
