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
            CCFileData data = new CCFileData(pszFile, "rt");
            ulong size = data.Size;
            byte[] pBuffer = (byte[])data.Buffer;

            if (pBuffer == null)
            {
                return false;
            }
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

        public static void textHandler(object ctx, string name, int len)
        {
            ((CCSAXParser)(ctx)).m_pDelegator.textHandler(ctx, name, len);
        }
    }
}
