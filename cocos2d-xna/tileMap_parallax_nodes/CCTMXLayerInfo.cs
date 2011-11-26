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
    public class CCTMXLayerInfo : CCObject
    {
        protected Dictionary<string, string> m_pProperties;
        public virtual Dictionary<string, string> Properties
        {
            get { return m_pProperties; }
            set { m_pProperties = value; }
        }

        public string m_sName;
        public CCSize m_tLayerSize;
        public UInt32[] m_pTiles;
        public bool m_bVisible;
        public byte m_cOpacity;
        public bool m_bOwnTiles;
        public UInt32 m_uMinGID;
        public UInt32 m_uMaxGID;
        public CCPoint m_tOffset;

        public CCTMXLayerInfo()
        {
            m_sName = "";
            m_pTiles = null;
            m_bOwnTiles = true;
            m_uMinGID = 100000;
            m_uMaxGID = 0;
            m_tOffset = new CCPoint(0, 0);
            m_pProperties =new Dictionary<string,string>(); ;
        }
    }
}
