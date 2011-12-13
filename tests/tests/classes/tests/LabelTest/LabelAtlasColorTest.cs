using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelAtlasColorTest : AtlasDemo
    {
        //ccTime m_time;
        float m_time;
        ccColor3B ccRED = new ccColor3B
        {
            r = 255,
            g = 0,
            b = 0
        };

        public LabelAtlasColorTest()
        {
            CCLabelAtlas label1 = CCLabelAtlas.labelWithString("123 Test", "fonts/fnt/images/tuffy_bold_italic-charmap", 48, 64, ' ');
            addChild(label1, 0, (int)TagSprite.kTagSprite1);
            label1.position = new CCPoint(10, 100);
            label1.Opacity = 200;

            CCLabelAtlas label2 = CCLabelAtlas.labelWithString("0123456789", "fonts/fnt/images/tuffy_bold_italic-charmap", 48, 64, ' ');
            addChild(label2, 0, (int)TagSprite.kTagSprite2);
            label2.position = new CCPoint(10, 200);
            //label2.setColor( ccRED );
            label2.Color = ccRED;

            CCActionInterval fade = CCFadeOut.actionWithDuration(1.0f);
            CCFiniteTimeAction fade_in = fade.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(fade, fade_in);
            CCAction repeat = CCRepeatForever.actionWithAction((CCActionInterval)seq);
            label2.runAction(repeat);

            m_time = 0;

            schedule(step); //:@selector(step:)];
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[12] = {0};
            string stepstring;

            //sprintf(string, "%2.2f Test", m_time);
            stepstring = string.Format("{0,2:f2} Test", m_time);
            //std::string string = std::string::stringWithFormat("%2.2f Test", m_time);
            CCLabelAtlas label1 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagSprite1);
            label1.setString(stepstring);

            CCLabelAtlas label2 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagSprite2);
            //sprintf(string, "%d", (int)m_time);
            stepstring = string.Format("{0} Test", m_time);
            label2.setString(stepstring);
        }

        public override string title()
        {
            return "CCLabelAtlas";
        }

        public override string subtitle()
        {
            return "Opacity + Color should work at the same time";
        }
    }
}
