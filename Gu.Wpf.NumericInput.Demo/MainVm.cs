namespace Gu.Wpf.NumericInput.Demo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class MainVm : INotifyPropertyChanged
    {
        public MainVm()
        {
            //Vms = new ObservableCollection<IBoxVm>
            //           {
            //               new BoxVm<double>(typeof(DoubleBox)),
            //               new BoxVm<double>(typeof(DoubleBox), -10, 10, 1),
            //               new BoxVm<int>(typeof(IntBox)),
            //               new BoxVm<int>(typeof(IntBox), -10, 10, 1)
            //           };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<IBoxVm> Vms { get; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
