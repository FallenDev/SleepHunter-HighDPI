﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

using SleepHunterIO.Common;

namespace SleepHunter.Settings
{
  [Serializable]
  public sealed class UserSetting : ObservableObject
  {
    string key;
    string displayText;
    object value;

    [XmlAttribute("Key")]
    public string Key
    {
      get { return key; }
      set { SetProperty(ref key, value); }
    }

    [XmlIgnore]
    public string DisplayText
    {
      get { return displayText; }
      set { SetProperty(ref displayText, value); }
    }

    [XmlAttribute("Value")]
    [DefaultValue(null)]
    public object Value
    {
      get { return value; }
      set { SetProperty(ref this.value, value); }
    }

    public UserSetting()
    { }

    public UserSetting(string key, string displayText, object value = null)
    {
      this.key = key;
      this.displayText = displayText;
      this.value = value;
    }

    public override string ToString()
    {
      return DisplayText ?? this.Key;
    }
  }
}
