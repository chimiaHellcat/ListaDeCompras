namespace ListaCompras
{
    public partial class MainPage : ContentPage
    {
        private readonly FirestoreService firestore;

        public MainPage(FirestoreService firestoreservice)
        {
            InitializeComponent();

            firestore = firestoreservice;
            Testar();
        }
        private async void Testar()
        {
            await firestore.TestarConexao();
        }

    }
}
