/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011      Fulcrum Mobile Network, Inc.

http://www.cocos2d-x.org
http://www.openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using cocos2d.Framework;
using System.IO;
using System.Xml.Linq;

namespace cocos2d
{
    public class CCSAXParser
    {
        ICCSAXDelegator m_pDelegator;

        public CCSAXParser()
        {

        }

        public bool init(string pszEncoding)
        {
            // nothing to do
            return true;
        }

        static List<string> ParseAttribute(XElement doc)
        {
            IEnumerable<XAttribute> attributes = doc.Attributes();
            List<string> atts = new List<string>();
            foreach (var item in attributes)
            {
                atts.Add(item.Name.ToString());
                atts.Add(item.Value);
            }
            return atts;
        }

        static void ForeachNode(XElement doc, object ctx)
        {
            List<string> atts = new List<string>();
            if (doc.Elements() != null)
            {
                foreach (var item in doc.Elements())
                {
                    atts = ParseAttribute(item);
                    startElement(ctx, item.Name.ToString(), atts.ToArray());
                    ForeachNode(item, ctx);
                }
            }
        }

        public bool parse(string pszFile)
        {
            CCContent data = CCApplication.sharedApplication().content.Load<CCContent>(pszFile);
            string str = data.Content;
            if (data == null)
            {
                return false;
            }

            TextReader textReader = new StringReader(str);
            XmlReader xmlReader = XmlReader.Create(textReader);
            int dataindex = 0;

            int Width = 0;
            int Height = 0; ;

            while (xmlReader.Read())
            {
                string name = xmlReader.Name;

                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:

                        if (name == "map")
                        {
                            Width = int.Parse(xmlReader.GetAttribute("width"));
                            Height = int.Parse(xmlReader.GetAttribute("height"));
                        }

                        if (xmlReader.HasAttributes)
                        {
                            string[] attrs = new string[xmlReader.AttributeCount * 2];
                            xmlReader.MoveToFirstAttribute();
                            int i = 0;
                            attrs[0] = xmlReader.Name;
                            attrs[1] = xmlReader.Value;
                            i += 2;

                            while (xmlReader.MoveToNextAttribute())
                            {
                                attrs[i] = xmlReader.Name;
                                attrs[i + 1] = xmlReader.Value;
                                i += 2;
                            }

                            //GZipStream

                            // Move the reader back to the element node.
                            xmlReader.MoveToElement();
                            startElement(this, name, attrs);
                        }
                        else
                        {
                            startElement(this, name, null);
                        }

                        if (name == "data")
                        {
                            int dataSize = (Width * Height * 4) + 1024;
                            var buffer = new byte[dataSize];
                            xmlReader.ReadElementContentAsBase64(buffer, 0, dataSize);

                            textHandler(this, buffer, buffer.Length);
                            endElement(this, name);
                        }

                        break;

                    case XmlNodeType.EndElement:
                        endElement(this, xmlReader.Name);
                        dataindex++;
                        break;

                    default:
                        break;
                }
            }



            //XElement doc = XElement.Parse(str);
            //List<string> atts = new List<string>();
            //atts = ParseAttribute(doc);
            //startElement(this, "map", atts.ToArray());
            //ForeachNode(doc, this);

            //endElement(this, "data", data);

#warning about xml
            /// * this initialize the library and check potential ABI mismatches
            /// * between the version it was compiled for and the actual shared
            /// * library used.
            //LIBXML_TEST_VERSION
            //xmlSAXHandler saxHandler;
            //memset( &saxHandler, 0, sizeof(saxHandler) );
            //// Using xmlSAXVersion( &saxHandler, 2 ) generate crash as it sets plenty of other pointers...
            //saxHandler.initialized = XML_SAX2_MAGIC;  // so we do this to force parsing as SAX2.
            //saxHandler.startElement = &CCSAXParser::startElement;
            //saxHandler.endElement = &CCSAXParser::endElement;
            //saxHandler.characters = &CCSAXParser::textHandler;

            //int result = xmlSAXUserParseMemory( &saxHandler, this, pBuffer, size );
            //if ( result != 0 )
            //{
            //    return false;
            //}
            ///*
            // * Cleanup function for the XML library.
            // */
            //xmlCleanupParser();
            ///*
            // * this is to debug memory for regression tests
            // */
            //xmlMemoryDump();
            return true;
        }

        public void setDelegator(ICCSAXDelegator pDelegator)
        {
            m_pDelegator = pDelegator;
        }

        public static void startElement(object ctx, string name, string[] atts)
        {
            ((CCSAXParser)(ctx)).m_pDelegator.startElement(ctx, name, atts);
        }

        public static void endElement(object ctx, string name)
        {
            ((CCSAXParser)(ctx)).m_pDelegator.endElement(ctx, name);
        }

        public static void textHandler(object ctx, byte[] ch, int len)
        {
            ((CCSAXParser)(ctx)).m_pDelegator.textHandler(ctx, ch, len);
        }
    }
}
