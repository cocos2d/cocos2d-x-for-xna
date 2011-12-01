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
    //public enum CCGlesVersion
    //{
    //    GLES_VER_INVALID,
    //    GLES_VER_1_0,
    //    GLES_VER_1_1,
    //    GLES_VER_2_0
    //}
}
