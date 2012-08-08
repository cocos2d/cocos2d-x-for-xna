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
    public class CCTiledGrid3D : CCGridBase
    {
        /// <summary>
        ///  returns the tile at the given position
        /// </summary>
        public ccQuad3 tile(ccGridSize pos)
        {
            int idx = (m_sGridSize.y * pos.x + pos.y);
            ccQuad3[] vertArray = m_pVertices;

            ccQuad3 ret = new ccQuad3();
            ret.bl = new ccVertex3F(vertArray[idx].bl.x, vertArray[idx].bl.y, vertArray[idx].bl.z);
            ret.br = new ccVertex3F(vertArray[idx].br.x, vertArray[idx].br.y, vertArray[idx].br.z);
            ret.tl = new ccVertex3F(vertArray[idx].tl.x, vertArray[idx].tl.y, vertArray[idx].tl.z);
            ret.tr = new ccVertex3F(vertArray[idx].tr.x, vertArray[idx].tr.y, vertArray[idx].tr.z);

            return ret;
        }

        /// <summary>
        /// returns the original tile (untransformed) at the given position
        /// </summary>
        public ccQuad3 originalTile(ccGridSize pos)
        {
            int idx = (m_sGridSize.y * pos.x + pos.y);
            ccQuad3[] vertArray = m_pOriginalVertices;

            ccQuad3 ret = new ccQuad3();
            ret.bl = new ccVertex3F(vertArray[idx].bl.x, vertArray[idx].bl.y, vertArray[idx].bl.z);
            ret.br = new ccVertex3F(vertArray[idx].br.x, vertArray[idx].br.y, vertArray[idx].br.z);
            ret.tl = new ccVertex3F(vertArray[idx].tl.x, vertArray[idx].tl.y, vertArray[idx].tl.z);
            ret.tr = new ccVertex3F(vertArray[idx].tr.x, vertArray[idx].tr.y, vertArray[idx].tr.z);

            return ret;
        }

        /// <summary>
        /// sets a new tile
        /// </summary>
        public void setTile(ccGridSize pos, ccQuad3 coords)
        {
            int idx = (m_sGridSize.y * pos.x + pos.y);
            ccQuad3[] vertArray = m_pVertices;
            vertArray[idx] = coords;
        }

        public override void blit()
        {
            int n = m_sGridSize.x * m_sGridSize.y;

            // Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // Needed states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_TEXTURE_COORD_ARRAY
            // Unneeded states: GL_COLOR_ARRAY
            //glDisableClientState(GL_COLOR_ARRAY);

            //glVertexPointer(3, GL_FLOAT, 0, m_pVertices);
            //glTexCoordPointer(2, GL_FLOAT, 0, m_pTexCoordinates);
            //glDrawElements(GL_TRIANGLES, (GLsizei)n * 6, GL_UNSIGNED_SHORT, m_pIndices);

            //// restore default GL state
            //glEnableClientState(GL_COLOR_ARRAY);


            CCApplication app = CCApplication.sharedApplication();
            CCSize size = CCDirector.sharedDirector().getWinSize();

            //app.basicEffect.World = app.worldMatrix *TransformUtils.CGAffineToMatrix( this.nodeToWorldTransform());
            app.basicEffect.Texture = this.m_pTexture.getTexture2D();
            app.basicEffect.TextureEnabled = true;
            app.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            app.basicEffect.VertexColorEnabled = true;

            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();
            for (int i = 0; i < n; i++)
            {
                ccQuad3 quad = this.m_pVertices[i];
                ccQuad2 texQuad = this.m_pTexCoordinates[i];
                if (quad != null)
                {
                    VertexPositionColorTexture vt = new VertexPositionColorTexture();
                    vt.Position = new Vector3(quad.bl.x, quad.bl.y, quad.bl.z);
                    vt.Color = Color.White;
                    vt.TextureCoordinate = new Vector2(texQuad.bl.x, texQuad.bl.y);
                    vertices.Add(vt);

                    vt = new VertexPositionColorTexture();
                    vt.Position = new Vector3(quad.br.x, quad.br.y, quad.br.z);
                    vt.Color = Color.White;
                    vt.TextureCoordinate = new Vector2(texQuad.br.x, texQuad.br.y);
                    vertices.Add(vt);

                    vt = new VertexPositionColorTexture();
                    vt.Position = new Vector3(quad.tl.x, quad.tl.y, quad.tl.z);
                    vt.Color = Color.White;
                    vt.TextureCoordinate = new Vector2(texQuad.tl.x, texQuad.tl.y);
                    vertices.Add(vt);

                    vt = new VertexPositionColorTexture();
                    vt.Position = new Vector3(quad.tr.x, quad.tr.y, quad.tr.z);
                    vt.Color = Color.White;
                    vt.TextureCoordinate = new Vector2(texQuad.tr.x, texQuad.tr.y);
                    vertices.Add(vt);
                }
            }

            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.TriangleList,
                    vertices.ToArray(), 0, vertices.Count,
                    this.m_pIndices, 0, this.m_pIndices.Length / 3);
            }
        }

        public override void reuse()
        {
            if (m_nReuseGrid > 0)
            {
                int numQuads = m_sGridSize.x * m_sGridSize.y;

                Array.Copy(m_pVertices, m_pOriginalVertices, numQuads);
                //memcpy(m_pOriginalVertices, m_pVertices, numQuads * 12 * sizeof(GLfloat));
                m_pOriginalVertices = m_pVertices;
                --m_nReuseGrid;
            }
        }

        public override void calculateVertexPoints()
        {
            float width = m_pTexture.ContentSizeInPixels.width;// (float)m_pTexture.PixelsWide;
            float height = m_pTexture.ContentSizeInPixels.height;// (float)m_pTexture.PixelsHigh;
            float imageH = m_pTexture.ContentSizeInPixels.height;

            int numQuads = m_sGridSize.x * m_sGridSize.y;

            m_pVertices = new ccQuad3[numQuads];
            m_pOriginalVertices = new ccQuad3[numQuads];
            m_pTexCoordinates = new ccQuad2[numQuads];
            m_pIndices = new short[numQuads * 6];

            ccQuad3[] vertArray = m_pVertices;
            ccQuad2[] texArray = m_pTexCoordinates;
            short[] idxArray = m_pIndices;

            int x, y;
            int index = 0;

            for (x = 0; x < m_sGridSize.x; x++)
            {
                for (y = 0; y < m_sGridSize.y; y++)
                {
                    float x1 = x * m_obStep.x;
                    float x2 = x1 + m_obStep.x;
                    float y1 = y * m_obStep.y;
                    float y2 = y1 + m_obStep.y;

                    vertArray[index] = new ccQuad3();
                    vertArray[index].bl = new ccVertex3F(x1, y1, 0);
                    vertArray[index].br = new ccVertex3F(x2, y1, 0);
                    vertArray[index].tl = new ccVertex3F(x1, y2, 0);
                    vertArray[index].tr = new ccVertex3F(x2, y2, 0);

                    float newY1 = y1;
                    float newY2 = y2;

                    if (!m_bIsTextureFlipped)
                    {
                        newY1 = imageH - y1;
                        newY2 = imageH - y2;
                    }

                    texArray[index] = new ccQuad2();
                    texArray[index].bl = new ccVertex2F(x1 / width, newY1 / height);
                    texArray[index].br = new ccVertex2F(x2 / width, newY1 / height);
                    texArray[index].tl = new ccVertex2F(x1 / width, newY2 / height);
                    texArray[index].tr = new ccVertex2F(x2 / width, newY2 / height);

                    index++;
                }
            }

            for (x = 0; x < numQuads; x++)
            {
                idxArray[x * 6 + 0] = (short)(x * 4 + 0);
                idxArray[x * 6 + 1] = (short)(x * 4 + 2);
                idxArray[x * 6 + 2] = (short)(x * 4 + 1);

                idxArray[x * 6 + 3] = (short)(x * 4 + 1);
                idxArray[x * 6 + 4] = (short)(x * 4 + 2);
                idxArray[x * 6 + 5] = (short)(x * 4 + 3);
            }

            Array.Copy(m_pVertices, m_pOriginalVertices, numQuads);
        }

        public new static CCTiledGrid3D gridWithSize(ccGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            CCTiledGrid3D pRet = new CCTiledGrid3D();

            if (pRet.initWithSize(gridSize, pTexture, bFlipped))
            {
                return pRet;
            }

            return null;
        }
        public new static CCTiledGrid3D gridWithSize(ccGridSize gridSize)
        {
            CCTiledGrid3D pRet = new CCTiledGrid3D();

            if (pRet.initWithSize(gridSize))
            {
                return pRet;
            }

            return null;
        }

        protected ccQuad2[] m_pTexCoordinates;
        protected ccQuad3[] m_pVertices;
        protected ccQuad3[] m_pOriginalVertices;
        protected short[] m_pIndices;
    }
}
