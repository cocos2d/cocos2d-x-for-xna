using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Atlas4 : AtlasDemo
    {
        //ccTime m_time;
        float m_time;

        public Atlas4()
        {
            m_time = 0;

            // Upper Label
            CCLabelBMFont label = CCLabelBMFont.labelWithString("Bitmap Font Atlas", "fonts/fnt/bitmapFontTest");
            addChild(label);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            label.position = new CCPoint(s.width / 2, s.height / 2);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);


            CCSprite BChar = (CCSprite)label.getChildByTag(1);
            CCSprite FChar = (CCSprite)label.getChildByTag(7);
            CCSprite AChar = (CCSprite)label.getChildByTag(12);


            CCActionInterval rotate = CCRotateBy.actionWithDuration(2, 360);
            CCAction rot_4ever = CCRepeatForever.actionWithAction(rotate);

            CCActionInterval scale = CCScaleBy.actionWithDuration(2, 1.5f);
            CCFiniteTimeAction scale_back = scale.reverse();
            CCFiniteTimeAction scale_seq = CCSequence.actions(scale, scale_back);
            CCAction scale_4ever = CCRepeatForever.actionWithAction((CCActionInterval)scale_seq);

            CCActionInterval jump = CCJumpBy.actionWithDuration(0.5f, new CCPoint(), 60, 1);
            CCAction jump_4ever = CCRepeatForever.actionWithAction(jump);

            CCActionInterval fade_out = CCFadeOut.actionWithDuration(1);
            CCActionInterval fade_in = CCFadeIn.actionWithDuration(1);
            CCFiniteTimeAction seq = CCSequence.actions(fade_out, fade_in);
            CCAction fade_4ever = CCRepeatForever.actionWithAction((CCActionInterval)seq);

            BChar.runAction(rot_4ever);
            BChar.runAction(scale_4ever);
            FChar.runAction(jump_4ever);
            AChar.runAction(fade_4ever);


            // Bottom Label
            CCLabelBMFont label2 = CCLabelBMFont.labelWithString("00.0", "fonts/fnt/bitmapFontTest");
            addChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            label2.position = new CCPoint(s.width / 2.0f, 80);

            CCSprite lastChar = (CCSprite)label2.getChildByTag(3);
            lastChar.runAction((CCAction)(rot_4ever.copy()));

            //schedule( schedule_selector(Atlas4::step), 0.1f);
            base.schedule(step, 0.1f);
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[10] = {0};
            string Stepstring;
            //sprintf(string, "%04.1f", m_time);
            Stepstring = string.Format("{0,4:f1}", m_time);
            // 	std::string string;
            // 	string.format("%04.1f", m_time);

            CCLabelBMFont label1 = (CCLabelBMFont)getChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label1.setString(Stepstring);
        }

        public override void draw()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            //ccDrawLine(new CCPoint(0, s.height / 2), new CCPoint(s.width, s.height / 2));
            //ccDrawLine(new CCPoint(s.width / 2, 0), new CCPoint(s.width / 2, s.height));
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Using fonts as CCSprite objects. Some characters should rotate.";
        }
    }
}
