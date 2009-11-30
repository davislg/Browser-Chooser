using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace Browser_Chooser
{
    static class Browser
    {
        private static string defaultBrowser;
        private static ObservableCollection<string> browsers = new ObservableCollection<string>();
        private static Dictionary<string, string> browserToPath = new Dictionary<string,string>();

        static Browser()
        {
            var startMenuInternet = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            var defaultExe = (string)startMenuInternet.GetValue("");

            RegistryKey browserRegKey;
            string name, command;

            foreach (string exe in startMenuInternet.GetSubKeyNames())
            {
                browserRegKey = startMenuInternet.OpenSubKey(exe);
                name = (string)browserRegKey.GetValue("");
                command = (string)browserRegKey.OpenSubKey(@"shell\open\command").GetValue("");

                if (exe == defaultExe)
                {
                    defaultBrowser = name;
                }

                browsers.Add(name);
                browserToPath[name] = command;
            }
        }

        public static string Default
        {
            get { return defaultBrowser; }
        }

        public static ObservableCollection<string> Browsers
        {
            get { return browsers; }
        }

        public static Dictionary<string, string> BrowserToPath
        {
            get { return browserToPath; }
        }

        public static void SetAsDefault()
        {
            string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\RegisteredApplications").SetValue("Browser Chooser", @"SOFTWARE\\Browser Chooser\\Capabilities", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities").SetValue("ApplicationName", "Browser Chooser", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities").SetValue("ApplicationIcon", executablePath + ",0", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities").SetValue("ApplicationDescription", "", RegistryValueKind.String);

            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\FileAssociations").SetValue(".xhtml", "BrowserChooserHTML", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\FileAssociations").SetValue(".xht", "BrowserChooserHTML", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\FileAssociations").SetValue(".shtml", "BrowserChooserHTML", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\FileAssociations").SetValue(".html", "BrowserChooserHTML", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\FileAssociations").SetValue(".htm", "BrowserChooserHTML", RegistryValueKind.String);

            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\StartMenu").SetValue("StartMenuInternet", "Browser Chooser.exe", RegistryValueKind.String);

            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\URLAssociations").SetValue("https", "BrowserChooserHTML", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\URLAssociations").SetValue("http", "BrowserChooserHTML", RegistryValueKind.String);
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Browser Chooser\Capabilities\URLAssociations").SetValue("ftp", "BrowserChooserHTML", RegistryValueKind.String);

            Registry.ClassesRoot.CreateSubKey("BrowserChooserHTML").SetValue("", "Browser Chooser HTML", RegistryValueKind.String);
            Registry.ClassesRoot.CreateSubKey("BrowserChooserHTML").SetValue("URL Protocol", "", RegistryValueKind.String);

            Registry.ClassesRoot.CreateSubKey(@"BrowserChooserHTML\DefaultIcon").SetValue("", executablePath + ",0", RegistryValueKind.String);

            Registry.ClassesRoot.CreateSubKey(@"BrowserChooserHTML\shell\open\command").SetValue("", executablePath + " %1", RegistryValueKind.String);

            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice").SetValue("Progid", "BrowserChooserHTML", Microsoft.Win32.RegistryValueKind.String);
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice").SetValue("Progid", "BrowserChooserHTML", Microsoft.Win32.RegistryValueKind.String);
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\ftp\UserChoice").SetValue("Progid", "BrowserChooserHTML", Microsoft.Win32.RegistryValueKind.String);

            try
            {
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.htm\UserChoice");
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.html\UserChoice");
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.shtml\UserChoice");
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.xht\UserChoice");
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.xhtml\UserChoice");

                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.htm\UserChoice").SetValue("Progid", "BrowserChooserHTML", Microsoft.Win32.RegistryValueKind.String);
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.html\UserChoice").SetValue("Progid", "BrowserChooserHTML", Microsoft.Win32.RegistryValueKind.String);
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.shtml\UserChoice").SetValue("Progid", "BrowserChooserHTML", Microsoft.Win32.RegistryValueKind.String);
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.xht\UserChoice").SetValue("Progid", "BrowserChooserHTML", Microsoft.Win32.RegistryValueKind.String);
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.xhtml\UserChoice").SetValue("Progid", "BrowserChooserHTML", Microsoft.Win32.RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MessageBox.Show("Browser Chooser set as default browser.");
        }
    }
}
