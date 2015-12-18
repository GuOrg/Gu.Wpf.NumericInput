namespace Gu.Wpf.NumericInput.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class FocusVm : INotifyPropertyChanged
    {
        private double value1 = 1.234;
        private double value2 = 5.678;
        private string text1 = "1.234";
        private string text2 = "5.678";
        public event PropertyChangedEventHandler PropertyChanged;

        public string Text1
        {
            get { return text1; }
            set
            {
                if (value == text1) return;
                text1 = value;
                OnPropertyChanged();
            }
        }

        public string Text2
        {
            get { return text2; }
            set
            {
                if (value == text2) return;
                text2 = value;
                OnPropertyChanged();
            }
        }

        public double Value1
        {
            get { return value1; }
            set
            {
                if (value.Equals(value1)) return;
                value1 = value;
                OnPropertyChanged();
            }
        }

        public double Value2
        {
            get { return value2; }
            set
            {
                if (value.Equals(value2)) return;
                value2 = value;
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
