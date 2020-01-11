using System;
using System.Xml.Serialization;

using SleepHunterIO.Common;

namespace SleepHunter.Macro
{
  [Serializable]
  public sealed class SavedSkillState : ObservableObject
  {
    string skillName;

    [XmlAttribute("Name")]
    public string SkillName
    {
      get { return skillName; }
      set { SetProperty(ref skillName, value); }
    }

    public SavedSkillState() { }

    public SavedSkillState(string skillName)
    {
      this.skillName = skillName;
    }
  }
}
