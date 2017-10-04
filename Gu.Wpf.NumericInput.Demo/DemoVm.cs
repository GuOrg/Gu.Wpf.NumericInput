namespace Gu.Wpf.NumericInput.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class DemoVm : INotifyPropertyChanged
    {
        private short? shortValue = 0;
        private double? doubleValue = 0;
        private decimal? decimalValue = 0;
        private int? intValue = 0;
        private bool showTouchKeyboardOnTouchEnter = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public BoxVm<DoubleBox, double> Settings { get; } = new DoubleBoxVm { Min = -10, Max = 10 };

        public double? DoubleValue
        {
            get => this.doubleValue;
            set
            {
                if (value.Equals(this.doubleValue))
                {
                    return;
                }

                this.doubleValue = value;
                this.OnPropertyChanged();
            }
        }

        public decimal? DecimalValue
        {
            get => this.decimalValue;
            set
            {
                if (value == this.decimalValue)
                {
                    return;
                }

                this.decimalValue = value;
                this.OnPropertyChanged();
            }
        }

        public int? IntValue
        {
            get => this.intValue;
            set
            {
                if (value == this.intValue)
                {
                    return;
                }

                this.intValue = value;
                this.OnPropertyChanged();
            }
        }

        public short? ShortValue
        {
            get => this.shortValue;
            set
            {
                if (value == this.shortValue)
                {
                    return;
                }

                this.shortValue = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowTouchKeyboardOnTouchEnter
        {
            get => this.showTouchKeyboardOnTouchEnter;
            set
            {
                if (value == this.showTouchKeyboardOnTouchEnter)
                {
                    return;
                }

                this.showTouchKeyboardOnTouchEnter = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
