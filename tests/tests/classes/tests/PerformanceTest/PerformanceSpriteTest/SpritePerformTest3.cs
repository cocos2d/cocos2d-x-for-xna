using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpritePerformTest3 : SpriteMainScene
    {
        public override void doTest(CCSprite sprite)
        {
            performanceRotationScale(sprite);
        }

        public override string title()
        {
            //char str[32] = {0};
            string str;
            //sprintf(str, "C (%d) scale + rot", subtestNumber);
            str = string.Format("C {0:D} scale + rot", subtestNumber);
            string strRet = str;
            return strRet;
        }

        private void performanceRotationScale(CCSprite pSprite)
        {
            Random random = new Random();
            CCSize size = CCDirector.sharedDirector().getWinSize();
            pSprite.position = new CCPoint((random.Next() % (int)size.width), (random.Next() % (int)size.height));
            pSprite.rotation = random.Next() * 360;
            pSprite.scale = random.Next() * 2;
        }
    }
}
