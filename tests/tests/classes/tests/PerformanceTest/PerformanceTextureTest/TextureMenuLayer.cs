using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TextureMenuLayer : PerformBasicLayer
    {
        public TextureMenuLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void showCurrentTest()
        {
            CCScene pScene = null;

            switch (m_nCurCase)
            {
                case 0:
                    pScene = TextureTest.scene();
                    break;
            }
            PerformanceTextureTest.s_nTexCurCase = m_nCurCase;

            if (pScene != null)
            {
                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            // Title
            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 40);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 32);
            label.Color = new ccColor3B(255, 255, 40);

            // Subtitle
            string strSubTitle = subtitle();
            if (strSubTitle.Length > 0)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubTitle, "Thonburi", 16);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);
            }

            performTests();
        }

        public virtual string title()
        {
            return "no title";
        }

        public virtual string subtitle()
        {
            return "no subtitle";
        }

        public virtual void performTests()
        {
            throw new NotFiniteNumberException();
        }
    }
}
