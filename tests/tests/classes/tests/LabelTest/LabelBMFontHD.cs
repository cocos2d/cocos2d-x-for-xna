using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelBMFontHD : AtlasDemo
    {
        public LabelBMFontHD()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("TESTING RETINA DISPLAY", "fonts/fnt/konqa32");
            addChild(label1);
            label1.position = new CCPoint(s.width / 2, s.height / 2);
        }

        public override string title()
        {
            return "Testing Retina Display BMFont";
        }

        public override string subtitle()
        {
            return "loading arista16 or arista16-hd";
        }
    }
}
