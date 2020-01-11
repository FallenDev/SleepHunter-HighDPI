using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

using SleepHunter.Extensions;
using SleepHunter.Settings;

namespace SleepHunter.Views
{
    public partial class SettingsWindow : Window
    {
        static readonly string DotNetRegistryKey = @"Software\Microsoft\NET Framework Setup\NDP\v4\Full\";

        public static readonly int GeneralTabIndex = 0;
        public static readonly int UserInterfaceTabIndex = 1;
        public static readonly int GameClientTabIndex = 2;
        public static readonly int AllMacrosTabIndex = 3;
        public static readonly int SkillMacrosTabIndex = 4;
        public static readonly int SpellMacrosTabIndex = 5;
        public static readonly int FloweringTabIndex = 6;
        public static readonly int NotificationsTabIndex = 7;
        public static readonly int AboutTabIndex = 8;
        public static readonly int DebugTabIndex = 9;

        public int SelectedTabIndex
        {
            get { return (int)GetValue(SelectedTabIndexProperty); }
            set { SetValue(SelectedTabIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedTabIndexProperty =
            DependencyProperty.Register("SelectedTabIndex", typeof(int), typeof(SettingsWindow), new PropertyMetadata(0));

        public SettingsWindow()
        {
            InitializeComponent();
            GetVersion();
        }

        void GetVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var isDebug = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration == "Debug";
            versionText.Text = string.Format("Version {0}.{1}.{2}", version.Major.ToString(), version.Minor.ToString(), version.Build.ToString());
            var buildNumber = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            try
            {
                DateTime _dt = File.GetCreationTime(GetType().Assembly.Location);
                buildDateText.Text = _dt.ToString();
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw e;
            }

            buildText.Text = string.Format("Build {0}", buildNumber);

            if (isDebug)
                buildText.Text += "  (Debug)";

            var frameworkVersion = "???";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(DotNetRegistryKey))
            {
                var releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                if (releaseKey >= 528040)
                    frameworkVersion = "4.8";
                else if (releaseKey >= 461808)
                    frameworkVersion = "4.7.2";
                else if (releaseKey >= 461308)
                    frameworkVersion = "4.7.1";
                else if (releaseKey >= 460798)
                    frameworkVersion = "4.7";
                else if (releaseKey >= 394802)
                    frameworkVersion = "4.6.2";
            }

            frameworkVersionText.Text = string.Format(".NET Framework {0}", frameworkVersion);
        }

        void resetDefaultsButton_Click(object sender, RoutedEventArgs e)
        {
            bool? isOkToReset = this.ShowMessageBox("Reset Default Settings",
               "This will reset all settings to their default values.\nDo you wish to continue?",
               "This action cannot be undone.",
               MessageBoxButton.YesNo,
               460, 240);

            if (isOkToReset.Value)
                UserSettingsManager.Instance.Settings.ResetDefaults();
        }

        void resetVersionsButton_Click(object sender, RoutedEventArgs e)
        {
            bool? isOkToReset = this.ShowMessageBox("Reset Client Versions",
               "This will reset client version data to the defaults.\nDo you wish to continue?",
               "This action cannot be undone.",
               MessageBoxButton.YesNo,
               460, 240);

            if (isOkToReset.Value)
            {
                ClientVersionManager.Instance.ClearVersions();
                ClientVersionManager.Instance.LoadDefaultVersions();

                clientVersionComboBox.ItemsSource = ClientVersionManager.Instance.Versions;
            }
        }

        void resetThemesButton_Click(object sender, RoutedEventArgs e)
        {
            bool? isOkToReset = this.ShowMessageBox("Reset Color Themes",
               "This will restore all the default color themes.\nDo you wish to continue?",
               "This action cannot be undone.",
               MessageBoxButton.YesNo,
               460, 240);

            if (isOkToReset.Value)
            {
                ColorThemeManager.Instance.ClearThemes();
                ColorThemeManager.Instance.LoadDefaultThemes();

                themeColorComboBox.ItemsSource = ColorThemeManager.Instance.Themes;
            }
        }

        void metadataEditorButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.Owner as MainWindow;
            if (mainWindow == null)
                return;

            mainWindow.ShowMetadataWindow();
        }

        void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl == null)
                return;

            var tabItem = tabControl.SelectedItem as TabItem;
            if (tabItem == null)
            {
                this.Title = "Settings";
                return;
            }

            this.Title = string.Format("Settings - {0}", (tabItem.Header as string).Replace("_", string.Empty));
        }

        void updateButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.Owner as MainWindow;
            if (mainWindow == null)
                return;

            mainWindow.CheckForUpdate();
        }
    }
}
