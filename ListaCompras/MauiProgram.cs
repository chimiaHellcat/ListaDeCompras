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

        // Firestore como singleton
        builder.Services.AddSingleton<FirestoreService>();
        builder.Services.AddSingleton<ProdutoViewModel>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
