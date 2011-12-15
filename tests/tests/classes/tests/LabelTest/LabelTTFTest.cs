using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelTTFTest : AtlasDemo
    {
        public LabelTTFTest()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelTTF left = CCLabelTTF.labelWithString("align left", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentLeft, "Arial", 32);
            CCLabelTTF center = CCLabelTTF.labelWithString("align center", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentCenter, "Arial", 32);
            CCLabelTTF right = CCLabelTTF.labelWithString("align right", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentRight, "Arial", 32);

            left.position = new CCPoint(s.width / 2, 200);
            center.position = new CCPoint(s.width / 2, 150);
            right.position = new CCPoint(s.width / 2, 100);

            addChild(left);
            addChild(center);
            addChild(right);
        }

        public override string title()
        {
            return "Testing CCLabelTTF";
        }
        public override string subtitle()
        {
            return "You should see 3 labels aligned left, center and right";
        }
    }
}
