/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
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
    /// RGB color composed of bytes 3 bytes
    /// @since v0.8
    /// </summary>
    public class ccColor3B
    {
        public ccColor3B()
        {
            r = 0;
            g = 0;
            b = 0;
        }

        public ccColor3B(ccColor3B copy)
        {
            r = copy.r;
            g = copy.g;
            b = copy.b;
        }

        public ccColor3B(byte inr, byte ing, byte inb)
        {
            r = inr;
            g = ing;
            b = inb;
        }

        public Color XNAColor
        {
            get
            {
                return (new Color(r, g, b, 255));
            }
        }

        public ccColor3B copy()
        {
            return (new ccColor3B(r, g, b));
        }

        /// <summary>
        /// Convert Color value of XNA Framework to ccColor3B type
        /// </summary>
        public ccColor3B(Microsoft.Xna.Framework.Color color)
        {
            r = color.R;
            g = color.G; // was color.B
            b = color.B;
        }

        public byte r;
        public byte g;
        public byte b;
    }

    /// <summary>
    /// RGBA color composed of 4 bytes
    /// @since v0.8
    /// </summary>
    public class ccColor4B
    {
        /// <summary>
        /// Constructs a transparent (a=0) instance of color4B.
        /// </summary>
        public ccColor4B()
        {
            r = 0;
            g = 0;
            b = 0;
            a = 0;
        }

        public Color XNAColor
        {
            get
            {
                return (new Color(r, g, b, a));
            }
        }

        public ccColor4B(ccColor4B copy)
        {
            r = copy.r;
            g = copy.g;
            b = copy.b;
            a = copy.a;
        }
        public ccColor4B(ccColor3B c3)
        {
            r = c3.r;
            g = c3.g;
            b = c3.b;
            a = 255;
        }
        public ccColor4B(byte inr, byte ing, byte inb, byte ina)
        {
            r = inr;
            g = ing;
            b = inb;
            a = ina;
        }

        public ccColor4B copy()
        {
            return (new ccColor4B(r, g, b, a));
        }
        /// <summary>
        /// Convert Color value of XNA Framework to ccColor4B type
        /// </summary>
        public ccColor4B(Microsoft.Xna.Framework.Color color)
        {
            r = color.R;
            g = color.G; // was color.B
            b = color.B;
            a = color.A;
        }

        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public string ToString()
        {
            return (string.Format("{0},{1},{2},{3}", r, g, b, a));
        }

        public static ccColor4B Parse(string s)
        {
            string[] f = s.Split(',');
            return(new ccColor4B(byte.Parse(f[0]), byte.Parse(f[1]), byte.Parse(f[2]), byte.Parse(f[3])));
        }
    }

    /// <summary>
    /// RGBA color composed of 4 floats
    /// @since v0.8
    /// </summary>
    public class ccColor4F
    {
        public ccColor4F()
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
            a = 0.0f;
        }

        public ccColor4F(float inr, float ing, float inb, float ina)
        {
            r = inr;
            g = ing;
            b = inb;
            a = ina;
        }

        public ccColor4F copy()
        {
            return (new ccColor4F(r, g, b, a));
        }

        public float r;
        public float g;
        public float b;
        public float a;

        public string ToString()
        {
            return (string.Format("{0},{1},{2},{3}", r, g, b, a));
        }

        public static ccColor4F Parse(string s)
        {
            string[] f = s.Split(',');
            return(new ccColor4F(float.Parse(f[0]), float.Parse(f[1]), float.Parse(f[2]), float.Parse(f[3])));
        }
    }

    /// <summary>
    /// A vertex composed of 2 floats: x, y
    /// @since v0.8
    /// </summary>
    public class ccVertex2F
    {
        public ccVertex2F()
        {
            x = 0.0f;
            y = 0.0f;
        }

        public ccVertex2F(float inx, float iny)
        {
            x = inx;
            y = iny;
        }

        public ccVertex2F(ccVertex2F copy)
        {
            x = copy.x;
            y = copy.y;
        }

        public ccVertex2F copy()
        {
            return (new ccVertex2F(this));
        }
        public Vector3 ToVector3()
        {
            return (new Vector3(x, y, 0f));
        }
        public float x;
        public float y;
    }

    /// <summary>
    /// A vertex composed of 2 floats: x, y
    /// @since v0.8
    /// </summary>
    public class ccVertex3F
    {
        public ccVertex3F()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }

        public ccVertex3F(float inx, float iny, float inz)
        {
            x = inx;
            y = iny;
            z = inz;
        }

        public Vector3 ToVector3()
        {
            return (new Vector3(x, y, z));
        }

        public float x;
        public float y;
        public float z;
    }

    /// <summary>
    /// A texcoord composed of 2 floats: u, y
    /// @since v0.8
    /// </summary>
    public class ccTex2F
    {
        public ccTex2F()
        {
            u = 0.0f;
            v = 0.0f;
        }

        public ccTex2F(float inu, float inv)
        {
            u = inu;
            v = inv;
        }

        public Vector2 ToVector2()
        {
            return (new Vector2(u, v));
        }

        public float u;
        public float v;
    }

    /// <summary>
    /// Point Sprite component
    /// </summary>
    public class ccPointSprite
    {
        public ccPointSprite()
        {
            pos = new ccVertex2F();
            color = new ccColor4B();
            size = 0.0f;
        }

        public ccVertex2F pos;		// 8 bytes
        public ccColor4B color;		// 4 bytes
        public float size;		// 4 bytes
    }

    /// <summary>
    /// A 2D Quad. 4 * 2 floats
    /// </summary>
    public class ccQuad2
    {
        public ccQuad2()
        {
            tl = new ccVertex2F();
            tr = new ccVertex2F();
            bl = new ccVertex2F();
            br = new ccVertex2F();
        }

        public ccVertex2F tl;
        public ccVertex2F tr;
        public ccVertex2F bl;
        public ccVertex2F br;
    }

    /// <summary>
    /// A 3D Quad. 4 * 3 floats
    /// </summary>
    public class ccQuad3
    {
        public ccQuad3()
        {
            tl = new ccVertex3F();
            tr = new ccVertex3F();
            bl = new ccVertex3F();
            br = new ccVertex3F();
        }

        public ccVertex3F bl;
        public ccVertex3F br;
        public ccVertex3F tl;
        public ccVertex3F tr;
    }

    /// <summary>
    /// A 2D grid size
    /// </summary>
    public class ccGridSize
    {
        public ccGridSize()
        {
            x = 0;
            y = 0;
        }

        public ccGridSize(int inx, int iny)
        {
            x = inx;
            y = iny;
        }

        public void set(int inx, int iny)
        {
            x = inx;
            y = iny;
        }
        public int x;
        public int y;
    }

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4B
    /// </summary>
    public class ccV2F_C4B_T2F
    {
        public ccV2F_C4B_T2F()
        {
            vertices = new ccVertex2F();
            colors = new ccColor4B();
            texCoords = new ccTex2F();
        }

        /// <summary>
        /// vertices (2F)
        /// </summary>
        public ccVertex2F vertices;

        /// <summary>
        /// colors (4B)
        /// </summary>
        public ccColor4B colors;

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        public ccTex2F texCoords;
    }

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4F
    /// </summary>
    public class ccV2F_C4F_T2F
    {
        public ccV2F_C4F_T2F()
        {
            vertices = new ccVertex2F();
            colors = new ccColor4F();
            texCoords = new ccTex2F();
        }

        /// <summary>
        /// vertices (2F)
        /// </summary>
        public ccVertex2F vertices;

        /// <summary>
        /// colors (4F)
        /// </summary>
        public ccColor4F colors;

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        public ccTex2F texCoords;
    }

    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4B
    /// </summary>
    public class ccV3F_C4B_T2F
    {
        public ccV3F_C4B_T2F()
        {
            vertices = new ccVertex3F();
            colors = new ccColor4B();
            texCoords = new ccTex2F();
        }

        /// <summary>
        /// vertices (3F)
        /// </summary>
        public ccVertex3F vertices;			// 12 bytes

        /// <summary>
        /// colors (4B)
        /// </summary>
        public ccColor4B colors;				// 4 bytes

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        public ccTex2F texCoords;			// 8 byts
    }

    /// <summary>
    /// 4 ccVertex2FTex2FColor4B Quad
    /// </summary>
    public class ccV2F_C4B_T2F_Quad
    {
        public ccV2F_C4B_T2F_Quad()
        {
            bl = new ccV2F_C4B_T2F();
            br = new ccV2F_C4B_T2F();
            tl = new ccV2F_C4B_T2F();
            tr = new ccV2F_C4B_T2F();
        }

        /// <summary>
        /// bottom left
        /// </summary>
        public ccV2F_C4B_T2F bl;

        /// <summary>
        /// bottom right
        /// </summary>
        public ccV2F_C4B_T2F br;

        /// <summary>
        /// top left
        /// </summary>
        public ccV2F_C4B_T2F tl;

        /// <summary>
        /// top right
        /// </summary>
        public ccV2F_C4B_T2F tr;
    }

    /// <summary>
    /// 4 ccVertex3FTex2FColor4B
    /// </summary>
    public class ccV3F_C4B_T2F_Quad
    {
        public ccV3F_C4B_T2F_Quad()
        {
            tl = new ccV3F_C4B_T2F();
            bl = new ccV3F_C4B_T2F();
            tr = new ccV3F_C4B_T2F();
            br = new ccV3F_C4B_T2F();
        }

        /// <summary>
        /// top left
        /// </summary>
        public ccV3F_C4B_T2F tl;

        /// <summary>
        /// bottom left
        /// </summary>
        public ccV3F_C4B_T2F bl;

        /// <summary>
        /// top right
        /// </summary>
        public ccV3F_C4B_T2F tr;

        /// <summary>
        /// bottom right
        /// </summary>
        public ccV3F_C4B_T2F br;

        public VertexPositionColorTexture[] getVertices(ccDirectorProjection projection)
        {
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];

            if (projection == ccDirectorProjection.kCCDirectorProjection2D)
            {
                vertices[0] = new VertexPositionColorTexture(
                    new Vector3(this.bl.vertices.x, this.bl.vertices.y, this.bl.vertices.z),
                    new Color(this.tl.colors.r, this.tl.colors.g, this.tl.colors.b, this.tl.colors.a),
                    new Vector2(this.bl.texCoords.u, this.bl.texCoords.v));

                vertices[1] = new VertexPositionColorTexture(
                    new Vector3(this.br.vertices.x, this.br.vertices.y, this.br.vertices.z),
                    new Color(this.tr.colors.r, this.tr.colors.g, this.tr.colors.b, this.tr.colors.a),
                    new Vector2(this.br.texCoords.u, this.br.texCoords.v));

                vertices[2] = new VertexPositionColorTexture(
                    new Vector3(this.tl.vertices.x, this.tl.vertices.y, this.tl.vertices.z),
                    new Color(this.bl.colors.r, this.bl.colors.g, this.bl.colors.b, this.bl.colors.a),
                    new Vector2(this.tl.texCoords.u, this.tl.texCoords.v));

                vertices[3] = new VertexPositionColorTexture(
                    new Vector3(this.tr.vertices.x, this.tr.vertices.y, this.tr.vertices.z),
                    new Color(this.br.colors.r, this.br.colors.g, this.br.colors.b, this.br.colors.a),
                    new Vector2(this.tr.texCoords.u, this.tr.texCoords.v));
            }
            else
            {
                vertices[0] = new VertexPositionColorTexture(
                    new Vector3(this.tl.vertices.x, this.tl.vertices.y, this.tl.vertices.z),
                    new Color(this.tl.colors.r, this.tl.colors.g, this.tl.colors.b, this.tl.colors.a),
                    new Vector2(this.tl.texCoords.u, this.tl.texCoords.v));

                vertices[1] = new VertexPositionColorTexture(
                    new Vector3(this.tr.vertices.x, this.tr.vertices.y, this.tr.vertices.z),
                    new Color(this.tr.colors.r, this.tr.colors.g, this.tr.colors.b, this.tr.colors.a),
                    new Vector2(this.tr.texCoords.u, this.tr.texCoords.v));

                vertices[2] = new VertexPositionColorTexture(
                    new Vector3(this.bl.vertices.x, this.bl.vertices.y, this.bl.vertices.z),
                    new Color(this.bl.colors.r, this.bl.colors.g, this.bl.colors.b, this.bl.colors.a),
                     new Vector2(this.bl.texCoords.u, this.bl.texCoords.v));

                vertices[3] = new VertexPositionColorTexture(
                    new Vector3(this.br.vertices.x, this.br.vertices.y, this.br.vertices.z),
                    new Color(this.br.colors.r, this.br.colors.g, this.br.colors.b, this.br.colors.a),
                    new Vector2(this.br.texCoords.u, this.br.texCoords.v));
            }

            return vertices;
        }

        public short[] getIndexes(ccDirectorProjection projection)
        {
            short[] indexes = new short[6];

            indexes[0] = 0;
            indexes[1] = 1;
            indexes[2] = 2;
            indexes[3] = 2;
            indexes[4] = 1;
            indexes[5] = 3;

            return indexes;
        }
    }

    /// <summary>
    /// 4 ccVertex2FTex2FColor4F Quad
    /// </summary>
    public class ccV2F_C4F_T2F_Quad
    {
        public ccV2F_C4F_T2F_Quad()
        {
            tl = new ccV2F_C4F_T2F();
            bl = new ccV2F_C4F_T2F();
            tr = new ccV2F_C4F_T2F();
            br = new ccV2F_C4F_T2F();
        }

        /// <summary>
        /// bottom left
        /// </summary>
        public ccV2F_C4F_T2F bl;

        /// <summary>
        /// bottom right
        /// </summary>
        public ccV2F_C4F_T2F br;

        /// <summary>
        /// top left
        /// </summary>
        public ccV2F_C4F_T2F tl;

        /// <summary>
        /// top right
        /// </summary>
        public ccV2F_C4F_T2F tr;
    }

    /// <summary>
    /// Blend Function used for textures
    /// </summary>
    public class ccBlendFunc
    {
        public ccBlendFunc()
        {

        }

        public ccBlendFunc(int src, int dst)
        {
            this.src = src;
            this.dst = dst;
        }

        /// <summary>
        /// source blend function
        /// </summary>
        public int src;

        /// <summary>
        /// destination blend function
        /// </summary>
        public int dst;
    }

    public enum CCTextAlignment
    {
        CCTextAlignmentLeft,
        CCTextAlignmentCenter,
        CCTextAlignmentRight,
    }

    public class ccTypes
    {
        //ccColor3B predefined colors
        //! White color (255,255,255)
        public static readonly ccColor3B ccWHITE = new ccColor3B(255, 255, 255);
        //! Yellow color (255,255,0)
        public static readonly ccColor3B ccYELLOW = new ccColor3B(255, 255, 0);
        //! Blue color (0,0,255)
        public static readonly ccColor3B ccBLUE = new ccColor3B(0, 0, 255);
        //! Green Color (0,255,0)
        public static readonly ccColor3B ccGREEN = new ccColor3B(0, 255, 0);
        //! Red Color (255,0,0,)
        public static readonly ccColor3B ccRED = new ccColor3B(255, 0, 0);
        //! Magenta Color (255,0,255)
        public static readonly ccColor3B ccMAGENTA = new ccColor3B(255, 0, 255);
        //! Black Color (0,0,0)
        public static readonly ccColor3B ccBLACK = new ccColor3B(0, 0, 0);
        //! Orange Color (255,127,0)
        public static readonly ccColor3B ccORANGE = new ccColor3B(255, 127, 0);
        //! Gray Color (166,166,166)
        public static readonly ccColor3B ccGRAY = new ccColor3B(166, 166, 166);

        //! helper macro that creates an ccColor3B type
        static public ccColor3B ccc3(byte r, byte g, byte b)
        {
            ccColor3B c = new ccColor3B(r, g, b);
            return c;
        }

        //! helper macro that creates an ccColor4B type
        public static ccColor4B ccc4(byte r, byte g, byte b, byte o)
        {
            ccColor4B c = new ccColor4B(r, g, b, o);
            return c;
        }

        /** Returns a ccColor4F from a ccColor3B. Alpha will be 1.
         @since v0.99.1
         */
        public static ccColor4F ccc4FFromccc3B(ccColor3B c)
        {
            ccColor4F c4 = new ccColor4F(c.r / 255.0f, c.g / 255.0f, c.b / 255.0f, 1.0f);
            return c4;
        }

        /** Returns a ccColor4F from a ccColor4B.
         @since v0.99.1
         */
        public static ccColor4F ccc4FFromccc4B(ccColor4B c)
        {
            ccColor4F c4 = new ccColor4F(c.r / 255.0f, c.g / 255.0f, c.b / 255.0f, c.a / 255.0f);
            return c4;
        }

        /** returns YES if both ccColor4F are equal. Otherwise it returns NO.
         @since v0.99.1
         */
        public static bool ccc4FEqual(ccColor4F a, ccColor4F b)
        {
            return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
        }

        public static ccVertex2F vertex2(float x, float y)
        {
            ccVertex2F c = new ccVertex2F(x, y);
            return c;
        }

        public static ccVertex3F vertex3(float x, float y, float z)
        {
            ccVertex3F c = new ccVertex3F(x, y, z);
            return c;
        }

        public static ccTex2F tex2(float u, float v)
        {
            ccTex2F t = new ccTex2F(u, v);
            return t;
        }

        //! helper function to create a ccGridSize
        public static ccGridSize ccg(int x, int y)
        {
            ccGridSize v = new ccGridSize(x, y);
            return v;
        }
    }

}//namespace   cocos2d 

