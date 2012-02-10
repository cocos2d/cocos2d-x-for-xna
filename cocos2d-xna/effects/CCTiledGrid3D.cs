/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
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

namespace cocos2d
{
    public class CCTiledGrid3D : CCGridBase
    {
        /// <summary>
        ///  returns the tile at the given position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public ccQuad3 tile(ccGridSize pos)
        {
            int idx = (m_sGridSize.y * pos.x + pos.y) * 4 * 3;
            float[] vertArray = m_pVertices;

            ccQuad3 ret = new ccQuad3();
            //memcpy(&ret, &vertArray[idx], sizeof(ccQuad3));

            return ret;
        }

        /// <summary>
        /// returns the original tile (untransformed) at the given position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public ccQuad3 originalTile(ccGridSize pos)
        {
            int idx = (m_sGridSize.y * pos.x + pos.y) * 4 * 3;
            float[] vertArray = m_pOriginalVertices;

            ccQuad3 ret = new ccQuad3();
            //memcpy(&ret, &vertArray[idx], sizeof(ccQuad3));
            //ret = vertArray[idx];
            return ret;
        }

        /// <summary>
        /// sets a new tile
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="coords"></param>
        public void setTile(ccGridSize pos, ccQuad3 coords)
        {
            int idx = (m_sGridSize.y * pos.x + pos.y) * 4 * 3;
            float[] vertArray = m_pVertices;
            //memcpy(&vertArray[idx], &coords, sizeof(ccQuad3));
        }

        public virtual void blit()
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
        }
        public virtual void reuse()
        {
            if (m_nReuseGrid > 0)
            {
                int numQuads = m_sGridSize.x * m_sGridSize.y;

                //memcpy(m_pOriginalVertices, m_pVertices, numQuads * 12 * sizeof(GLfloat));
                m_pOriginalVertices = m_pVertices;
                --m_nReuseGrid;
            }
        }

        public virtual void calculateVertexPoints()
        {
            float width = (float)m_pTexture.PixelsWide;
            float height = (float)m_pTexture.PixelsHigh;
            float imageH = m_pTexture.ContentSizeInPixels.height;

            int numQuads = m_sGridSize.x * m_sGridSize.y;

            m_pVertices = new float[12];
            m_pOriginalVertices = new float[12];
            m_pTexCoordinates = new float[8];
            m_pIndices = new ushort[6];

            float[] vertArray = m_pVertices;
            float[] texArray = m_pTexCoordinates;
            ushort[] idxArray = m_pIndices;

            int x, y;

            for (x = 0; x < m_sGridSize.x; x++)
            {
                for (y = 0; y < m_sGridSize.y; y++)
                {
                    float x1 = x * m_obStep.x;
                    float x2 = x1 + m_obStep.x;
                    float y1 = y * m_obStep.y;
                    float y2 = y1 + m_obStep.y;

                    vertArray[0] = x1;
                    vertArray[1] = y1;
                    vertArray[2] = 0;
                    vertArray[3] = x2;
                    vertArray[4] = y1;
                    vertArray[5] = 0;
                    vertArray[6] = x1;
                    vertArray[7] = y2;
                    vertArray[8] = 0;
                    vertArray[9] = x2;
                    vertArray[10] = y2;
                    vertArray[11] = 0;

                    float newY1 = y1;
                    float newY2 = y2;

                    if (m_bIsTextureFlipped)
                    {
                        newY1 = imageH - y1;
                        newY2 = imageH - y2;
                    }

                    texArray[0] = x1 / width;
                    texArray[1] = newY1 / height;
                    texArray[2] = x2 / width;
                    texArray[3] = newY1 / height;
                    texArray[4] = x1 / width;
                    texArray[5] = newY2 / height;
                    texArray[6] = x2 / width;
                    texArray[7] = newY2 / height;
                }
            }

            for (x = 0; x < numQuads; x++)
            {
                idxArray[x * 6 + 0] = (ushort)(x * 4 + 0);
                idxArray[x * 6 + 1] = (ushort)(x * 4 + 1);
                idxArray[x * 6 + 2] = (ushort)(x * 4 + 2);

                idxArray[x * 6 + 3] = (ushort)(x * 4 + 1);
                idxArray[x * 6 + 4] = (ushort)(x * 4 + 2);
                idxArray[x * 6 + 5] = (ushort)(x * 4 + 3);
            }

            //memcpy(m_pOriginalVertices, m_pVertices, numQuads * 12 * sizeof(GLfloat));
        }

        public static CCTiledGrid3D gridWithSize(ccGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            CCTiledGrid3D pRet = new CCTiledGrid3D();

            if (pRet != null)
            {
                if (pRet.initWithSize(gridSize, pTexture, bFlipped))
                {
                    //pRet->autorelease();
                }
                else
                {
                    //delete pRet;
                    pRet = null;
                }
            }

            return pRet;
        }
        public static CCTiledGrid3D gridWithSize(ccGridSize gridSize)
        {
            CCTiledGrid3D pRet = new CCTiledGrid3D();

            if (pRet != null)
            {
                if (pRet.initWithSize(gridSize))
                {
                    //pRet->autorelease();
                }
                else
                {
                    //delete pRet;
                    pRet = null;
                }
            }
            return pRet;
        }

        protected float[] m_pTexCoordinates;
        protected float[] m_pVertices;
        protected float[] m_pOriginalVertices;
        protected ushort[] m_pIndices;
    }
}
