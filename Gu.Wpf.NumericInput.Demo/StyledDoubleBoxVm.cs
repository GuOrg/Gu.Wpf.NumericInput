namespace Gu.Wpf.NumericInput.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class StyledDoubleBoxVm : INotifyPropertyChanged
    {
        private string text = "1.2345";
        private double value = 1.2345;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get { return text; }
            set
            {
                if (Equals(value, text)) return;
                text = value;
                OnPropertyChanged();
            }
        }

        public double Value
        {
            get { return value; }
            set
            {
                if (value.Equals(this.value)) return;
                this.value = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
