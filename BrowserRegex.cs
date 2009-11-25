using System;
using System.Collections.Generic;
using System.Configuration;

namespace Browser_Chooser
{
    [Serializable]
    public class BrowserRegex
    {
        public string Regex { get; set; }
        public string Browser { get; set; }
    }

    internal class BrowserRegexSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public List<BrowserRegex> BrowserRegexen
        {
            get { return (List<BrowserRegex>)this["BrowserRegexen"]; }
            set { this["BrowserRegexen"] = value; }
        }
    }
}
