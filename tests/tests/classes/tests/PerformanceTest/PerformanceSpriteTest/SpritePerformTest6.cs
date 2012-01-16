using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpritePerformTest6 : SpriteMainScene
    {

        public override void doTest(CCSprite sprite)
        {
            performanceActions(sprite);
        }

        public override string title()
        {
            //char str[32] = {0};
            string str;
            //sprintf(str, "F (%d) actions", subtestNumber);
            str = string.Format("F {0:D} actions", subtestNumber);
            string strRet = str;
            return strRet;
        }

        private void performanceActions(CCSprite pSprite)
        {
            Random random = new Random();
            CCSize size = CCDirector.sharedDirector().getWinSize();
            pSprite.position = new CCPoint((random.Next() % (int)size.width), (random.Next() % (int)size.height));

            float period = 0.5f + (random.Next() % 1000) / 500.0f;
            CCRotateBy rot = CCRotateBy.actionWithDuration(period, 360.0f * random.Next());
            CCActionInterval rot_back = null;
            CCAction permanentRotation = CCRepeatForever.actionWithAction((CCActionInterval)CCSequence.actions(rot, rot_back));
            pSprite.runAction(permanentRotation);

            float growDuration = 0.5f + (random.Next() % 1000) / 500.0f;
            CCActionInterval grow = CCScaleBy.actionWithDuration(growDuration, 0.5f, 0.5f);
            CCAction permanentScaleLoop = CCRepeatForever.actionWithAction((CCActionInterval)CCSequence.actionOneTwo(grow, grow.reverse()));
            pSprite.runAction(permanentScaleLoop);
        }
    }
}
