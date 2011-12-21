using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SceneTestLayer3 : CCLayerColor
    {
        string s_pPathGrossini = "Images/grossini";

        public SceneTestLayer3()
        {
            base.isTouchEnabled = true;
            CCLabelTTF label = CCLabelTTF.labelWithString("Touch to popScene", "Arial", 28);
            addChild(label);
            CCSize s = CCDirector.sharedDirector().getWinSize();
            label.position = (new CCPoint(s.width / 2, s.height / 2));

            CCSprite sprite = CCSprite.spriteWithFile(s_pPathGrossini);
            addChild(sprite);
            sprite.position = (new CCPoint(s.width - 40, s.height / 2));
            CCActionInterval rotate = CCRotateBy.actionWithDuration(2, 360);
            CCAction repeat = CCRepeatForever.actionWithAction(rotate);
            sprite.runAction(repeat);

            //schedule();
        }

        public virtual void testDealloc(float dt)
        {

        }

        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
            //	static int i = 0;
            //UXLOG("SceneTestLayer3::ccTouchesEnded(%d)", ++i);
            CCDirector.sharedDirector().popScene();
        }
        //CREATE_NODE(SceneTestLayer3);
    }
}
