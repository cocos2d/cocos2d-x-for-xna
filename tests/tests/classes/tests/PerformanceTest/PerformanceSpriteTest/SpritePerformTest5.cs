using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpritePerformTest5 : SpriteMainScene
    {
        public override void doTest(CCSprite sprite)
        {
            performanceout20(sprite);
        }

        public override string title()
        {
            //char str[32] = {0};
            string str;
            //sprintf(str, "E (%d) 80%% out", subtestNumber);
            str = string.Format("E {0:D} 80%% out", subtestNumber);
            string strRet = str;
            return strRet;
        }

        private void performanceout20(CCSprite pSprite)
        {
            Random random = new Random();
            CCSize size = CCDirector.sharedDirector().getWinSize();

            if (random.Next() < 0.2f)
                pSprite.position = new CCPoint((random.Next() % (int)size.width), (random.Next() % (int)size.height));
            else
                pSprite.position = new CCPoint(-1000, -1000);
        }
    }
}
