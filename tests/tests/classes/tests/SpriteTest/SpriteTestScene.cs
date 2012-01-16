using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public enum kTags
    {
        kTagTileMap = 1,
        kTagSpriteBatchNode = 1,
        kTagNode = 2,
        kTagAnimation1 = 1,
        kTagSpriteLeft,
        kTagSpriteRight,
    }

    public enum kTagSprite
    {
        kTagSprite1,
        kTagSprite2,
        kTagSprite3,
        kTagSprite4,
        kTagSprite5,
        kTagSprite6,
        kTagSprite7,
        kTagSprite8,
    }

    public class SpriteTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = nextSpriteTestAction();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }

        static int sceneIdx = -1;
        static int MAX_LAYER = 44;

        public static CCLayer createSpriteTestLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new Sprite1();
                case 1: return new SpriteBatchNode1();
                case 2: return new SpriteFrameTest();
                //case 3: return new SpriteFrameAliasNameTest();
                case 3: return new SpriteAnchorPoint();
                case 4: return new SpriteBatchNodeAnchorPoint();
                case 5: return new SpriteOffsetAnchorRotation();
                case 6: return new SpriteBatchNodeOffsetAnchorRotation();
                case 7: return new SpriteOffsetAnchorScale();
                case 8: return new SpriteBatchNodeOffsetAnchorScale();
                case 9: return new SpriteAnimationSplit();
                case 10: return new SpriteColorOpacity();
                case 11: return new SpriteBatchNodeColorOpacity();
                case 12: return new SpriteZOrder();
                case 13: return new SpriteBatchNodeZOrder();
                case 14: return new SpriteBatchNodeReorder();
                case 15: return new SpriteBatchNodeReorderIssue744();
                case 16: return new SpriteBatchNodeReorderIssue766();
                case 17: return new SpriteBatchNodeReorderIssue767();
                //case 18: return new SpriteZVertex();
                //case 19: return new SpriteBatchNodeZVertex();
                case 18: return new Sprite6();
                case 19: return new SpriteFlip();
                case 20: return new SpriteBatchNodeFlip();
                case 21: return new SpriteAliased();
                case 22: return new SpriteBatchNodeAliased();
                case 23: return new SpriteNewTexture();
                case 24: return new SpriteBatchNodeNewTexture();
                case 25: return new SpriteHybrid();
                case 26: return new SpriteBatchNodeChildren();
                case 27: return new SpriteBatchNodeChildren2();
                case 28: return new SpriteBatchNodeChildrenZ();
                case 29: return new SpriteChildrenVisibility();
                case 30: return new SpriteChildrenVisibilityIssue665();
                case 31: return new SpriteChildrenAnchorPoint();
                case 32: return new SpriteBatchNodeChildrenAnchorPoint();

                case 33: return new SpriteBatchNodeChildrenScale();
                case 34: return new SpriteChildrenChildren();
                case 35: return new SpriteBatchNodeChildrenChildren();
                case 36: return new SpriteNilTexture();
                case 37: return new SpriteSubclass();
                //case 5: return new AnimationCache();
                case 38: return new SpriteOffsetAnchorSkew();
                case 39: return new SpriteBatchNodeOffsetAnchorSkew();
                case 40: return new SpriteOffsetAnchorSkewScale();
                case 41: return new SpriteBatchNodeOffsetAnchorSkewScale();
                case 42: return new SpriteOffsetAnchorFlip();
                case 43: return new SpriteBatchNodeOffsetAnchorFlip();
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
