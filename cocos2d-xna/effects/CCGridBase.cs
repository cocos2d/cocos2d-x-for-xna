/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

http://www.cocos2d-x.org
http://www.openxlive.com

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
using System.Diagnostics;

namespace cocos2d
{

    public class CCGridBase : CCObject
    {
        protected bool m_bActive;
        /// <summary>
        ///  wheter or not the grid is active
        /// </summary>
        public bool Active
        {
            get { return m_bActive; }
            set { m_bActive = value; }
        }

        protected int m_nReuseGrid;
        /// <summary>
        /// number of times that the grid will be reused 
        /// </summary>
        public int ReuseGrid
        {
            get { return m_nReuseGrid; }
            set { m_nReuseGrid = value; }
        }

        protected ccGridSize m_sGridSize;
        /// <summary>
        /// size of the grid 
        /// </summary>
        public ccGridSize GridSize
        {
            get { return m_sGridSize; }
            set { m_sGridSize = value; }
        }

        protected CCPoint m_obStep;
        /// <summary>
        /// pixels between the grids 
        /// </summary>
        public CCPoint Step
        {
            get { return m_obStep; }
            set { m_obStep = value; }
        }

        protected bool m_bIsTextureFlipped;
        /// <summary>
        /// is texture flipped 
        /// </summary>
        public bool TextureFlipped
        {
            get { return m_bIsTextureFlipped; }
            set { m_bIsTextureFlipped = value; }
        }

        public bool initWithSize(ccGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            bool bRet = true;

            m_bActive = false;
            m_nReuseGrid = 0;
            m_sGridSize = gridSize;

            m_pTexture = pTexture;
            if (m_pTexture != null)
            {
                //m_pTexture.release();
            }
            m_bIsTextureFlipped = bFlipped;

            CCSize texSize = m_pTexture.ContentSizeInPixels;
            m_obStep.x = texSize.width / m_sGridSize.x;
            m_obStep.y = texSize.height / m_sGridSize.y;

            m_pGrabber = new CCGrabber();
            if (m_pGrabber != null)
            {
                m_pGrabber.grab(m_pTexture);
            }
            else
            {
                bRet = false;
            }


            calculateVertexPoints();

            return bRet;
        }

        public ulong ccNextPOT(ulong x)
        {
            x = x - 1;
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return x + 1;
        }

        public bool initWithSize(ccGridSize gridSize)
        {
            CCDirector pDirector = CCDirector.sharedDirector();
            CCSize s = pDirector.winSizeInPixels;

            ulong POTWide = ccNextPOT((uint)s.width);
            ulong POTHigh = ccNextPOT((uint)s.height);

            // we only use rgba8888
            CCTexture2DPixelFormat format = CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888;

            object data;
            //object data = calloc((int)(POTWide * POTHigh * 4), 1);
            //if (! data)
            //{
            //    CCLOG("cocos2d: CCGrid: not enough memory.");
            //    this->release();
            //    return false;
            //}

            CCTexture2D pTexture = new CCTexture2D();
            //pTexture.initWithData(data, format, POTWide, POTHigh, s);

            //free(data);

            if (pTexture == null)
            {
                Debug.WriteLine("cocos2d: CCGrid: error creating texture");
                return false;
            }

            initWithSize(gridSize, pTexture, false);

            //pTexture.release();

            return true;
        }

        public void beforeDraw()
        {
            set2DProjection();
            m_pGrabber.beforeRender(m_pTexture);
        }

        public void afterDraw(CCNode pTarget)
        {
            m_pGrabber.afterRender(m_pTexture);

            set3DProjection();
            applyLandscape();

            //if (pTarget.getCamera()->getDirty())
            //{
            //    const CCPoint& offset = pTarget->getAnchorPointInPixels();

            //    //
            //    // XXX: Camera should be applied in the AnchorPoint
            //    //
            //    ccglTranslate(offset.x, offset.y, 0);
            //    pTarget->getCamera()->locate();
            //    ccglTranslate(-offset.x, -offset.y, 0);
            //}

            //glBindTexture(GL_TEXTURE_2D, m_pTexture->getName());

            //// restore projection for default FBO .fixed bug #543 #544
            ////CCDirector.sharedDirector().Projection=CCDirector.sharedDirector().Projection;
            //CCDirector.sharedDirector().applyOrientation();
            blit();

        }

