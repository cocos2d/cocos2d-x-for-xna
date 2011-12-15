using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelTTFMultiline : AtlasDemo
    {
        public LabelTTFMultiline()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelTTF center = CCLabelTTF.labelWithString("word wrap \"testing\" (bla0) bla1 'bla2' [bla3] (bla4) {bla5} {bla6} [bla7] (bla8) [bla9] 'bla0' \"bla1\"",
                new CCSize(s.width / 2, 200), CCTextAlignment.CCTextAlignmentCenter, "Arial", 32);
            center.position = new CCPoint(s.width / 2, 150);

            addChild(center);
        }

        public override string title()
        {
            return "Testing CCLabelTTF Word Wrap";
        }

        public override string subtitle()
        {
            return "Word wrap using CCLabelTTF";
        }
    }
}
