using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;

namespace tests
{
    public class IntervalLayer : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";

        protected CCLabelBMFont m_label0;
        protected CCLabelBMFont m_label1;
        protected CCLabelBMFont m_label2;
        protected CCLabelBMFont m_label3;
        protected CCLabelBMFont m_label4;

        float m_time0, m_time1, m_time2, m_time3, m_time4;

        public IntervalLayer()
        {
            m_time0 = m_time1 = m_time2 = m_time3 = m_time4 = 0.0f;

            CCSize s = CCDirector.sharedDirector().getWinSize();
            // sun
            //CCParticleSystem sun = CCParticleSun.node();
            //sun.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
            //sun.position = (new CCPoint(s.width - 32, s.height - 32));

            ////sun.setTotalParticles(130);
            //sun.Life = (0.6f);
            //this.addChild(sun);

            // timers
            m_label0 = CCLabelBMFont.labelWithString("0", "fonts/fnt/bitmapFontTest4");
            m_label1 = CCLabelBMFont.labelWithString("0", "fonts/fnt/bitmapFontTest4");
            m_label2 = CCLabelBMFont.labelWithString("0", "fonts/fnt/bitmapFontTest4");
            m_label3 = CCLabelBMFont.labelWithString("0", "fonts/fnt/bitmapFontTest4");
            m_label4 = CCLabelBMFont.labelWithString("0", "fonts/fnt/bitmapFontTest4");

            base.scheduleUpdate();
            schedule(step1);
            schedule(step2, 0);
            schedule(step3, 1.0f);
            schedule(step4, 2.0f);

            m_label1.position = new CCPoint(s.width * 2 / 6, s.height / 2);
            m_label2.position = new CCPoint(s.width * 3 / 6, s.height / 2);
            m_label3.position = new CCPoint(s.width * 4 / 6, s.height / 2);
            m_label4.position = new CCPoint(s.width * 5 / 6, s.height / 2);

            addChild(m_label0);
            addChild(m_label1);
            addChild(m_label2);
            addChild(m_label3);
            addChild(m_label4);

            // Sprite
            CCSprite sprite = CCSprite.spriteWithFile(s_pPathGrossini);
            sprite.position = new CCPoint(40, 50);

            CCJumpBy jump = CCJumpBy.actionWithDuration(3, new CCPoint(s.width - 80, 0), 50, 4);

            addChild(sprite);
            sprite.runAction(CCRepeatForever.actionWithAction(
                                                                    (CCActionInterval)(CCSequence.actions(jump, jump.reverse()))
                                                                )
                             );
            // pause button
            CCMenuItem item1 = CCMenuItemFont.itemFromString("Pause", this, onPause);
            CCMenu menu = CCMenu.menuWithItems(item1);
            menu.position = new CCPoint(s.width / 2, s.height - 50);

            addChild(menu);
        }

        public void onPause(CCObject pSender)
        {
            if (CCDirector.sharedDirector().isPaused)
                CCDirector.sharedDirector().resume();
            else
                CCDirector.sharedDirector().pause();
        }

        public void step1(float dt)
        {
            m_time1 += dt;

            string str;
            str = string.Format("{0,3:f1}", m_time1);
            m_label1.setString(str);
        }
        public void step2(float dt)
        {
            m_time2 += dt;

            string str;
            str = string.Format("{0,3:f1}", m_time2);
            m_label2.setString(str);
        }
        public void step3(float dt)
        {
            m_time3 += dt;
            string str;
            str = string.Format("{0,3:f1}", m_time3);
            m_label3.setString(str);
        }
        public void step4(float dt)
        {
            m_time4 += dt;
            string str;
            str = string.Format("{0,3:f1}", m_time4);
            m_label4.setString(str);
        }
        
        public new void update(float dt)
        {
            m_time0 += dt;
            string str;
            str = string.Format("{0,3:f1}", m_time0);
            m_label0.setString(str);
        }

        //CREATE_NODE(IntervalLayer);
    }
}
