using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteMainScene : CCScene
    {

        public virtual string title()
        {
            return "No title";
        }

        public void initWithSubTest(int asubtest, int nNodes)
        {
            //srandom(0);

            subtestNumber = asubtest;
            m_pSubTest = new SubTest();
            m_pSubTest.initWithSubTest(asubtest, this);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            lastRenderedCount = 0;
            quantityNodes = 0;

            CCMenuItemFont.FontSize = 65;
            CCMenuItemFont decrease = CCMenuItemFont.itemFromString(" - ", this, onDecrease);
            decrease.Color = new ccColor3B(0, 200, 20);
            CCMenuItemFont increase = CCMenuItemFont.itemFromString(" + ", this, onIncrease);
            increase.Color = new ccColor3B(0, 200, 20);

            CCMenu menu = CCMenu.menuWithItems(decrease, increase);
            menu.alignItemsHorizontally();
            menu.position = new CCPoint(s.width / 2, s.height - 65);
            addChild(menu, 1);

            CCLabelTTF infoLabel = CCLabelTTF.labelWithString("0 nodes", "Marker Felt", 30);
            infoLabel.Color = new ccColor3B(0, 200, 20);
            infoLabel.position = new CCPoint(s.width / 2, s.height - 90);
            addChild(infoLabel, 1, PerformanceSpriteTest.kTagInfoLayer);

            // add menu
            SpriteMenuLayer pMenu = new SpriteMenuLayer(true, PerformanceSpriteTest.TEST_COUNT, PerformanceSpriteTest.s_nSpriteCurCase);
            addChild(pMenu, 1, PerformanceSpriteTest.kTagMenuLayer);

            // Sub Tests
            CCMenuItemFont.FontSize = 32;
            CCMenu pSubMenu = CCMenu.menuWithItems(null);
            for (int i = 1; i <= 9; ++i)
            {
                //char str[10] = {0};
                string str;
                //sprintf(str, "%d ", i);
                str = string.Format("{0:D}", i);
                CCMenuItemFont itemFont = CCMenuItemFont.itemFromString(str, this, testNCallback);
                itemFont.tag = i;
                pSubMenu.addChild(itemFont, 10);

                if (i <= 3)
                    itemFont.Color = new ccColor3B(200, 20, 20);
                else if (i <= 6)
                    itemFont.Color = new ccColor3B(0, 200, 20);
                else
                    itemFont.Color = new ccColor3B(0, 20, 200);
            }

            pSubMenu.alignItemsHorizontally();
            pSubMenu.position = new CCPoint(s.width / 2, 80);
            addChild(pSubMenu, 2);

            // add title label
            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 40);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 32);
            label.Color = new ccColor3B(255, 255, 40);

            while (quantityNodes < nNodes)
                onIncrease(this);
        }

        public void updateNodes()
        {
            if (quantityNodes != lastRenderedCount)
            {
                CCLabelTTF infoLabel = (CCLabelTTF)getChildByTag(PerformanceSpriteTest.kTagInfoLayer);
                //char str[16] = {0};
                string str;
                //sprintf(str, "%u nodes", quantityNodes);
                str = string.Format("{0:U} nodes", quantityNodes);
                infoLabel.setString(str);

                lastRenderedCount = quantityNodes;
            }
        }

        public void testNCallback(CCObject pSender)
        {
            subtestNumber = ((CCMenuItemFont)pSender).tag;
            SpriteMenuLayer pMenu = (SpriteMenuLayer)getChildByTag(PerformanceSpriteTest.kTagMenuLayer);
            pMenu.restartCallback(pSender);
        }

        public void onIncrease(CCObject pSender)
        {
            if (quantityNodes >= PerformanceSpriteTest.kMaxNodes)
                return;

            for (int i = 0; i < PerformanceSpriteTest.kNodesIncrease; i++)
            {
                CCSprite sprite = m_pSubTest.createSpriteWithTag(quantityNodes);
                doTest(sprite);
                quantityNodes++;
            }

            updateNodes();
        }

        public void onDecrease(CCObject pSender)
        {
            if (quantityNodes <= 0)
                return;

            for (int i = 0; i < PerformanceSpriteTest.kNodesIncrease; i++)
            {
                quantityNodes--;
                m_pSubTest.removeByTag(quantityNodes);
            }

            updateNodes();
        }

        public virtual void doTest(CCSprite sprite)
        {
            throw new NotFiniteNumberException();
        }

        public int getSubTestNum()
        { return subtestNumber; }

        public int getNodesNum()
        { return quantityNodes; }


        protected int lastRenderedCount;
        protected int quantityNodes;
        protected SubTest m_pSubTest;
        protected int subtestNumber;
    }
}
