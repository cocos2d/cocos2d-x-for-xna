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
    public class CCTMXTilesetInfo : CCObject
    {
        public string m_sName;
        public int m_uFirstGid;
        public CCSize m_tTileSize;
        public int m_uSpacing;
        public int m_uMargin;
        //! filename containing the tiles (should be spritesheet / texture atlas)
        public string m_sSourceImage;
        //! size in pixels of the image
        public CCSize m_tImageSize;

        public CCTMXTilesetInfo()
        {
        }
        public CCRect rectForGID(int gid)
        {
            CCRect rect = new CCRect();
            rect.size = m_tTileSize;
            gid = gid - m_uFirstGid;
            int max_x = (int)((m_tImageSize.width - m_uMargin * 2 + m_uSpacing) / (m_tTileSize.width + m_uSpacing));
            //	int max_y = (imageSize.height - margin*2 + spacing) / (tileSize.height + spacing);
            rect.origin.x = (gid % max_x) * (m_tTileSize.width + m_uSpacing) + m_uMargin;
            rect.origin.y = (gid / max_x) * (m_tTileSize.height + m_uSpacing) + m_uMargin;
            return rect;
        }
    }
}
