using ArxWidgetManager.Models;
using ArxWidgetManager.ViewModels;
using Microsoft.CSharp;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Windows;

namespace ArxWidgetManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainModel;
        }

        private CompilerResults Compile()
        {
            var codeProvider = new CSharpCodeProvider();
            var compilerParameters = new CompilerParameters();
            compilerParameters.GenerateExecutable = true;
            compilerParameters.GenerateInMemory = false;
            compilerParameters.OutputAssembly = "test.exe";
            compilerParameters.IncludeDebugInformation = true;
            compilerParameters.CompilerOptions = @"/win32icon:temp.ico";
            // Add needed assemblies
            compilerParameters.ReferencedAssemblies.Add("System.dll");

            var result = codeProvider.CompileAssemblyFromFile(compilerParameters, @"D:\_RBW Files\Development\MyProjects\DotNet\ArxWidgetManager\ArxPluginLoader\Program.cs");
            return result;
        }

        private void MenuItemAddWidget_Click(object sender, RoutedEventArgs e)
        {
            var widgetPath = BrowseForWidget();
            if (!String.IsNullOrWhiteSpace(widgetPath))
            {
                var widget = Widget.LoadFromFile(widgetPath);
                widget.WidgetStartedEvent += widget_WidgetStartedEvent;
                widget.WidgetStoppedEvent += widget_WidgetStoppedEvent;
                var widgetViewModel = new WidgetViewModel(widget);
                _mainModel.Widgets.Add(widgetViewModel);
                _mainModel.StatusText = String.Format("Widget '{0}' (v. {1}) loaded", widget.Name, widget.Version);
            }
        }

        private void widget_WidgetStartedEvent(Widget obj)
        {
            _mainModel.StatusText = String.Format("Widget '{0}' started", obj.Name);
        }

        private void widget_WidgetStoppedEvent(Widget obj)
        {
            _mainModel.StatusText = String.Format("Widget '{0}' stopped", obj.Name);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private string BrowseForWidget()
        {
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executables (*.exe)|*.exe|DLL Files (*.dll)|*.dll";
            var result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                return filename;
            }
            return null;
        }
    }
}
