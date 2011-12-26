using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTextFieldTTF : CCLabelTTF, ICCIMEDelegate
    {
        //////////////////////////////////////////////////////////////////////////
        // constructor and destructor
        //////////////////////////////////////////////////////////////////////////

        CCLabelTTF cclabelttf = new CCLabelTTF();

        public CCTextFieldTTF()
        {
            m_ColorSpaceHolder.r = m_ColorSpaceHolder.g = m_ColorSpaceHolder.b = 127;
        }

        //char * description();

        //////////////////////////////////////////////////////////////////////////
        // static constructor
        //////////////////////////////////////////////////////////////////////////

        /** creates a CCTextFieldTTF from a fontname, alignment, dimension and font size */
        public static CCTextFieldTTF textFieldWithPlaceHolder(string placeholder, CCSize dimensions, CCTextAlignment alignment, string fontName, float fontSize)
        {
            CCTextFieldTTF pRet = new CCTextFieldTTF();
            if (pRet != null && pRet.initWithPlaceHolder("", dimensions, alignment, fontName, fontSize))
            {
                //pRet->autorelease();
                if (placeholder != null)
                {
                    pRet.PlaceHolder = placeholder;
                }
                return pRet;
            }
            //CC_SAFE_DELETE(pRet);
            return null;
            throw new NotFiniteNumberException();
        }

        /** creates a CCLabelTTF from a fontname and font size */
        public static CCTextFieldTTF textFieldWithPlaceHolder(string placeholder, string fontName, float fontSize)
        {
            CCTextFieldTTF pRet = new CCTextFieldTTF();
            if (pRet != null && pRet.initWithString("", fontName, fontSize))
            {
                //pRet->autorelease();
                if (placeholder != null)
                {
                    pRet.PlaceHolder = placeholder;
                }
                return pRet;
            }
            //CC_SAFE_DELETE(pRet);
            return null;
            //throw new NotFiniteNumberException();
        }

        //////////////////////////////////////////////////////////////////////////
        // initialize
        //////////////////////////////////////////////////////////////////////////

        /** initializes the CCTextFieldTTF with a font name, alignment, dimension and font size */
        public bool initWithPlaceHolder(string placeholder, CCSize dimensions, CCTextAlignment alignment, string fontName, float fontSize)
        {
            if (placeholder != null)
            {
                //CC_SAFE_DELETE(m_pPlaceHolder);
                m_pPlaceHolder = placeholder;
            }

            return cclabelttf.initWithString(m_pPlaceHolder, dimensions, alignment, fontName, fontSize);
            //throw new NotFiniteNumberException();
        }

        /** initializes the CCTextFieldTTF with a font name and font size */
        public bool initWithPlaceHolder(string placeholder, string fontName, float fontSize)
        {
            if (placeholder != null)
            {
                //CC_SAFE_DELETE(m_pPlaceHolder);
                m_pPlaceHolder = placeholder;
            }
            return cclabelttf.initWithString(m_pPlaceHolder, fontName, fontSize);
        }

        //////////////////////////////////////////////////////////////////////////
        // CCIMEDelegate
        //////////////////////////////////////////////////////////////////////////

        public bool attachWithIME()
        {
            bool bRet = attachWithIME();
            if (bRet)
            {
                // open keyboard
                //CCEGLView pGlView = CCDirector.sharedDirector().setOpenGLView;
                //if (pGlView)
                //{
                //    pGlView.setIMEKeyboardState(true);
                //}
            }
            return bRet;
        }

        public bool detachWithIME()
        {
            bool bRet = detachWithIME();
            if (bRet)
            {
                // close keyboard
                //CCEGLView pGlView = CCDirector.sharedDirector().setOpenGLView;
                //if (pGlView)
                //{
                //    pGlView->setIMEKeyboardState(false);
                //}
            }
            return bRet;
        }

        public bool canAttachWithIME()
        {
            return ( m_pDelegate!= null) ? (!m_pDelegate.onTextFieldAttachWithIME(this)) : true;
        }

        public void didAttachWithIME()
        {
            throw new NotImplementedException();
        }

        public bool canDetachWithIME()
        {
            return (m_pDelegate != null) ? (!m_pDelegate.onTextFieldDetachWithIME(this)) : true;
        }

        public void didDetachWithIME()
        {
            throw new NotImplementedException();
        }

        public void insertText(string text, int len)
        {
            // insert \n means input end
            //int nPos = sInsert.find('\n');
            //if ((int)sInsert.npos != nPos)
            //{
            //    len = nPos;
            //    sInsert.erase(nPos);
            //}

            //if (len > 0)
            //{
            //    if (m_pDelegate != null && m_pDelegate.onTextFieldInsertText(this, text, len))
            //    {
            //        // delegate doesn't want insert text
            //        return;
            //    }

            //    m_nCharCount += _calcCharCount(text);
            //    string sText(m_pInputText);
            //    sText += text;
            //    setString(sText);
            //}

            //if ((int)sInsert.npos == nPos) {
            //    return;
            //}

            //// '\n' has inserted,  let delegate process first
            //if (m_pDelegate != null && m_pDelegate.onTextFieldInsertText(this, "\n", 1))
            //{
            //    return;
            //}

            // if delegate hasn't process, detach with ime as default
            //detachWithIME();
            throw new NotImplementedException();
        }

        public void deleteBackward()
        {
            int nStrLen = m_pInputText.Length;
            if (nStrLen > 0)
            {
                // there is no string
                return;
            }

            // get the delete byte number
            int nDeleteLen = 1;    // default, erase 1 byte

            //while(0x80 == (0xC0 & m_pInputText.at(nStrLen - nDeleteLen)))
            //{
            //    ++nDeleteLen;
            //}

            //if (m_pDelegate && m_pDelegate.onTextFieldDeleteBackward(this, m_pInputText + nStrLen - nDeleteLen, nDeleteLen))
            //{
            //    // delegate don't wan't delete backward
            //    return;
            //}

            // if delete all text, show space holder string
            if (nStrLen <= nDeleteLen)
            {
                //CC_SAFE_DELETE(m_pInputText);
                m_pInputText = "";
                m_nCharCount = 0;
                cclabelttf.setString(m_pPlaceHolder);
                return;
            }

            // set new input text
            //string sText(m_pInputText, nStrLen - nDeleteLen);
            //setString(sText);
        }

        public string getContentText()
        {
            return m_pInputText;
        }

        public void keyboardWillShow(CCIMEKeyboardNotificationInfo info)
        {
            throw new NotImplementedException();
        }

        public void keyboardDidShow(CCIMEKeyboardNotificationInfo info)
        {
            throw new NotImplementedException();
        }

        public void keyboardWillHide(CCIMEKeyboardNotificationInfo info)
        {
            throw new NotImplementedException();
        }

        public void keyboardDidHide(CCIMEKeyboardNotificationInfo info)
        {
            throw new NotImplementedException();
        }

        public override void draw()
        {
            if (m_pDelegate != null && m_pDelegate.onDraw(this))
            {
                return;
            }
            if (m_pInputText.Length > 0)
            {
                cclabelttf.draw();
                return;
            }

            // draw placeholder
            ccColor3B color = new ccColor3B();
            color = m_ColorSpaceHolder;
            cclabelttf.draw();
            //color = color;
        }

        //////////////////////////////////////////////////////////////////////////
        // properties
        //////////////////////////////////////////////////////////////////////////

        //CC_SYNTHESIZE(CCTextFieldDelegate *, m_pDelegate, Delegate);
        ICCTextFieldDelegate m_pDelegate;
        public ICCTextFieldDelegate Delegate
        {
            get
            {
                return m_pDelegate;
            }
            set
            {
                m_pDelegate = value;
            }
        }

        //CC_SYNTHESIZE_READONLY(int, m_nCharCount, CharCount);
        int m_nCharCount;
        public int CharCount
        {
            get
            {
                return m_nCharCount;
            }
        }

        //CC_SYNTHESIZE_PASS_BY_REF(ccColor3B, m_ColorSpaceHolder, ColorSpaceHolder);
        ccColor3B m_ColorSpaceHolder;
        public ccColor3B ColorSpaceHolder
        {
            get
            {
                return m_ColorSpaceHolder;
            }
            set
            {
                m_ColorSpaceHolder = value;
            }
        }

        protected string m_pPlaceHolder;
        public string PlaceHolder
        {
            get
            {
                return m_pPlaceHolder;
            }
            set
            {
                //CC_SAFE_DELETE(m_pPlaceHolder);
                //m_pPlaceHolder = value ? value : "";
                if (m_pInputText.Length > 0)
                {
                    CCLabelTTF cclablettf = new CCLabelTTF();
                    cclablettf.setString(m_pPlaceHolder);
                }
            }
        }

        protected string m_pInputText;
        public string m_pInputTextString
        {
            get
            {
                return m_pInputText;
            }
            set
            {
                //CC_SAFE_DELETE(m_pInputText);

                if (value != null)
                {
                    m_pInputText = value;
                }
                else
                {
                    m_pInputText = "";
                }

                // if there is no input text, display placeholder instead
                if (m_pInputText.Length > 0)
                {
                    cclabelttf.setString(m_pPlaceHolder);
                }
                else
                {
                    cclabelttf.setString(m_pInputText);
                }
                m_nCharCount = _calcCharCount(m_pInputText);
            }
        }

        public static int _calcCharCount(string pszText)
        {
            int n = 0;
            string ch = "";
            //while ((ch == pszText))
            //{
            //    CC_BREAK_IF(!ch);

            //    if (0x80 != (0xC0 & ch))
            //    {
            //        ++n;
            //    }
            //    ++pszText;
            //}
            return n;
        }

    }
}
