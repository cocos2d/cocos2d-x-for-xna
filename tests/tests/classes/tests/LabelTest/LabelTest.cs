using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using tests;
using System.Diagnostics;

namespace cocos2d
{
    public enum TagSprite
    {
        kTagTileMap = 1,
        kTagSpriteManager = 1,
        kTagAnimation1 = 1,
        kTagBitmapAtlas1 = 1,
        kTagBitmapAtlas2 = 2,
        kTagBitmapAtlas3 = 3,

        kTagSprite1,
        kTagSprite2,
        kTagSprite3,
        kTagSprite4,
        kTagSprite5,
        kTagSprite6,
        kTagSprite7,
        kTagSprite8
    }

    public class AtlasDemo : CCLayer
    {
        //protected:

        public AtlasDemo()
        {

        }

        public enum LabelTestConstant
        {
            IDC_NEXT = 100,
            IDC_BACK,
            IDC_RESTART
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 28);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Arial", 16);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, new SEL_MenuHandler(backCallback));
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, new SEL_MenuHandler(restartCallback));
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, new SEL_MenuHandler(nextCallback));

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint();
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1);

        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new AtlasTestScene();
            s.addChild(AtlasTestScene.restartAtlasAction());

            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {

            CCScene s = new AtlasTestScene();

            s.addChild(AtlasTestScene.nextAtlasAction());

            CCDirector.sharedDirector().replaceScene(s);

        }

        public void backCallback(CCObject pSender)
        {

            CCScene s = new AtlasTestScene();

            s.addChild(AtlasTestScene.backAtlasAction());

            CCDirector.sharedDirector().replaceScene(s);

        }

    }

    public class Atlas1 : AtlasDemo
    {
        CCTextureAtlas m_textureAtlas;

        public Atlas1()
        {
            m_textureAtlas = CCTextureAtlas.textureAtlasWithFile(TestResource.s_AtlasTest, 3);
            //m_textureAtlas.retain();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            //
            // Notice: u,v tex coordinates are inverted
            //
            //ccV3F_C4B_T2F_Quad quads[] = 
            //{
            //    {
            //        {{0,0,0},new ccColor4B(0,0,255,255),{0.0f,1.0f},},				// bottom left
            //        {{s.width,0,0},new ccColor4B(0,0,255,0),{1.0f,1.0f},},			// bottom right
            //        {{0,s.height,0},new ccColor4B(0,0,255,0),{0.0f,0.0f},},	    // top left
            //        {{s.width,s.height,0},{0,0,255,255},{1.0f,0.0f},},	                // top right
            //    },		
            //    {
            //        {{40,40,0}, new ccColor4B(255,255,255,255),{0.0f,0.2f},},			// bottom left
            //        {{120,80,0},new ccColor4B(255,0,0,255),{0.5f,0.2f},},			        // bottom right
            //        {{40,160,0},new ccColor4B(255,255,255,255),{0.0f,0.0f},},		    // top left
            //        {{160,160,0},new ccColor4B(0,255,0,255),{0.5f,0.0f},},			    // top right
            //    },

            //    {
            //        {{s.width/2,40,0},new ccColor4B(255,0,0,255),{0.0f,1.0f},},		         // bottom left
            //        {{s.width,40,0},new ccColor4B(0,255,0,255),{1.0f,1.0f},},		        // bottom right
            //        {{s.width/2-50,200,0},new ccColor4B(0,0,255,255),{0.0f,0.0f},},		// top left
            //        {{s.width,100,0},new ccColor4B(255,255,0,255),{1.0f,0.0f},},		    // top right
            //    },		
            //};

            //for( int i=0;i<3;i++) 
            //{
            //    m_textureAtlas.updateQuad(&quads[i], i);
            //}
        }

        public override string title()
        {
            return "CCTextureAtlas";
        }

        public override string subtitle()
        {
            return "Manual creation of CCTextureAtlas";
        }

        public override void draw()
        {
            // GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // GL_TEXTURE_2D

            m_textureAtlas.drawQuads();

            //	[textureAtlas drawNumberOfQuads:3];
        }

    }

    public class LabelAtlasTest : AtlasDemo
    {
        //ccTime m_time;
        float m_time;

        public LabelAtlasTest()
        {
            m_time = 0;

            CCLabelAtlas label1 = CCLabelAtlas.labelWithString("123 Test", "Images/tuffy_bold_italic-charmap", 48, 64, ' ');
            addChild(label1, 0, (int)TagSprite.kTagSprite1);
            label1.position = new CCPoint(10, 100);
            label1.Opacity = 200;

            CCLabelAtlas label2 = CCLabelAtlas.labelWithString("0123456789", "Images/tuffy_bold_italic-charmap", 48, 64, ' ');
            addChild(label2, 0, (int)TagSprite.kTagSprite2);
            label2.position = new CCPoint(10, 200);
            label2.Opacity = 32;

            //schedule(schedule_selector(LabelAtlasTest.step)); 
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[12] = {0};
            string Stepstring;

            //sprintf(Stepstring, "%2.2f Test", m_time);
            Stepstring = string.Format("{0,2:f2} Test", m_time);
            //Stepstring.format("%2.2f Test", m_time);

            CCLabelAtlas label1 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagSprite1);
            label1.setString(Stepstring);

            CCLabelAtlas label2 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagSprite2);
            //sprintf(Stepstring, "%d", (int)m_time);
            Stepstring = m_time.ToString();
            //Stepstring.format("%d", (int)m_time);
            label2.setString(Stepstring);
        }

        public override string title()
        {
            return "LabelAtlas";
        }

        public override string subtitle()
        {
            return "Updating label should be fast";
        }

    }

    public class LabelAtlasColorTest : AtlasDemo
    {
        //ccTime m_time;
        float m_time;
        ccColor3B ccRED = new ccColor3B
        {
            r = 255,
            g = 0,
            b = 0
        };

        public LabelAtlasColorTest()
        {
            CCLabelAtlas label1 = CCLabelAtlas.labelWithString("123 Test", "fonts/tuffy_bold_italic-charmap.png", 48, 64, ' ');
            addChild(label1, 0, (int)TagSprite.kTagSprite1);
            label1.position = new CCPoint(10, 100);
            label1.Opacity = 200;

            CCLabelAtlas label2 = CCLabelAtlas.labelWithString("0123456789", "fonts/tuffy_bold_italic-charmap.png", 48, 64, ' ');
            addChild(label2, 0, (int)TagSprite.kTagSprite2);
            label2.position = new CCPoint(10, 200);
            //label2.setColor( ccRED );
            label2.ccColor = ccRED;

            CCActionInterval fade = CCFadeOut.actionWithDuration(1.0f);
            //CCActionInterval fade_in = fade.reverse();
            CCActionInterval fade_in = null;
            CCFiniteTimeAction seq = CCSequence.actions(fade, fade_in, null);
            CCAction repeat = CCRepeatForever.actionWithAction((CCActionInterval)seq);
            label2.runAction(repeat);

            m_time = 0;

            //schedule( schedule_selector(LabelAtlasColorTest.step) ); //:@selector(step:)];
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[12] = {0};
            string stepstring;
            //sprintf(string, "%2.2f Test", m_time);
            stepstring = string.Format("{0,2:f2} Test", m_time);
            //std::string string = std::string::stringWithFormat("%2.2f Test", m_time);
            CCLabelAtlas label1 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagSprite1);
            label1.setString(stepstring);

            CCLabelAtlas label2 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagSprite2);
            //sprintf(string, "%d", (int)m_time);
            stepstring = m_time.ToString();
            label2.setString(stepstring);
        }

        public override string title()
        {
            return "CCLabelAtlas";
        }

        public override string subtitle()
        {
            return "Opacity + Color should work at the same time";
        }

    }

    public class Atlas4 : AtlasDemo
    {
        //ccTime m_time;
        float m_time;

        public Atlas4()
        {
            m_time = 0;

            // Upper Label
            CCLabelBMFont label = CCLabelBMFont.labelWithString("Bitmap Font Atlas", "fonts/bitmapFontTest.fnt");
            addChild(label);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            label.position = new CCPoint(s.width / 2, s.height / 2);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);


            CCSprite BChar = (CCSprite)label.getChildByTag(1);
            CCSprite FChar = (CCSprite)label.getChildByTag(7);
            CCSprite AChar = (CCSprite)label.getChildByTag(12);


            CCActionInterval rotate = CCRotateBy.actionWithDuration(2, 360);
            CCAction rot_4ever = CCRepeatForever.actionWithAction(rotate);

            CCActionInterval scale = CCScaleBy.actionWithDuration(2, 1.5f);
            //CCActionInterval scale_back = scale.reverse();
            CCActionInterval scale_back = null;
            CCFiniteTimeAction scale_seq = CCSequence.actions(scale, scale_back, null);
            CCAction scale_4ever = CCRepeatForever.actionWithAction((CCActionInterval)scale_seq);

            CCActionInterval jump = CCJumpBy.actionWithDuration(0.5f, new CCPoint(), 60, 1);
            CCAction jump_4ever = CCRepeatForever.actionWithAction(jump);

            CCActionInterval fade_out = CCFadeOut.actionWithDuration(1);
            CCActionInterval fade_in = CCFadeIn.actionWithDuration(1);
            CCFiniteTimeAction seq = CCSequence.actions(fade_out, fade_in, null);
            CCAction fade_4ever = CCRepeatForever.actionWithAction((CCActionInterval)seq);

            BChar.runAction(rot_4ever);
            BChar.runAction(scale_4ever);
            FChar.runAction(jump_4ever);
            AChar.runAction(fade_4ever);


            // Bottom Label
            CCLabelBMFont label2 = CCLabelBMFont.labelWithString("00.0", "fonts/bitmapFontTest.fnt");
            addChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            label2.position = new CCPoint(s.width / 2.0f, 80);

            CCSprite lastChar = (CCSprite)label2.getChildByTag(3);
            lastChar.runAction((CCAction)(rot_4ever.copy()));

            //schedule( schedule_selector(Atlas4::step), 0.1f);
            base.schedule(step, 0.1f);
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[10] = {0};
            string Stepstring;
            //sprintf(string, "%04.1f", m_time);
            Stepstring = string.Format("{0,4:1f}", m_time);
            // 	std::string string;
            // 	string.format("%04.1f", m_time);

            CCLabelBMFont label1 = (CCLabelBMFont)getChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label1.setString(Stepstring);
        }

        public override void draw()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            //ccDrawLine(new CCPoint(0, s.height / 2), new CCPoint(s.width, s.height / 2));
            //ccDrawLine(new CCPoint(s.width / 2, 0), new CCPoint(s.width / 2, s.height));
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Using fonts as CCSprite objects. Some characters should rotate.";
        }
    }

    public class Atlas5 : AtlasDemo
    {

        public Atlas5()
        {
            CCLabelBMFont label = CCLabelBMFont.labelWithString("abcdefg", "fonts/bitmapFontTest4.fnt");
            addChild(label);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            label.position = new CCPoint(s.width / 2, s.height / 2);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);
        }
        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing padding";
        }

    }

    public class Atlas6 : AtlasDemo
    {

        public Atlas6()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelBMFont label = null;
            label = CCLabelBMFont.labelWithString("FaFeFiFoFu", "fonts/bitmapFontTest5.fnt");
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height / 2 + 50);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.labelWithString("fafefifofu", "fonts/bitmapFontTest5.fnt");
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height / 2);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.labelWithString("aeiou", "fonts/bitmapFontTest5.fnt");
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height / 2 - 50);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Rendering should be OK. Testing offset";
        }

    }

    public class AtlasBitmapColor : AtlasDemo
    {
        ccColor3B ccBLUE = new ccColor3B
      {
          r = 0,
          g = 0,
          b = 255
      };

        ccColor3B ccRED = new ccColor3B
       {
           r = 255,
           g = 0,
           b = 0
       };

        ccColor3B ccGREEN = new ccColor3B
       {
           r = 0,
           g = 255,
           b = 0
       };
        public AtlasBitmapColor()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelBMFont label = null;
            label = CCLabelBMFont.labelWithString("Blue", "fonts/bitmapFontTest5.fnt");
            label.setColor(ccBLUE);
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height / 4);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.labelWithString("Red", "fonts/bitmapFontTest5.fnt");
            addChild(label);
            label.position = new CCPoint(s.width / 2, 2 * s.height / 4);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);
            label.setColor(ccRED);

            label = CCLabelBMFont.labelWithString("G", "fonts/bitmapFontTest5.fnt");
            addChild(label);
            label.position = new CCPoint(s.width / 2, 3 * s.height / 4);
            label.anchorPoint = new CCPoint(0.5f, 0.5f);
            label.setColor(ccGREEN);
            label.setString("Green");
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing color";
        }

    }

    public class AtlasFastBitmap : AtlasDemo
    {

        public AtlasFastBitmap()
        {
            // Upper Label
            for (int i = 0; i < 100; i++)
            {
                //char str[6] = {0};
                string str;
                //sprintf(str, "-%d-", i);
                str = string.Format("-{0,d}-", i);
                CCLabelBMFont label = CCLabelBMFont.labelWithString(str, "fonts/bitmapFontTest.fnt");
                addChild(label);

                CCSize s = CCDirector.sharedDirector().getWinSize();

                CCPoint p = new CCPoint(ccMacros.CCRANDOM_0_1() * s.width, ccMacros.CCRANDOM_0_1() * s.height);
                label.position = p;
                label.anchorPoint = new CCPoint(0.5f, 0.5f);
            }
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Creating several CCLabelBMFont with the same .fnt file should be fast";
        }

    }

    public class BitmapFontMultiLine : AtlasDemo
    {
        public BitmapFontMultiLine()
        {
            CCSize s;

            // Left
            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("Multi line\nLeft", "fonts/bitmapFontTest3.fnt");
            label1.anchorPoint = new CCPoint(0, 0);
            addChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

            s = label1.contentSize;

            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            Debug.WriteLine("content size: {0,0:2f}x{1,0:2f}", s.width, s.height);


            // Center
            CCLabelBMFont label2 = CCLabelBMFont.labelWithString("Multi line\nCenter", "fonts/bitmapFontTest3.fnt");
            label2.anchorPoint = new CCPoint(0.5f, 0.5f);
            addChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

            s = label2.contentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            Debug.WriteLine("content size: {0,0:2f}x{1,0:2f}", s.width, s.height);

            // right
            CCLabelBMFont label3 = CCLabelBMFont.labelWithString("Multi line\nRight\nThree lines Three", "fonts/bitmapFontTest3.fnt");
            label3.anchorPoint = new CCPoint(1, 1);
            addChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            s = label3.contentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);

            s = CCDirector.sharedDirector().getWinSize();
            label1.position = new CCPoint();
            label2.position = new CCPoint(s.width / 2, s.height / 2);
            label3.position = new CCPoint(s.width, s.height);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Multiline + anchor point";
        }
    }

    public class LabelsEmpty : AtlasDemo
    {

        public LabelsEmpty()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("", "fonts/bitmapFontTest3.fnt");
            addChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);
            label1.position = new CCPoint(s.width / 2, s.height - 100);

            // CCLabelTTF
            CCLabelTTF label2 = CCLabelTTF.labelWithString("", "Arial", 24);
            addChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            label2.position = new CCPoint(s.width / 2, s.height / 2);

            // CCLabelAtlas
            CCLabelAtlas label3 = CCLabelAtlas.labelWithString("", "fonts/tuffy_bold_italic-charmap.png", 48, 64, ' ');
            addChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);
            label3.position = new CCPoint(s.width / 2, 0 + 100);

            base.schedule(updateStrings, 1.0f);

            setEmpty = false;
        }

        public void updateStrings(float dt)
        {
            CCLabelBMFont label1 = (CCLabelBMFont)getChildByTag((int)TagSprite.kTagBitmapAtlas1);
            CCLabelTTF label2 = (CCLabelTTF)getChildByTag((int)TagSprite.kTagBitmapAtlas2);
            CCLabelAtlas label3 = (CCLabelAtlas)getChildByTag((int)TagSprite.kTagBitmapAtlas3);

            if (!setEmpty)
            {
                label1.setString("not empty");
                label2.setString("not empty");
                label3.setString("hi");

                setEmpty = true;
            }
            else
            {
                label1.setString("");
                label2.setString("");
                label3.setString("");

                setEmpty = false;
            }
        }

        public override string title()
        {
            return "Testing empty labels";
        }

        public override string subtitle()
        {
            return "3 empty labels: LabelAtlas, LabelTTF and LabelBMFont";
        }

        private bool setEmpty;

    }

    public class LabelBMFontHD : AtlasDemo
    {

        public LabelBMFontHD()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("TESTING RETINA DISPLAY", "fonts/konqa32.fnt");
            addChild(label1);
            label1.position = new CCPoint(s.width / 2, s.height / 2);
        }

        public override string title()
        {
            return "Testing Retina Display BMFont";
        }

        public override string subtitle()
        {
            return "loading arista16 or arista16-hd";
        }

    }

    public class LabelAtlasHD : AtlasDemo
    {
        public LabelAtlasHD()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelAtlas label1 = CCLabelAtlas.labelWithString("TESTING RETINA DISPLAY", "fonts/larabie-16.png", 10, 20, 'A');
            label1.anchorPoint = new CCPoint(0.5f, 0.5f);

            addChild(label1);
            label1.position = new CCPoint(s.width / 2, s.height / 2);
        }

        public override string title()
        {
            return "LabelAtlas with Retina Display";
        }

        public override string subtitle()
        {
            return "loading larabie-16 / larabie-16-hd";
        }
    }

    public class LabelGlyphDesigner : AtlasDemo
    {

        public LabelGlyphDesigner()
        {
            //CCSize s = CCDirector.sharedDirector().getWinSize();

            //CCLayerColor layer = CCLayerColor.layerWithColor(new ccColor4B(128, 128, 128, 255));
            //addChild(layer, -10);

            //// CCLabelBMFont
            //CCLabelBMFont label1 = CCLabelBMFont.labelWithString("Testing Glyph Designer", "fonts/futura-48.fnt");
            //addChild(label1);
            //label1.position = new CCPoint(s.width / 2, s.height / 2);
        }

        public override string title()
        {
            return "Testing Glyph Designer";
        }

        public override string subtitle()
        {
            return "You should see a font with shawdows and outline";
        }

        //  void AtlasTestScene::runThisTest()
        //{
        //    CCLayer pLayer = nextAtlasAction();
        //    addChild(pLayer);

        //    CCDirector.sharedDirector().replaceScene(this);
        //}

    }



    public class LabelTTFTest : AtlasDemo
    {
        public LabelTTFTest()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelTTF left = CCLabelTTF.labelWithString("align left", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentLeft, "Arial", 32);
            CCLabelTTF center = CCLabelTTF.labelWithString("align center", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentCenter, "Arial", 32);
            CCLabelTTF right = CCLabelTTF.labelWithString("align right", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentRight, "Arial", 32);

            left.position = new CCPoint(s.width / 2, 200);
            center.position = new CCPoint(s.width / 2, 150);
            right.position = new CCPoint(s.width / 2, 100);

            addChild(left);
            addChild(center);
            addChild(right);
        }

        public override string title()
        {
            return "Testing CCLabelTTF";
        }
        public override string subtitle()
        {
            return "You should see 3 labels aligned left, center and right";
        }

    }

    public class LabelTTFMultiline : AtlasDemo
    {

        public LabelTTFMultiline()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // CCLabelBMFont
            CCLabelTTF center = CCLabelTTF.labelWithString("word wrap \"testing\" (bla0) bla1 'bla2' [bla3] (bla4) {bla5} {bla6} [bla7] (bla8) [bla9] 'bla0' \"bla1\"",
                new CCSize(s.width / 2, 200), CCTextAlignment.CCTextAlignmentCenter, "MarkerFelt.ttc", 32);
            center.position = new CCPoint(s.width / 2, 150);

            addChild(center);
        }

        public override string title()
        {
            return "Testing CCLabelTTF Word Wrap";
        }

        public override string subtitle()
        {
            return "Word wrap using CCLabelTTF";
        }

    }

    public class LabelTTFChinese : AtlasDemo
    {

        public LabelTTFChinese()
        {
            CCSize size = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF pLable = CCLabelTTF.labelWithString("ÖÐ¹ú", "Marker Felt", 30);
            pLable.position = new CCPoint(size.width / 2, size.height / 2);
            this.addChild(pLable);
        }

        public override string title()
        {
            return "Testing CCLabelTTF with Chinese character";
        }

    }
}
