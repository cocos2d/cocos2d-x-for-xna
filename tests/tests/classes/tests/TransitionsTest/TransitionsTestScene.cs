using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public enum kCCiOSVersion
    {
        kCCiOSVersion_3_0 = 0x03000000,
        kCCiOSVersion_3_1 = 0x03010000,
        kCCiOSVersion_3_1_1 = 0x03010100,
        kCCiOSVersion_3_1_2 = 0x03010200,
        kCCiOSVersion_3_1_3 = 0x03010300,
        kCCiOSVersion_3_2 = 0x03020000,
        kCCiOSVersion_3_2_1 = 0x03020100,
        kCCiOSVersion_4_0 = 0x04000000,
        kCCiOSVersion_4_0_1 = 0x04000100,
        kCCiOSVersion_4_1 = 0x04010000,
        kCCiOSVersion_4_2 = 0x04020000,
        kCCiOSVersion_4_3 = 0x04030000,
        kCCiOSVersion_4_3_1 = 0x04030100,
        kCCiOSVersion_4_3_2 = 0x04030200,
        kCCiOSVersion_4_3_3 = 0x04030300,
    }

    public class TransitionsTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = new TestLayer1();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }

        public static CCTransitionScene createTransition(int nIndex, float t, CCScene s)
        {
            // fix bug #486, without setDepthTest(false), FlipX,Y will flickers
            CCDirector.sharedDirector().setDepthTest(false);

            switch (nIndex)
            {
                case 3: return CCTransitionJumpZoom.transitionWithDuration(t, s);
                case 4: return CCTransitionFade.transitionWithDuration(t, s);
                case 2: return FadeWhiteTransition.transitionWithDuration(t, s);
                case 17: return FlipXLeftOver.transitionWithDuration(t, s);
                case 18: return FlipXRightOver.transitionWithDuration(t, s);
                case 5: return FlipYUpOver.transitionWithDuration(t, s);
                case 6: return FlipYDownOver.transitionWithDuration(t, s);
                case 7: return FlipAngularLeftOver.transitionWithDuration(t, s);
                case 8: return FlipAngularRightOver.transitionWithDuration(t, s);
                case 9: return ZoomFlipXLeftOver.transitionWithDuration(t, s);
                case 10: return ZoomFlipXRightOver.transitionWithDuration(t, s);
                case 11: return ZoomFlipYUpOver.transitionWithDuration(t, s);
                case 12: return ZoomFlipYDownOver.transitionWithDuration(t, s);
                case 13: return ZoomFlipAngularLeftOver.transitionWithDuration(t, s);
                case 14: return ZoomFlipAngularRightOver.transitionWithDuration(t, s);
                case 15: return CCTransitionShrinkGrow.transitionWithDuration(t, s);
                case 16: return CCTransitionRotoZoom.transitionWithDuration(t, s);
                case 0: return CCTransitionMoveInL.transitionWithDuration(t, s);
                case 1: return CCTransitionMoveInR.transitionWithDuration(t, s);
                case 19: return CCTransitionMoveInT.transitionWithDuration(t, s);
                case 20: return CCTransitionMoveInB.transitionWithDuration(t, s);
                case 21: return CCTransitionSlideInL.transitionWithDuration(t, s);
                case 22: return CCTransitionSlideInR.transitionWithDuration(t, s);
                case 23: return CCTransitionSlideInT.transitionWithDuration(t, s);
                case 24: return CCTransitionSlideInB.transitionWithDuration(t, s);
                case 25:
                    {
                        if (CCConfiguration.sharedConfiguration().getGlesVersion() <= CCGlesVersion.GLES_VER_1_0)
                        {
                            //CCMessageBox("The Opengl ES version is lower than 1.1, and TransitionCrossFade may not run correctly, it is ignored.", "Cocos2d-x Hint");
                            return null;
                        }
                        else
                        {
                            return CCTransitionCrossFade.transitionWithDuration(t, s);
                        }
                    }
                    break;
                case 26:
                    {
                        if (CCConfiguration.sharedConfiguration().getGlesVersion() <= CCGlesVersion.GLES_VER_1_0)
                        {
                            //CCMessageBox("The Opengl ES version is lower than 1.1, and TransitionRadialCCW may not run correctly, it is ignored.", "Cocos2d-x Hint");
                            return null;
                        }
                        else
                        {
                            //return CCTransitionRadialCCW.transitionWithDuration(t,s);
                        }
                    }
                    break;
                case 27:
                    {
                        if (CCConfiguration.sharedConfiguration().getGlesVersion() <= CCGlesVersion.GLES_VER_1_0)
                        {
                            //CCMessageBox("The Opengl ES version is lower than 1.1, and TransitionRadialCW may not run correctly, it is ignored.", "Cocos2d-x Hint");
                            return null;
                        }
                        else
                        {
                            //return CCTransitionRadialCW::transitionWithDuration(t,s);
                        }
                    }
                    break;
                //case 28: return PageTransitionForward.transitionWithDuration(t, s);
                //case 29: return PageTransitionBackward.transitionWithDuration(t, s);
                case 30: return CCTransitionFadeTR.transitionWithDuration(t, s);
                case 31: return CCTransitionFadeBL.transitionWithDuration(t, s);
                case 32: return CCTransitionFadeUp.transitionWithDuration(t, s);
                case 33: return CCTransitionFadeDown.transitionWithDuration(t, s);
                case 34: return CCTransitionTurnOffTiles.transitionWithDuration(t, s);
                case 35: return CCTransitionSplitRows.transitionWithDuration(t, s);
                case 36: return CCTransitionSplitCols.transitionWithDuration(t, s);
                default: break;
            }

            return null;
        }
    }
}
