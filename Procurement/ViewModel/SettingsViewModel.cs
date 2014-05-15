﻿using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using POEApi.Model;
using Procurement.View;

namespace Procurement.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsView view;

        private string currentCharacter;
        private string currentLeague;
        public string CurrentLeague
        {
            get { return currentLeague; }
            set
            {
                if (currentLeague == value)
                    return;

                currentLeague = value;
                Settings.UserSettings["FavoriteLeague"] = value;
                Settings.Save();
                refreshCharacters();
            }
        }

        private bool compactMode;
        public bool CompactMode
        {
            get { return compactMode; }
            set
            {
                compactMode = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CompactMode"));

                Settings.UserSettings["CompactMode"] = Convert.ToString(value);
                Settings.Save();
            }
        }

        public string CurrentCharacter
        {
            get { return currentCharacter; }
            set 
            {
                if (currentCharacter == value)
                    return;
                currentCharacter = value;
                Settings.UserSettings["FavoriteCharacter"] = value;
                Settings.Save();
            }
        }

        public Dictionary<string, List<string>> AllCharactersByLeague
        {
            get
            {
                return ApplicationState.AllCharactersByLeague;
            }
        }

        public List<string> MyCharacters
        {
            get
            {
                return Settings.Lists["MyCharacters"];
            }
        }

        public List<string> MyLeagues
        {
            get
            {
                return Settings.Lists["MyLeagues"];
            }
        }

        public List<string> Characters { get; set; }

        public List<string> Leagues { get; set; }

        public List<CurrencyRatio> CurrencyRatios
        {
            get { return Settings.CurrencyRatios.Values.ToList(); }
        }

        public ICommand UpdateRates { get; private set; }

        private bool downloadOnlyMyLeagues;
        public bool DownloadOnlyMyLeagues
        {
            get { return downloadOnlyMyLeagues; }
            set
            {
                downloadOnlyMyLeagues = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DownloadOnlyMyLeagues"));

                Settings.UserSettings["DownloadOnlyMyLeagues"] = Convert.ToString(value);
                Settings.Save();
            }
        }

        private bool downloadOnlyMyCharacters;
        public bool DownloadOnlyMyCharacters
        {
            get { return downloadOnlyMyCharacters; }
            set
            {
                downloadOnlyMyCharacters = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DownloadOnlyMyCharacters"));

                Settings.UserSettings["DownloadOnlyMyCharacters"] = Convert.ToString(value);
                Settings.Save();
            }
        }

        private List<TabInfo> stashTabs;
        public List<TabInfo> StashTabs 
        {
            get { return stashTabs; }
            set
            {
                stashTabs = value;
                onPropertyChanged("StashItems");
            }
        }
        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public SettingsViewModel(SettingsView view)
        {
            this.view = view;
            this.Leagues = ApplicationState.Leagues;
            this.CurrentLeague = Settings.UserSettings["FavoriteLeague"];
            refreshCharacters();
            this.CurrentCharacter = Settings.UserSettings["FavoriteCharacter"];
            this.CompactMode = Convert.ToBoolean(Settings.UserSettings["CompactMode"]);
            this.DownloadOnlyMyLeagues = Convert.ToBoolean(Settings.UserSettings["DownloadOnlyMyLeagues"]);
            this.DownloadOnlyMyCharacters = Convert.ToBoolean(Settings.UserSettings["DownloadOnlyMyCharacters"]);

            stashTabs = getAllTabs();
        }

        private List<TabInfo> getAllTabs()
        {
            var savedTabs = Settings.Lists["IgnoreTabsInRecipes"];
            var tabNames = ApplicationState.Stash.Values.SelectMany(s => s.Tabs)
                                                        .Select(t => t.Name).Distinct();

            cullInvalidTabs(tabNames);
            return tabNames.Select<string, TabInfo>(t => new TabInfo() { Name = t, IsChecked = savedTabs.Contains(t) }).ToList();
        }

        private static void cullInvalidTabs(IEnumerable<string> tabs)
        {
            var invalidTabs = Settings.Lists["IgnoreTabsInRecipes"].Where(t => !tabs.Contains(t));

            if (invalidTabs.Count() == 0)
                return;

            Settings.Lists["IgnoreTabsInRecipes"].RemoveAll(t => invalidTabs.Contains(t));
            Settings.Save();
        }

        public void AddDownloadLeague(string leagueName)
        {
            addToList("MyLeagues", leagueName);
        }

        public void RemoveDownloadLeague(string leagueName)
        {
            removeFromList("MyLeagues", leagueName);
        }

        public void AddDownloadCharacter(string characterName)
        {
            addToList("MyCharacters", characterName);
        }

        public void RemoveDownloadCharacter(string characterName)
        {
            removeFromList("MyCharacters", characterName);
        }

        private void refreshCharacters()
        {
            this.Characters = ApplicationState.Model.GetCharacters().Where(c => c.League == CurrentLeague).Select(c => c.Name).ToList();
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Characters"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RecipeTabChecked(string tabName)
        {
            addToList("IgnoreTabsInRecipes", tabName);
            ScreenController.Instance.InvalidateRecipeScreen();
        }

        internal void RecipeTabUnchecked(string tabName)
        {
            removeFromList("IgnoreTabsInRecipes", tabName);
            ScreenController.Instance.InvalidateRecipeScreen();
        }

        private static void addToList(string list, string value)
        {
            if (Settings.Lists[list].Contains(value))
                return;

            Settings.Lists[list].Add(value);
            Settings.SaveLists();
        }

        private static void removeFromList(string list, string value)
        {
            if (!Settings.Lists[list].Contains(value))
                return;

            Settings.Lists[list].Remove(value);
            Settings.SaveLists();
        }
    }
}
