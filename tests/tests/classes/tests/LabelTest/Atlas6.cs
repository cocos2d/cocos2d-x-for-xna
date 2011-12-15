using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
namespace tests
{
    public class Atlas6 : AtlasDemo
    {
        public Atlas6()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelBMFont label = null;
            label = CCLabelBMFont.labelWithString("FaFeFiFoFu", "fonts/fnt/bitmapFontTest5");
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height / 2 + 50);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.labelWithString("fafefifofu", "fonts/fnt/bitmapFontTest5");
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height / 2);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.labelWithString("aeiou", "fonts/fnt/bitmapFontTest5");
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height / 2 - 50);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Rendering should be OK. Testing offset";
        }
    }
}
