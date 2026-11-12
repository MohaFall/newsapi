using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Api_call
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Without secrets.config the app cannot reach NewsAPI nor MySQL: say it clearly and stop
            if (!Config.IsComplete)
            {
                MessageBox.Show(
                    "Fichier secrets.config manquant ou incomplet.\n" +
                    "Copiez secrets.config.example en secrets.config et mettez-y votre clé NewsAPI " +
                    "et votre chaîne de connexion MySQL.",
                    "NewsAPI", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }
            base.OnStartup(e);
        }
    }
}
