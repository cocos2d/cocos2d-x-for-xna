/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    // A CCAnimation object is used to perform animations on the CCSprite objects.
    // The CCAnimation object contains CCSpriteFrame objects, and a possible delay between the frames.
    // You can animate a CCAnimation object by using the CCAnimate action. Example:
    // [sprite runAction:[CCAnimate actionWithAnimation:animation]];
    public class CCAnimation : CCObject
    {
        protected string m_nameStr;
        protected float m_fDelay;
        protected List<CCSpriteFrame> m_pobFrames;

        public CCAnimation()
        {
            // CLOGINFO("cocos2d, deallocing %p", this);
            // [name_ release];
            // m_nameStr.clear();
            m_nameStr = null;
            // CC_SAFE_RELEASE(m_pobFrames);

            m_pobFrames = new List<CCSpriteFrame>();
        }

        /** Initializes a CCAnimation with frames.
        @since v0.99.5
        */
        public bool initWithFrames(List<CCSpriteFrame> pFrames)
        {
            return initWithFrames(pFrames, 0);
        }

        /** Initializes a CCAnimation with frames and a delay between frames
        @since v0.99.5
        */
        public bool initWithFrames(List<CCSpriteFrame> pFrames, float delay)
        {
            m_fDelay = delay;
            m_pobFrames = pFrames;
            return true;
            throw new NotFiniteNumberException();
        }

        /** adds a frame to a CCAnimation */
        public void addFrame(CCSpriteFrame pFrame)
        {
            // m_pobFrames.addObject(pFrame);
            m_pobFrames.Add(pFrame);
        }

        /** Adds a frame with an image filename. Internally it will create a CCSpriteFrame and it will add it.
        Added to facilitate the migration from v0.8 to v0.9.
        */
        public void addFrameWithFileName(string pszFileName)
        {
            CCTexture2D pTexture = CCTextureCache.sharedTextureCache().addImage(pszFileName);
            //CCRect rect = CCRectZero;
            CCRect rect = new CCRect(0, 0, 0, 0);
            rect.size = pTexture.getContentSize();
            CCSpriteFrame pFrame = CCSpriteFrame.frameWithTexture(pTexture, rect);
            //// m_pobFrames.addObject(pFrame);
            m_pobFrames.Add(pFrame);
        }

        /** Adds a frame with a texture and a rect. Internally it will create a CCSpriteFrame and it will add it.
        Added to facilitate the migration from v0.8 to v0.9.
        */
        public void addFrameWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCSpriteFrame pFrame = CCSpriteFrame.frameWithTexture(pobTexture, rect);
            //// m_pobFrames.addObject(pFrame);
            m_pobFrames.Add(pFrame);
            throw new NotFiniteNumberException();
        }

        public bool init()
        {
            return initWithFrames(new List<CCSpriteFrame>(), 0);
        }

        /** Creates an animation
        @since v0.99.5
        */
        public static CCAnimation animation()
        {
            CCAnimation pAnimation = new CCAnimation();
            pAnimation.init();
            // pAnimation->autorelease();

            return pAnimation;
        }

        /** Creates an animation with frames.
        @since v0.99.5
        */
        public static CCAnimation animationWithFrames(List<CCSpriteFrame> frames)
        {
            CCAnimation pAnimation = new CCAnimation();
            pAnimation.initWithFrames(frames);
            // pAnimation->autorelease();

            return pAnimation;
        }

        /* Creates an animation with frames and a delay between frames.
        @since v0.99.5
        */
        public static CCAnimation animationWithFrames(List<CCSpriteFrame> frames, float delay)
        {
            CCAnimation pAnimation = new CCAnimation();
            pAnimation.initWithFrames(frames, delay);
            // pAnimation->autorelease();

            return pAnimation;
        }

        /** get name of the animation */
        public string getName()
        {
            return m_nameStr;
        }

        /** set name of the animation */
        public void setName(string pszName)
        {
            m_nameStr = pszName;
        }

        /** get delay between frames in seconds */
        public float getDelay()
        {
            return m_fDelay;
        }


        /** set delay between frames in seconds */
        public void setDelay(float fDelay)
        {
            m_fDelay = fDelay;
        }

        /** get array of frames */
        public List<CCSpriteFrame> getFrames()
        {
            return m_pobFrames;
        }

        /** set array of frames, the Frames is retained */
        public void setFrames(List<CCSpriteFrame> pFrames)
        {
            m_pobFrames = pFrames;
        }
    }
}
