using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using ListaCompras.Models;
using ListaCompras.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ListaCompras.Services
{
    public class FirestoreService : BaseViewModel
    {
        private readonly FirestoreDb db;

        public FirestoreService()
        {
            // Caminho local onde a chave será copiada
            string localPath = Path.Combine(FileSystem.AppDataDirectory, "firestore-key.json");

            // Copia a chave do pacote para o armazenamento interno do app (Android precisa disso!)
            if (!File.Exists(localPath))
            {
                using var asset = FileSystem.OpenAppPackageFileAsync("firestore-key.json").Result;
                using var dest = File.Create(localPath);
                asset.CopyTo(dest);
            }

            // Carrega credenciais pela stream
            GoogleCredential credential;

            using (var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }

            // Conecta no Firestore com credenciais
            db = new FirestoreDbBuilder
            {
                ProjectId = "listacomprasapp-a0891",
                Credential = credential
            }.Build();
        }

        public FirestoreDb GetDb() => db;


        // 🔥 Testar conexão
        public async Task TestarConexao()
        {
            try
            {
                var col = db.Collection("teste_maui");
                await col.AddAsync(new
                {
                    mensagem = "Olá Firebase!",
                    data = DateTime.UtcNow
                });

                Console.WriteLine("🔥 Conectado ao Firestore com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro no Firestore: " + ex.Message);
                throw;
            }
        }


        // 🔥 Adicionar produto
        public async Task AdicionarProdutoAsync(ProdutoModel produto)
        {
            await db.Collection("produtos").AddAsync(new
            {
                descricao = produto.Descricao
            });
        }


        // 🔥 Remover produto
        public async Task RemoverProdutoAsync(string id)
        {
            await db.Collection("produtos").Document(id).DeleteAsync();
        }


        // 🔥 Atualizar produto
        public async Task AtualizarProdutoAsync(ProdutoModel produto)
        {
            await db.Collection("produtos")
                .Document(produto.Id)
                .UpdateAsync("descricao", produto.Descricao);
        }


        // 🔥 Realtime listener


        private FirestoreChangeListener _listener;

        public void CarregarListaProdutos(Action<List<ProdutoModel>> callback)
        {
            // ⚠ Só cria o listener se ainda não existir
            if (_listener != null)
                return;

            _listener = db.Collection("produtos").Listen(snapshot =>
            {
                var lista = new List<ProdutoModel>();

                foreach (var doc in snapshot.Documents)
                {
                    lista.Add(new ProdutoModel
                    {
                        Id = doc.Id,
                        Descricao = doc.GetValue<string>("descricao")
                    });
                }

                callback(lista);
            });
        }


        public void PararListener()
        {
            _listener?.StopAsync(); // ← método correto
            _listener = null;
        }




    }
}
