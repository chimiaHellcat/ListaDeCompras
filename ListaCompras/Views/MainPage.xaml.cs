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

    }
}
