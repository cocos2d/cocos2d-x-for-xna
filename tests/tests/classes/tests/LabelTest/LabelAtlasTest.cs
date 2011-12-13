using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelAtlasTest : AtlasDemo
    {
        //ccTime m_time;
        float m_time;

        public LabelAtlasTest()
        {
            m_time = 0;

            CCLabelAtlas label1 = CCLabelAtlas.labelWithString("123 Test", "fonts/fnt/images/tuffy_bold_italic-charmap", 48, 64, ' ');
            addChild(label1, 0, (int)TagSprite.kTagSprite1);
            label1.position = new CCPoint(10, 100);
            label1.Opacity = 200;

            CCLabelAtlas label2 = CCLabelAtlas.labelWithString("0123456789", "fonts/fnt/images/tuffy_bold_italic-charmap", 48, 64, ' ');
            addChild(label2, 0, (int)TagSprite.kTagSprite2);
            label2.position = new CCPoint(10, 200);
            label2.Opacity = 32;

            schedule(step);
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[12] = {0};
            string Stepstring;

            //sprintf(Stepstring, "%2.2f Test", m_time);
            Stepstring = string.Format("{0,2:f2} Test", m_time);
            //Stepstring.format("%2.2f Test", m_time);

            CCLabelAtlas label1 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagSprite1);
            label1.setString(Stepstring);

            CCLabelAtlas label2 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagSprite2);
            //sprintf(Stepstring, "%d", (int)m_time);
            Stepstring = m_time.ToString();
            //Stepstring.format("%d", (int)m_time);
            label2.setString(Stepstring);
        }

        public override string title()
        {
            return "LabelAtlas";
        }

        public override string subtitle()
        {
            return "Updating label should be fast";
        }

    }
}
