using AISParser.View;
using AISParser.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AISParser
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow main = new MainWindow();
            main.DataContext = new MainWindowViewModel();
            main.Closing += (o, c) =>
            {
                (main.DataContext as MainWindowViewModel).Close();
            };
            main.Show();
        }
    }
}
