using ListaCompras.Views;

namespace ListaCompras
{
    public partial class App : Application
    {
        public App(MainPage page)
        {
            InitializeComponent();
            MainPage = page; // usa a página criada pelo DI
        }
    }
}
