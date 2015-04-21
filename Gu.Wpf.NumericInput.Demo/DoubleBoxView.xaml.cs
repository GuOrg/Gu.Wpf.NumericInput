namespace Gu.Wpf.NumericInput.Demo
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DobleBoxView.xaml
    /// </summary>
    public partial class DoubleBoxView : UserControl
    {
        public DoubleBoxView()
        {
            InitializeComponent();
            DataContext = new BoxVm<double>(typeof(DoubleBox), -50, 50, 1);
        }
    }
}
