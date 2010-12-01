using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using YaTools.Yaml;

namespace YumlMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string baseUrl = "http://yuml.me/diagram/class/";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = ConstructUrl(textBox.Text);
            if (url == null)
            {
                return;
            }
            urlBox.Text = url;
            displayBox.Navigate(url);
        }

        private void displayBox_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            progressBar.IsIndeterminate = true;
        }

        private void displayBox_LoadCompleted(object sender, NavigationEventArgs e)
        {
            progressBar.IsIndeterminate = false;
        }

        private string ConstructUrl(string input)
        {
            var classes = GetClasses(YaTools.Yaml.YamlLanguage.StringTo<IDictionary>(input));

            var mainPart = classes
                            .Select(klass => klass.ToMainPart())
                            .Where(part => !string.IsNullOrEmpty(part))
                            .Aggregate((acc, next) => String.Format("{0},{1}", acc, next));

            var restPart = classes
                            .Select(klass => klass.ToRestPart())
                            .Where(part => !string.IsNullOrEmpty(part))
                            .Aggregate((acc, next) => String.Format("{0},{1}", acc, next));

            var builder = new StringBuilder();
            builder.Append(baseUrl);
            builder.Append(mainPart);
            if (!string.IsNullOrEmpty(restPart))
            {
                builder.Append("," + restPart);
            }

            builder.Append(".");
            return builder.ToString();
        }

        private static IEnumerable<Class> GetClasses(IDictionary yaml)
        {
            return GetClassesYaml(yaml)
                .Where(dict => dict != null)
                .Select(dict => new Class(dict));
        }

        private static IEnumerable<IDictionary> GetClassesYaml(IDictionary yaml)
        {
            var classesYaml = new List<IDictionary>();

            if (yaml.Contains("imports"))
            {
                foreach (string import in (yaml["imports"] as IList))
                {
                    classesYaml.AddRange(GetClassesYaml(YamlLanguage.FileTo<IDictionary>(import)));
                }
            }

            if (yaml.Contains("classes"))
            {
                foreach (IDictionary classYaml in (yaml["classes"] as IList))
                {
                    classesYaml.Add(classYaml);
                }
            }
            return classesYaml;
        }
    }
}
