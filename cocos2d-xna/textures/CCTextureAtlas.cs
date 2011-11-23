/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011      Fulcrum Mobile Network, Inc.
 
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
    public class CCTextureAtlas : CCObject
    {
        protected ushort[] m_pIndices;
        //#if CC_USES_VBO
        //GLuint				m_pBuffersVBO[2]; //0: vertex  1: indices
        //bool				m_bDirty; //indicates whether or not the array buffer of the VBO needs to be updated
        //#endif // CC_USES_VBO

        #region quantity of quads that are going to be drawn
        /// <summary>
        /// quantity of quads that are going to be drawn
        /// </summary>
        #endregion
        protected uint m_uTotalQuads;
        public uint TotalQuads
        {
            get
            {
                return m_uTotalQuads;
            }
            set
            {
                m_uTotalQuads = value;
            }
        }

        #region quantity of quads that can be stored with the current texture atlas size
        /// <summary>
        /// quantity of quads that can be stored with the current texture atlas size
        /// </summary>
        #endregion
        protected uint m_uCapacity;
        public uint Capacity
        {
            get
            {
                return m_uCapacity;
            }
            set
            {
                m_uCapacity = value;
            }
        }

        #region Texture of the texture atlas
        /// <summary>
        /// Texture of the texture atlas
        /// </summary>
        #endregion
        protected CCTexture2D m_pTexture;
        public CCTexture2D Texture
        {
            get
            {
                return m_pTexture;
            }
            set
            {
                m_pTexture = value;
            }
        }

        #region Quads that are going to be rendered
        /// <summary>
        /// Quads that are going to be rendered
        /// </summary>
        #endregion
        protected ccV3F_C4B_T2F_Quad m_pQuads;
        public ccV3F_C4B_T2F_Quad Quads
        {
            get
            {
                return m_pQuads;
            }
            set
            {
                m_pQuads = value;
            }
        }

        public CCTextureAtlas()
        {

        }

        public string description()
        {
            string[] ret = new string[100];
            //sprintf(ret, "<CCTextureAtlas | totalQuads = %u>", m_uTotalQuads);
            return ret.ToString();
        }

        #region creates a TextureAtlas with an filename and with an initial capacity for Quads.
        /// <summary>
        /// creates a TextureAtlas with an filename and with an initial capacity for Quads.
        /// The TextureAtlas capacity can be increased in runtime.
        /// </summary>
        #endregion
        public static CCTextureAtlas textureAtlasWithFile(string file, uint capacity)
        {
            CCTextureAtlas pTextureAtlas = new CCTextureAtlas();
            if (pTextureAtlas != null && pTextureAtlas.initWithFile(file, capacity))
            {
                //pTextureAtlas->autorelease();
                return pTextureAtlas;
            }
            //CC_SAFE_DELETE(pTextureAtlas);
            return null;
        }

        #region initializes a TextureAtlas with a filename and with a certain capacity for Quads.
        /// <summary>
        /// initializes a TextureAtlas with a filename and with a certain capacity for Quads.
        /// The TextureAtlas capacity can be increased in runtime.
        /// WARNING: Do not reinitialize the TextureAtlas because it will leak memory (issue #706)
        /// </summary>
        #endregion
        public bool initWithFile(string file, uint capacity)
        {
            // retained in property
            CCTexture2D texture = CCTextureCache.sharedTextureCache().addImage(file);

            if (texture != null)
            {
                return initWithTexture(texture, capacity);
            }
            else
            {
                //CCLOG("cocos2d: Could not open file: %s", file);
                //delete this;

                return false;
            }
        }

        #region creates a TextureAtlas with a previously initialized Texture2D object...
        /// <summary>
        /// creates a TextureAtlas with a previously initialized Texture2D object, and
        /// with an initial capacity for n Quads. 
        /// The TextureAtlas capacity can be increased in runtime.
        /// </summary>
        #endregion
        public static CCTextureAtlas textureAtlasWithTexture(CCTexture2D texture, uint capacity)
        {
            CCTextureAtlas pTextureAtlas = new CCTextureAtlas();
            if (pTextureAtlas != null && pTextureAtlas.initWithTexture(texture, capacity))
            {
                //pTextureAtlas->autorelease();
                return pTextureAtlas;
            }
            //CC_SAFE_DELETE(pTextureAtlas);
            return null;
        }

        #region initializes a TextureAtlas with a previously initialized Texture2D object...
        /// <summary>
        /// initializes a TextureAtlas with a previously initialized Texture2D object, and
        /// with an initial capacity for Quads. 
        /// The TextureAtlas capacity can be increased in runtime.
        /// WARNING: Do not reinitialize the TextureAtlas because it will leak memory (issue #706)
        /// </summary>
        #endregion
        public bool initWithTexture(CCTexture2D texture, uint capacity)
        {
            Debug.Assert(texture != null);
            m_uCapacity = capacity;
            m_uTotalQuads = 0;

            // retained in property
            this.m_pTexture = texture;

            // CC_SAFE_RETAIN(m_pTexture);

            // Re-initialization is not allowed
            Debug.Assert(m_pIndices == null);

            // m_pQuads = (ccV3F_C4B_T2F_Quad)calloc( sizeof(ccV3F_C4B_T2F_Quad) * m_uCapacity, 1 );
            m_pQuads = new ccV3F_C4B_T2F_Quad();

            //m_pIndices = (ushort)calloc( sizeof(ushort) * m_uCapacity * 6, 1 );            
            m_pIndices = new ushort[m_uCapacity * 6];

            // if( ! ( m_pQuads && m_pIndices) ) 
            // {
            //    //CCLOG("cocos2d: CCTextureAtlas: not enough memory");
            //    CC_SAFE_FREE(m_pQuads)
            //    CC_SAFE_FREE(m_pIndices)

            //    // release texture, should set it to null, because the destruction will
            //    // release it too. see cocos2d-x issue #484
            //    CC_SAFE_RELEASE_NULL(m_pTexture);
            //    return false;
            //}

            //#if CC_USES_VBO
            //    // initial binding
            //    glGenBuffers(2, &m_pBuffersVBO[0]);	
            //    m_bDirty = true;
            //#endif // CC_USES_VBO
            this.initIndices();
            return true;
        }

        #region updates a Quad (texture, vertex and color) at a certain index
        /// <summary>
        /// updates a Quad (texture, vertex and color) at a certain index
        /// index must be between 0 and the atlas capacity - 1
        /// @since v0.8
        /// </summary>
        #endregion
        public void updateQuad(ccV3F_C4B_T2F_Quad quad, uint index)
        {
            Debug.Assert(index >= 0 && index < m_uCapacity, "updateQuadWithTexture: Invalid index");

            m_uTotalQuads = (uint)(Math.Max(index + 1, m_uTotalQuads));
            //m_pQuads[index] = quad;

            //#if CC_USES_VBO
            //m_bDirty = true;
            //#endif
        }

        #region Inserts a Quad (texture, vertex and color) at a certain index
        /// <summary>
        /// Inserts a Quad (texture, vertex and color) at a certain index
        /// index must be between 0 and the atlas capacity - 1
        /// @since v0.8
        /// </summary>
        #endregion
        public void insertQuad(ccV3F_C4B_T2F_Quad quad, uint index)
        {
            //Debug.Assert( index < m_uCapacity, "insertQuadWithTexture: Invalid index");

            //m_uTotalQuads++;
            //Debug.Assert( m_uTotalQuads <= m_uCapacity, "invalid totalQuads");

            //// issue #575. index can be > totalQuads
            //uint remaining = (m_uTotalQuads-1) - index;

            ////last object doesn't need to be moved
            //if( remaining > 0)
            //{
            //// texture coordinates
            //memmove( &m_pQuads[index+1],&m_pQuads[index], sizeof(m_pQuads[0]) * remaining );		
            //}

            //m_pQuads[index] = quad;

            //#if CC_USES_VBO
            // = true;
            //#endif
            throw new NotImplementedException();
        }

        #region Removes the quad that is located at a certain index and inserts it at a new index
        /// <summary>
        /// Removes the quad that is located at a certain index and inserts it at a new index
        /// This operation is faster than removing and inserting in a quad in 2 different steps
        /// @since v0.7.2
        /// </summary>
        #endregion
        public void insertQuadFromIndex(uint oldIndex, uint newIndex)
        {
            //Debug.Assert(newIndex >= 0 && newIndex < m_uTotalQuads, "insertQuadFromIndex:atIndex: Invalid index");
            //Debug.Assert(oldIndex >= 0 && oldIndex < m_uTotalQuads, "insertQuadFromIndex:atIndex: Invalid index");

            //if (oldIndex == newIndex)
            //    return;

            //// because it is ambigious in iphone, so we implement abs ourself
            //// unsigned int howMany = abs( oldIndex - newIndex);
            //uint howMany = (oldIndex - newIndex) > 0 ? (oldIndex - newIndex) : (newIndex - oldIndex);
            //uint dst = oldIndex;
            //uint src = oldIndex + 1;
            //if (oldIndex > newIndex)
            //{
            //    dst = newIndex + 1;
            //    src = newIndex;
            //}

            //// texture coordinates

            //ccV3F_C4B_T2F_Quad quadsBackup = m_pQuads[oldIndex];
            //memmove( &m_pQuads[dst],&m_pQuads[src], sizeof(m_pQuads[0]) * howMany );
            //m_pQuads[newIndex] = quadsBackup;

            ////#if CC_USES_VBO
            ////m_bDirty = true;
            ////#endif
            throw new NotImplementedException();
        }

        #region removes a quad at a given index number.
        /// <summary>
        /// removes a quad at a given index number.
        /// The capacity remains the same, but the total number of quads to be drawn is reduced in 1
        /// @since v0.7.2
        /// </summary>
        #endregion
        public void removeQuadAtIndex(uint index)
        {
            // Debug.Assert( index < m_uTotalQuads, "removeQuadAtIndex: Invalid index");

            // uint remaining = (m_uTotalQuads-1) - index;

            // // last object doesn't need to be moved
            // if( remaining!=null )
            // {
            // // texture coordinates
            // memmove( &m_pQuads[index],&m_pQuads[index+1], sizeof(m_pQuads[0]) * remaining );
            // }

            // m_uTotalQuads--;

            ////#if CC_USES_VBO
            ////m_bDirty = true;
            ////#endif
            throw new NotImplementedException();
        }

        #region removes all Quads.
        /// <summary>
        /// removes all Quads.
        /// The TextureAtlas capacity remains untouched. No memory is freed.
        /// The total number of quads to be drawn will be 0
        /// @since v0.7.2
        /// </summary>
        #endregion
        public void removeAllQuads()
        {
            m_uTotalQuads = 0;
        }

        #region resize the capacity of the CCTextureAtlas.
        /// <summary>
        /// resize the capacity of the CCTextureAtlas.
        /// The new capacity can be lower or higher than the current one
        /// It returns YES if the resize was successful.
        ///  If it fails to resize the capacity it will return NO with a new capacity of 0.
        /// </summary>
        #endregion
        public bool resizeCapacity(uint newCapacity)
        {
            //    if( newCapacity == m_uCapacity )
            //    return true;

            //// update capacity and totolQuads
            //m_uTotalQuads = Math.Min(m_uTotalQuads, newCapacity);
            //m_uCapacity = newCapacity;

            //void * tmpQuads = realloc( m_pQuads, sizeof(m_pQuads[0]) * m_uCapacity );
            //void * tmpIndices = realloc( m_pIndices, sizeof(m_pIndices[0]) * m_uCapacity * 6 );

            //if( ! ( tmpQuads && tmpIndices) ) {
            //    //CCLOG("cocos2d: CCTextureAtlas: not enough memory");
            //    if( tmpQuads )
            //        free(tmpQuads);
            //    else
            //        free(m_pQuads);

            //    if( tmpIndices )
            //        free(tmpIndices);
            //    else
            //        free(m_pIndices);

            //    m_pQuads = NULL;
            //    m_pIndices = NULL;
            //    m_uCapacity = m_uTotalQuads = 0;
            //    return false;
            //}

            //m_pQuads = (ccV3F_C4B_T2F_Quad)tmpQuads;
            //m_pIndices = (ushort)tmpIndices;

            //this.initIndices();

            //#if CC_USES_VBO
            //m_bDirty = true;
            //#endif

            //return true;
            throw new NotImplementedException();
        }

        #region draws n quads
        /// <summary>
        /// draws n quads
        ///  can't be greater than the capacity of the Atlas
        ///  n
        /// </summary>
        #endregion
        public void drawNumberOfQuads(uint n)
        {
            this.drawNumberOfQuads(n, 0);
        }

        #region draws n quads from an index (offset).
        /// <summary>
        /// draws n quads from an index (offset).
        /// n + start can't be greater than the capacity of the atlas
        /// @since v1.0
        /// </summary>
        #endregion
        public void drawNumberOfQuads(uint n, uint start)
        {
            //glBindTexture(GL_TEXTURE_2D, m_pTexture.getName());
        }

        #region draws all the Atlas's Quads
        /// <summary>
        /// draws all the Atlas's Quads
        /// </summary>
        #endregion
        public void drawQuads()
        {
            this.drawNumberOfQuads(m_uTotalQuads, 0);
        }

        private void initIndices()
        {
            for (uint i = 0; i < m_uCapacity; i++)
            {
                //#if CC_TEXTURE_ATLAS_USE_TRIANGLE_STRIP
                m_pIndices[i * 6 + 0] = (ushort)(i * 4 + 0);
                m_pIndices[i * 6 + 1] = (ushort)(i * 4 + 0);
                m_pIndices[i * 6 + 2] = (ushort)(i * 4 + 2);
                m_pIndices[i * 6 + 3] = (ushort)(i * 4 + 1);
                m_pIndices[i * 6 + 4] = (ushort)(i * 4 + 3);
                m_pIndices[i * 6 + 5] = (ushort)(i * 4 + 3);
                //#else
                m_pIndices[i * 6 + 0] = (ushort)(i * 4 + 0);
                m_pIndices[i * 6 + 1] = (ushort)(i * 4 + 1);
                m_pIndices[i * 6 + 2] = (ushort)(i * 4 + 2);

                // inverted index. issue #179
                m_pIndices[i * 6 + 3] = (ushort)(i * 4 + 3);
                m_pIndices[i * 6 + 4] = (ushort)(i * 4 + 2);
                m_pIndices[i * 6 + 5] = (ushort)(i * 4 + 1);
                //		m_pIndices[i*6+3] = i*4+2;
                //		m_pIndices[i*6+4] = i*4+3;
                //		m_pIndices[i*6+5] = i*4+1;	
                //#endif	
            }
        }

        public uint getTotalQuads()
        {
            return m_uTotalQuads;
        }

        public uint getCapacity()
        {
            return m_uCapacity;
        }

        //public CCTexture2D getTexture
        //{
        //    get
        //    {
        //        return m_pTexture;
        //    }
        //    set
        //    {
        //        m_pTexture = value;
        //    }
        //}
        public CCTexture2D getTexture()
        {
            return m_pTexture;
        }
        public void setTexture(CCTexture2D var)
        {
            //CC_SAFE_RETAIN(var);
            //CC_SAFE_RELEASE(m_pTexture);
            m_pTexture = var;
        }

        //public ccV3F_C4B_T2F_Quad getQuads
        //{
        //    get 
        //    {
        //        return m_pQuads;
        //    }
        //    set
        //    {
        //        m_pQuads = value;
        //    }
        //}
        public ccV3F_C4B_T2F_Quad getQuads()
        {
            return m_pQuads;
        }
        public void setQuads(ccV3F_C4B_T2F_Quad var)
        {
            m_pQuads = var;
        }
    }
}
