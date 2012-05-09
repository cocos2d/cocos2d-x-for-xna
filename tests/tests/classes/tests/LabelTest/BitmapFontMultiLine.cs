using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class BitmapFontMultiLine : AtlasDemo
    {
        public BitmapFontMultiLine()
        {
            CCSize s;

            // Left
            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("Multi line\nLeft", "fonts/fnt/bitmapFontTest3");
            label1.anchorPoint = new CCPoint(0, 0);
            addChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

            s = label1.contentSize;

            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            CCLog.Log("content size: {0,0:2f}x{1,0:2f}", s.width, s.height);


            // Center
            CCLabelBMFont label2 = CCLabelBMFont.labelWithString("Multi line\nCenter", "fonts/fnt/bitmapFontTest3");
            label2.anchorPoint = new CCPoint(0.5f, 0.5f);
            addChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

            s = label2.contentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            CCLog.Log("content size: {0,0:2f}x{1,0:2f}", s.width, s.height);

            // right
            CCLabelBMFont label3 = CCLabelBMFont.labelWithString("Multi line\nRight\nThree lines Three", "fonts/fnt/bitmapFontTest3");
            label3.anchorPoint = new CCPoint(1, 1);
            addChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            s = label3.contentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);

            s = CCDirector.sharedDirector().getWinSize();
            label1.position = new CCPoint();
            label2.position = new CCPoint(s.width / 2, s.height / 2);
            label3.position = new CCPoint(s.width, s.height);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Multiline + anchor point";
        }
    }
}
