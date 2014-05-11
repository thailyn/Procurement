using System.Windows.Controls;
using Procurement.ViewModel;
using POEApi.Model;

namespace Procurement.View
{
    public partial class SettingsView : UserControl, IView
    {
        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = new SettingsViewModel(this);
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }

        private void CurrencyGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Settings.Save();
        }

        private void AboutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ScreenController.Instance.LoadView(new AboutView());
        }

        private void LeagueCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            string leagueName = ((sender as CheckBox).Content as TextBlock).Text;
            (DataContext as SettingsViewModel).AddDownloadLeague(leagueName);
        }

        private void LeagueCheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            string characterName = ((sender as CheckBox).Content as TextBlock).Text;
            (DataContext as SettingsViewModel).RemoveDownloadLeague(characterName);
        }

        private void CharacterCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            string characterName = ((sender as CheckBox).Content as TextBlock).Text;
            (DataContext as SettingsViewModel).AddDownloadCharacter(characterName);
        }

        private void CharacterCheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            string characterName = ((sender as CheckBox).Content as TextBlock).Text;
            (DataContext as SettingsViewModel).RemoveDownloadCharacter(characterName);
        }
    }
}
