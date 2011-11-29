using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;
using tests.classes.tests.Layer;

namespace tests
{
    public static class LayerTestStaticLibrary
    {

        public static int kTagLayer = 1;
        public static int sceneIdx = -1;
        public const int MAX_LAYER = 4;



        public static CCLayer createTestLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return (CCLayer)new LayerTest1();
                case 1: return (CCLayer)new LayerTest2();
                case 2: return (CCLayer)new LayerTestBlend();
                case 3: return (CCLayer)new LayerGradient();
            }
            return null;
        }
        public static CCLayer nextTestAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createTestLayer(sceneIdx);

            return pLayer;
        }
        public static CCLayer backTestAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createTestLayer(sceneIdx);

            return pLayer;
        }
        public static CCLayer restartTestAction()
        {
            CCLayer pLayer = createTestLayer(sceneIdx);

            return pLayer;
        }

    }

    public static class CCF
    {
        public static CCPoint ccp(float x, float y)
        {
            return new CCPoint(x, y);
        }
        public static CCPoint CCPointMake(float x, float y)
        {
            return new CCPoint(x, y);
        }
        public static CCPoint CCPointZero
        {
            get { return CCF.ccp(0, 0); }
        }

        public static CCPoint ccpNormalize(CCPoint p)
        {
            float d = (float)Math.Sqrt(p.x * p.x + p.y * p.y);
            if (d == 0) return CCPointZero;

            return ccp(p.x / d, p.y / d);
        }
        public static CCPoint ccpSub(CCPoint p1, CCPoint p2)
        {
            return ccp(p1.x - p2.x, p1.y - p2.y);
        }

        public static CCSize CCSizeMake(float w, float h)
        {
            return new CCSize(w, h);
        }

        public static CCColor ccc4(byte a, byte b, byte c, byte d) { return new CCColor(); }
    }



    public class LayerTest : CCLayer
    {
        protected string m_strTitle;

        public LayerTest() { }
        ~LayerTest() { }

        public virtual string title() { return "No title"; }
        public virtual string subtitle() { return ""; }
        public virtual void onEnter()
        {
            base.onEnter();
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 32);
            addChild(label, 1);
            label.position = (CCF.CCPointMake(s.width / 2, s.height - 50));

            string subtitle_ = subtitle();
            if (subtitle_.Length > 0)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(subtitle_, "Thonburi", 16);
                addChild(l, 1);
                l.position = (CCF.ccp(s.width / 2, s.height - 80));
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, new SEL_MenuHandler(this.backCallback));
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, new SEL_MenuHandler(this.restartCallback));
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, new SEL_MenuHandler(this.nextCallback));

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = (CCF.CCPointZero);
            item1.position = (CCF.CCPointMake(s.width / 2 - 100, 30));
            item2.position = (CCF.CCPointMake(s.width / 2, 30));
            item3.position = (CCF.CCPointMake(s.width / 2 + 100, 30));

            addChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new LayerTestScene();
            s.addChild(LayerTestStaticLibrary.restartTestAction());

            CCDirector.sharedDirector().replaceScene(s);
        }
        public void nextCallback(CCObject pSender)
        {
            CCScene s = new LayerTestScene();
            s.addChild(LayerTestStaticLibrary.restartTestAction());

            CCDirector.sharedDirector().replaceScene(s);
        }
        public void backCallback(CCObject pSender)
        {
            CCScene s = new LayerTestScene();
            s.addChild(LayerTestStaticLibrary.backTestAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
    }

    class LayerTest1 : LayerTest
    {
        public override void onEnter()
        {
            base.onEnter();



            isTouchEnabled = (true);

            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLayerColor layer = CCLayerColor.layerWithColorWidthHeight(CCF.ccc4(0xFF, 0x00, 0x00, 0x80), 200, 200);

            layer.isRelativeAnchorPoint = (true);
            layer.position = (CCF.CCPointMake(s.width / 2, s.height / 2));
            addChild(layer, 1, LayerTestStaticLibrary.kTagLayer);
        }
        public override string title()
        {
            return "ColorLayer resize (tap & move)";
        }

        public void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, -128 + 1, true);
        }
        public void updateSize(CCTouch touch)
        {
            CCPoint touchLocation = touch.locationInView(touch.view());
            touchLocation = CCDirector.sharedDirector().convertToGL(touchLocation);

            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCSize newSize = CCF.CCSizeMake(Math.Abs(touchLocation.x - s.width / 2) * 2, Math.Abs(touchLocation.y - s.height / 2) * 2);
            CCLayerColor l = (CCLayerColor)getChildByTag(LayerTestStaticLibrary.kTagLayer);

            l.contentSize = (newSize);
        }

        public override bool ccTouchBegan(CCTouch touch, CCEvent @event)
        {
            updateSize(touch);

            return true;
        }
        public override void ccTouchMoved(CCTouch touch, CCEvent @event)
        {
            updateSize(touch);
        }
        public override void ccTouchEnded(CCTouch touch, CCEvent @event)
        {
            updateSize(touch);
        }
    }

    class LayerTest2 : LayerTest
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLayerColor layer1 = CCLayerColor.layerWithColorWidthHeight(CCF.ccc4(255, 255, 0, 80), 100, 300);
            layer1.position = (CCF.CCPointMake(s.width / 3, s.height / 2));
            layer1.isRelativeAnchorPoint = (true);
            addChild(layer1, 1);

            CCLayerColor layer2 = CCLayerColor.layerWithColorWidthHeight(CCF.ccc4(0, 0, 255, 255), 100, 300);
            layer2.position = (CCF.CCPointMake((s.width / 3) * 2, s.height / 2));
            layer2.isRelativeAnchorPoint = (true);
            addChild(layer2, 1);

            CCActionInterval actionTint = CCTintBy.actionWithDuration(2, -255, -127, 0);
            CCActionInterval actionTintBack = actionTint.reverse() as CCActionInterval;
            CCActionInterval seq1 = (CCActionInterval)CCSequence.actions(actionTint, actionTintBack);
            layer1.runAction(seq1);

            CCActionInterval actionFade = CCFadeOut.actionWithDuration(2.0f);
            CCActionInterval actionFadeBack = actionFade.reverse() as CCActionInterval;
            CCActionInterval seq2 = (CCActionInterval)CCSequence.actions(actionFade, actionFadeBack);
            layer2.runAction(seq2);
        }
        public override string title()
        {
            return "ColorLayer: fade and tint";
        }
    }

    class LayerTestBlend : LayerTest
    {
        public LayerTestBlend()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLayerColor layer1 = CCLayerColor.layerWithColor(CCF.ccc4(255, 255, 255, 80));

            CCSprite sister1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sister2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            addChild(sister1);
            addChild(sister2);
            addChild(layer1, 100, LayerTestStaticLibrary.kTagLayer);

            sister1.position = (CCF.CCPointMake(160, s.height / 2));
            sister2.position = (CCF.CCPointMake(320, s.height / 2));

            //schedule(SEL_SCHEDULE(LayerTestBlend.newBlend), 1.0f);
        }

        //    public void newBlend(ccTime dt)
        //    {
        //CCLayerColor layer = (CCLayerColor)getChildByTag(LayerTestStaticLibrary.kTagLayer);

        //GLenum src;
        //GLenum dst;

        //if( layer.getBlendFunc().dst == GL_ZERO )
        //{
        //    src = CC_BLEND_SRC;
        //    dst = CC_BLEND_DST;
        //}
        //else
        //{
        //    src = GL_ONE_MINUS_DST_COLOR;
        //    dst = GL_ZERO;
        //}

        //ccBlendFunc bf = {src, dst};
        //layer.setBlendFunc( bf );
        //    }

        public override string title()
        {
            return "ColorLayer: blend";
        }
    }

    class LayerGradient : LayerTest
    {
        public LayerGradient()
        {
            CCLayerGradient layer1 = CCLayerGradient.layerWithColor(CCF.ccc4(255, 0, 0, 255), CCF.ccc4(0, 255, 0, 255), CCF.ccp(0.9f, 0.9f));
            addChild(layer1, 0, LayerTestStaticLibrary.kTagLayer);

            isTouchEnabled = (true);

            CCLabelTTF label1 = CCLabelTTF.labelWithString("Compressed Interpolation: Enabled", "Marker Felt", 26);
            CCLabelTTF label2 = CCLabelTTF.labelWithString("Compressed Interpolation: Disabled", "Marker Felt", 26);
            CCMenuItemLabel item1 = CCMenuItemLabel.itemWithLabel(label1);
            CCMenuItemLabel item2 = CCMenuItemLabel.itemWithLabel(label2);
            CCMenuItemToggle item = CCMenuItemToggle.itemWithTarget(this, new SEL_MenuHandler(this.toggleItem), item1, item2);

            CCMenu menu = CCMenu.menuWithItems(item);
            addChild(menu);
            CCSize s = CCDirector.sharedDirector().getWinSize();
        }
        public override void ccTouchesMoved(List<CCTouch> touches, CCEvent @event)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCTouch touch = touches[0] as CCTouch;

            CCPoint start = touch.locationInView(touch.view());
            start = CCDirector.sharedDirector().convertToGL(start);

            CCPoint diff = CCF.ccpSub(CCF.ccp(s.width / 2, s.height / 2), start);
            diff = CCF.ccpNormalize(diff);

            CCLayerGradient gradient = (CCLayerGradient)getChildByTag(1);
            gradient.vector = (diff);

        }
        public override string title()
        {
            return "LayerGradient";
        }
        public override string subtitle()
        {
            return "Touch the screen and move your finger";
        }
        public void toggleItem(CCObject sender)
        {
            CCLayerGradient gradient = (CCLayerGradient)getChildByTag(LayerTestStaticLibrary.kTagLayer);
            gradient.isCompressedInterpolation = (!gradient.isCompressedInterpolation);
        }
    }

    public class LayerTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = LayerTestStaticLibrary.nextTestAction();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}



namespace tests.classes.tests.Layer
{
    /// <summary>
    /// fake
    /// </summary>
    public class CCLayerGradient : cocos2d.CCLayer
    {
        public static CCLayerGradient layerWithColor(CCColor c1, CCColor c2, cocos2d.CCPoint p) { return null; }

        public bool isCompressedInterpolation { get; set; }
        public cocos2d.CCPoint vector { get; set; }
    }
    
    /// <summary>
    /// dummy
    /// </summary>
    public struct CCColor
    {
    }

    /// <summary>
    /// dummy
    /// </summary>
    public class CCLayerColor : cocos2d.CCLayer
    {
        public static CCLayerColor layerWithColor(CCColor color) { return null; }
        public static CCLayerColor layerWithColorWidthHeight(CCColor color, int x, int y) { return null; }
    }

    /// <summary>
    /// dummy
    /// </summary>
    public class CCSet : List<object>
    {
    }
}