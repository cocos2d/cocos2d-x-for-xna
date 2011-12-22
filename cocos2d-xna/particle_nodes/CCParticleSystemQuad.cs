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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{

    /** @brief CCParticleSystemQuad is a subclass of CCParticleSystem

    It includes all the features of ParticleSystem.

    Special features and Limitations:	
    - Particle size can be any float number.
    - The system can be scaled
    - The particles can be rotated
    - On 1st and 2nd gen iPhones: It is only a bit slower that CCParticleSystemPoint
    - On 3rd gen iPhone and iPads: It is MUCH faster than CCParticleSystemPoint
    - It consumes more RAM and more GPU memory than CCParticleSystemPoint
    - It supports subrects
    @since v0.8
    */
    public class CCParticleSystemQuad : CCParticleSystem
    {
        ccV2F_C4B_T2F_Quad[] m_pQuads;		// quads to be rendered
	    uint[] m_pIndices;	// indices
    #if CC_USES_VBO
	    uint				m_uQuadsID;	// VBO id
    #endif

        public CCParticleSystemQuad()
	    {
            m_pQuads = null;
		    m_pIndices = null;
        }
	    ~CCParticleSystemQuad()
        {
        }

        /** creates an initializes a CCParticleSystemQuad from a plist file.
        This plist files can be creted manually or with Particle Designer:  
        */
        public static CCParticleSystemQuad particleWithFile(string plistFile)
        {
            CCParticleSystemQuad pRet = new CCParticleSystemQuad();
            if (pRet != null && pRet.initWithFile(plistFile))
            {
                return pRet;
            }
            return pRet;
        }

	    /** initialices the indices for the vertices*/
	    public void initIndices()
        {
           	for(uint i = 0; i < this.TotalParticles; ++i)
	        {
                uint i6 = i*6;
                uint i4 = i*4;
		        m_pIndices[i6+0] = (uint) i4+0;
		        m_pIndices[i6+1] = (uint) i4+1;
		        m_pIndices[i6+2] = (uint) i4+2;

		        m_pIndices[i6+5] = (uint) i4+1;
		        m_pIndices[i6+4] = (uint) i4+2;
		        m_pIndices[i6+3] = (uint) i4+3;
	        }
        }

        /** initilizes the texture with a rectangle measured Points */
	    public void initTexCoordsWithRect(CCRect pointRect)
        {
            // convert to pixels coords

            CCRect rect = new CCRect(
                pointRect.origin.x * CCDirector.sharedDirector().ContentScaleFactor,
                pointRect.origin.y * CCDirector.sharedDirector().ContentScaleFactor,
                pointRect.size.width * CCDirector.sharedDirector().ContentScaleFactor,
                pointRect.size.height * CCDirector.sharedDirector().ContentScaleFactor);

            float wide = pointRect.size.width;
            float high = pointRect.size.height;

            if (this.Texture != null)
            {
                wide = this.Texture.PixelsWide;
                high = this.Texture.PixelsHigh;
            }

        //#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
        //    GLfloat left = (rect.origin.x*2+1) / (wide*2);
        //    GLfloat bottom = (rect.origin.y*2+1) / (high*2);
        //    GLfloat right = left + (rect.size.width*2-2) / (wide*2);
        //    GLfloat top = bottom + (rect.size.height*2-2) / (high*2);
        //#else
            float left = rect.origin.x / wide;
            float bottom = rect.origin.y / high;
            float right = left + rect.size.width / wide;
            float top = bottom + rect.size.height / high;
        // #endif // ! CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

	        // Important. Texture in cocos2d are inverted, so the Y component should be inverted
	        ccMacros.CC_SWAP<float>(ref top, ref bottom);

	        for(uint i=0; i<this.TotalParticles; i++) 
	        {
                m_pQuads[i] = new ccV2F_C4B_T2F_Quad();
		        // bottom-left vertex:
		        m_pQuads[i].bl.texCoords.u = left;
		        m_pQuads[i].bl.texCoords.v = bottom;
		        // bottom-right vertex:
		        m_pQuads[i].br.texCoords.u = right;
		        m_pQuads[i].br.texCoords.v = bottom;
		        // top-left vertex:
		        m_pQuads[i].tl.texCoords.u = left;
		        m_pQuads[i].tl.texCoords.v = top;
		        // top-right vertex:
		        m_pQuads[i].tr.texCoords.u = right;
		        m_pQuads[i].tr.texCoords.v = top;
	        }
        }

	    /** Sets a new CCSpriteFrame as particle.
	    WARNING: this method is experimental. Use setTexture:withRect instead.
	    @since v0.99.4
	    */
	    public void setDisplayFrame(CCSpriteFrame spriteFrame)
        {
           	Debug.Assert( CCPoint.CCPointEqualToPoint( spriteFrame.getOffsetInPixels() , new CCPoint(0,0) ), "QuadParticle only supports SpriteFrames with no offsets");

	        // update texture before updating texture rect
	        if ( null == this.Texture || spriteFrame.getTexture().Name != this.Texture.Name)
	        {
		        this.Texture = spriteFrame.getTexture();
	        }
        }

        /** Sets a new texture with a rect. The rect is in Points.
	    @since v0.99.4
	    */
	    public void setTextureWithRect(CCTexture2D texture, CCRect rect)
        {
            // Only update the texture if is different from the current one
	        if( null == this.Texture || texture.Name != this.Texture.Name )
	        {
		        base.Texture = texture;
	        }

	        initTexCoordsWithRect(rect);
        }


        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
            // base initialization
	        if( base.initWithTotalParticles(numberOfParticles) ) 
	        {
		        // allocating data space
		        m_pQuads = new ccV2F_C4B_T2F_Quad[this.TotalParticles];
		        m_pIndices = new uint[this.TotalParticles * 6];

		        if( null == m_pQuads || null == m_pIndices) 
		        {
			        Debug.WriteLine("cocos2d: Particle system: not enough memory");
			        return false;
		        }

		        // initialize only once the texCoords and the indices
                if (this.Texture != null)
                {
                    initTexCoordsWithRect(new CCRect((float)0, (float)0, (float)this.Texture.PixelsWide, (float)this.Texture.PixelsHigh));
                }
                else
                {
                    initTexCoordsWithRect(new CCRect((float)0, (float)0, (float)1, (float)1));
                }

		        initIndices();

        //#if CC_USES_VBO
        //        glEnable(GL_VERTEX_ARRAY);

        //        // create the VBO buffer
        //        glGenBuffers(1, &m_uQuadsID);

        //        // initial binding
        //        glBindBuffer(GL_ARRAY_BUFFER, m_uQuadsID);
        //        glBufferData(GL_ARRAY_BUFFER, sizeof(m_pQuads[0])*m_uTotalParticles, m_pQuads, GL_DYNAMIC_DRAW);
        //        glBindBuffer(GL_ARRAY_BUFFER, 0);
        //#endif
		        return true;
	        }
	        return false;
        
        }

        public override CCTexture2D Texture
        {
            get
            {
                return base.Texture;
            }
            set
            {
                CCSize s = value.getContentSize();
                setTextureWithRect(value, new CCRect(0, 0, s.width, s.height));
            }
        }

        public override void updateQuadWithParticle(CCParticle particle, CCPoint newPosition)
        {
            // colors
            ccV2F_C4B_T2F_Quad quad = m_pQuads[m_uParticleIdx];

            ccColor4B color = new ccColor4B( (Byte)(particle.color.r * 255), (Byte)(particle.color.g * 255), (Byte)(particle.color.b * 255), 
		(Byte)(particle.color.a * 255));
            quad.bl.colors = color;
            quad.br.colors = color;
            quad.tl.colors = color;
            quad.tr.colors = color;

            // vertices
            float size_2 = particle.size / 2;
            if (particle.rotation != 0)
            {
                float x1 = -size_2;
                float y1 = -size_2;

                float x2 = size_2;
                float y2 = size_2;
                float x = newPosition.x;
                float y = newPosition.y;

                float r =  - particle.rotation * (float)System.Math.PI / 180;
                float cr = (float)System.Math.Cos(r);
                float sr = (float)System.Math.Sin(r);
                float ax = x1 * cr - y1 * sr + x;
                float ay = x1 * sr + y1 * cr + y;
                float bx = x2 * cr - y1 * sr + x;
                float by = x2 * sr + y1 * cr + y;
                float cx = x2 * cr - y2 * sr + x;
                float cy = x2 * sr + y2 * cr + y;
                float dx = x1 * cr - y2 * sr + x;
                float dy = x1 * sr + y2 * cr + y;

                // bottom-left
                quad.bl.vertices.x = ax;
                quad.bl.vertices.y = ay;

                // bottom-right vertex:
                quad.br.vertices.x = bx;
                quad.br.vertices.y = by;

                // top-left vertex:
                quad.tl.vertices.x = dx;
                quad.tl.vertices.y = dy;

                // top-right vertex:
                quad.tr.vertices.x = cx;
                quad.tr.vertices.y = cy;
            }
            else
            {
                // bottom-left vertex:
                quad.bl.vertices.x = newPosition.x - size_2;
                quad.bl.vertices.y = newPosition.y - size_2;

                // bottom-right vertex:
                quad.br.vertices.x = newPosition.x + size_2;
                quad.br.vertices.y = newPosition.y - size_2;

                // top-left vertex:
                quad.tl.vertices.x = newPosition.x - size_2;
                quad.tl.vertices.y = newPosition.y + size_2;

                // top-right vertex:
                quad.tr.vertices.x = newPosition.x + size_2;
                quad.tr.vertices.y = newPosition.y + size_2;
            }
        }

        public override void postStep()
        {
            //#if CC_USES_VBO
            //    glBindBuffer(GL_ARRAY_BUFFER, m_uQuadsID);
            //    glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(m_pQuads[0])*m_uParticleCount, m_pQuads);
            //    glBindBuffer(GL_ARRAY_BUFFER, 0);
            //#endif
        }

        public override void draw()
        {
            base.draw();

            CCApplication.sharedApplication().spriteBatch.Begin();
                for (int i = 0; i < this.ParticleCount; i++)
                {
                    Vector2 vecPosition = new Vector2(m_pQuads[i].bl.vertices.x, m_pQuads[i].bl.vertices.y);
                    Vector2 origin = new Vector2(Texture.getTexture2D().Width/2, Texture.getTexture2D().Height/2);
                    CCApplication.sharedApplication().spriteBatch.Draw(this.Texture.getTexture2D(), vecPosition, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0);
                }
            CCApplication.sharedApplication().spriteBatch.End();

        //    // Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
        //    // Needed states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
        //    // Unneeded states: -
        //    glBindTexture(GL_TEXTURE_2D, m_pTexture->getName());

        //    #define kQuadSize sizeof(m_pQuads[0].bl)

    
        //    int offset = (int) m_pQuads;

        //    // vertex
        //    int diff = offsetof( ccV2F_C4B_T2F, vertices);
        //    glVertexPointer(2,GL_FLOAT, kQuadSize, (GLvoid*) (offset+diff) );

        //    // color
        //    diff = offsetof( ccV2F_C4B_T2F, colors);
        //    glColorPointer(4, GL_UNSIGNED_BYTE, kQuadSize, (GLvoid*)(offset + diff));

        //    // tex coords
        //    diff = offsetof( ccV2F_C4B_T2F, texCoords);
        //    glTexCoordPointer(2, GL_FLOAT, kQuadSize, (GLvoid*)(offset + diff));		


        //    bool newBlend = (m_tBlendFunc.src != CC_BLEND_SRC || m_tBlendFunc.dst != CC_BLEND_DST) ? true : false;
        //    if( newBlend ) 
        //    {
        //        glBlendFunc( m_tBlendFunc.src, m_tBlendFunc.dst );
        //    }

        //    CCAssert( m_uParticleIdx == m_uParticleCount, "Abnormal error in particle quad");

        //    glDrawElements(GL_TRIANGLES, (GLsizei)(m_uParticleIdx*6), GL_UNSIGNED_SHORT, m_pIndices);	

        //    // restore blend state
        //    if( newBlend )
        //        glBlendFunc( CC_BLEND_SRC, CC_BLEND_DST );

        //#if CC_USES_VBO
        //    glBindBuffer(GL_ARRAY_BUFFER, 0);
        //#endif

        }

    };

}
