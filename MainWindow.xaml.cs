using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace YumlMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string baseUrl = "http://yuml.me/diagram/class/";
        DispatcherTimer _timer;
        string _url;

        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer(TimeSpan.FromSeconds(30), DispatcherPriority.Background, _timer_Elapsed, this.Dispatcher);
            _timer.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //refreshButton.IsEnabled = false;
            var url = ConstructUrl(textBox.Text);
            if (url == null)
            {
                refreshButton.IsEnabled = true;
                return;
            }
            urlBox.Text = url;
            //displayBox.Navigate(url);
            _url = url;
            //_timer.Start();
        }

        void _timer_Elapsed(object sender, EventArgs e)
        {
            displayBox.Navigate("about:blank");
            mshtml.IHTMLDocument2 doc = displayBox.Document as mshtml.IHTMLDocument2;
            doc.clear();
            doc.writeln("<h3>There was an error loading the page.  Please retry or visit the following URL.</h3><p>URL:</p><p><code>" + _url + "</code></p>");
            doc.close();
            refreshButton.IsEnabled = true;
            progressBar.IsIndeterminate = false;
            _timer.Stop();
        }

        private void displayBox_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            progressBar.IsIndeterminate = true;
        }

        private void displayBox_LoadCompleted(object sender, NavigationEventArgs e)
        {
            refreshButton.IsEnabled = true;
            progressBar.IsIndeterminate = false;
            _timer.Stop();
        }

        private string ConstructUrl(string input)
        {
            IList<IDictionary> yaml;

            try
            {
                yaml = new List<IDictionary>();
                foreach (IDictionary dict in (IList)YaTools.Yaml.YamlLanguage.StringTo(input))
                {
                    yaml.Add(dict);
                }
            }
            catch  (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                return null;
            }

            var builder = new StringBuilder();

            builder.Append(baseUrl);

            builder.Append(
                yaml
                    .Select(dict => new Class(dict["class"] as IDictionary))
                    .Select(klass => klass.ToUrlPart())
                    .Aggregate((acc, next) => String.Format("{0},{1}", acc, next))
            );

            builder.Append(".");
            return builder.ToString();
        }

        private static void ConstructClassString(StringBuilder builder, IDictionary node)
        {
            var name = (String)node["name"];
            var props = (IList)node["properties"];
            var meths = (IList)node["methods"];

            builder.Append("[");

            builder.Append(name);

            if (props != null && props.Count > 0)
            {
                builder.Append("|");
                foreach (String prop in props)
                {
                    builder.Append(prop);
                    builder.Append(";");
                }
                builder.Remove(builder.Length - 1, 1);
            }

            if (meths != null && meths.Count > 0)
            {
                if (props == null || props.Count == 0)
                    builder.Append("|");

                builder.Append("|");
                foreach (String meth in meths)
                {
                    builder.Append(meth);
                    builder.Append(";");
                }
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");
        }
    }
}
