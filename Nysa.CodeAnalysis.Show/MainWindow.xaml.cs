using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

using HtmlAgilityPack;

using Nysa.Logics;
using Nysa.Text;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.Show
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static String BaseTestFilesPath = String.Empty;

        public MainWindow()
        {
            var args = Environment.GetCommandLineArgs();
            
            BaseTestFilesPath = args[1];

            InitializeComponent();
        }

        private void ShowParseButton_Click(object sender, RoutedEventArgs e)
        {
            var file = System.IO.Path.Combine(BaseTestFilesPath, @"AccessStash.vbs");
            // var file = Path.Combine(BaseTestFilesPath, @"Hangs\CallSBContainer.ModifiedForTesting.vbs");
            // var file = Path.Combine(BaseTestFilesPath, @"Validate.vbs");
            // var file = Path.Combine(BaseTestFilesPath, @"ParseFail\AddModifyFinancialGroupCall.vbs");
            // var file = Path.Combine(BaseTestFilesPath, @"ParseFail\Bonds.vbs");
            // var file = Path.Combine(BaseTestFilesPath, @"ParseFail\Sentence_ScreenBuilder.vbs");
            // var file = Path.Combine(BaseTestFilesPath, @"ParseFail\JobRecurrence.ModifiedForTesting.vb");

            var source = String.Concat(File.ReadAllText(file), "\r\n");

            var parseOrErr = VBScriptX.Parse(source);

            if (parseOrErr is Confirmed<Node> goodNode)
            {
                var display = new ParseTree(goodNode.Value, source);

                display.Show();
            }

        }

        private void TryParseScripts_Click(object sender, RoutedEventArgs e)
        {
            var search = System.IO.Path.Combine(BaseTestFilesPath, "ParseFail");
            var fails = new List<String>();

            foreach (var filePath in System.IO.Directory.EnumerateFiles(search, "*.vbs"))
            {
                var source = String.Concat(System.IO.File.ReadAllText(filePath), "\r\n");

                var parseOrErr = VBScriptX.Parse(source);

                if (parseOrErr is Failed<Node>)
                    fails.Add(filePath);
            }

            var failFiles = String.Join("\r\n", fails);

            System.Windows.Clipboard.Clear();
            System.Windows.Clipboard.SetText(failFiles);

            System.Windows.MessageBox.Show($"Failed parses {fails.Count}. File names copied to clipboard.");
        }


        private void TryParseHtml_Click(object sender, RoutedEventArgs e)
        {
            var search = System.IO.Path.Combine(BaseTestFilesPath, "HtmlFails");
            var fails = new List<String>();

            foreach (var file in Directory.EnumerateFiles(search, "*.htm", SearchOption.AllDirectories))
            {
                var doc = new HtmlDocument();

                using (var reader = new StreamReader(File.OpenRead(file)))
                {
                    doc.LoadHtml(reader.ReadToEnd());
                }

                var build = new StringBuilder();
                var includes = new List<String>();

                foreach (var script in doc.DocumentNode.Descendants("script"))
                {
                    var lang = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("language"));
                    var type = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("type"));
                    var src = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("src"));

                    if ((lang?.Value ?? String.Empty).DataEquals("vbscript") ||
                        (type?.Value ?? String.Empty).DataEquals(@"text/vbscript"))
                    {
                        var element = Return.Try(() => XElement.Parse(script.OuterHtml));

                        if (src != null && !String.IsNullOrWhiteSpace(src.Value))
                        {
                            includes.Add(src.Value);
                        }
                        else if (element is Confirmed<XElement> goodElement)
                        {
                            // we have an empty element, do nothing
                            build.Append(goodElement.Value.Value);
                            build.AppendLine();
                        }
                        else if (element.Match(elem => true, err => false))
                        {
                            // just skip
                        }
                        else if (!String.IsNullOrWhiteSpace(script.InnerText))
                        {
                            build.Append(script.InnerText);
                            build.AppendLine();
                        }
                    }
                }


                if (build.Length > 0)
                {
                    var parseOrErr = VBScriptX.Parse(build.ToString());

                    if (parseOrErr is Failed<Node>)
                        fails.Add(file);
                }

            } // foreach (var file

        }


    }
}
