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
using cocos2d;

namespace tests
{
    public class MenuLayer2 : CCLayer
    {
        string s_PlayNormal = "Images/btn-play-normal";
        string s_PlaySelect = "Images/btn-play-selected";
        string s_HighNormal = "Images/btn-highscores-normal";
        string s_HighSelect = "Images/btn-highscores-selected";
        string s_AboutNormal = "Images/btn-about-normal";
        string s_AboutSelect = "Images/btn-about-selected";


        protected CCPoint m_centeredMenu;
        protected bool m_alignedH;

        protected void alignMenusH()
        {
            for (int i = 0; i < 2; i++)
            {
                CCMenu menu = new CCMenu();// (CCMenu)getChildByTag(100 + i);
                menu.position = m_centeredMenu;
                if (i == 0)
                {
                    // TIP: if no padding, padding = 5
                    menu.alignItemsHorizontally();
                    CCPoint p = menu.position;
                    menu.position = new CCPoint(p.x + 0, p.y + 30);

                }
                else
                {
                    // TIP: but padding is configurable
                    menu.alignItemsHorizontallyWithPadding(40);
                    CCPoint p = menu.position;
                    menu.position = new CCPoint(p.x, p.y + 30);
                }
            }
        }
        protected void alignMenusV()
        {
            for (int i = 0; i < 2; i++)
            {
                CCMenu menu = (CCMenu)getChildByTag(100 + i);
                menu.position = m_centeredMenu;
                if (i == 0)
                {
                    // TIP: if no padding, padding = 5
                    menu.alignItemsVertically();
                    CCPoint p = menu.position;
                    menu.position = new CCPoint(p.x + 100, p.y);
                }
                else
                {
                    // TIP: but padding is configurable
                    menu.alignItemsVerticallyWithPadding(40);
                    CCPoint p = menu.position;
                    menu.position = new CCPoint(p.x - 100, p.y);
                }
            }
        }

        public MenuLayer2()
        {
            for (int i = 0; i < 2; i++)
            {
                CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(s_PlayNormal, s_PlaySelect, this, this.menuCallback);
                CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(s_HighNormal, s_HighSelect, this, this.menuCallbackOpacity);
                CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(s_AboutNormal, s_AboutSelect, this, this.menuCallbackAlign);

                item1.scaleX = 1.5f;
                item2.scaleX = 0.5f;
                item3.scaleX = 0.5f;

                CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

                menu.tag = (int)kTag.kTagMenu;

                addChild(menu, 0, 100 + i);

                m_centeredMenu = menu.position;
            }

            m_alignedH = true;
            alignMenusH();
        }
        public void menuCallback(CCObject pSender)
        {
            //CCLayerMultiplex m = m_pParent as CCLayerMultiplex;
            //m.switchTo(0);
            //((CCLayerMultiplex)m_pParent).switchTo(0);
        }
        public void menuCallbackOpacity(CCObject pSender)
        {
            CCMenu menu = (CCMenu)(((CCNode)(pSender)).parent);
            //GLubyte opacity = menu->getOpacity();
            //if (opacity == 128)
            //    menu->setOpacity(255);
            //else
            //    menu->setOpacity(128);	 
        }
        public void menuCallbackAlign(CCObject pSender)
        {
            m_alignedH = !m_alignedH;

            if (m_alignedH)
                alignMenusH();
            else
                alignMenusV();
        }
    }
}
