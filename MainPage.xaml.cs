using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TP_WP8.src;
using System.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Core;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=391641

namespace TP_WP8
{
    
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {
        Connexion con;
        
        public MainPage()
        {
     
            con = new Connexion(this);
            
            this.InitializeComponent();
            con.connect();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            con.newLineEvent += con_newLineEvent;
            this.thread();
          
        }

        void con_newLineEvent(object sender, NewLineEventArgs args)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (Outputview.Text == null)
                {
                    Outputview.Text = args.message;
                }
                else
                {
                    Outputview.Text += args.message;
                }
               
            });
            
            
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d’événement décrivant la manière dont l’utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: préparer la page pour affichage ici.

            // TODO: si votre application comporte plusieurs pages, assurez-vous que vous
            // gérez le bouton Retour physique en vous inscrivant à l’événement
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si vous utilisez le NavigationHelper fourni par certains modèles,
            // cet événement est géré automatiquement.
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public async Task thread()
        {          
            var task = Task.Run(
                () =>
            {
                
                    System.Diagnostics.Debug.WriteLine("je boucle");
                    con.receive();
                
            });
           
               
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(InputText.Text);
            con.send(InputText.Text);            
        }
    }
}
