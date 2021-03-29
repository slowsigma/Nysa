using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Nysa.ComponentModel;
using Nysa.Windows.Input;

namespace Nysa.Windows
{

    public class NormalWindowViewModel : ModelObject
    {

        public SimpleCommand WindowCloseCommand     { get; private set; }
        public SimpleCommand WindowMaximizeCommand  { get; private set; }
        public SimpleCommand WindowMinimizeCommand  { get; private set; }
        public SimpleCommand WindowRestoreCommand   { get; private set; }
        public SimpleCommand WindowInfoCommand      { get; private set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Visibility _WindowCloseVisibility;
        public Visibility WindowCloseVisibility
        {
            get { return this._WindowCloseVisibility; }
            private set { this.UpdateValueProperty(ref this._WindowCloseVisibility, value, nameof(WindowCloseVisibility)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Visibility _WindowRestoreVisibility;
        public Visibility WindowRestoreVisibility
        {
            get { return this._WindowRestoreVisibility; }
            private set { this.UpdateValueProperty(ref this._WindowRestoreVisibility, value, nameof(WindowRestoreVisibility)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Visibility _WindowMaximizeVisibility;
        public Visibility WindowMaximizeVisibility
        {
            get { return this._WindowMaximizeVisibility; }
            private set { this.UpdateValueProperty(ref this._WindowMaximizeVisibility, value, nameof(WindowMaximizeVisibility)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Visibility _WindowMinimizeVisibility;
        public Visibility WindowMinimizeVisibility
        {
            get { return this._WindowMinimizeVisibility; }
            private set { this.UpdateValueProperty(ref this._WindowMinimizeVisibility, value, nameof(WindowMinimizeVisibility)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Visibility _WindowInfoVisibility;
        public Visibility WindowInfoVisibility
        {
            get { return this._WindowInfoVisibility; }
            private set { this.UpdateValueProperty(ref this._WindowInfoVisibility, value, nameof(WindowInfoVisibility)); }
        }

        public Boolean ShowWindowClose
        {
            get { return this._WindowCloseVisibility == Visibility.Visible; }
            set { this.WindowCloseVisibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Boolean ShowWindowMaximizeRestore
        {
            get { return this._WindowMaximizeVisibility == Visibility.Visible || this._WindowRestoreVisibility == Visibility.Visible; }
            set
            {
                this.WindowMaximizeVisibility = value && this.Window.WindowState != WindowState.Maximized
                                                ? Visibility.Visible
                                                : Visibility.Collapsed;
                this.WindowRestoreVisibility  = value && this.Window.WindowState == WindowState.Maximized
                                                ? Visibility.Visible
                                                : Visibility.Collapsed;
            }
        }

        public Boolean ShowWindowMinimize
        {
            get { return this._WindowMaximizeVisibility == Visibility.Visible; }
            set { this.WindowMinimizeVisibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Boolean ShowWindowInfo
        {
            get { return this._WindowInfoVisibility == Visibility.Visible; }
            set { this.WindowInfoVisibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }


        protected virtual Boolean CanWindowClose(Object parameter)
            => true;
        protected virtual void OnWindowClose(Object parameter)
            => this.Window.Close();

        protected virtual void OnWindowInfo(Object parameter) { }

        private Boolean ReturnTrue(Object parameter) => true;


        public NormalWindow Window { get; private set; }

        public NormalWindowViewModel(NormalWindow window)
        {
            this.Window = window;
            this.ShowWindowMaximizeRestore = true;
            this.ShowWindowInfo = false;

            this.Window.StateChanged += View_StateChanged;

            this.WindowCloseCommand = new SimpleCommand()
            {
                CanExecuteDelegate = this.CanWindowClose,
                ExecuteDelegate = this.OnWindowClose
            };

            this.WindowMaximizeCommand = new SimpleCommand()
            {
                CanExecuteDelegate = this.ReturnTrue,
                ExecuteDelegate = o => this.Window.WindowState = WindowState.Maximized
            };
            this.WindowMinimizeCommand = new SimpleCommand()
            {
                CanExecuteDelegate = this.ReturnTrue,
                ExecuteDelegate = o => this.Window.WindowState = WindowState.Minimized
            };
            this.WindowRestoreCommand = new SimpleCommand()
            {
                CanExecuteDelegate = this.ReturnTrue,
                ExecuteDelegate = o => this.Window.WindowState = WindowState.Normal
            };
            this.WindowInfoCommand = new SimpleCommand()
            {
                CanExecuteDelegate = this.ReturnTrue,
                ExecuteDelegate = this.OnWindowInfo
            };
        }

        private void View_StateChanged(object sender, EventArgs e)
        {
            if (this.ShowWindowMaximizeRestore && this.Window.WindowState == WindowState.Maximized)
            {
                this.WindowMaximizeVisibility = Visibility.Collapsed;
                this.WindowRestoreVisibility = Visibility.Visible;
            }
            else if (this.ShowWindowMaximizeRestore)
            {
                this.WindowMaximizeVisibility = Visibility.Visible;
                this.WindowRestoreVisibility = Visibility.Collapsed;
            }
        }
    }

}
