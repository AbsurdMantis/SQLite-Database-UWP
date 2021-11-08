using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace PokedexX
{
    class ManageDB
    {
        public ManageDB()
        {

        }

        public async static void InitDB()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("storedPokemon.db", CreationCollisionOption.OpenIfExists);
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "storedPokemon.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                string initCMD = "CREATE TABLE IF NOT EXISTS " +
                    "Pokemon (idPoke	INTEGER NOT NULL UNIQUE, " +
                    "nameP  TEXT NOT NULL, " +
                    "typeP  TEXT NOT NULL)";

                SqliteCommand CMDcreateTable = new SqliteCommand(initCMD, con);
                CMDcreateTable.ExecuteReader();
                con.Close();
            }
        }

        public static void addRecord(int idPoke, String nameP, String typeP)
        {
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "storedPokemon.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                SqliteCommand CMD_Insert = new SqliteCommand();
                CMD_Insert.Connection = con;

                CMD_Insert.CommandText = "INSERT INTO Pokemon VALUES(@idPoke, @nameP, @typeP);";
                CMD_Insert.Parameters.AddWithValue("@idPoke", idPoke);
                CMD_Insert.Parameters.AddWithValue("@nameP", nameP);
                CMD_Insert.Parameters.AddWithValue("@typeP", typeP);

                CMD_Insert.ExecuteReader();

                con.Close();
            }
        }
        
        public class storedPokeData
        {
            public int idPokemon { get; set; }
            public String namePokemon { get; set; }
            public String typePokemon  { get; set; }

            public storedPokeData(int idPoke, String nameP, String typeP)
            {
                idPokemon = idPoke;
                namePokemon = nameP;
                typePokemon = typeP;
                
            }
        }

        public static List<storedPokeData> GetStoredPokeData()
        {
            List<storedPokeData> pokeList = new List<storedPokeData>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "storedPokemon.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                String selectCmd = "SELECT idPoke, nameP, typeP FROM Pokemon";
                SqliteCommand cmd_getRec = new SqliteCommand(selectCmd, con);

                SqliteDataReader reader = cmd_getRec.ExecuteReader();

                while (reader.Read())
                {
                    int teste = int.Parse(reader.GetString(0));
                    pokeList.Add(new storedPokeData(teste, reader.GetString(1), reader.GetString(2))) ; 
                }

                con.Close();
            }

            return pokeList;
        }
    }
}
