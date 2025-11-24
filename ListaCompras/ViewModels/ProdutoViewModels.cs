using ListaCompras.Models;
using ListaCompras.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ListaCompras.ViewModels
{
    public class ProdutoViewModel : INotifyPropertyChanged
    {
        private readonly FirestoreService _firestore;

        public ObservableCollection<ProdutoModel> ListaProdutos { get; set; }
            = new ObservableCollection<ProdutoModel>();

        private string _descricao;
        public string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged(nameof(Descricao));
            }
        }

        private ProdutoModel _itemSelecionado;
        public ProdutoModel ItemSelecionado
        {
            get => _itemSelecionado;
            set
            {
                _itemSelecionado = value;
                OnPropertyChanged(nameof(ItemSelecionado));

                if (value != null)
                    Descricao = value.Descricao;
            }
        }

        public ICommand AdicionarProdutoCmd => new Command(async () => await Adicionar());
        public ICommand RemoverProdutoCmd => new Command(async () => await Remover());
        public ICommand AlterarProdutoCmd => new Command(async () => await Alterar());

        public ProdutoViewModel(FirestoreService firestore)
        {
            _firestore = firestore;

            _firestore.CarregarListaProdutos((produtos) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Evita processamento se o snapshot é igual ao atual
                    if (ListaProdutos.Count == produtos.Count)
                    {
                        bool iguais = true;

                        for (int i = 0; i < produtos.Count; i++)
                        {
                            if (ListaProdutos[i].Id != produtos[i].Id ||
                                ListaProdutos[i].Descricao != produtos[i].Descricao)
                            {
                                iguais = false;
                                break;
                            }
                        }

                        if (iguais)
                            return; // Não atualiza se o snapshot é duplicado
                    }

                    ListaProdutos.Clear();
                    foreach (var item in produtos)
                        ListaProdutos.Add(item);
                });
            });

        }

        private async Task Adicionar()
        {
            if (string.IsNullOrWhiteSpace(Descricao))
                return;

            await _firestore.AdicionarProdutoAsync(new ProdutoModel { Descricao = Descricao });
            Descricao = "";
        }

        private async Task Remover()
        {
            if (ItemSelecionado == null)
                return;

            await _firestore.RemoverProdutoAsync(ItemSelecionado.Id);
        }

        private async Task Alterar()
        {
            if (ItemSelecionado == null || string.IsNullOrWhiteSpace(Descricao))
                return;

            ItemSelecionado.Descricao = Descricao;
            await _firestore.AtualizarProdutoAsync(ItemSelecionado);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
