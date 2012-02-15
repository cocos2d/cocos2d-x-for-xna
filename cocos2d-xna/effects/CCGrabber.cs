/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

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
    public class CCGrabber : CCObject
    {
        public CCGrabber()
        {
            m_eGlesVersion = CCConfiguration.sharedConfiguration().getGlesVersion();

            // If the gles version is lower than GLES_VER_1_0, 
            // all the functions in CCGrabber return directly.
            if (m_eGlesVersion <= CCGlesVersion.GLES_VER_1_0)
            {
                return;
            }

            // generate FBO
            //ccglGenFramebuffers(1, &m_fbo);
        }

        public void grab(CCTexture2D pTexture)
        {
            // If the gles version is lower than GLES_VER_1_0, 
            // all the functions in CCGrabber return directly.
            if (m_eGlesVersion <= CCGlesVersion.GLES_VER_1_0)
            {
                return;
            }

            //glGetIntegerv(CC_GL_FRAMEBUFFER_BINDING, &m_oldFBO);

            // bind
            //ccglBindFramebuffer(CC_GL_FRAMEBUFFER, m_fbo);

            // associate texture with FBO
            //ccglFramebufferTexture2D(CC_GL_FRAMEBUFFER, CC_GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D,
                //pTexture->getName(), 0);

            // check if it worked (probably worth doing :) )
            //GLuint status = ccglCheckFramebufferStatus(CC_GL_FRAMEBUFFER);
            //if (status != CC_GL_FRAMEBUFFER_COMPLETE)
            //{
            //    CCLOG("Frame Grabber: could not attach texture to frmaebuffer");
            //}

            //ccglBindFramebuffer(CC_GL_FRAMEBUFFER, m_oldFBO);
        }
        public void beforeRender(CCTexture2D pTexture)
        {
            // If the gles version is lower than GLES_VER_1_0, 
            // all the functions in CCGrabber return directly.
            if (m_eGlesVersion <= CCGlesVersion.GLES_VER_1_0)
            {
                return;
            }

            //glGetIntegerv(CC_GL_FRAMEBUFFER_BINDING, &m_oldFBO);
            //ccglBindFramebuffer(CC_GL_FRAMEBUFFER, m_fbo);

            // BUG XXX: doesn't work with RGB565.

            /*glClearColor(0, 0, 0, 0);*/

            // BUG #631: To fix #631, uncomment the lines with #631
            // Warning: But it CCGrabber won't work with 2 effects at the same time
            //glClearColor(0.0f, 0.0f, 0.0f, 1.0f);	// #631

            //glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

            //glColorMask(true, true, true, false);	// #631
        }
        public void afterRender(CCTexture2D pTexture)
        {
            // If the gles version is lower than GLES_VER_1_0, 
            // all the functions in CCGrabber return directly.
            if (m_eGlesVersion <= CCGlesVersion.GLES_VER_1_0)
            {
                return;
            }

            //ccglBindFramebuffer(CC_GL_FRAMEBUFFER, m_oldFBO);
            //glColorMask(true, true, true, true);	// #631
        }

        protected uint m_fbo;
        protected int m_oldFBO;
        protected CCGlesVersion m_eGlesVersion;
    }
}
