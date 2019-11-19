using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow window;

            if (e.Args != null && e.Args.Count() > 0)
            {
                this.Properties["ArbitraryArgName"] = e.Args[0];
                string fname = e.Args[0];
                window = new MainWindow(fname);
            }
            else
            {
                window = new MainWindow();
            }

            window.Show();

            //I have no idea why I added to this code
            /**
            if (AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments.ActivationData != null
                && AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments.ActivationData.Length > 0)
            {
                string fname = "No filename given";
                try
                {
                    fname = AppDomain.CurrentDomain.SetupInformation
                            .ActivationArguments.ActivationData[0];

                    // It comes in as a URI; this helps to convert it to a path.
                    Uri uri = new Uri(fname);
                    fname = uri.LocalPath;

                    this.Properties["ArbitraryArgName"] = fname;
                    MessageBox.Show(fname);
                }
                catch (Exception ex)
                {
                    // For some reason, this couldn't be read as a URI.
                    // Do what you must...
                }
            }
            **/

        }


    }
}
