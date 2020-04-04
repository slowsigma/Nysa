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

using Nysa.Logics;
using Nysa.Scheduling.Calendars;
using Nysa.Scheduling.Dates;

namespace Nysa.Scheduling.RuleVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RuleVisualizerView : NormalWindow
    {
        private RulesViewModel _RulesViewModel;

        public RuleVisualizerView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var rules = File.ReadAllText(@"..\..\..\HolidayRules.json")
                            .ToDateRules();

            this._RulesViewModel = new RulesViewModel(rules);
            var selectorV = new RuleSelectorView();
            selectorV.DataContext = this._RulesViewModel;

            selectorV.Show();

            foreach (var rvm in this._RulesViewModel.Items)
                rvm.PropertyChanged += Rvm_PropertyChanged;

            this.RefreshContent();
        }

        private void RefreshContent()
        {
            var logic = this._RulesViewModel.Items.Where(i => i.IsSelected).Select(j => j.Rule).ToArray().ToLogic();

            var cal = 5.CalendarYears(2016, logic);

            var view = cal.ToView();

            this.Content = view;
        }

        private void Rvm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.RefreshContent();
        }
    }
}
