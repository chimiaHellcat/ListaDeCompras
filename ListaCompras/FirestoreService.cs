using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaCompras
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

    }
}
