using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Browser_Chooser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                var url = e.Args[0];
                var settings = new BrowserRegexSettings();
                var browserRegexen = settings.BrowserRegexen;

                foreach (BrowserRegex browserRegex in browserRegexen)
                {
                    if (Regex.IsMatch(url, browserRegex.Regex))
                    {
                        var command = Browser.BrowserToPath[browserRegex.Browser];
                        
                        Process.Start(command, url);

                        break;
                    }
                }

                Application.Current.Shutdown();
            }
        }
    }
}
