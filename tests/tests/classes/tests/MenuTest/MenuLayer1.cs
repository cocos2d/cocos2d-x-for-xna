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
using System.Diagnostics;
using cocos2d.menu_nodes;

namespace tests
{
    public class MenuLayer1 : CCLayer
    {
        protected CCMenuItemLabel m_disabledItem;
        string s_SendScore = "Images/SendScoreButton";
        string s_MenuItem = "Images/menuitemsprite";
        string s_PressSendScore = "Images/SendScoreButtonPressed";

        public MenuLayer1()
        {
            CCMenuItemFont.FontSize = 30;
            CCMenuItemFont.FontName = "Arial";
            base.isTouchEnabled = true;
            // Font Item

            CCSprite spriteNormal = CCSprite.spriteWithFile(s_MenuItem, new CCRect(0, 23 * 2, 115, 23));
            CCSprite spriteSelected = CCSprite.spriteWithFile(s_MenuItem, new CCRect(0, 23 * 1, 115, 23));
            CCSprite spriteDisabled = CCSprite.spriteWithFile(s_MenuItem, new CCRect(0, 23 * 0, 115, 23));

            CCMenuItemSprite item1 = CCMenuItemSprite.itemFromNormalSprite(spriteNormal, spriteSelected, spriteDisabled, this, this.menuCallback);

            // Image Item
            CCMenuItem item2 = CCMenuItemImage.itemFromNormalImage(s_SendScore, s_PressSendScore, this, this.menuCallback2);

            // Label Item (LabelAtlas)
            CCLabelAtlas labelAtlas = CCLabelAtlas.labelWithString("0123456789", "Images/fps_images", 16, 24, '.');
            CCMenuItemLabel item3 = CCMenuItemLabel.itemWithLabel(labelAtlas, this, this.menuCallbackDisabled);
            item3.DisabledColor = new ccColor3B(32, 32, 64);
            item3.Color = new ccColor3B(200, 200, 255);

            // Font Item
            CCMenuItemFont item4 = CCMenuItemFont.itemFromString("I toggle enable items", this, this.menuCallbackEnable);

            item4.FontSizeObj = 20;
            item4.FontNameObj = "Arial";

            // Label Item (CCLabelBMFont)
            CCLabelBMFont label = CCLabelBMFont.labelWithString("configuration", "fonts/fnt/bitmapFontTest3");
            CCMenuItemLabel item5 = CCMenuItemLabel.itemWithLabel(label, this, this.menuCallbackConfig);
            

            // Testing issue #500
            item5.scale = 0.8f;

            // Font Item
            CCMenuItemFont item6 = CCMenuItemFont.itemFromString("Quit", this, this.onQuit);

            CCActionInterval color_action = CCTintBy.actionWithDuration(0.5f, 0, -255, -255);
            CCActionInterval color_back = (CCActionInterval)color_action.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(color_action, color_back);
            item6.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq));

            CCMenu menu = CCMenu.menuWithItems(item1, item2 ,item3, item4, item5, item6);
            menu.alignItemsVertically();

            // elastic effect
            CCSize s = CCDirector.sharedDirector().getWinSize();
            int i = 0;
            CCNode child;
            List<CCNode> pArray = menu.children;
            CCObject pObject = null;
            if (pArray.Count > 0)
            {
                for (int j = 0; j < pArray.Count; j++)
                {
                    pObject = pArray[j];
                    if (pObject == null)

                        break;
                    child = (CCNode)pObject;
                    CCPoint dstPoint = child.position;
                    int offset = (int)(s.width / 2 + 50);
                    if (i % 2 == 0)
                        offset = -offset;

                    child.position = new CCPoint(dstPoint.x + offset, dstPoint.y);
                    child.runAction(CCEaseElasticOut.actionWithAction(CCMoveBy.actionWithDuration(2, new CCPoint(dstPoint.x - offset, 0)), 0.35f));
                    i++;

                }
            }
            m_disabledItem = item3;
            m_disabledItem.Enabled = false;

            addChild(menu);
        }


        public override void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, -128 + 1, true);
        }
        public override bool ccTouchBegan(CCTouch touch, CCEvent pEvent)
        {
            return true;
        }

        public override void ccTouchEnded(CCTouch touch, CCEvent pEvent)
        {
        }

        public override void ccTouchCancelled(CCTouch touch, CCEvent pEvent)
        {
        }

        public override void ccTouchMoved(CCTouch touch, CCEvent pEvent)
        {
        }

        public void allowTouches(float dt)
        {
            CCTouchDispatcher.sharedDispatcher().setPriority(-128 + 1, this);
            base.unsheduleAllSelectors();
            Debug.WriteLine("TOUCHES ALLOWED AGAIN");
        }
        public void menuCallback(CCObject pSender)
        {
            ((CCLayerMultiplex)m_pParent).switchTo(1);
        }
        public void menuCallbackConfig(CCObject pSender)
        {
            ((CCLayerMultiplex)m_pParent).switchTo(3);
        }
        public void menuCallbackDisabled(CCObject pSender)
        {
            // hijack all touch events for 5 seconds
            CCTouchDispatcher.sharedDispatcher().setPriority(-128 - 1, this);
            base.schedule(this.allowTouches, 5.0f);
            Debug.WriteLine("TOUCHES DISABLED FOR 5 SECONDS");
        }
        public void menuCallbackEnable(CCObject pSender)
        {
            m_disabledItem.Enabled = !m_disabledItem.Enabled;
        }
        public void menuCallback2(CCObject pSender)
        {
            (m_pParent as CCLayerMultiplex).switchTo(2);
        }
        public void onQuit(CCObject pSender)
        {
            //[[Director sharedDirector] end];
            //getCocosApp()->exit();
        }
    }
}
