using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TextLayer : CCLayerColor
    {
        //UxString	m_strTitle;
        static int MAX_LAYER = 20;

        public TextLayer()
        {
            initWithColor(ccTypes.ccc4(32, 32, 32, 255));

            float x, y;

            CCSize size = CCDirector.sharedDirector().getWinSize();
            x = size.width;
            y = size.height;

            CCNode node = CCNode.node();
            CCActionInterval effect = getAction();
            node.runAction(effect);
            addChild(node, 0, EffectTestScene.kTagBackground);

            CCSprite bg = CCSprite.spriteWithFile(TestResource.s_back3);
            node.addChild(bg, 0);
            bg.anchorPoint = new CCPoint(0, 0);

            CCSprite grossini = CCSprite.spriteWithFile(TestResource.s_pPathSister2);
            node.addChild(grossini, 1);
            grossini.position = new CCPoint(x / 3, y / 2);
            CCActionInterval sc = CCScaleBy.actionWithDuration(2, 5);
            CCFiniteTimeAction sc_back = sc.reverse();
            grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(sc, sc_back))));

            CCSprite tamara = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            node.addChild(tamara, 1);
            tamara.position = new CCPoint(2 * x / 3, y / 2);
            CCActionInterval sc2 = CCScaleBy.actionWithDuration(2, 5);
            CCFiniteTimeAction sc2_back = sc2.reverse();
            tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(sc2, sc2_back))));

            CCLabelTTF label = CCLabelTTF.labelWithString(EffectTestScene.effectsList[EffectTestScene.actionIdx], "Arial", 32);

            label.position = new CCPoint(x / 2, y - 80);
            addChild(label);
            label.tag = EffectTestScene.kTagLabel;

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(size.width / 2 - 100, 30);
            item2.position = new CCPoint(size.width / 2, 30);
            item3.position = new CCPoint(size.width / 2 + 100, 30);

            addChild(menu, 1);

            schedule(checkAnim);
        }

        public CCActionInterval createEffect(int nIndex, float t)
        {
            CCDirector.sharedDirector().setDepthTest(false);

            switch (nIndex)
            {
                case 0: return Shaky3DDemo.actionWithDuration(t);
                case 1: return Waves3DDemo.actionWithDuration(t);
                //case 2: return FlipX3DDemo.actionWithDuration(t);
                //case 3: return FlipY3DDemo.actionWithDuration(t);
                case 4: return Lens3DDemo.actionWithDuration(t);
                case 5: return Ripple3DDemo.actionWithDuration(t);
                case 6: return LiquidDemo.actionWithDuration(t);
                case 7: return WavesDemo.actionWithDuration(t);
                case 8: return TwirlDemo.actionWithDuration(t);
                case 9: return ShakyTiles3DDemo.actionWithDuration(t);
                case 10: return ShatteredTiles3DDemo.actionWithDuration(t);
                case 11: return ShuffleTilesDemo.actionWithDuration(t);
                case 12: return FadeOutTRTilesDemo.actionWithDuration(t);
                case 13: return FadeOutBLTilesDemo.actionWithDuration(t);
                case 14: return FadeOutUpTilesDemo.actionWithDuration(t);
                case 15: return FadeOutDownTilesDemo.actionWithDuration(t);
                case 16: return TurnOffTilesDemo.actionWithDuration(t);
                case 17: return WavesTiles3DDemo.actionWithDuration(t);
                case 18: return JumpTiles3DDemo.actionWithDuration(t);
                case 19: return SplitRowsDemo.actionWithDuration(t);
                case 2: return SplitColsDemo.actionWithDuration(t);
                case 3: return PageTurn3DDemo.actionWithDuration(t);
            }

            return null;
        }

        public CCActionInterval getAction()
        {
            CCActionInterval pEffect = createEffect(EffectTestScene.actionIdx, 3);

            return pEffect;
        }

        public void checkAnim(float dt)
        {
            CCNode s2 = getChildByTag(EffectTestScene.kTagBackground);
            if (s2.numberOfRunningActions() == 0 && s2.Grid != null)
                s2.Grid = null; ;
        }

        public override void onEnter()
        {
            base.onEnter();
        }

        public void restartCallback(CCObject pSender)
        {
            /*newOrientation();*/
            newScene();
        }

        public void nextCallback(CCObject pSender)
        {
            // update the action index
            EffectTestScene.actionIdx++;
            EffectTestScene.actionIdx = EffectTestScene.actionIdx % MAX_LAYER;

            /*newOrientation();*/
            newScene();
        }

        public void backCallback(CCObject pSender)
        {
            // update the action index
            EffectTestScene.actionIdx--;
            int total = MAX_LAYER;
            if (EffectTestScene.actionIdx < 0)
                EffectTestScene.actionIdx += total;

            /*newOrientation();*/
            newScene();
        }

        public void newOrientation()
        {
            ccDeviceOrientation orientation = CCDirector.sharedDirector().deviceOrientation;
            switch (orientation)
            {
                case ccDeviceOrientation.CCDeviceOrientationLandscapeLeft:
                    orientation = ccDeviceOrientation.CCDeviceOrientationPortrait;
                    break;
                case ccDeviceOrientation.CCDeviceOrientationPortrait:
                    orientation = ccDeviceOrientation.CCDeviceOrientationLandscapeRight;
                    break;
                case ccDeviceOrientation.CCDeviceOrientationLandscapeRight:
                    orientation = ccDeviceOrientation.CCDeviceOrientationPortraitUpsideDown;
                    break;
                case ccDeviceOrientation.CCDeviceOrientationPortraitUpsideDown:
                    orientation = ccDeviceOrientation.CCDeviceOrientationLandscapeLeft;
                    break;
            }

            CCDirector.sharedDirector().deviceOrientation = orientation;
        }

        public void newScene()
        {
            CCScene s = new EffectTestScene();
            CCNode child = TextLayer.node();
            s.addChild(child);
            CCDirector.sharedDirector().replaceScene(s);
        }

        public new static TextLayer node()
        {
            TextLayer pLayer = new TextLayer();

            return pLayer;
        }
    }
}
