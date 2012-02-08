using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Atlas3 : AtlasDemo
    {
        //ccTime	m_time;
        float m_time;
        ccColor3B ccRED = new ccColor3B
        {
            r = 255,
            g = 0,
            b = 0
        };

        public Atlas3()
        {
            m_time = 0;

            CCLayerColor col = CCLayerColor.layerWithColor(new ccColor4B(128, 128, 128, 255));
            addChild(col, -10);

            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("Test", "fonts/fnt/bitmapFontTest2");

            // testing anchors
            label1.anchorPoint = new CCPoint(0, 0);
            addChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);
            CCActionInterval fade = CCFadeOut.actionWithDuration(1.0f);
            CCFiniteTimeAction fade_in = fade.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(fade, fade_in);
            CCAction repeat = CCRepeatForever.actionWithAction((CCActionInterval)seq);
            label1.runAction(repeat);


            // VERY IMPORTANT
            // color and opacity work OK because bitmapFontAltas2 loads a BMP image (not a PNG image)
            // If you want to use both opacity and color, it is recommended to use NON premultiplied images like BMP images
            // Of course, you can also tell XCode not to compress PNG images, but I think it doesn't work as expected
            CCLabelBMFont label2 = CCLabelBMFont.labelWithString("Test", "fonts/fnt/bitmapFontTest2");
            // testing anchors
            label2.anchorPoint = new CCPoint(0.5f, 0.5f);
            label2.Color = ccRED;
            addChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            label2.runAction((CCAction)(repeat.copy()));

            CCLabelBMFont label3 = CCLabelBMFont.labelWithString("Test", "fonts/fnt/bitmapFontTest2");
            // testing anchors
            label3.anchorPoint = new CCPoint(1, 1);
            addChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);


            CCSize s = CCDirector.sharedDirector().getWinSize();
            label1.position = new CCPoint();
            label2.position = new CCPoint(s.width / 2, s.height / 2);
            label3.position = new CCPoint(s.width, s.height);

            base.schedule(step);//:@selector(step:)];
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            string stepString;
            stepString = string.Format("{0,2:f2} Testj", m_time);

            CCLabelBMFont label1 = (CCLabelBMFont)getChildByTag((int)TagSprite.kTagBitmapAtlas1);
            label1.setString(stepString);

            CCLabelBMFont label2 = (CCLabelBMFont)getChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label2.setString(stepString);

            CCLabelBMFont label3 = (CCLabelBMFont)getChildByTag((int)TagSprite.kTagBitmapAtlas3);
            label3.setString(stepString);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing alignment. Testing opacity + tint";
        }

    }
}
