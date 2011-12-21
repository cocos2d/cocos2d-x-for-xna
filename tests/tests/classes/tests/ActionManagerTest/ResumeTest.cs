using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ResumeTest : ActionManagerTest
    {
        string s_pPathGrossini = "Images/grossini";
        
        public override string title() 
        {
            return "Resume Test";    
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF l = CCLabelTTF.labelWithString("Grossini only rotate/scale in 3 seconds", "Arial", 16);
            addChild(l);
            l.position = (new CCPoint(s.width / 2, 245));

            CCSprite pGrossini = CCSprite.spriteWithFile(s_pPathGrossini);
            addChild(pGrossini, 0, (int)KTag.kTagGrossini);
            pGrossini.position = new CCPoint(s.width / 2, s.height / 2);

            pGrossini.runAction(CCScaleBy.actionWithDuration(2, 2));

            CCActionManager.sharedManager().pauseTarget(pGrossini);
            pGrossini.runAction(CCRotateBy.actionWithDuration(2, 360));

            this.schedule(resumeGrossini, 3.0f);
        }

        public void resumeGrossini(float time)
        {
            this.unschedule(resumeGrossini);

            CCNode pGrossini = getChildByTag((int)KTag.kTagGrossini);
            CCActionManager.sharedManager().resumeTarget(pGrossini);
        }
    }
}
