/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 Fulcrum Mobile Network, Inc
 
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
using Microsoft.Xna.Framework.Graphics;

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
        protected short[] m_pIndices;
#if CC_USES_VBO
        int[] m_pBuffersVBO; //0: vertex  1: indices
        bool m_bDirty; //indicates whether or not the array buffer of the VBO needs to be updated
#endif // CC_USES_VBO

        public CCTextureAtlas()
        {

        }

        public string description()
        {
            return string.Format("<CCTextureAtlas | totalQuads = {0}>", m_uTotalQuads);
        }

        #region create and init

        /// <summary>
        /// creates a TextureAtlas with an filename and with an initial capacity for Quads.
        /// The TextureAtlas capacity can be increased in runtime.
        /// </summary>
        public static CCTextureAtlas textureAtlasWithFile(string file, int capacity)
        {
            CCTextureAtlas pTextureAtlas = new CCTextureAtlas();
            if (pTextureAtlas.initWithFile(file, capacity))
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
            if (pTextureAtlas.initWithTexture(texture, capacity))
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
            m_uTotalQuads = 0;

            // retained in property
            this.m_pTexture = texture;

            // Re-initialization is not allowed
            Debug.Assert(m_pIndices == null && m_pIndices == null);

            m_pQuads = new ccV3F_C4B_T2F_Quad[m_uCapacity];
            m_pIndices = new short[m_uCapacity * 6];

#if CC_USES_VBO
            // initial binding
            //glGenBuffers(2, &m_pBuffersVBO[0]);	
            m_bDirty = true;
#endif // CC_USES_VBO

            this.initIndices();

            return true;
        }

        private void initIndices()
        {
            for (int i = 0; i < m_uCapacity; i++)
            {
#if CC_TEXTURE_ATLAS_USE_TRIANGLE_STRIP
                m_pIndices[i * 6 + 0] = (ushort)(i * 4 + 0);
                m_pIndices[i * 6 + 1] = (ushort)(i * 4 + 0);
                m_pIndices[i * 6 + 2] = (ushort)(i * 4 + 2);
                m_pIndices[i * 6 + 3] = (ushort)(i * 4 + 1);
                m_pIndices[i * 6 + 4] = (ushort)(i * 4 + 3);
                m_pIndices[i * 6 + 5] = (ushort)(i * 4 + 3);
#else
                m_pIndices[i * 6 + 0] = (short)(i * 4 + 0);
                m_pIndices[i * 6 + 1] = (short)(i * 4 + 1);
                m_pIndices[i * 6 + 2] = (short)(i * 4 + 2);

                // inverted index. issue #179
                m_pIndices[i * 6 + 3] = (short)(i * 4 + 3);
                m_pIndices[i * 6 + 4] = (short)(i * 4 + 2);
                m_pIndices[i * 6 + 5] = (short)(i * 4 + 1);
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
        public void updateQuad(ccV3F_C4B_T2F_Quad quad, int index)
        {
            Debug.Assert(index >= 0 && index < m_uCapacity, "updateQuadWithTexture: Invalid index");
            m_uTotalQuads = Math.Max(index + 1, m_uTotalQuads);
            m_pQuads[index] = quad;

#if CC_USES_VBO
            m_bDirty = true;
#endif
        }

        /// <summary>
        /// Inserts a Quad (texture, vertex and color) at a certain index
        /// index must be between 0 and the atlas capacity - 1
        /// @since v0.8
        /// </summary>
        public void insertQuad(ccV3F_C4B_T2F_Quad quad, int index)
        {
            Debug.Assert(index < m_uCapacity, "insertQuadWithTexture: Invalid index");

            m_uTotalQuads++;
            Debug.Assert(m_uTotalQuads <= m_uCapacity, "invalid totalQuads");

            // issue #575. index can be > totalQuads
            int remaining = (m_uTotalQuads - 1) - index;

            // last object doesn't need to be moved
            if (remaining > 0)
            {
                // texture coordinates
                Array.Copy(m_pQuads, index, m_pQuads, index + 1, remaining);
            }

            m_pQuads[index] = quad;

#if CC_USES_VBO
            m_bDirty = true;
#endif
        }

        /// <summary>
        /// Removes the quad that is located at a certain index and inserts it at a new index
        /// This operation is faster than removing and inserting in a quad in 2 different steps
        /// @since v0.7.2
        public void insertQuadFromIndex(int oldIndex, int newIndex)
        {
            Debug.Assert(newIndex >= 0 && newIndex < m_uTotalQuads, "insertQuadFromIndex:atIndex: Invalid index");
            Debug.Assert(oldIndex >= 0 && oldIndex < m_uTotalQuads, "insertQuadFromIndex:atIndex: Invalid index");

            if (oldIndex == newIndex)
                return;

            // because it is ambigious in iphone, so we implement abs ourself
            // unsigned int howMany = abs( oldIndex - newIndex);
            int howMany = (oldIndex - newIndex) > 0 ? (oldIndex - newIndex) : (newIndex - oldIndex);
            int dst = oldIndex;
            int src = oldIndex + 1;
            if (oldIndex > newIndex)
            {
                dst = newIndex + 1;
                src = newIndex;
            }

            ccV3F_C4B_T2F_Quad quadsBackup = m_pQuads[oldIndex];

            if (oldIndex > newIndex)
            {
                Array.Copy(m_pQuads, newIndex, m_pQuads, newIndex + 1, howMany);
                m_pQuads[newIndex] = quadsBackup;
            }
            else
            {
                Array.Copy(m_pQuads, newIndex + 1, m_pQuads, newIndex, howMany);
                m_pQuads[newIndex] = quadsBackup;
            }

#if CC_USES_VBO
            m_bDirty = true;
#endif
        }

        /// <summary>
        /// removes a quad at a given index number.
        /// The capacity remains the same, but the total number of quads to be drawn is reduced in 1
        /// @since v0.7.2
        /// </summary>
        public void removeQuadAtIndex(int index)
        {

#warning 颜色问题能解决但是出现了头掉的bug
            //            Debug.Assert(index < m_uTotalQuads, "removeQuadAtIndex: Invalid index");

            //            ccV3F_C4B_T2F_Quad[] temp = new ccV3F_C4B_T2F_Quad[m_uTotalQuads - 1];
            //            Array.Copy(m_pQuads, temp, index);
            //            Array.Copy(m_pQuads, index + 1, temp, index, m_uTotalQuads - index);
            //            m_pQuads = temp;
            //            m_uTotalQuads--;
            //#if CC_USES_VBO
            //            m_bDirty = true;
            //#endif
        }

        /// <summary>
        /// removes all Quads.
        /// The TextureAtlas capacity remains untouched. No memory is freed.
        /// The total number of quads to be drawn will be 0
        /// @since v0.7.2
        /// </summary>
        public void removeAllQuads()
        {
            m_pQuads = null;
            m_uTotalQuads = 0;
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

            m_uTotalQuads = Math.Min(m_uTotalQuads, newCapacity);
            m_uCapacity = newCapacity;

            ccV3F_C4B_T2F_Quad[] temp = new ccV3F_C4B_T2F_Quad[newCapacity];
            Array.Copy(m_pQuads, temp, m_pQuads.Length);
            m_pQuads = temp;
            m_pIndices = new short[m_uCapacity * 6];

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
            this.drawNumberOfQuads(m_uTotalQuads, 0);
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
            if (n == start)
            {
                return;
            }

            if (this.m_pQuads == null || this.m_pQuads.Length < 1)
            {
                return;
            }

            CCApplication app = CCApplication.sharedApplication();
            CCSize size = CCDirector.sharedDirector().getWinSize();

            //app.basicEffect.World = app.worldMatrix *TransformUtils.CGAffineToMatrix( this.nodeToWorldTransform());
            app.basicEffect.Texture = this.Texture.getTexture2D();
            app.basicEffect.TextureEnabled = true;
            app.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            app.basicEffect.VertexColorEnabled = true;

            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();
            short[] indexes = new short[n * 6];
            for (int i = start; i < start + n; i++)
            {
                ccV3F_C4B_T2F_Quad quad = this.m_pQuads[i];
                if (quad != null)
                {
                    vertices.AddRange(quad.getVertices(ccDirectorProjection.CCDirectorProjection3D).ToList());

                    indexes[i * 6 + 0] = (short)(i * 4 + 0);
                    indexes[i * 6 + 1] = (short)(i * 4 + 1);
                    indexes[i * 6 + 2] = (short)(i * 4 + 2);

                    // inverted index. issue #179
                    indexes[i * 6 + 3] = (short)(i * 4 + 2);
                    indexes[i * 6 + 4] = (short)(i * 4 + 1);
                    indexes[i * 6 + 5] = (short)(i * 4 + 3);
                }
            }

            VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Color, 0),
                    new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                });

            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.TriangleList,
                    vertices.ToArray(), 0, vertices.Count,
                    indexes, 0, vertices.Count / 2);
            }
        }

        #region properties

        int m_uTotalQuads;

        /// <summary>
        /// quantity of quads that are going to be drawn
        /// </summary>
        public int TotalQuads
        {
            get { return m_uTotalQuads; }
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

        protected ccV3F_C4B_T2F_Quad[] m_pQuads;
        /// <summary>
        /// Quads that are going to be rendered
        /// </summary>
        public ccV3F_C4B_T2F_Quad[] Quads
        {
            get { return m_pQuads; }
            set { m_pQuads = value; }
        }

        #endregion
    }
}
