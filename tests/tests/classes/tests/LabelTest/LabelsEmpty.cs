using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelsEmpty : AtlasDemo
    {
        public LabelsEmpty()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("", "fonts/fnt/bitmapFontTest3");
            addChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);
            label1.position = new CCPoint(s.width / 2, s.height - 100);

            // CCLabelTTF
            //CCLabelTTF label2 = CCLabelTTF.labelWithString("", "Arial", 24);
            //addChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            //label2.position = new CCPoint(s.width / 2, s.height / 2);

            // CCLabelAtlas
            CCLabelAtlas label3 = CCLabelAtlas.labelWithString("", "fonts/fnt/images/tuffy_bold_italic-charmap", 48, 64, ' ');
            addChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);
            label3.position = new CCPoint(s.width / 2, 0 + 100);

            base.schedule(updateStrings, 1.0f);

            setEmpty = false;
        }

        public void updateStrings(float dt)
        {
            CCLabelBMFont label1 = (CCLabelBMFont)getChildByTag((int)TagSprite.kTagBitmapAtlas1);
            //CCLabelTTF label2 = (CCLabelTTF)getChildByTag((int)TagSprite.kTagBitmapAtlas2);
            CCLabelAtlas label3 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagBitmapAtlas3);

            if (!setEmpty)
            {
                label1.setString("not empty");
                //label2.setString("not empty");
                label3.setString("hi");

                setEmpty = true;
            }
            else
            {
                label1.setString("");
                //label2.setString("");
                label3.setString("");

                setEmpty = false;
            }
        }

        public override string title()
        {
            return "Testing empty labels";
        }

        public override string subtitle()
        {
            return "3 empty labels: LabelAtlas, LabelTTF and LabelBMFont";
        }

        private bool setEmpty;
    }
}
