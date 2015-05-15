namespace Gu.Wpf.NumericInput.Demo
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for NullableDoubleBoxView.xaml
    /// </summary>
    public partial class NullableDoubleBoxView : UserControl
    {
        public NullableDoubleBoxView()
        {
            InitializeComponent();
            DataContext = new NullbableDoubleViewModel();
        }
    }
}
