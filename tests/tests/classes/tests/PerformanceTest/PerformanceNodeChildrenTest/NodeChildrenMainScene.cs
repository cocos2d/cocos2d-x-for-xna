using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class NodeChildrenMainScene : CCScene
    {
        public virtual void initWithQuantityOfNodes(int nNodes)
        {
            //srand(time());
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // Title
            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 40);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 32);
            label.Color = new ccColor3B(255, 255, 40);

            // Subtitle
            string strSubTitle = subtitle();
            if (strSubTitle.Length > 0)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubTitle, "Thonburi", 16);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);
            }

            lastRenderedCount = 0;
            currentQuantityOfNodes = 0;
            quantityOfNodes = nNodes;

            CCMenuItemFont.FontSize = 65;
            CCMenuItemFont decrease = CCMenuItemFont.itemFromString(" - ", this, onDecrease);
            decrease.Color = new ccColor3B(0, 200, 20);
            CCMenuItemFont increase = CCMenuItemFont.itemFromString(" + ", this, onIncrease);
            increase.Color = new ccColor3B(0, 200, 20);

            CCMenu menu = CCMenu.menuWithItems(decrease, increase);
            menu.alignItemsHorizontally();
            menu.position = new CCPoint(s.width / 2, s.height / 2 + 15);
            addChild(menu, 1);

            CCLabelTTF infoLabel = CCLabelTTF.labelWithString("0 nodes", "Marker Felt", 30);
            infoLabel.Color = new ccColor3B(0, 200, 20);
            infoLabel.position = new CCPoint(s.width / 2, s.height / 2 - 15);
            addChild(infoLabel, 1, PerformanceNodeChildrenTest.kTagInfoLayer);

            NodeChildrenMenuLayer pMenu = new NodeChildrenMenuLayer(true, PerformanceNodeChildrenTest.TEST_COUNT, PerformanceNodeChildrenTest.s_nCurCase);
            addChild(pMenu);

            updateQuantityLabel();
            updateQuantityOfNodes();
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public virtual void updateQuantityOfNodes()
        {
            throw new NotFiniteNumberException();
        }

        public void onDecrease(CCObject pSender)
        {
            quantityOfNodes -= PerformanceNodeChildrenTest.kNodesIncrease;
            if (quantityOfNodes < 0)
                quantityOfNodes = 0;

            updateQuantityLabel();
            updateQuantityOfNodes();
        }

        public void onIncrease(CCObject pSender)
        {
            quantityOfNodes += PerformanceNodeChildrenTest.kNodesIncrease;
            if (quantityOfNodes > PerformanceNodeChildrenTest.kMaxNodes)
                quantityOfNodes = PerformanceNodeChildrenTest.kMaxNodes;

            updateQuantityLabel();
            updateQuantityOfNodes();
        }

        public void updateQuantityLabel()
        {
            if (quantityOfNodes != lastRenderedCount)
            {
                CCLabelTTF infoLabel = (CCLabelTTF)getChildByTag(PerformanceNodeChildrenTest.kTagInfoLayer);
                string str;
                //sprintf(str, "%u nodes", quantityOfNodes);
                str = string.Format("{u} nodes", quantityOfNodes);
                infoLabel.setString(str);

                lastRenderedCount = quantityOfNodes;
            }
        }

        public int getQuantityOfNodes()
        {
            return quantityOfNodes;
        }

        protected int lastRenderedCount;
        protected int quantityOfNodes;
        protected int currentQuantityOfNodes;
    }
}
