using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace cocos2d
{
    //@brief	TextInputTest for retain prev, reset, next, main menu buttons.
    public class TextInputTest : CCLayer
    {
        KeyboardNotificationLayer m_pNotificationLayer;
        TextInputTestScene textinputTestScene = new TextInputTestScene();

        public TextInputTest()
        {

        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new TextInputTestScene();
            s.addChild(textinputTestScene.restartTextInputTest());
            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new TextInputTestScene();
            s.addChild(textinputTestScene.nextTextInputTest());
            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new TextInputTestScene();
            s.addChild(textinputTestScene.backTextInputTest());
            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }

        public virtual string title()
        {
            return "text input test";
        }

        public void addKeyboardNotificationLayer(KeyboardNotificationLayer pLayer)
        {
            m_pNotificationLayer = pLayer;
            addChild(pLayer);
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 24);
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height - 50);

            string subTitle = m_pNotificationLayer.subtitle();
            if (subTitle != null)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(subTitle, "Thonburi", 16);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage("Images/b1.png", "Images/b2.png", this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage("Images/r1.png", "Images/r2.png", this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage("Images/f1.png", "Images/f2.png", this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);
            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1);
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // KeyboardNotificationLayer for test IME keyboard notification.
    //////////////////////////////////////////////////////////////////////////

    public class KeyboardNotificationLayer : CCLayer
    {
        public KeyboardNotificationLayer()
        {
            base.isTouchEnabled = true;
        }

        public string subtitle()
        {
            throw new NotFiniteNumberException();
        }

        public void onClickTrackNode(bool bClicked)
        {
            throw new NotFiniteNumberException();
        }

        public virtual void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, 0, false);
        }

        public virtual void keyboardWillShow(CCIMEKeyboardNotificationInfo info)
        {
            CCLog.Log("TextInputTest:keyboardWillShowAt(origin:%f,%f, size:%f,%f)",
        info.end.origin.x, info.end.origin.y, info.end.size.width, info.end.size.height);

            if (m_pTrackNode != null)
            {
                return;
            }

            CCRect rectTracked = TextInputTestScene.getRect(m_pTrackNode);
            CCLog.Log("TextInputTest:trackingNodeAt(origin:%f,%f, size:%f,%f)",
                rectTracked.origin.x, rectTracked.origin.y, rectTracked.size.width, rectTracked.size.height);

            // if the keyboard area doesn't intersect with the tracking node area, nothing need to do.
            if (!CCRect.CCRectIntersetsRect(rectTracked, info.end))
            {
                return;
            }

            // assume keyboard at the bottom of screen, calculate the vertical adjustment.
            float adjustVert = CCRect.CCRectGetMaxY(info.end) - CCRect.CCRectGetMinY(rectTracked);
            CCLog.Log("TextInputTest:needAdjustVerticalPosition(%f)", adjustVert);

            // move all the children node of KeyboardNotificationLayer
            CCNode ccnoed = new CCNode();

            List<CCNode> children = ccnoed.children;
            CCNode node;
            int count = children.Count;
            CCPoint pos;
            for (int i = 0; i < count; ++i)
            {
                node = (CCNode)children[i];
                pos = node.position;
                pos.y += adjustVert;
                node.position = pos;
            }
        }

        // CCLayer
        public override bool ccTouchBegan(CCTouch pTouch, CCEvent pEvent)
        {
            CCLog.Log("++++++++++++++++++++++++++++++++++++++++++++");
            m_beginPos = pTouch.locationInView(pTouch.view());
            m_beginPos = CCDirector.sharedDirector().convertToGL(m_beginPos);
            return true;
        }

        public override void ccTouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
            if (m_pTrackNode != null)
            {
                return;
            }

            CCPoint endPos = pTouch.locationInView(pTouch.view());
            endPos = CCDirector.sharedDirector().convertToGL(endPos);

            float delta = 5.0f;
            if (Math.Abs(endPos.x - m_beginPos.x) > delta
                || Math.Abs(endPos.y - m_beginPos.y) > delta)
            {
                // not click
                m_beginPos.x = m_beginPos.y = -1;
                return;
            }

            // decide the trackNode is clicked.
            CCRect rect;
            CCPoint point = convertTouchToNodeSpaceAR(pTouch);
            CCLog.Log("KeyboardNotificationLayer:clickedAt(%f,%f)", point.x, point.y);

            rect = TextInputTestScene.getRect(m_pTrackNode);
            CCLog.Log("KeyboardNotificationLayer:TrackNode at(origin:%f,%f, size:%f,%f)",
                rect.origin.x, rect.origin.y, rect.size.width, rect.size.height);

            this.onClickTrackNode(CCRect.CCRectContainsPoint(rect, point));
            CCLog.Log("----------------------------------");
        }

        protected CCNode m_pTrackNode;
        protected CCPoint m_beginPos;
    }

    //////////////////////////////////////////////////////////////////////////
    // TextFieldTTFDefaultTest for test TextFieldTTF default behavior.
    //////////////////////////////////////////////////////////////////////////

    public class TextFieldTTFDefaultTest : KeyboardNotificationLayer
    {
        // KeyboardNotificationLayer
        public virtual string subtitle()
        {
            return "TextFieldTTF with default behavior test";
        }

        public virtual void onClickTrackNode(bool bClicked)
        {
            CCTextFieldTTF pTextField = (CCTextFieldTTF)m_pTrackNode;
            if (bClicked)
            {
                // TextFieldTTFTest be clicked
                CCLog.Log("TextFieldTTFDefaultTest:CCTextFieldTTF attachWithIME");
                pTextField.attachWithIME();
            }
            else
            {
                // TextFieldTTFTest not be clicked
                CCLog.Log("TextFieldTTFDefaultTest:CCTextFieldTTF detachWithIME");
                pTextField.detachWithIME();
            }
        }

        // CCLayer
        public override void onEnter()
        {
            base.onEnter();

            // add CCTextFieldTTF
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCTextFieldTTF pTextField = CCTextFieldTTF.textFieldWithPlaceHolder("<click here for input>",
                TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            addChild(pTextField);

            //#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)	
            //    // on android, CCTextFieldTTF cannot auto adjust its position when soft-keyboard pop up
            //    // so we had to set a higher position to make it visable
            //    pTextField->setPosition(ccp(s.width / 2, s.height/2 + 50));
            //#else
            //    pTextField->setPosition(ccp(s.width / 2, s.height / 2));
            //#endif

            m_pTrackNode = pTextField;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // TextFieldTTFActionTest
    //////////////////////////////////////////////////////////////////////////

    public class TextFieldTTFActionTest : KeyboardNotificationLayer
    {
        CCTextFieldTTF m_pTextField;
        CCAction m_pTextFieldAction;
        bool m_bAction;
        int m_nCharLimit;       // the textfield max char limit

        public void callbackRemoveNodeWhenDidAction(CCNode pNode)
        {
            this.removeChild(pNode, true);
        }

        // KeyboardNotificationLayer
        public virtual string subtitle()
        {
            return "CCTextFieldTTF with action and char limit test";
        }

        public virtual void onClickTrackNode(bool bClicked)
        {
            CCTextFieldTTF pTextField = (CCTextFieldTTF)m_pTrackNode;
            if (bClicked)
            {
                // TextFieldTTFTest be clicked
                CCLog.Log("TextFieldTTFActionTest:CCTextFieldTTF attachWithIME");
                pTextField.attachWithIME();
            }
            else
            {
                // TextFieldTTFTest not be clicked
                CCLog.Log("TextFieldTTFActionTest:CCTextFieldTTF detachWithIME");
                pTextField.detachWithIME();
            }
        }

        // CCLayer
        public override void onEnter()
        {
            base.onEnter();

            m_nCharLimit = 12;

            m_pTextFieldAction = CCRepeatForever.actionWithAction(
                (CCActionInterval)CCSequence.actions(
                    CCFadeOut.actionWithDuration(0.25f),
                    CCFadeIn.actionWithDuration(0.25f)));
            //m_pTextFieldAction->retain();
            m_bAction = false;

            // add CCTextFieldTTF
            CCSize s = CCDirector.sharedDirector().getWinSize();

            m_pTextField = CCTextFieldTTF.textFieldWithPlaceHolder("<click here for input>",
            TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            addChild(m_pTextField);

            //m_pTextField.setDelegate(this);


            //#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)	
            //    // on android, CCTextFieldTTF cannot auto adjust its position when soft-keyboard pop up
            //    // so we had to set a higher position
            //    m_pTextField->setPosition(new CCPoint(s.width / 2, s.height/2 + 50));
            //#else
            //    m_pTextField->setPosition(ccp(s.width / 2, s.height / 2));
            //#endif

            m_pTrackNode = m_pTextField;
        }

        public override void onExit()
        {
            base.onExit();
            //m_pTextFieldAction->release();
        }

        // CCTextFieldDelegate
        public virtual bool onTextFieldAttachWithIME(CCTextFieldTTF pSender)
        {
            if (m_bAction != null)
            {
                m_pTextField.runAction(m_pTextFieldAction);
                m_bAction = true;
            }
            return false;
        }

        public virtual bool onTextFieldDetachWithIME(CCTextFieldTTF pSender)
        {
            if (m_bAction != null)
            {
                m_pTextField.stopAction(m_pTextFieldAction);
                m_pTextField.Opacity = 255;
                m_bAction = false;
            }
            return false;
        }

        public virtual bool onTextFieldInsertText(CCTextFieldTTF pSender, string text, int nLen)
        {
            // if insert enter, treat as default to detach with ime
            if ("\n" == text)
            {
                return false;
            }

            // if the textfield's char count more than m_nCharLimit, doesn't insert text anymore.
            if (pSender.CharCount >= m_nCharLimit)
            {
                return true;
            }

            // create a insert text sprite and do some action
            CCLabelTTF label = CCLabelTTF.labelWithString(text, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            this.addChild(label);
            ccColor3B color = new ccColor3B { r = 226, g = 121, b = 7 };
            label.Color = color;

            // move the sprite from top to position
            CCPoint endPos = pSender.position;
            if (pSender.CharCount > 0)
            {
                endPos.x += pSender.contentSize.width / 2;
            }
            CCSize inputTextSize = label.contentSize;
            CCPoint beginPos = new CCPoint(endPos.x, CCDirector.sharedDirector().getWinSize().height - inputTextSize.height * 2);

            float duration = 0.5f;
            label.position = beginPos;
            label.scale = 8;

            CCAction seq = CCSequence.actions(
                CCSpawn.actions(
                    CCMoveTo.actionWithDuration(duration, endPos),
                    CCScaleTo.actionWithDuration(duration, 1),
                    CCFadeOut.actionWithDuration(duration)),
                CCCallFuncN.actionWithTarget(this, callbackRemoveNodeWhenDidAction));
            label.runAction(seq);
            return false;
        }

        public virtual bool onTextFieldDeleteBackward(CCTextFieldTTF pSender, string delText, int nLen)
        {
            // create a delete text sprite and do some action
            CCLabelTTF label = CCLabelTTF.labelWithString(delText, TextInputTestScene.FONT_NAME, TextInputTestScene.FONT_SIZE);
            this.addChild(label);

            // move the sprite to fly out
            CCPoint beginPos = pSender.position;
            CCSize textfieldSize = pSender.contentSize;
            CCSize labelSize = label.contentSize;
            beginPos.x += (textfieldSize.width - labelSize.width) / 2.0f;

            int RAND_MAX = 32767;
            Random rand = new Random();

            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            CCPoint endPos = new CCPoint(-winSize.width / 4.0f, winSize.height * (0.5f + (float)rand.Next() / (2.0f * RAND_MAX)));
            float duration = 1;
            float rotateDuration = 0.2f;
            int repeatTime = 5;
            label.position = beginPos;

            CCAction seq = CCSequence.actions(
                CCSpawn.actions(
                    CCMoveTo.actionWithDuration(duration, endPos),
                    CCRepeat.actionWithAction(
                        CCRotateBy.actionWithDuration(rotateDuration, (rand.Next() % 2 > 0) ? 360 : -360),
                        (uint)repeatTime),
                    CCFadeOut.actionWithDuration(duration)),
                CCCallFuncN.actionWithTarget(this, callbackRemoveNodeWhenDidAction));
            label.runAction(seq);
            return false;
        }

        public virtual bool onDraw(CCTextFieldTTF pSender)
        {
            return false;
        }
    }
}
