namespace Gu.Wpf.NumericInput.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class FocusVm : INotifyPropertyChanged
    {
        private double value1 = 1.234;
        private double value2 = 2.345;
        private double value3 = 3.456;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Value1
        {
            get { return this.value1; }
            set
            {
                if (value.Equals(this.value1)) return;
                this.value1 = value;
                this.OnPropertyChanged();
            }
        }

        public double Value2
        {
            get { return this.value2; }
            set
            {
                if (value.Equals(this.value2)) return;
                this.value2 = value;
                this.OnPropertyChanged();
            }
        }

        public double Value3
        {
            get { return this.value3; }
            set
            {
                if (value.Equals(this.value3)) return;
                this.value3 = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
