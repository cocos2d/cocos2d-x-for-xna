/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /// <summary> 
    /// CCGrid3D is a 3D grid implementation. Each vertex has 3 dimensions: x,y,z
    /// </summary>
    public class CCGrid3D : CCGridBase
    {
        /// <summary>
        /// returns the vertex at a given position
        /// </summary>
        public ccVertex3F vertex(ccGridSize pos)
        {
            int index = (pos.x * (m_sGridSize.y + 1) + pos.y);
            ccVertex3F[] vertArray = m_pVertices;

            ccVertex3F vert = new ccVertex3F()
            {
                x = vertArray[index].x,
                y = vertArray[index].y,
                z = vertArray[index].z
            };

            return vert;
        }

        /// <summary>
        /// returns the original (non-transformed) vertex at a given position
        /// </summary>
        public ccVertex3F originalVertex(ccGridSize pos)
        {
            int index = (pos.x * (m_sGridSize.y + 1) + pos.y);
            ccVertex3F[] vertArray = m_pOriginalVertices;

            ccVertex3F vert = new ccVertex3F()
            {
                x = vertArray[index].x,
                y = vertArray[index].y,
                z = vertArray[index].z
            };

            return vert;
        }

        /// <summary>
        /// sets a new vertex at a given position
        /// </summary>
        public void setVertex(ccGridSize pos, ccVertex3F vertex)
        {
            int index = pos.x * (m_sGridSize.y + 1) + pos.y;
            ccVertex3F[] vertArray = m_pVertices;
            vertArray[index].x = vertex.x;
            vertArray[index].y = vertex.y;
            vertArray[index].z = vertex.z;
        }

        public override void blit()
        {
            int n = m_sGridSize.x * m_sGridSize.y;

            //////// Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            //////// Needed states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_TEXTURE_COORD_ARRAY
            //////// Unneeded states: GL_COLOR_ARRAY
            //////glDisableClientState(GL_COLOR_ARRAY);

            //////glVertexPointer(3, GL_FLOAT, 0, m_pVertices);
            //////glTexCoordPointer(2, GL_FLOAT, 0, m_pTexCoordinates);
            //////glDrawElements(GL_TRIANGLES, (GLsizei)n * 6, GL_UNSIGNED_SHORT, m_pIndices);

            //////// restore GL default state
            //////glEnableClientState(GL_COLOR_ARRAY);


            CCApplication app = CCApplication.sharedApplication();
            CCSize size = CCDirector.sharedDirector().getWinSize();

            //app.basicEffect.World = app.worldMatrix * TransformUtils.CGAffineToMatrix(this.nodeToWorldTransform());
            app.basicEffect.Texture = this.m_pTexture.getTexture2D();
            app.basicEffect.TextureEnabled = true;
            app.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            app.basicEffect.VertexColorEnabled = true;
            //RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.None;
            //app.GraphicsDevice.RasterizerState = rs;

            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();
            for (int i = 0; i < (m_sGridSize.x + 1) * (m_sGridSize.y + 1); i++)
            {
                VertexPositionColorTexture vct = new VertexPositionColorTexture();
                vct.Position = new Vector3(m_pVertices[i].x, m_pVertices[i].y, m_pVertices[i].z);
                vct.TextureCoordinate = new Vector2(m_pTexCoordinates[i].x, m_pTexCoordinates[i].y);
                vct.Color = Color.White;
                vertices.Add(vct);
            }

            short[] indexes = m_pIndices;

            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.TriangleList,
                    vertices.ToArray(), 0, vertices.Count,
                    indexes, 0, indexes.Length / 3);
            }
        }

        public override void reuse()
        {
            if (m_nReuseGrid > 0)
            {
                Array.Copy(m_pVertices, m_pOriginalVertices, (m_sGridSize.x + 1) * (m_sGridSize.y + 1));
                --m_nReuseGrid;
            }
        }

        public override void calculateVertexPoints()
        {
            float width = (float)m_pTexture.ContentSizeInPixels.width;
            float height = (float)m_pTexture.ContentSizeInPixels.height;
            float imageH = m_pTexture.ContentSizeInPixels.height;

            m_pVertices = new ccVertex3F[(m_sGridSize.x + 1) * (m_sGridSize.y + 1)];
            m_pOriginalVertices = new ccVertex3F[(m_sGridSize.x + 1) * (m_sGridSize.y + 1)];
            m_pTexCoordinates = new CCPoint[(m_sGridSize.x + 1) * (m_sGridSize.y + 1)];
            m_pIndices = new short[m_sGridSize.x * m_sGridSize.y * 6];

            ccVertex3F[] vertArray = m_pVertices;
            CCPoint[] texArray = m_pTexCoordinates;
            short[] idxArray = m_pIndices;

            //int idx = -1;
            for (int x = 0; x < m_sGridSize.x; ++x)
            {
                for (int y = 0; y < m_sGridSize.y; ++y)
                {
                    int idx = (y * m_sGridSize.x) + x;

                    float x1 = x * m_obStep.x;
                    float x2 = x1 + m_obStep.x;
                    float y1 = y * m_obStep.y;
                    float y2 = y1 + m_obStep.y;

                    int a = x * (m_sGridSize.y + 1) + y;
                    int b = (x + 1) * (m_sGridSize.y + 1) + y;
                    int c = (x + 1) * (m_sGridSize.y + 1) + (y + 1);
                    int d = x * (m_sGridSize.y + 1) + (y + 1);

                    short[] tempidx = new short[6] { (short)a, (short)d, (short)b, (short)b, (short)d, (short)c };
                    Array.Copy(tempidx, 0, idxArray, 6 * idx, tempidx.Length);

                    int[] l1 = new int[4] { a, b, c, d };
                    ccVertex3F e = new ccVertex3F(x1, y1, 0);
                    ccVertex3F f = new ccVertex3F(x2, y1, 0);
                    ccVertex3F g = new ccVertex3F(x2, y2, 0);
                    ccVertex3F h = new ccVertex3F(x1, y2, 0);

                    ccVertex3F[] l2 = new ccVertex3F[4] { e, f, g, h };

                    int[] tex1 = new int[4] { a, b, c, d };
                    CCPoint[] tex2 = new CCPoint[4] 
                    {
                    new CCPoint(x1, y1), 
                    new CCPoint(x2, y1), 
                    new CCPoint(x2, y2),
                    new CCPoint(x1, y2)
                    };

                    for (int i = 0; i < 4; ++i)
                    {
                        vertArray[l1[i]] = new ccVertex3F();
                        vertArray[l1[i]].x = l2[i].x;
                        vertArray[l1[i]].y = l2[i].y;
                        vertArray[l1[i]].z = l2[i].z;

                        texArray[tex1[i]] = new CCPoint();
                        texArray[tex1[i]].x = tex2[i].x / width;
                        if (m_bIsTextureFlipped)
                        {
                            texArray[tex1[i]].y = tex2[i].y / height;
                        }
                        else
                        {
                            texArray[tex1[i]].y = (imageH - tex2[i].y) / height;
                        }
                    }
                }
            }

            Array.Copy(m_pVertices, m_pOriginalVertices, (m_sGridSize.x + 1) * (m_sGridSize.y + 1));
        }

        public new static CCGrid3D gridWithSize(ccGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            CCGrid3D pRet = new CCGrid3D();

            if (pRet.initWithSize(gridSize, pTexture, bFlipped))
            {
                return pRet;
            }

            return null;
        }

        public new static CCGrid3D gridWithSize(ccGridSize gridSize)
        {
            CCGrid3D pRet = new CCGrid3D();

            if (pRet.initWithSize(gridSize))
            {
                return pRet;
            }

            return null;
        }

        protected CCPoint[] m_pTexCoordinates;
        protected ccVertex3F[] m_pVertices;
        protected ccVertex3F[] m_pOriginalVertices;
        protected short[] m_pIndices;
    }
}
