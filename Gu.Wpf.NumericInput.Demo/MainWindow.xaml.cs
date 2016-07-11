namespace Gu.Wpf.NumericInput.Demo
{
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainVm();
        }
    }
}
