using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Atlas5 : AtlasDemo
    {
        public Atlas5()
        {
            CCLabelBMFont label = CCLabelBMFont.labelWithString("abcdefg", "fonts/fnt/bitmapFontTest4");
            addChild(label);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            label.position = new CCPoint(s.width / 2, s.height / 2);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);
        }
        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing padding";
        }
    }
}
