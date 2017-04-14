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
            get => this.text;
            set
            {
                if (Equals(value, this.text))
                {
                    return;
                }

                this.text = value;
                this.OnPropertyChanged();
            }
        }

        public double Value
        {
            get => this.value;
            set
            {
                if (value.Equals(this.value))
                {
                    return;
                }

                this.value = value;
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
