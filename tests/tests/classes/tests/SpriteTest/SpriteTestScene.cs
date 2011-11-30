using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = nextSpriteTestAction();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }

        static int sceneIdx = -1;
        static int MAX_LAYER = 48;

        public static CCLayer createSpriteTestLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new Sprite1();
                //case 1: return new SpriteBatchNode1();
                //case 2: return new SpriteFrameTest();
                //case 3: return new SpriteFrameAliasNameTest();
                //case 4: return new SpriteAnchorPoint();
                //case 5: return new SpriteBatchNodeAnchorPoint();
                //case 6: return new SpriteOffsetAnchorRotation();
                //case 7: return new SpriteBatchNodeOffsetAnchorRotation();
                //case 8: return new SpriteOffsetAnchorScale();
                //case 9: return new SpriteBatchNodeOffsetAnchorScale();
                //case 10: return new SpriteAnimationSplit();
                //case 11: return new SpriteColorOpacity();
                //case 12: return new SpriteBatchNodeColorOpacity();
                //case 13: return new SpriteZOrder();
                //case 14: return new SpriteBatchNodeZOrder();
                //case 15: return new SpriteBatchNodeReorder();
                //case 16: return new SpriteBatchNodeReorderIssue744();
                //case 17: return new SpriteBatchNodeReorderIssue766();
                //case 18: return new SpriteBatchNodeReorderIssue767();
                //case 19: return new SpriteZVertex();
                //case 20: return new SpriteBatchNodeZVertex();
                //case 21: return new Sprite6();
                //case 22: return new SpriteFlip();
                //case 23: return new SpriteBatchNodeFlip();
                //case 24: return new SpriteAliased();
                //case 25: return new SpriteBatchNodeAliased();
                //case 26: return new SpriteNewTexture();
                //case 27: return new SpriteBatchNodeNewTexture();
                //case 28: return new SpriteHybrid();
                //case 29: return new SpriteBatchNodeChildren();
                //case 30: return new SpriteBatchNodeChildren2();
                //case 31: return new SpriteBatchNodeChildrenZ();
                //case 32: return new SpriteChildrenVisibility();
                //case 33: return new SpriteChildrenVisibilityIssue665();
                //case 34: return new SpriteChildrenAnchorPoint();
                //case 35: return new SpriteBatchNodeChildrenAnchorPoint();
                //case 36: return new SpriteBatchNodeChildrenScale();
                //case 37: return new SpriteChildrenChildren();
                //case 38: return new SpriteBatchNodeChildrenChildren();
                //case 39: return new SpriteNilTexture();
                //case 40: return new SpriteSubclass();
                //case 41: return new AnimationCache();
                //case 42: return new SpriteOffsetAnchorSkew();
                //case 43: return new SpriteBatchNodeOffsetAnchorSkew();
                //case 44: return new SpriteOffsetAnchorSkewScale();
                //case 45: return new SpriteBatchNodeOffsetAnchorSkewScale();
                //case 46: return new SpriteOffsetAnchorFlip();
                //case 47: return new SpriteBatchNodeOffsetAnchorFlip();
            }

            return null;
        }

        public static CCLayer nextSpriteTestAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createSpriteTestLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer backSpriteTestAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createSpriteTestLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartSpriteTestAction()
        {
            CCLayer pLayer = createSpriteTestLayer(sceneIdx);

            return pLayer;
        }
    }
}
