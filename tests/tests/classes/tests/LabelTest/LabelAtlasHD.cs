using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelAtlasHD : AtlasDemo
    {
        public LabelAtlasHD()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelAtlas label1 = CCLabelAtlas.labelWithString("TESTING RETINA DISPLAY", "fonts/fnt/images/larabie-16", 10, 20, 'A');
            label1.anchorPoint = new CCPoint(0.5f, 0.5f);

            addChild(label1);
            label1.position = new CCPoint(s.width / 2, s.height / 2);
        }

        public override string title()
        {
            return "LabelAtlas with Retina Display";
        }

        public override string subtitle()
        {
            return "loading larabie-16 / larabie-16-hd";
        }
    }
}
