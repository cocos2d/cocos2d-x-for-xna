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
using cocos2d.Framework;
using System.Xml.Linq;

namespace cocos2d
{
    public class CCSAXParser
    {
        //g:\cocos2d\cocos2d-1.0.1-x-0.9.2\cocos2d-1.0.1-x-0.9.2\cocos2dx\platform\ccsaxparser.h
        ICCSAXDelegator m_pDelegator;

        public CCSAXParser()
        {
            m_pDelegator = null;
        }

        public bool init(string pszEncoding)
        {
            // nothing to do
            return true;
        }
        
        public bool parse(string pszFile)
        {

            //CCFileData data = new CCFileData(pszFile, "rt");
            //ulong size = data.Size;
            //byte[] pBuffer = (byte[])data.Buffer;
            CCContent data = CCApplication.sharedApplication().content.Load<CCContent>(pszFile);
  
            if (data == null)
            {
                return false;
            }
            //Òª¸Éµôhttp
            string str = data.Content;
            XElement doc = XElement.Parse(str);
            List<string> atts1 = new List<string>();

            atts1 = (from item in doc.Attributes()
                    select item.Value.ToString()).ToList();

            List<string> atts2 = new List<string>();
            atts2 = (from item in doc.Attributes()
                     select item.Name.ToString()).ToList();

            List<string> atts = new List<string>();
            if (atts1.Count==atts2.Count)
            {
                for (int i = 0; i < atts1.Count; i++)
                {
                    atts.Add(atts2[i]);
                    atts.Add(atts1[i]);
                }
            }
            startElement(this, "map", atts.ToArray());

            List<string> atts3 = new List<string>();
            atts3 = (from item in doc.Descendants("tileset").LastOrDefault().Attributes()
                    select item.Name.ToString()).ToList();
            List<string> atts4 = new List<string>();
            atts4 = (from item in doc.Descendants("tileset").LastOrDefault().Attributes()
                     select item.Value.ToString()).ToList();

            List<string> atts5 = new List<string>();
            if (atts3.Count == atts4.Count)
            {
                for (int i = 0; i < atts3.Count; i++)
                {
                    atts5.Add(atts3[i]);
                    atts5.Add(atts4[i]);
                }
            }
            startElement(this, "tileset",atts5.ToArray());

            List<string> atts6 = new List<string>();
            atts6 = (from item in doc.Descendants("image").LastOrDefault().Attributes()
                     select item.Name.ToString()).ToList();
            List<string> atts7 = new List<string>();
            atts7 = (from item in doc.Descendants("image").LastOrDefault().Attributes()
                     select item.Value.ToString()).ToList();
            List<string> atts8 = new List<string>();
            if (atts6.Count == atts7.Count)
            {
                for (int i = 0; i < atts6.Count; i++)
                {
                    atts8.Add(atts6[i]);
                    atts8.Add(atts7[i]);
                }
            }
            startElement(this, "image", atts8.ToArray());

            List<string> atts20 = new List<string>();
            atts20 = (from item in doc.Descendants("layer").FirstOrDefault().Attributes()
                      select item.Name.ToString()).ToList();
            List<string> atts21 = new List<string>();
            atts21 = (from item in doc.Descendants("layer").FirstOrDefault().Attributes()
                      select item.Value.ToString()).ToList();
            List<string> atts22 = new List<string>();
            if (atts21.Count == atts20.Count)
            {
                for (int i = 0; i < atts21.Count; i++)
                {
                    atts22.Add(atts20[i]);
                    atts22.Add(atts21[i]);
                }
            }
            startElement(this, "layer", atts22.ToArray());

            List<string> atts9 = new List<string>();
            atts9 = (from item in doc.Descendants("data").FirstOrDefault().Attributes()
                     select item.Name.ToString()).ToList();
            List<string> atts10 = new List<string>();
            atts10 = (from item in doc.Descendants("data").FirstOrDefault().Attributes()
                     select item.Value.ToString()).ToList();
            List<string> atts11 = new List<string>();
            if (atts9.Count==atts10.Count)
            {
                for (int i = 0; i < atts10.Count; i++)
                {
                    atts11.Add(atts9[i]);
                    atts11.Add(atts10[i]);
                }
            }
            startElement(this, "data", atts11.ToArray());
             //str= UnicodeEncoding.UTF8.GetString(data.Date, 0, data.Date.Length);
            endElement(this ,"data",data);
            
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

        public static void endElement(object ctx, string name,object o)
        {
            ((CCSAXParser)(ctx)).m_pDelegator.endElement(o, name);
        }

        public static void textHandler(object ctx, string name, int len)
        {
            ((CCSAXParser)(ctx)).m_pDelegator.textHandler(ctx, name, len);
        }
    }
}
