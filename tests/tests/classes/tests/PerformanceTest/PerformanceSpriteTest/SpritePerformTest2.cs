using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpritePerformTest2 : SpriteMainScene
    {

        public override void doTest(CCSprite sprite)
        {
            performanceScale(sprite);
        }
        public override string title()
        {
            //char str[32] = {0};
            string str;
            //sprintf(str, "B (%d) scale", subtestNumber);
            str = string.Format("B {0:D} scale", subtestNumber);
            string strRet = str;
            return strRet;
        }

        private void performanceScale(CCSprite pSprite)
        {
            Random random = new Random();
            CCSize size = CCDirector.sharedDirector().getWinSize();
            pSprite.position = new CCPoint((random.Next() % (int)size.width), (random.Next() % (int)size.height));
            pSprite.scale = random.Next() * 100 / 50;
        }
    }
}
