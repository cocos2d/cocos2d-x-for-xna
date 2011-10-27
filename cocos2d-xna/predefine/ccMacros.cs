/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org

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
namespace cocos2d
{
    class ccMacros
    {
        // how to implement it by c#
        /** @def CC_SWAP
            simple macro that swaps 2 variables
          */
        //#define CC_SWAP(x, y, type)	\
        //{	type temp = (x);		\
	    //x = y; y = temp;		\
        //}

        /** @def CCRANDOM_MINUS1_1
            returns a random float between -1 and 1
          */
        private static Random rand = new Random();
        public static float CCRANDOM_MINUS1_1()
        {
            return (2.0f * ((float)rand.Next() / int.MaxValue)) - 1.0f;
        }

        /** @def CCRANDOM_0_1
            returns a random float between 0 and 1
         */
        public static float CCRANDOM_0_1()
        {
            return (float)rand.Next() / int.MaxValue;
        }

        /** @def CC_DEGREES_TO_RADIANS
            converts degrees to radians
        */
        public static float CC_DEGREES_TO_RADIANS(float angle)
        {
            return angle * 0.01745329252f; // PI / 180
        }

        /** @def CC_RADIANS_TO_DEGREES
            converts radians to degrees
        */
        public static float CC_RADIANS_TO_DEGREES(float angle)
        {
            return angle * 57.29577951f; // PI * 180
        }

        public static readonly double FLT_EPSILON = 1.192092896e-07F;

        // Retina is only supported on iOS.
#if CC_IS_RETINA_DISPLAY_SUPPORTED
#else
        public static int CC_CONTENT_SCALE_FACTOR()
        {
            return 1;
        }

        public static CCRect CC_RECT_PIXELS_TO_POINTS(CCRect pixels)
        {
            return pixels;
        }

        public static CCRect CC_RECT_POINTS_TO_PIXELS(CCRect points)
        {
            return points;
        }
#endif // CC_IS_RETINA_DISPLAY_SUPPORTED

        public static bool CC_HOST_IS_BIG_ENDIAN()
        {
            return ! BitConverter.IsLittleEndian;
        }

        // Only unsigned int can use these functions.

        public static uint CC_SWAP32(uint i)
        {
            return ((i & 0x000000ff) << 24 | (i & 0x0000ff00) << 8 | (i & 0x00ff0000) >> 8 | (i & 0xff000000) >> 24);
        }

        public static ushort CC_SWAP16(ushort i)
        {
            return (ushort)((i & 0x00ff) << 8 | (i & 0xff00) >> 8);
        }

        public static uint CC_SWAP_INT32_LITTLE_TO_HOST(uint i)
        {
            return ((CC_HOST_IS_BIG_ENDIAN() == true) ? CC_SWAP32(i) : (i));
        }

        public static ushort CC_SWAP_INT16_LITTLE_TO_HOST(ushort i)
        {
            return ((CC_HOST_IS_BIG_ENDIAN() == true) ? CC_SWAP16(i) : (i));
        }

        public static uint CC_SWAP_INT32_BIG_TO_HOST(uint i)
        {
            return ((CC_HOST_IS_BIG_ENDIAN() == true) ? (i) : CC_SWAP32(i));
        }

        public static ushort CC_SWAP_INT16_BIG_TO_HOST(ushort i)
        {
            return ((CC_HOST_IS_BIG_ENDIAN() == true) ? (i) : CC_SWAP16(i));
        }

        /*
         * macros of CCGeometry.h
         */
        public static CCPoint CCPointMake(float x, float y)
        {
            return new CCPoint(x, y);
        }
        public static CCSize CCSizeMake(float width, float height)
        {
            return new CCSize(width, height);
        }
        public static CCRect CCRectMake(float x, float y, float width, float height)
        {
            return new CCRect(x, y, width, height);
        }


        // The following macros are defined for opengl es, they are not needed.

        // #if CC_OPTIMIZE_BLEND_FUNC_FOR_PREMULTIPLIED_ALPHA
        // #define CC_BLEND_SRC GL_ONE
        // #define CC_BLEND_DST GL_ONE_MINUS_SRC_ALPHA
        // #else
        // #define CC_BLEND_SRC GL_SRC_ALPHA
        // #define CC_BLEND_DST GL_ONE_MINUS_SRC_ALPHA
        // #endif

        // #define CC_BLEND_DST GL_ONE_MINUS_SRC_ALPHA

        // #define CC_ENABLE_DEFAULT_GL_STATES() {				\
	    // glEnableClientState(GL_VERTEX_ARRAY);			\
	    // glEnableClientState(GL_COLOR_ARRAY);			\
	    // glEnableClientState(GL_TEXTURE_COORD_ARRAY);	\
	    // glEnable(GL_TEXTURE_2D);			
        
        // #define CC_DISABLE_DEFAULT_GL_STATES() {			\
	    // glDisable(GL_TEXTURE_2D);						\
	    // glDisableClientState(GL_COLOR_ARRAY);			\
	    // glDisableClientState(GL_TEXTURE_COORD_ARRAY);	\
	    // glDisableClientState(GL_VERTEX_ARRAY);			\
    }
}
