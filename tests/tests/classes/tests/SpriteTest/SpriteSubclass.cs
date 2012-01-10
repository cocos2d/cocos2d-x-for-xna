using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{

    class MySprite1 : CCSprite
    {
        public MySprite1()
        {
            ivar = 10;
        }
        public static MySprite1 spriteWithSpriteFrameName(string pszSpriteFrameName)
        {
            CCSpriteFrame pFrame = CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(pszSpriteFrameName);
            MySprite1 pobSprite = new MySprite1();
            pobSprite.initWithSpriteFrame(pFrame);

            return pobSprite;
        }

        private int ivar;
    }

    class MySprite2 : CCSprite
    {

        public MySprite2()
        {
            ivar = 10;
        }
        public static MySprite2 spriteWithFile(string pszName)
        {
            MySprite2 pobSprite = new MySprite2();
            pobSprite.initWithFile(pszName);

            return pobSprite;
        }

        private int ivar;
    }

    public class SpriteSubclass : SpriteTestDemo
    {



        public SpriteSubclass()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/ghosts");
            CCSpriteBatchNode aParent = CCSpriteBatchNode.batchNodeWithFile("animations/images/ghosts");

            // MySprite1
            MySprite1 sprite = MySprite1.spriteWithSpriteFrameName("father.gif");
            sprite.position = (new CCPoint(s.width / 4 * 1, s.height / 2));
            aParent.addChild(sprite);
            addChild(aParent);

            // MySprite2
            MySprite2 sprite2 = MySprite2.spriteWithFile("Images/grossini");
            addChild(sprite2);
            sprite2.position = (new CCPoint(s.width / 4 * 3, s.height / 2));
        }

        public override string title()
        {
            return "Sprite subclass";
        }

        public override string subtitle()
        {
            return "Testing initWithTexture:rect method";
        }
    }
}
