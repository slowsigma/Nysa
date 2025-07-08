using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.ComponentModel;
using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class DemoViewModel : NormalWindowViewModel
    {
        private IReadOnlyList<DemoPage> _DemoPages;
        private Int32                   _DemoPageIndex;
        private String                  _PageTitle;
        private Visibility              _PageTitleVisibility;
        private BitmapImage?            _Image;
        private Visibility              _ImageVisibility;
        private UserControl?            _Control;
        private Visibility              _ControlVisibility;
        private StackPanel              _RawContent;
        private Visibility              _TextVisibility;
        private String                  _PageIdentifier;
        private Boolean                 _PreviousEnabled;
        private Boolean                 _NextEnabled;

        public BitmapImage? Image
        {
            get { return this._Image; }
            set
            {
                this._Image = value;

                this.NotifyChanged(nameof(Image));
            }
        }

        public String PageTitle
        {
            get { return this._PageTitle; }
            set { this.UpdateObjectProperty<String>(ref this._PageTitle, value, nameof(PageTitle)); }
        }

        public Visibility PageTitleVisibility
        {
            get { return this._PageTitleVisibility; }
            set { this.UpdateValueProperty<Visibility>(ref this._PageTitleVisibility, value, nameof(PageTitleVisibility)); }
        }

        public Visibility ImageVisibility
        {
            get { return this._ImageVisibility; }
            set { this.UpdateValueProperty<Visibility>(ref this._ImageVisibility, value, nameof(ImageVisibility)); }
        }

        public UserControl? Control
        {
            get { return this._Control; }
            set
            {
                if (!Object.Equals(this._Control, value))
                {
                    this._Control = value;
                    this.NotifyChanged(nameof(Control));
                }
            }
        }

        public Visibility ControlVisibility
        {
            get { return this._ControlVisibility; }
            set { this.UpdateValueProperty(ref this._ControlVisibility, value, nameof(ControlVisibility)); }
        }

        public Visibility TextVisibility
        {
            get { return this._TextVisibility; }
            set { this.UpdateValueProperty(ref this._TextVisibility, value, nameof(TextVisibility)); }
        }

        public String PageIdentifier
        {
            get { return this._PageIdentifier; }
            set { this.UpdateObjectProperty(ref this._PageIdentifier, value, nameof(PageIdentifier)); }
        }

        public Boolean PreviousEnabled
        {
            get { return this._PreviousEnabled; }
            set { this.UpdateValueProperty(ref this._PreviousEnabled, value, nameof(PreviousEnabled)); }
        }

        public Boolean NextEnabled
        {
            get { return this._NextEnabled; }
            set { this.UpdateValueProperty(ref this._NextEnabled, value, nameof(NextEnabled)); }
        }
        
        public ICommand NextCommand { get; private set; }
        public ICommand PrevCommand { get; private set; }

        private void LeavePage()
        {
            var page = this._DemoPages[this._DemoPageIndex];

            if (page is DemoPageControl controlPage && controlPage.OnLeave != null)
                controlPage.OnLeave();
        }

        private void RefreshPage()
        {
            var page = this._DemoPages[this._DemoPageIndex];

            this.PageTitle = page.PageTitle ?? String.Empty;
            this.PageTitleVisibility = page.PageTitle != null
                                          ? Visibility.Visible
                                          : Visibility.Collapsed;

            this.PreviousEnabled = this._DemoPageIndex > 0;
            this.NextEnabled = this._DemoPageIndex < (this._DemoPages.Count - 1);

            if (page is DemoPageImage imagePage)
            {
                var bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePage.ImagePath);
                bitmap.EndInit();

                this.Image = bitmap;
                this.ImageVisibility = Visibility.Visible;
                this.Control = null;
                this.ControlVisibility = Visibility.Collapsed;
                this.TextVisibility = Visibility.Collapsed;
            }
            else if (page is DemoPageControl controlPage)
            {
                if (controlPage.OnEnter != null)
                    controlPage.OnEnter();

                this.Image = null;
                this.ImageVisibility = Visibility.Collapsed;
                this.Control = controlPage.Control;
                this.ControlVisibility = Visibility.Visible;
                this.TextVisibility = Visibility.Collapsed;
            }
            else if (page is DemoPageText textPage)
            {
                this.Image = null;
                this.ImageVisibility = Visibility.Collapsed;
                this.Control = null;
                this.ControlVisibility = Visibility.Collapsed;

                this._RawContent.Children.Clear();
                this._RawContent.Children.Add(textPage.Text);
                this.TextVisibility = Visibility.Visible;
            }
        }

        public DemoViewModel(IReadOnlyList<DemoPage> demoPages, DemoView window)
            : base(window)
        {
            this._DemoPages     = demoPages;
            this._DemoPageIndex = 0;

            this._PageTitle     = String.Empty;

            this._RawContent    = (StackPanel)window.FindName("_StackPanel");

            this.PageIdentifier = $"{(this._DemoPageIndex + 1)} / {this._DemoPages.Count}";
            this.NextCommand    = new Command(this.NextPage, null);
            this.PrevCommand    = new Command(this.PrevPage, null);

            this.RefreshPage();
        }

        private void PrevPage()
        {
            this.LeavePage();
            
            this._DemoPageIndex = this._DemoPageIndex > 0 ? this._DemoPageIndex - 1 : this._DemoPageIndex;
            this.PageIdentifier = $"{(this._DemoPageIndex + 1)} / {this._DemoPages.Count}";

            this.RefreshPage();
        }

        private void NextPage()
        {
            this.LeavePage();

            this._DemoPageIndex = this._DemoPageIndex < (this._DemoPages.Count - 1) ? this._DemoPageIndex + 1 : this._DemoPageIndex;
            this.PageIdentifier = $"{(this._DemoPageIndex + 1)} / {this._DemoPages.Count}";

            this.RefreshPage();
        }

    }

}