using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using cocos2d;
using CocosDenshion;
using System.Diagnostics;

namespace tests 
{
    public class CocosDenshionTest : CCLayer
    {
        string EFFECT_FILE = "Sounds/effect1";
        string MUSIC_FILE = "Sounds/background";
        int LINE_SPACE = 40;

        CCMenu m_pItmeMenu;
	    CCPoint m_tBeginPos;
	    int m_nTestCount;
	    uint m_nSoundId;

        public CocosDenshionTest()
        {
            m_pItmeMenu = null;
            m_tBeginPos = new CCPoint(0,0);
            m_nSoundId = 0;

	        string[] testItems = {
		        "play background music",
		        "stop background music",
		        "pause background music",
		        "resume background music",
		        "rewind background music",
		        "is background music playing",
		        "play effect",
                "play effect repeatly",
		        "stop effect",
		        "unload effect",
		        "add background music volume",
		        "sub background music volume",
		        "add effects volume",
		        "sub effects volume"
	        };

	        // add menu items for tests
	        m_pItmeMenu = CCMenu.menuWithItems(null);
	        CCSize s = CCDirector.sharedDirector().getWinSize();
	        m_nTestCount = testItems.Count<string>();

	        for (int i = 0; i < m_nTestCount; ++i)
	        {
                CCLabelTTF label = CCLabelTTF.labelWithString(testItems[i], "Arial", 24);
                CCMenuItemLabel pMenuItem = CCMenuItemLabel.itemWithLabel(label, this, new SEL_MenuHandler(menuCallback));
		
		        m_pItmeMenu.addChild(pMenuItem, i + 10000);
		        pMenuItem.position = new CCPoint( s.width / 2, (s.height - (i + 1) * LINE_SPACE) );
	        }

	        m_pItmeMenu.contentSize = new CCSize(s.width, (m_nTestCount + 1) * LINE_SPACE);
	        m_pItmeMenu.position = new CCPoint(0,0);
	        addChild(m_pItmeMenu);

	        this.isTouchEnabled = true;

	        // preload background music and effect
	        SimpleAudioEngine.sharedEngine().preloadBackgroundMusic(CCFileUtils.fullPathFromRelativePath(MUSIC_FILE));
	        SimpleAudioEngine.sharedEngine().preloadEffect(CCFileUtils.fullPathFromRelativePath(EFFECT_FILE));
    
            // set default volume
            SimpleAudioEngine.sharedEngine().setEffectsVolume(0.5f);
            SimpleAudioEngine.sharedEngine().setBackgroundMusicVolume(0.5f);
        }

        ~CocosDenshionTest()
        {
        }

        public override void onExit()
        {
	        base.onExit();

	        SimpleAudioEngine.sharedEngine().end();
        }

        public void menuCallback(CCObject pSender)
        {
	        // get the userdata, it's the index of the menu item clicked
	        CCMenuItem pMenuItem = (CCMenuItem)(pSender);
	        int nIdx = pMenuItem.zOrder - 10000;

	        switch(nIdx)
	        {
	        // play background music
	        case 0:

		        SimpleAudioEngine.sharedEngine().playBackgroundMusic(CCFileUtils.fullPathFromRelativePath(MUSIC_FILE), true);
		        break;
	        // stop background music
	        case 1:
		        SimpleAudioEngine.sharedEngine().stopBackgroundMusic();
		        break;
	        // pause background music
	        case 2:
		        SimpleAudioEngine.sharedEngine().pauseBackgroundMusic();
		        break;
	        // resume background music
	        case 3:
		        SimpleAudioEngine.sharedEngine().resumeBackgroundMusic();
		        break;
	        // rewind background music
	        case 4:
		        SimpleAudioEngine.sharedEngine().rewindBackgroundMusic();
		        break;
	        // is background music playing
	        case 5:
		        if (SimpleAudioEngine.sharedEngine().isBackgroundMusicPlaying())
		        {
			        CCLog.Log("background music is playing");
		        }
		        else
		        {
                    CCLog.Log("background music is not playing");
		        }
		        break;
	        // play effect
	        case 6:
		        m_nSoundId = SimpleAudioEngine.sharedEngine().playEffect(CCFileUtils.fullPathFromRelativePath(EFFECT_FILE));
		        break;
            // play effect
            case 7:
                m_nSoundId = SimpleAudioEngine.sharedEngine().playEffect(CCFileUtils.fullPathFromRelativePath(EFFECT_FILE), true);
                break;
            // stop effect
	        case 8:
		        SimpleAudioEngine.sharedEngine().stopEffect(m_nSoundId);
		        break;
	        // unload effect
	        case 9:
		        SimpleAudioEngine.sharedEngine().unloadEffect(CCFileUtils.fullPathFromRelativePath(EFFECT_FILE));
		        break;
		        // add bakcground music volume
	        case 10:
		        SimpleAudioEngine.sharedEngine().setBackgroundMusicVolume(SimpleAudioEngine.sharedEngine().getBackgroundMusicVolume() + 0.1f);
		        break;
		        // sub backgroud music volume
	        case 11:
		        SimpleAudioEngine.sharedEngine().setBackgroundMusicVolume(SimpleAudioEngine.sharedEngine().getBackgroundMusicVolume() - 0.1f);
		        break;
		        // add effects volume
	        case 12:
		        SimpleAudioEngine.sharedEngine().setEffectsVolume(SimpleAudioEngine.sharedEngine().getEffectsVolume() + 0.1f);
		        break;
		        // sub effects volume
	        case 13:
		        SimpleAudioEngine.sharedEngine().setEffectsVolume(SimpleAudioEngine.sharedEngine().getEffectsVolume() - 0.1f);
		        break;
	        }
	
        }

        public override void ccTouchesBegan(List<CCTouch> pTouches, CCEvent pEvent)
        {
            CCTouch touch = pTouches.FirstOrDefault();

            m_tBeginPos = touch.locationInView(touch.view());
            m_tBeginPos = CCDirector.sharedDirector().convertToGL(m_tBeginPos);
        }

        public override void ccTouchesMoved(List<CCTouch> pTouches, CCEvent pEvent)
        {
            CCTouch touch = pTouches.FirstOrDefault();

	        CCPoint touchLocation = touch.locationInView( touch.view() );	
	        touchLocation = CCDirector.sharedDirector().convertToGL( touchLocation );
	        float nMoveY = touchLocation.y - m_tBeginPos.y;

	        CCPoint curPos  = m_pItmeMenu.position;
	        CCPoint nextPos = new CCPoint(curPos.x, curPos.y + nMoveY);
	        CCSize winSize = CCDirector.sharedDirector().getWinSize();
	        if (nextPos.y < 0.0f)
	        {
		        m_pItmeMenu.position = new CCPoint(0,0);
		        return;
	        }

	        if (nextPos.y > ((m_nTestCount + 1)* LINE_SPACE - winSize.height))
	        {
		        m_pItmeMenu.position = new CCPoint(0, ((m_nTestCount + 1)* LINE_SPACE - winSize.height));
		        return;
	        }

	        m_pItmeMenu.position = nextPos;
	        m_tBeginPos = touchLocation;
        }


    }


    public class CocosDenshionTestScene : TestScene
    {
        public override void runThisTest()
        {
	        CCLayer pLayer = new CocosDenshionTest();
	        addChild(pLayer);

	        CCDirector.sharedDirector().replaceScene(this);
        }
    }
}