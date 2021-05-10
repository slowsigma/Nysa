using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Nysa.Snakes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly Int32 TravelX = 8;
        public static readonly Int32 TravelY = 8;

        private Random                  _Rnd;
        private Queue<FrameworkElement> _Segments;
        private Int32                   _Length;
        private Int32                   _CurrentX;
        private Int32                   _CurrentY;
        private Int32                   _DirectionX; // -1, 0, 1
        private Int32                   _DirectionY; // -1, 0, 1
        private DispatcherTimer         _Timer;
        private SolidColorBrush         _SegmentBrush;

        public MainWindow()
        {
            this._Rnd        = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));
            this._Segments   = new Queue<FrameworkElement>();
            this._Length     = 10;
            this._DirectionX = 1;
            this._DirectionY = 0;
            this._Timer      = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), DispatcherPriority.Normal, (s, a) => this.CheckState(), this.Dispatcher);
            this._Timer.Start();
            this._SegmentBrush = new SolidColorBrush() { Color = Color.FromRgb(255, 0, 0) };

            InitializeComponent();

            this._CurrentX = this._Rnd.Next(0, Convert.ToInt32(this._Canvas.ActualWidth));
            this._CurrentY = this._Rnd.Next(0, Convert.ToInt32(this._Canvas.ActualHeight));

            this.KeyDown += (s, a) => this.CheckKey(a);
        }

        private void CheckKey(KeyEventArgs keyEvent)
        {
            if (keyEvent.Key == Key.Up)
            {
                this._DirectionX = 0;
                this._DirectionY = -1;
            }
            else if (keyEvent.Key == Key.Down)
            {
                this._DirectionX = 0;
                this._DirectionY = 1;
            }
            else if (keyEvent.Key == Key.Right)
            {
                this._DirectionX = 1;
                this._DirectionY = 0;
            }
            else if (keyEvent.Key == Key.Left)
            {
                this._DirectionX = -1;
                this._DirectionY = 0;
            }
        }

        private Int32 Wrapped(Int32 newValue, Int32 limit)
            => (newValue < 0) ? limit : newValue % limit;
        

        private void CheckState()
        {
            this._CurrentX = Wrapped(this._CurrentX + (TravelX * this._DirectionX), Convert.ToInt32(this._Canvas.ActualWidth));
            this._CurrentY = Wrapped(this._CurrentY + (TravelY * this._DirectionY), Convert.ToInt32(this._Canvas.ActualHeight));

            var segment = new Ellipse()
            {
                Fill = this._SegmentBrush,
                Height = 10,
                Width = 10,
                RenderTransform = new TranslateTransform() { X = this._CurrentX, Y = this._CurrentY }
            };

            this._Canvas.Children.Add(segment);

            if (this._Segments.Count >= this._Length && this._Segments.Count > 0)
            {
                var delete = this._Segments.Dequeue();
                this._Canvas.Children.Remove(delete);
            }

            this._Segments.Enqueue(segment);
        }

        private void CreateSegment()
        {

        }


    }
}
