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
        public static int s_nSceneIdx = 0;
        public static int MAX_LAYER = 33;
        public static float TRANSITION_DURATION = 1.2f;
        public static string s_back1 = "Images/background1";
        public static string s_back2 = "Images/background2";

        public static string s_pPathB1 = "Images/b1";
        public static string s_pPathB2 = "Images/b2";
        public static string s_pPathR1 = "Images/r1";
        public static string s_pPathR2 = "Images/r2";
        public static string s_pPathF1 = "Images/f1";
        public static string s_pPathF2 = "Images/f2";

        public static string[] transitions = new string[]  {
    "JumpZoomTransition",
    "FadeTransition",
    "FadeWhiteTransition",
    "FlipXLeftOver",
    "FlipXRightOver",
    "FlipYUpOver",
    "FlipYDownOver",
    "FlipAngularLeftOver",
    "FlipAngularRightOver",
    "ZoomFlipXLeftOver",
    "ZoomFlipXRightOver",
    "ZoomFlipYUpOver",
    "ZoomFlipYDownOver",
    "ZoomFlipAngularLeftOver",
    "ZoomFlipAngularRightOver",
    "ShrinkGrowTransition",
    "RotoZoomTransition",
    //"MoveInLTransition",
    //"MoveInRTransition",
    //"MoveInTTransition",
    //"MoveInBTransition",
    "SlideInLTransition",
    "SlideInRTransition",
    "SlideInTTransition",
    "SlideInBTransition",

    "CCTransitionCrossFade",
    "CCTransitionRadialCCW",
    "CCTransitionRadialCW",
    "PageTransitionForward",
    "PageTransitionBackward",
    "FadeTRTransition",
    "FadeBLTransition",
    "FadeUpTransition",
    "FadeDownTransition",
    "TurnOffTilesTransition",
    "SplitRowsTransition",
    "SplitColsTransition",
};

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
                case 0: return CCTransitionJumpZoom.transitionWithDuration(t, s);
                case 1: return CCTransitionFade.transitionWithDuration(t, s);
                case 2: return FadeWhiteTransition.transitionWithDuration(t, s);
                case 3: return FlipXLeftOver.transitionWithDuration(t, s);
                case 4: return FlipXRightOver.transitionWithDuration(t, s);
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
                //case 17: return CCTransitionMoveInL.transitionWithDuration(t, s);
                //case 18: return CCTransitionMoveInR.transitionWithDuration(t, s);
                //case 19: return CCTransitionMoveInT.transitionWithDuration(t, s);
                //case 20: return CCTransitionMoveInB.transitionWithDuration(t, s);
                case 17: return CCTransitionSlideInL.transitionWithDuration(t, s);
                case 18: return CCTransitionSlideInR.transitionWithDuration(t, s);
                case 19: return CCTransitionSlideInT.transitionWithDuration(t, s);
                case 20: return CCTransitionSlideInB.transitionWithDuration(t, s);
                case 21:
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
                case 22:
                    {
                        if (CCConfiguration.sharedConfiguration().getGlesVersion() <= CCGlesVersion.GLES_VER_1_0)
                        {
                            //CCMessageBox("The Opengl ES version is lower than 1.1, and TransitionRadialCCW may not run correctly, it is ignored.", "Cocos2d-x Hint");
                            return null;
                        }
                        else
                        {
                            return CCTransitionRadialCCW.transitionWithDuration(t, s);
                        }
                    }
                case 23:
                    {
                        if (CCConfiguration.sharedConfiguration().getGlesVersion() <= CCGlesVersion.GLES_VER_1_0)
                        {
                            //CCMessageBox("The Opengl ES version is lower than 1.1, and TransitionRadialCW may not run correctly, it is ignored.", "Cocos2d-x Hint");
                            return null;
                        }
                        else
                        {
                            return CCTransitionRadialCW.transitionWithDuration(t, s);
                        }
                    }
                case 24: return PageTransitionForward.transitionWithDuration(t, s);
                case 25: return PageTransitionBackward.transitionWithDuration(t, s);
                case 26: return CCTransitionFadeTR.transitionWithDuration(t, s);
                case 27: return CCTransitionFadeBL.transitionWithDuration(t, s);
                case 28: return CCTransitionFadeUp.transitionWithDuration(t, s);
                case 29: return CCTransitionFadeDown.transitionWithDuration(t, s);
                case 30: return CCTransitionTurnOffTiles.transitionWithDuration(t, s);
                case 31: return CCTransitionSplitRows.transitionWithDuration(t, s);
                case 32: return CCTransitionSplitCols.transitionWithDuration(t, s);
                default: break;
            }

            return null;
        }
    }
}
