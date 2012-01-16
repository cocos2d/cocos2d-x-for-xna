using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpritePerformTest1 : SpriteMainScene
    {

        public override void doTest(CCSprite sprite)
        {
            performancePosition(sprite);
        }
        public override string title()
        {
            //char str[32] = {0};
            string str;
            //sprintf(str, "A (%d) position", subtestNumber);
            str = string.Format("A {0:D} position", subtestNumber);
            string strRet = str;
            return strRet;
        }

        private void performancePosition(CCSprite pSprite)
        {
            Random random = new Random();
            CCSize size = CCDirector.sharedDirector().getWinSize();
            pSprite.position = new CCPoint((random.Next() % (int)size.width), (random.Next() % (int)size.height));
        }
    }
}
