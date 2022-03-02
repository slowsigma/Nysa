using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public static class ViewExtensions
    {

        public static Func<Boolean?> ShowDialog<TView, TViewModel>(this TView view, TViewModel viewModel )
            where TView : Window
        {
            return () =>
            {
                view.DataContext = viewModel;

                return view.ShowDialog();
            };
        }

        public static (TView View, TViewModel ViewModel) Bound<TView, TViewModel>(this TView view, Func<TView, TViewModel> createViewModel)
            where TView : NormalWindow
            where TViewModel : NormalWindowViewModel
        {
            var viewModel = createViewModel(view);

            view.DataContext = viewModel;

            return (view, viewModel);
        }

        public static TViewModel WithViewModel<TView, TViewModel>(this TView view, TViewModel viewModel)
            where TView : NormalWindow
        {
            view.DataContext = viewModel;
            return viewModel;
        }

    }

}
