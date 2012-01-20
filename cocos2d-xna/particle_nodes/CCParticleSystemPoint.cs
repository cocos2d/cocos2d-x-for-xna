/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (C) 2008      Apple Inc. All Rights Reserved.

http://www.cocos2d-x.org

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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /** @brief CCParticleSystemPoint is a subclass of CCParticleSystem
    Attributes of a Particle System:
    * All the attributes of Particle System

    Features:
    * consumes small memory: uses 1 vertex (x,y) per particle, no need to assign tex coordinates
    * size can't be bigger than 64
    * the system can't be scaled since the particles are rendered using GL_POINT_SPRITE

    Limitations:
    * On 3rd gen iPhone devices and iPads, this node performs MUCH slower than CCParticleSystemQuad.
    */
    public class CCParticleSystemPoint : CCParticleSystem
    {	
        public int CC_MAX_PARTICLE_SIZE = 64;
        public CCParticleSystemPoint()
		{
            m_pVertices = null;
        }
	    ~CCParticleSystemPoint()
        {
//#if CC_USES_VBO
//            glDeleteBuffers(1, &m_uVerticesID);
//#endif
        }

        /** creates an initializes a CCParticleSystemPoint from a plist file.
        This plist files can be creted manually or with Particle Designer:  
        */
        public static CCParticleSystemPoint particleWithFile(string plistFile)
        { 
            CCParticleSystemPoint pRet = new CCParticleSystemPoint();
            if (pRet != null && pRet.initWithFile(plistFile))
            {
                return pRet;
            }

            return pRet;
        }

	    // super methods
        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
            if(base.initWithTotalParticles(numberOfParticles) )
	        {
		        m_pVertices = new ccPointSprite[this.TotalParticles];

                if (null == m_pVertices)
                {
                    Debug.WriteLine("cocos2d: Particle system: not enough memory");
                    return false;
                }

                for (int i = 0; i < this.TotalParticles; i++)
                {
                    m_pVertices[i] = new ccPointSprite();
                }

        //#if CC_USES_VBO
        //        glGenBuffers(1, &m_uVerticesID);

        //        // initial binding
        //        glBindBuffer(GL_ARRAY_BUFFER, m_uVerticesID);
        //        glBufferData(GL_ARRAY_BUFFER, sizeof(ccPointSprite)*m_uTotalParticles, m_pVertices, GL_DYNAMIC_DRAW);
        //        glBindBuffer(GL_ARRAY_BUFFER, 0);
        //#endif
		        return true;
	        }
	        return false;
        }

        public override void updateQuadWithParticle(CCParticle particle, CCPoint newPosition)
        {
            // place vertices and colos in array
            m_pVertices[m_uParticleIdx].pos = ccTypes.vertex2(newPosition.x, newPosition.y);
            m_pVertices[m_uParticleIdx].size = particle.size;
            ccColor4B color = new ccColor4B((Byte)(particle.color.r * 255), (Byte)(particle.color.g * 255), (Byte)(particle.color.b * 255), 
		(Byte)(particle.color.a * 255));
            m_pVertices[m_uParticleIdx].color = color;
        }

        public override void postStep()
        {
//#if CC_USES_VBO
//            glBindBuffer(GL_ARRAY_BUFFER, m_uVerticesID);
//            glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(ccPointSprite) * m_uParticleCount, m_pVertices);
//            glBindBuffer(GL_ARRAY_BUFFER, 0);
//#endif
        }

        public override void draw()
        {
            // ccparticlesystempoint depends on opengl, can't be realized in xna
            throw new NotImplementedException();

            // base.draw();

        //    if (m_uParticleIdx==0)
        //    {
        //        return;
        //    }

        //    // Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
        //    // Needed states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY
        //    // Unneeded states: GL_TEXTURE_COORD_ARRAY
        //    glDisableClientState(GL_TEXTURE_COORD_ARRAY);

        //    glBindTexture(GL_TEXTURE_2D, m_pTexture->getName());

        //    glEnable(GL_POINT_SPRITE_OES);
        //    glTexEnvi( GL_POINT_SPRITE_OES, GL_COORD_REPLACE_OES, GL_TRUE );	

        //#define kPointSize sizeof(m_pVertices[0])

        //#if CC_USES_VBO
        //    glBindBuffer(GL_ARRAY_BUFFER, m_uVerticesID);

        //#if CC_ENABLE_CACHE_TEXTTURE_DATA
        //    glBufferData(GL_ARRAY_BUFFER, sizeof(ccPointSprite)*m_uTotalParticles, m_pVertices, GL_DYNAMIC_DRAW);
        //#endif

        //    glVertexPointer(2,GL_FLOAT,kPointSize,0);

        //    glColorPointer(4, GL_UNSIGNED_BYTE, kPointSize,(GLvoid*)offsetof(ccPointSprite,color) );

        //    glEnableClientState(GL_POINT_SIZE_ARRAY_OES);
        //    glPointSizePointerOES(GL_FLOAT,kPointSize,(GLvoid*) offsetof(ccPointSprite,size) );
        //#else // Uses Vertex Array List
        //    int offset = (int)m_pVertices;
        //    glVertexPointer(2,GL_FLOAT, kPointSize, (GLvoid*) offset);

        //    int diff = offsetof(ccPointSprite, color);
        //    glColorPointer(4, GL_UNSIGNED_BYTE, kPointSize, (GLvoid*) (offset+diff));

        //    glEnableClientState(GL_POINT_SIZE_ARRAY_OES);
        //    diff = offsetof(ccPointSprite, size);
        //    glPointSizePointerOES(GL_FLOAT, kPointSize, (GLvoid*) (offset+diff));
        //#endif 

        //    bool newBlend = (m_tBlendFunc.src != CC_BLEND_SRC || m_tBlendFunc.dst != CC_BLEND_DST) ? true : false;
        //    if( newBlend ) 
        //    {
        //        glBlendFunc( m_tBlendFunc.src, m_tBlendFunc.dst );
        //    }

        //    glDrawArrays(GL_POINTS, 0, m_uParticleIdx);

        //    // restore blend state
        //    if( newBlend )
        //        glBlendFunc( CC_BLEND_SRC, CC_BLEND_DST);

        //#if CC_USES_VBO
        //    // unbind VBO buffer
        //    glBindBuffer(GL_ARRAY_BUFFER, 0);
        //#endif

        //    glDisableClientState(GL_POINT_SIZE_ARRAY_OES);
        //    glDisable(GL_POINT_SPRITE_OES);

        //    // restore GL default state
        //    glEnableClientState(GL_TEXTURE_COORD_ARRAY);
        }

        ////
        //// SPIN IS NOT SUPPORTED
        ////
        //public virtual void setStartSpin(float var)
        //{
        //    Debug.Assert(var == 0, "PointParticleSystem doesn't support spinning");
        //    base.setStartSpin(var);
        //}
        //public virtual void CCParticleSystemPoint::setStartSpinVar(float var)
        //{
        //    CCAssert(var == 0, "PointParticleSystem doesn't support spinning");
        //    CCParticleSystem::setStartSpinVar(var);
        //}
        //public virtual void CCParticleSystemPoint::setEndSpin(float var)
        //{
        //    CCAssert(var == 0, "PointParticleSystem doesn't support spinning");
        //    CCParticleSystem::setEndSpin(var);
        //}
        //public virtual void CCParticleSystemPoint::setEndSpinVar(float var)
        //{
        //    CCAssert(var == 0, "PointParticleSystem doesn't support spinning");
        //    CCParticleSystem::setEndSpinVar(var);
        //}
        ////
        //// SIZE > 64 IS NOT SUPPORTED
        ////
        //public virtual void CCParticleSystemPoint::setStartSize(float size)
        //{
        //    CCAssert(size >= 0 && size <= CC_MAX_PARTICLE_SIZE, "PointParticleSystem only supports 0 <= size <= 64");
        //    CCParticleSystem::setStartSize(size);
        //}
        //public virtual void CCParticleSystemPoint::setEndSize(float size)
        //{
        //    CCAssert( (size == kCCParticleStartSizeEqualToEndSize) ||
        //        ( size >= 0 && size <= CC_MAX_PARTICLE_SIZE), "PointParticleSystem only supports 0 <= size <= 64");
        //    CCParticleSystem::setEndSize(size);
        //}
    
	    //! Array of (x,y,size) 
	    ccPointSprite[] m_pVertices;
	    
        //! vertices buffer id
    # if CC_USES_VBO
	    uint m_uVerticesID;	
    #endif
    };
}
