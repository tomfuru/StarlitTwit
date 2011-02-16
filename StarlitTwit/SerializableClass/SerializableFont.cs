using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Xml;

namespace StarlitTwit
{
    /// <summary>
    /// XmlSerializeを可能にしたFontです．
    /// </summary>
    [XmlRoot("font")]
    public class SerializableFont :IXmlSerializable
    {
        /// <summary>フォント</summary>
        private Font _font = SettingsData.DEFAULT_FONT_TEXT;

        private SerializableFont()
        { }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement) { return; }

            FontConverter fconv = new FontConverter();
            if (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
                this._font = (Font)fconv.ConvertFromString(reader.ReadString());
            }
            reader.MoveToContent();
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            FontConverter fconv = new FontConverter();
            writer.WriteString(fconv.ConvertToString(_font));
        }

        //-------------------------------------------------------------------------------
        #region SerializableFont->Font implicit キャスト
        //-------------------------------------------------------------------------------
        //
        public static implicit operator Font(SerializableFont sf)
        {
            return sf._font;
        }
        #endregion (SerializableFont->Font)
        //-------------------------------------------------------------------------------
        #region Font->SerializableFont implicit キャスト
        //-------------------------------------------------------------------------------
        //
        public static implicit operator SerializableFont(Font font)
        {
            if (font == null) { throw new InvalidCastException(); }

            return new SerializableFont() { _font = font };
        }
        #endregion (Font->SerializableFont)
    }
}
