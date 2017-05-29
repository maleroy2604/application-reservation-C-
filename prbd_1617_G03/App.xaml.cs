using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace prbd_1617_G03
{
    
    public partial class App : Application
    {


        public static Messenger Messenger { get; } = new Messenger();
        public static Entities Model { get; } = new Entities();
        public static User CurrentUser { get; set; }
        public const string MSG_VIEW_SHOW = "MSG_VIEW_SHOW";
        public const string MSG_VIEW_PRICE = "MSG_VIEW_PRICE";
        public const string MSG_NEW_SHOW = "MSG_NEW_SHOW";
        public const string MSG_NAMESHOW_CHANGED = "MSG_NAMESHOW_CHANGED";
        public const string MSG_SHOW_CHANGED = "MSG_SHOW_CHANGED";
        public const string MSG_CLOSE_TAB = "MSG_CLOSE_TAB";
        public const string MSG_DISPLAY_SHOW="MSG_DISPLAY_SHOW";
        public const string MSG_DISPLAY_RES = "MSG_DISPLAY_RES";
        public const string MSG_DISPLAY_CLIENT = "MSG_DISPLAY_CLIENT";
        public const string MSG_NAMECLIENT_CHANGED = "MSG_NAMECLIENT_CHANGED";

        public App()
        {
            PrepareDatabase();
           
        }
        
        private void PrepareDatabase()
        {
            // Donne une valeur à la propriété "DataProperty" qui est utilisée comme dossier de base dans App.config pour
            // la connection string vers la DB. Cette valeur est calculée en chemin relatif à partir du dossier de 
            // l'exécutable, soit <dossier projet>/bin/Debug.
            var dbPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Database"));
            Console.WriteLine("Database path: " + dbPath);
            AppDomain.CurrentDomain.SetData("DataDirectory", dbPath);

            // Si la base de données n'existe pas, la créer en exécutant le script SQL
            if (!File.Exists(Path.Combine(dbPath, "ReservationManager.mdf")))
            {
                Console.WriteLine("Creating database...");
                string script = File.ReadAllText(Path.Combine(dbPath, "ReservationManager.sql"));

                // dans le script, on remplace "{DBPATH}" par le dossier où on veut créer la DB
                script = script.Replace("{DBPATH}", dbPath);

                // On splitte le contenu du script en une liste de strings, chacune contenant une commande SQL.
                // Pour faire le split, on se sert des commandes "GO" comme délimiteur.
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                // On se connecte au driver de base de données "(localdb)\MSSQLLocalDB" qui permet de travailler avec des
                // fichiers de données SQL Server attachés sans nécessiter qu'une instance de SQL Server ne soit présente.
                string sqlConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";
                System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(sqlConnectionString);
                connection.Open();
                // On exécute les commandes SQL une par une.
                foreach (string commandString in commandStrings)
                    if (commandString.Trim() != "")
                        using (var command = new SqlCommand(commandString, connection))
                            command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
