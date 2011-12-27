using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class AtlasBitmapColor : AtlasDemo
    {
        ccColor3B ccBLUE = new ccColor3B
        {
            r = 0,
            g = 0,
            b = 255
        };

        ccColor3B ccRED = new ccColor3B
        {
            r = 255,
            g = 0,
            b = 0
        };

        ccColor3B ccGREEN = new ccColor3B
        {
            r = 0,
            g = 255,
            b = 0
        };
        public AtlasBitmapColor()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelBMFont label = null;
            label = CCLabelBMFont.labelWithString("Blue", "fonts/fnt/bitmapFontTest5");
            label.Color = ccBLUE;
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height / 4);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.labelWithString("Red", "fonts/fnt/bitmapFontTest5");
            addChild(label);
            label.position = new CCPoint(s.width / 2, 2 * s.height / 4);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);
            label.Color = ccRED;

            label = CCLabelBMFont.labelWithString("G", "fonts/fnt/bitmapFontTest5");
            addChild(label);
            label.position = new CCPoint(s.width / 2, 3 * s.height / 4);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);
            label.Color = ccGREEN;
            label.setString("Green");
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing color";
        }
    }
}
