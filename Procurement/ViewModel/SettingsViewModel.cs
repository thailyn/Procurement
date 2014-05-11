﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using POEApi.Model;
using Procurement.View;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using System.Windows;

namespace Procurement.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsView view;

        private string currentCharacter;
        private string currentLeague;
        private bool isBusy;
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

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
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
            isBusy = false;
        }

        public void AddDownloadLeague(string leagueName)
        {
            if (!Settings.Lists["MyLeagues"].Contains(leagueName))
            {
                Settings.Lists["MyLeagues"].Add(leagueName);
                Settings.Save();
            }
        }

        public void RemoveDownloadLeague(string leagueName)
        {
            if (Settings.Lists["MyLeagues"].Contains(leagueName))
            {
                Settings.Lists["MyLeagues"].Remove(leagueName);
                Settings.Save();
            }
        }

        public void AddDownloadCharacter(string characterName)
        {
            if (!Settings.Lists["MyCharacters"].Contains(characterName))
            {
                Settings.Lists["MyCharacters"].Add(characterName);
                Settings.Save();
            }
        }

        public void RemoveDownloadCharacter(string characterName)
        {
            if (Settings.Lists["MyCharacters"].Contains(characterName))
            {
                Settings.Lists["MyCharacters"].Remove(characterName);
                Settings.Save();
            }
        }

        private void refreshCharacters()
        {
            this.Characters = ApplicationState.Model.GetCharacters().Where(c => c.League == CurrentLeague).Select(c => c.Name).ToList();
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Characters"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
