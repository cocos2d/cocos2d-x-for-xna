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
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /// <summary>
    /// A class that implements a Texture Atlas.
    /// </summary>
    ///<remarks>
    /// Supported features:
    ///  The atlas file can be a PVRTC, PNG or any other fomrat supported by Texture2D
    ///  Quads can be udpated in runtime
    ///  Quads can be added in runtime
    ///  Quads can be removed in runtime
    ///  Quads can be re-ordered in runtime
    ///  The TextureAtlas capacity can be increased or decreased in runtime
    ///  OpenGL component: V3F, C4B, T2F.
    /// The quads are rendered using an OpenGL ES VBO.
    /// To render the quads using an interleaved vertex array list, you should modify the ccConfig.h file 
    ///</remarks>
    public class CCTextureAtlas : CCObject
    {
        protected ushort[] m_pIndices;
#if CC_USES_VBO
        ulong[] m_pBuffersVBO; //0: vertex  1: indices
        bool m_bDirty; //indicates whether or not the array buffer of the VBO needs to be updated
#endif // CC_USES_VBO

        public CCTextureAtlas()
        {

        }

        public string description()
        {
            string[] ret = new string[100];
            //sprintf(ret, "<CCTextureAtlas | totalQuads = %u>", m_uTotalQuads);
            return ret.ToString();
        }

        #region create and init

        /// <summary>
        /// creates a TextureAtlas with an filename and with an initial capacity for Quads.
        /// The TextureAtlas capacity can be increased in runtime.
        /// </summary>
        public static CCTextureAtlas textureAtlasWithFile(string file, int capacity)
        {
            CCTextureAtlas pTextureAtlas = new CCTextureAtlas();
            if (pTextureAtlas != null && pTextureAtlas.initWithFile(file, capacity))
            {
                return pTextureAtlas;
            }

            return null;
        }

        /// <summary>
        /// initializes a TextureAtlas with a filename and with a certain capacity for Quads.
        /// The TextureAtlas capacity can be increased in runtime.
        /// WARNING: Do not reinitialize the TextureAtlas because it will leak memory (issue #706)
        /// </summary>
        public bool initWithFile(string file, int capacity)
        {
            // retained in property
            CCTexture2D texture = CCTextureCache.sharedTextureCache().addImage(file);
            if (texture != null)
            {
                return initWithTexture(texture, capacity);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// creates a TextureAtlas with a previously initialized Texture2D object, and
        /// with an initial capacity for n Quads. 
        /// The TextureAtlas capacity can be increased in runtime.
        /// </summary>
        public static CCTextureAtlas textureAtlasWithTexture(CCTexture2D texture, int capacity)
        {
            CCTextureAtlas pTextureAtlas = new CCTextureAtlas();
            if (pTextureAtlas != null && pTextureAtlas.initWithTexture(texture, capacity))
            {
                return pTextureAtlas;
            }

            return null;
        }

        /// <summary>
        /// initializes a TextureAtlas with a previously initialized Texture2D object, and
        /// with an initial capacity for Quads. 
        /// The TextureAtlas capacity can be increased in runtime.
        /// WARNING: Do not reinitialize the TextureAtlas because it will leak memory (issue #706)
        /// </summary>
        public bool initWithTexture(CCTexture2D texture, int capacity)
        {
            Debug.Assert(texture != null);
            m_uCapacity = capacity;

            // retained in property
            this.m_pTexture = texture;

            // CC_SAFE_RETAIN(m_pTexture);

            // Re-initialization is not allowed
            Debug.Assert(m_pIndices == null);

            m_pQuads = new List<ccV3F_C4B_T2F_Quad>();
            m_pIndices = new ushort[m_uCapacity * 6];

            this.initIndices();
            return true;
        }

        private void initIndices()
        {
            for (uint i = 0; i < m_uCapacity; i++)
            {
#if CC_TEXTURE_ATLAS_USE_TRIANGLE_STRIP
                m_pIndices[i * 6 + 0] = (ushort)(i * 4 + 0);
                m_pIndices[i * 6 + 1] = (ushort)(i * 4 + 0);
                m_pIndices[i * 6 + 2] = (ushort)(i * 4 + 2);
                m_pIndices[i * 6 + 3] = (ushort)(i * 4 + 1);
                m_pIndices[i * 6 + 4] = (ushort)(i * 4 + 3);
                m_pIndices[i * 6 + 5] = (ushort)(i * 4 + 3);
#else
                m_pIndices[i * 6 + 0] = (ushort)(i * 4 + 0);
                m_pIndices[i * 6 + 1] = (ushort)(i * 4 + 1);
                m_pIndices[i * 6 + 2] = (ushort)(i * 4 + 2);

                // inverted index. issue #179
                m_pIndices[i * 6 + 3] = (ushort)(i * 4 + 3);
                m_pIndices[i * 6 + 4] = (ushort)(i * 4 + 2);
                m_pIndices[i * 6 + 5] = (ushort)(i * 4 + 1);
#endif
            }
        }

        #endregion

        #region Quads

        /// <summary>
        /// updates a Quad (texture, vertex and color) at a certain index
        /// index must be between 0 and the atlas capacity - 1
        /// @since v0.8
        /// </summary>
        public void updateQuad(ccV3F_C4B_T2F_Quad quad, uint index)
        {
            Debug.Assert(index >= 0 && index < m_uCapacity, "updateQuadWithTexture: Invalid index");

            m_pQuads[(int)index] = quad;

#if CC_USES_VBO
            m_bDirty = true;
#endif
        }

        /// <summary>
        /// Inserts a Quad (texture, vertex and color) at a certain index
        /// index must be between 0 and the atlas capacity - 1
        /// @since v0.8
        /// </summary>
        public void insertQuad(ccV3F_C4B_T2F_Quad quad, uint index)
        {
            Debug.Assert(index >= 0 && index < m_uCapacity, "insertQuadWithTexture: Invalid index");

            m_pQuads.Insert((int)index, quad);

#if CC_USES_VBO
            m_bDirty = true;
#endif
        }

        /// <summary>
        /// Removes the quad that is located at a certain index and inserts it at a new index
        /// This operation is faster than removing and inserting in a quad in 2 different steps
        /// @since v0.7.2
        public void insertQuadFromIndex(uint oldIndex, uint newIndex)
        {
            Debug.Assert(newIndex >= 0 && newIndex < m_pQuads.Count, "insertQuadFromIndex:atIndex: Invalid index");
            Debug.Assert(oldIndex >= 0 && oldIndex < m_pQuads.Count, "insertQuadFromIndex:atIndex: Invalid index");

            ccV3F_C4B_T2F_Quad quadsBackup = m_pQuads[(int)oldIndex];
            m_pQuads.Remove(quadsBackup);
            m_pQuads[(int)newIndex] = quadsBackup;

#if CC_USES_VBO
            m_bDirty = true;
#endif
        }

        /// <summary>
        /// removes a quad at a given index number.
        /// The capacity remains the same, but the total number of quads to be drawn is reduced in 1
        /// @since v0.7.2
        /// </summary>
        public void removeQuadAtIndex(uint index)
        {
            Debug.Assert(index < m_pQuads.Count, "removeQuadAtIndex: Invalid index");

            m_pQuads.RemoveAt((int)index);

#if CC_USES_VBO
            m_bDirty = true;
#endif
        }

        /// <summary>
        /// removes all Quads.
        /// The TextureAtlas capacity remains untouched. No memory is freed.
        /// The total number of quads to be drawn will be 0
        /// @since v0.7.2
        /// </summary>
        public void removeAllQuads()
        {
            m_pQuads.Clear();
        }

        #endregion

        /// <summary>
        /// resize the capacity of the CCTextureAtlas.
        /// The new capacity can be lower or higher than the current one
        /// It returns YES if the resize was successful.
        ///  If it fails to resize the capacity it will return NO with a new capacity of 0.
        /// </summary>
        public bool resizeCapacity(int newCapacity)
        {
            if (newCapacity == m_uCapacity)
                return true;

            m_uCapacity = newCapacity;

            m_pQuads = new List<ccV3F_C4B_T2F_Quad>();
            m_pIndices = new ushort[m_uCapacity * 6];

            this.initIndices();

#if CC_USES_VBO
            m_bDirty = true;
#endif

            return true;
        }

        /// <summary>
        /// draws all the Atlas's Quads
        /// </summary>
        public void drawQuads()
        {
            this.drawNumberOfQuads(m_pQuads.Count, 0);
        }

        /// <summary>
        /// draws n quads
        ///  can't be greater than the capacity of the Atlas
        ///  n
        /// </summary>
        public void drawNumberOfQuads(int n)
        {
            this.drawNumberOfQuads(n, 0);
        }

        /// <summary>
        /// draws n quads from an index (offset).
        /// n + start can't be greater than the capacity of the atlas
        /// @since v1.0
        /// </summary>
        public void drawNumberOfQuads(int n, int start)
        {
            //glBindTexture(GL_TEXTURE_2D, m_pTexture.getName());

            ccV3F_C4B_T2F_Quad quad = m_pQuads[0];

            CCApplication.sharedApplication().spriteBatch.Begin();
            CCApplication.sharedApplication().spriteBatch.Draw(m_pTexture.getTexture2D(), new Microsoft.Xna.Framework.Vector2(0, 0), Color.White);
            CCApplication.sharedApplication().spriteBatch.End();
        }

        #region properties

        /// <summary>
        /// quantity of quads that are going to be drawn
        /// </summary>
        public int TotalQuads
        {
            get { return m_pQuads.Count; }
        }

        protected int m_uCapacity;
        /// <summary>
        /// quantity of quads that can be stored with the current texture atlas size
        /// </summary>
        public int Capacity
        {
            get { return m_uCapacity; }
            set { m_uCapacity = value; }
        }

        protected CCTexture2D m_pTexture;
        /// <summary>
        /// Texture of the texture atlas
        /// </summary>
        public CCTexture2D Texture
        {
            get { return m_pTexture; }
            set { m_pTexture = value; }
        }

        protected List<ccV3F_C4B_T2F_Quad> m_pQuads;
        /// <summary>
        /// Quads that are going to be rendered
        /// </summary>
        public List<ccV3F_C4B_T2F_Quad> Quads
        {
            get { return m_pQuads; }
            set { m_pQuads = value; }
        }

        #endregion
    }
}
