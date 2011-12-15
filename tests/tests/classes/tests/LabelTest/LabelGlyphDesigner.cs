using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelGlyphDesigner : AtlasDemo
    {
        public LabelGlyphDesigner()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLayerColor layer = CCLayerColor.layerWithColor(new ccColor4B(128, 128, 128, 255));
            addChild(layer, -10);

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("Testing Glyph Designer", "fonts/fnt/futura-48");
            addChild(label1);
            label1.position = new CCPoint(s.width / 2, s.height / 2);
        }

        public override string title()
        {
            return "Testing Glyph Designer";
        }

        public override string subtitle()
        {
            return "You should see a font with shawdows and outline";
        }
    }
}
