/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.

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

//#include "CCGeometry.h"
//#include "CCGL.h"

namespace cocos2d
{

    /** RGB color composed of bytes 3 bytes
    @since v0.8
     */
    public struct ccColor3B
    {
        public ccColor3B(byte inr, byte ing, byte inb)
        {
            r = inr;
            g = ing;
            b = inb;
        }

        // Convert Color value of XNA Framework to ccColor3B type
        public ccColor3B(Microsoft.Xna.Framework.Color color)
        {
            r = color.R;
            g = color.B;
            b = color.B;
        }

        public byte r;
        public byte g;
        public byte b;
    };

    /** RGBA color composed of 4 bytes
    @since v0.8
    */
    public struct ccColor4B
    {
        public ccColor4B(byte inr, byte ing, byte inb, byte ina)
        {
            r = inr;
            g = ing;
            b = inb;
            a = ina;
        }

        // Convert Color value of XNA Framework to ccColor4B type
        public ccColor4B(Microsoft.Xna.Framework.Color color)
        {
            r = color.R;
            g = color.B;
            b = color.B;
            a = color.A;
        }

        public byte r;
        public byte g;
        public byte b;
        public byte a;
    };

    /** RGBA color composed of 4 floats
    @since v0.8
    */
    public struct ccColor4F
    {
        public ccColor4F(float inr, float ing, float inb, float ina)
        {
            r = inr;
            g = ing;
            b = inb;
            a = ina;
        }

        public float r;
        public float g;
        public float b;
        public float a;
    };

    /** A vertex composed of 2 floats: x, y
     @since v0.8
     */
    public struct ccVertex2F
    {
        public ccVertex2F(float inx, float iny)
        {
            x = inx;
            y = iny;
        }

        public float x;
        public float y;
    };

    /** A vertex composed of 2 floats: x, y
     @since v0.8
     */
    public struct ccVertex3F
    {
        public ccVertex3F(float inx, float iny, float inz)
        {
            x = inx;
            y = iny;
            z = inz;
        }

        public float x;
        public float y;
        public float z;
    };
    /** A texcoord composed of 2 floats: u, y
    @since v0.8
    */
    public struct ccTex2F
    {
        public ccTex2F(float inu, float inv)
        {
            u = inu;
            v = inv;
        }

        public float u;
        public float v;
    };

    //! Point Sprite component
    public struct ccPointSprite
    {
        public ccVertex2F pos;		// 8 bytes
        public ccColor4B color;		// 4 bytes
        public float size;		// 4 bytes
    };

    //!	A 2D Quad. 4 * 2 floats
    public struct ccQuad2
    {
        public ccVertex2F tl;
        public ccVertex2F tr;
        public ccVertex2F bl;
        public ccVertex2F br;
    };


    //!	A 3D Quad. 4 * 3 floats
    public struct ccQuad3
    {
        public ccVertex3F bl;
        public ccVertex3F br;
        public ccVertex3F tl;
        public ccVertex3F tr;
    };

    //! A 2D grid size
    public struct ccGridSize
    {
        public ccGridSize(int inx, int iny)
        {
            x = inx;
            y = iny;
        }

        public int x;
        public int y;
    };

    //! a Point with a vertex point, a tex coord point and a color 4B
    public struct ccV2F_C4B_T2F
    {
        //! vertices (2F)
        public ccVertex2F vertices;
        //! colors (4B)
        public ccColor4B colors;
        //! tex coords (2F)
        public ccTex2F texCoords;
    };

    //! a Point with a vertex point, a tex coord point and a color 4F
    public struct ccV2F_C4F_T2F
    {
        //! vertices (2F)
        public ccVertex2F vertices;
        //! colors (4F)
        public ccColor4F colors;
        //! tex coords (2F)
        public ccTex2F texCoords;
    };

    //! a Point with a vertex point, a tex coord point and a color 4B
    public struct ccV3F_C4B_T2F
    {
        //! vertices (3F)
        public ccVertex3F vertices;			// 12 bytes
        //	char __padding__[4];

        //! colors (4B)
        public ccColor4B colors;				// 4 bytes
        //	char __padding2__[4];

        // tex coords (2F)
        public ccTex2F texCoords;			// 8 byts
    } ;

    //! 4 ccVertex2FTex2FColor4B Quad
    public struct ccV2F_C4B_T2F_Quad
    {
        //! bottom left
        public ccV2F_C4B_T2F bl;
        //! bottom right
        public ccV2F_C4B_T2F br;
        //! top left
        public ccV2F_C4B_T2F tl;
        //! top right
        public ccV2F_C4B_T2F tr;
    } ;

    //! 4 ccVertex3FTex2FColor4B
    public struct ccV3F_C4B_T2F_Quad
    {
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
    } ;

    //! 4 ccVertex2FTex2FColor4F Quad
    public struct ccV2F_C4F_T2F_Quad
    {
        //! bottom left
        public ccV2F_C4F_T2F bl;
        //! bottom right
        public ccV2F_C4F_T2F br;
        //! top left
        public ccV2F_C4F_T2F tl;
        //! top right
        public ccV2F_C4F_T2F tr;
    } ;

    //! Blend Function used for textures
    public struct ccBlendFunc
    {
        //! source blend function
        public uint src;
        //! destination blend function
        public uint dst;
    };

    //! delta time type
    //! if you want more resolution redefine it as a double
    // #define ccTime float
    //typedef double ccTime;

    public enum CCTextAlignment
    {
        CCTextAlignmentLeft,
        CCTextAlignmentCenter,
        CCTextAlignmentRight,
    };

    public class ccTypes
    {
        //ccColor3B predefined colors
        //! White color (255,255,255)
        public readonly ccColor3B ccWHITE = new ccColor3B(255, 255, 255);
        //! Yellow color (255,255,0)
        public readonly ccColor3B ccYELLOW = new ccColor3B(255, 255, 0);
        //! Blue color (0,0,255)
        public readonly ccColor3B ccBLUE = new ccColor3B(0, 0, 255);
        //! Green Color (0,255,0)
        public readonly ccColor3B ccGREEN = new ccColor3B(0, 255, 0);
        //! Red Color (255,0,0,)
        public readonly ccColor3B ccRED = new ccColor3B(255, 0, 0);
        //! Magenta Color (255,0,255)
        public readonly ccColor3B ccMAGENTA = new ccColor3B(255, 0, 255);
        //! Black Color (0,0,0)
        public readonly ccColor3B ccBLACK = new ccColor3B(0, 0, 0);
        //! Orange Color (255,127,0)
        public readonly ccColor3B ccORANGE = new ccColor3B(255, 127, 0);
        //! Gray Color (166,166,166)
        public readonly ccColor3B ccGRAY = new ccColor3B(166, 166, 166);

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

