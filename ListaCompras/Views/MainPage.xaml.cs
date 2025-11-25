using ListaCompras.Services;
using ListaCompras.ViewModels;

namespace ListaCompras.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ProdutoViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var view = (sender as CheckBox)?.Parent as View;
            if (view != null)
            {
                await view.FadeTo(0.5, 200); 
                await view.FadeTo(1, 200);
            }
        }
    }
}
