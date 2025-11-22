using Google.Cloud.Firestore;
using ListaCompras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaCompras.Services
{
    public class FirestoreService
    {

        private readonly FirestoreDb db;

        public FirestoreService()
        {
            //caminho chave primaria
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "firestore-key.json");



            if (!File.Exists(filePath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("firestore-key.json").Result;
                using var fs = File.Create(filePath);
                stream.CopyTo(fs);
            }

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);



            db = FirestoreDb.Create("listacomprasapp-a0891");



        }

        public FirestoreDb GetDb() => db;



        //testar a conexao


        public async Task TestarConexao()
        {
            try
            {
                var col = db.Collection("teste_maui");
                await col.AddAsync(new { mensagem = "Olá Firebase!", data = DateTime.UtcNow });

                Console.WriteLine("🔥 Conexão OK — Dados enviados ao Firestore");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro na comunicação: " + ex.Message);
                throw;
            }
        }




        // adicionar produto firestone


        public async Task AdicionarProdutoAsync(ProdutoModel produto)
        {
            
            await db.Collection("produtos").AddAsync(new
            {
                descricao = produto.Descricao
            });
        }


        // remover produto 
        public async Task RemoverProdutoAsync(string id)
        {
            await db.Collection("produtos").Document(id).DeleteAsync();
        }


        //atualizar produto

        public async Task AtualizarProdutoAsync(ProdutoModel produto)
        {
            await db.Collection("produtos").Document(produto.Id).UpdateAsync("descricao", produto.Descricao);
        }


        // atualizar  a lista em tempo real
        public void CarregarListaProdutos(Action<List<ProdutoModel>> callback)
        {
            db.Collection("produtos").Listen(snapshot =>
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

    }
}
