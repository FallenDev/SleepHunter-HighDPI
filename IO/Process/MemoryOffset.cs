using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace SleepHunter.IO.Process
{
    [Serializable]
    public sealed class MemoryOffset
    {
        long offset;

        [XmlIgnore]
        public long Offset
        {
            get { return offset; }
            set
            {
                IsNegative = value < 0;
                offset = Math.Abs(value);
            }
        }

        [XmlAttribute("Value")]
        public string OffsetHex
        {
            get { return offset.ToString("X"); }

            set
            {

                if (long.TryParse(value, NumberStyles.HexNumber, null, out long parsedLong))
                    offset = parsedLong;
            }
        }

        [XmlAttribute("IsNegative")]
        [DefaultValue(false)]
        public bool IsNegative { get; set; }

        public MemoryOffset()
           : this(0) { }

        public MemoryOffset(long offset)
        {
            Offset = offset;
        }

        public override string ToString()
        {
            return OffsetHex;
        }
    }
}
