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
using cocos2d.menu_nodes;

namespace tests
{
    public class MenuLayer3 : CCLayer
    {
        protected CCMenuItem m_disabledItem;

        string s_MenuItem = "Images/menuitemsprite";
        public MenuLayer3()
        {
            CCMenuItemFont.FontName = "Marker Felt";
            CCMenuItemFont.FontSize = 28;

            CCLabelBMFont label = CCLabelBMFont.labelWithString("Enable AtlasItem", "fonts/bitmapFontTest3.fnt");
            CCMenuItemLabel item1 = CCMenuItemLabel.itemWithLabel(label, this, this.menuCallback2);
            CCMenuItemFont item2 = CCMenuItemFont.itemFromString("--- Go Back ---", this, this.menuCallback);

            CCSprite spriteNormal = CCSprite.spriteWithFile(s_MenuItem, new CCRect(0, 23 * 2, 115, 23));
            CCSprite spriteSelected = CCSprite.spriteWithFile(s_MenuItem, new CCRect(0, 23 * 1, 115, 23));
            CCSprite spriteDisabled = CCSprite.spriteWithFile(s_MenuItem, new CCRect(0, 23 * 0, 115, 23));


            CCMenuItemSprite item3 = CCMenuItemSprite.itemFromNormalSprite(spriteNormal, spriteSelected, spriteDisabled, this, this.menuCallback3);
            m_disabledItem = item3;
            m_disabledItem.Enabled = false;

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3, null);
            menu.position = new CCPoint(0, 0);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            item1.position = new CCPoint(s.width / 2 - 150, s.height / 2);
            item2.position = new CCPoint(s.width / 2 - 200, s.height / 2);
            item3.position = new CCPoint(s.width / 2, s.height / 2 - 100);
            CCJumpBy jump = CCJumpBy.actionWithDuration(3, new CCPoint(400, 0), 50, 4);
            item2.runAction(CCRepeatForever.actionWithAction(
                                        (CCActionInterval)(CCSequence.actions(jump, jump.reverse(), null))
                                        )
                            );
            CCActionInterval spin1 = CCRotateBy.actionWithDuration(3, 360);
            CCActionInterval spin2 = (CCActionInterval)(spin1.copy());
            CCActionInterval spin3 = (CCActionInterval)(spin1.copy());

            item1.runAction(CCRepeatForever.actionWithAction(spin1));
            item2.runAction(CCRepeatForever.actionWithAction(spin2));
            item3.runAction(CCRepeatForever.actionWithAction(spin3));

            addChild(menu);
        }
        public void menuCallback(CCObject pSender)
        {
            ((CCLayerMultiplex)m_pParent).switchTo(0);
        }
        public void menuCallback2(CCObject pSender)
        {
            //UXLOG("Label clicked. Toogling AtlasSprite");
            m_disabledItem.Enabled = !m_disabledItem.Enabled;
            m_disabledItem.stopAllActions();
        }
        public void menuCallback3(CCObject pSender)
        {
            //UXLOG("MenuItemSprite clicked");
        }
    }
}
