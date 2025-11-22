using ListaCompras.Services;
using ListaCompras.ViewModels;
using ListaCompras.Views;

namespace ListaCompras;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Serviços
        builder.Services.AddSingleton<FirestoreService>();

        // ViewModels
        builder.Services.AddSingleton<ProdutoViewModel>();

        // Páginas
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
