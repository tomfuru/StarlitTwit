using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace StarlitTwit
{
    /// <summary>
    /// XmlSerializeを可能にしたColorです．
    /// </summary>
    [XmlRoot("color")]
    public class SerializableColor : IXmlSerializable
    {
        private Color _color;

        private SerializableColor()
        { }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.IsEmptyElement) { return; }

            ColorConverter cconv = new ColorConverter();
            if (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
                this._color = (Color)cconv.ConvertFromString(reader.ReadString());
            }
            reader.MoveToContent();
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            ColorConverter cconv = new ColorConverter();
            writer.WriteString(cconv.ConvertToString(_color));
        }

        //-------------------------------------------------------------------------------
        #region SerializableColor->Color implicit キャスト
        //-------------------------------------------------------------------------------
        //
        public static implicit operator Color(SerializableColor sc)
        {
            return sc._color;
        }
        #endregion (SerializableColor->Color)
        //-------------------------------------------------------------------------------
        #region Color->SerializableColor implicit キャスト
        //-------------------------------------------------------------------------------
        //
        public static implicit operator SerializableColor(Color color)
        {
            return new SerializableColor() { _color = color };
        }
        #endregion (Color->SerializableColor)
    }
}
