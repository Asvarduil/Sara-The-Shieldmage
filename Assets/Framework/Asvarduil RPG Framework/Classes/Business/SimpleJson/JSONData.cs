using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleJSON
{
    public class JSONData : JSONNode
    {
        #region Variables / Properties

        static Regex m_Regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        private JSONBinaryTag m_Type = JSONBinaryTag.String;
        private string m_Data;

        public override string Value
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        #endregion Variables / Properties

        #region JSON Data Set Methods

        public JSONData(string aData)
        {
            m_Data = aData;

            // check for number
            if (m_Regex.IsMatch(m_Data))
                m_Type = JSONBinaryTag.Number;
            else
                m_Type = JSONBinaryTag.String;

        }

        public JSONData(float aData)
        {
            AsFloat = aData;
        }

        public JSONData(double aData)
        {
            AsDouble = aData;
        }

        public JSONData(bool aData)
        {
            AsBool = aData;
        }

        public JSONData(int aData)
        {
            AsInt = aData;
        }

        #endregion JSON Data Set Methods

        #region Serialization

        public override string ToString()
        {
            if (m_Type == JSONBinaryTag.String)
                return "\"" + Escape(m_Data) + "\"";
            else
                return Escape(m_Data);
        }

        public override string ToString(string aPrefix)
        {
            if (m_Type == JSONBinaryTag.String)
                return "\"" + Escape(m_Data) + "\"";
            else
                return Escape(m_Data);
        }

        public override void Serialize(System.IO.BinaryWriter aWriter)
        {
            var tmp = new JSONData("");

            tmp.AsInt = AsInt;
            if (tmp.m_Data == this.m_Data)
            {
                aWriter.Write((byte)JSONBinaryTag.IntValue);
                aWriter.Write(AsInt);
                return;
            }
            tmp.AsFloat = AsFloat;
            if (tmp.m_Data == this.m_Data)
            {
                aWriter.Write((byte)JSONBinaryTag.FloatValue);
                aWriter.Write(AsFloat);
                return;
            }
            tmp.AsDouble = AsDouble;
            if (tmp.m_Data == this.m_Data)
            {
                aWriter.Write((byte)JSONBinaryTag.DoubleValue);
                aWriter.Write(AsDouble);
                return;
            }

            tmp.AsBool = AsBool;
            if (tmp.m_Data == this.m_Data)
            {
                aWriter.Write((byte)JSONBinaryTag.BoolValue);
                aWriter.Write(AsBool);
                return;
            }
            aWriter.Write((byte)JSONBinaryTag.Value);
            aWriter.Write(m_Data);
        }

        #endregion Serialization
    }
}