        public virtual void blit()
        {
            Debug.Assert(false);
        }

        public virtual void reuse()
        {
            Debug.Assert(false);
        }

        public virtual void calculateVertexPoints()
        {
            Debug.Assert(false);
        }

        public static CCGridBase gridWithSize(ccGridSize gridSize, CCTexture2D texture, bool flipped)
        {
            CCGridBase pGridBase = new CCGridBase();

            if (pGridBase != null)
            {
                if (pGridBase.initWithSize(gridSize, texture, flipped))
                {
                    //pGridBase->autorelease();
                }
                else
                {
                    //CC_SAFE_RELEASE_NULL(pGridBase);
                }
            }

            return pGridBase;
        }

        public static CCGridBase gridWithSize(ccGridSize gridSize)
        {
            CCGridBase pGridBase = new CCGridBase();

            if (pGridBase != null)
            {
                if (pGridBase.initWithSize(gridSize))
                {
                    //pGridBase->autorelease();
                }
                else
                {
                    //CC_SAFE_RELEASE_NULL(pGridBase);
                }
            }

            return pGridBase;


        }

        public void set2DProjection()
        {
            CCSize winSize = CCDirector.sharedDirector().winSizeInPixels;

            //glLoadIdentity();

            // set view port for user FBO, fixed bug #543 #544
            //glViewport((GLsizei)0, (GLsizei)0, (GLsizei)winSize.width, (GLsizei)winSize.height);
            //glMatrixMode(GL_PROJECTION);
            //glLoadIdentity();
            //ccglOrtho(0, winSize.width, 0, winSize.height, -1024, 1024);
            //glMatrixMode(GL_MODELVIEW);
        }

        public void set3DProjection()
        {
            CCSize winSize = CCDirector.sharedDirector().displaySizeInPixels;

            //// set view port for user FBO, fixed bug #543 #544
            //glViewport(0, 0, (GLsizei)winSize.width, (GLsizei)winSize.height);
            //glMatrixMode(GL_PROJECTION);
            //glLoadIdentity();
            //gluPerspective(60, (GLfloat)winSize.width/winSize.height, 0.5f, 1500.0f);

            //glMatrixMode(GL_MODELVIEW);	
            //glLoadIdentity();
            //gluLookAt( winSize.width/2, winSize.height/2, CCDirector::sharedDirector()->getZEye(),
            //    winSize.width/2, winSize.height/2, 0,
            //    0.0f, 1.0f, 0.0f
            //    );
        }

        protected void applyLandscape()
        {
            CCDirector pDirector = CCDirector.sharedDirector();

            CCSize winSize = pDirector.displaySizeInPixels;
            float w = winSize.width / 2;
            float h = winSize.height / 2;

            ccDeviceOrientation orientation = pDirector.deviceOrientation;
            switch (orientation)
            {
                case ccDeviceOrientation.CCDeviceOrientationPortraitUpsideDown:
                    //glTranslatef(w,h,0);
                    //glRotatef(180,0,0,1);
                    //glTranslatef(-w,-h,0);
                    break;
                case ccDeviceOrientation.CCDeviceOrientationLandscapeLeft:
                    //glTranslatef(w,h,0);
                    //glRotatef(-90,0,0,1);
                    //glTranslatef(-h,-w,0);
                    break;
                case ccDeviceOrientation.CCDeviceOrientationLandscapeRight:
                    //glTranslatef(w,h,0);
                    //glRotatef(90,0,0,1);
                    //glTranslatef(-h,-w,0);
                    break;
                default:
                    break;
            }
        }

        protected CCTexture2D m_pTexture;

        protected CCGrabber m_pGrabber;

    }
}
