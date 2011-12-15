using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelTTFChinese : AtlasDemo
    {
        public LabelTTFChinese()
        {
            CCSize size = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF pLable = CCLabelTTF.labelWithString("Chinaese", "Arial", 30);
            pLable.position = new CCPoint(size.width / 2, size.height / 2);
            addChild(pLable);
        }

        public override string title()
        {
            return "Testing CCLabelTTF with Chinese character";
        }

        public override string subtitle()
        {
            return "You should see Chinese font";
        }
    }
}
