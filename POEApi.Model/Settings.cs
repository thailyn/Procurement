﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using POEApi.Infrastructure;
using System;

namespace POEApi.Model
{
    public static class Settings
    {
        private const string location = "Settings.xml";
        public static Dictionary<OrbType, CurrencyRatio> CurrencyRatios { get; private set; }
        public static Dictionary<string, string> UserSettings { get; private set; }
        public static Dictionary<string, string> ProxySettings { get; private set; }
        public static Dictionary<string, List<string>> Lists { get; private set; }
        public static Dictionary<int, string> Buyouts { get; private set; }
        private static XElement originalDoc;

        static Settings()
        {
            originalDoc = XElement.Load(location);
            CurrencyRatios = originalDoc.Elements("Ratios").Descendants().ToDictionary(orb => orb.Attribute("type").GetEnum<OrbType>(), orb => new CurrencyRatio(orb.Attribute("type").GetEnum<OrbType>(), (double)orb.Attribute("orbamount"), (double)orb.Attribute("gcpamount")));

            UserSettings = getStandardNameValue("UserSettings");
            ProxySettings = getStandardNameValue("ProxySettings");

            Lists = new Dictionary<string, List<string>>();
            if (originalDoc.Element("Lists") != null)
                Lists = originalDoc.Element("Lists").Elements("List").ToDictionary(list => list.Attribute("name").Value, list => list.Elements("Item").Select(e => e.Attribute("value").Value).ToList());

            Buyouts = new Dictionary<int, string>();
            if (originalDoc.Element("Buyouts") != null)
                Buyouts = originalDoc.Element("Buyouts").Elements("Item").ToDictionary(list => (int)list.Attribute("id"), list => list.Attribute("value").Value);
        }

        private static Dictionary<string, string> getStandardNameValue(string root)
        {
            return originalDoc.Elements(root).Descendants().ToDictionary(setting => setting.Attribute("name").Value, setting => setting.Attribute("value").Value);
        }

        public static void Save()
        {
            foreach (string key in UserSettings.Keys)
            {
                XElement update = originalDoc.Elements("UserSettings").Descendants().First(x => x.Attribute("name").Value == key);
                if (UserSettings[key] != null)
                    update.Attribute("value").SetValue(UserSettings[key]);
            }

            foreach (OrbType key in CurrencyRatios.Keys)
            {
                XElement update = originalDoc.Elements("Ratios").Descendants().First(x => x.Attribute("type").Value == key.ToString());
                update.Attribute("orbamount").SetValue(CurrencyRatios[key].OrbAmount.ToString());
                update.Attribute("gcpamount").SetValue(CurrencyRatios[key].GCPAmount.ToString());
            }

            originalDoc.Element("Buyouts").RemoveNodes();
            foreach (int key in Buyouts.Keys)
            {
                XElement buyout = new XElement("Item", new XAttribute("id", key), new XAttribute("value", Buyouts[key]));
                originalDoc.Element("Buyouts").Add(buyout);
            }

            try
            {
                originalDoc.Save(location);
            }
            catch (Exception ex)
            {
                Logger.Log("Couldn't save settings: " + ex.ToString());
            }
        }
    }
}