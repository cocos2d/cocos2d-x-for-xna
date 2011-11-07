/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada

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

namespace cocos2d
{
    public interface CCRGBAProtocol
    {
        /** sets Color
         @since v0.8
         */
        /** returns the color
	     @since v0.8
	     */
        ccColor3B Color { get; set; }

        // returns the opacity
        /** sets the opacity.
	     @warning If the the texture has premultiplied alpha then, the R, G and B channels will be modifed.
	     Values goes from 0 to 255, where 255 means fully opaque.
	     */
        byte Opacity { get; set; }

        /** sets the premultipliedAlphaOpacity property.
	     If set to NO then opacity will be applied as: glColor(R,G,B,opacity);
	     If set to YES then oapcity will be applied as: glColor(opacity, opacity, opacity, opacity );
	     Textures with premultiplied alpha will have this property by default on YES. Otherwise the default value is NO
	     @since v0.8
	     */

        bool IsOpacityModifyRGB { get; set; }

        /** returns whether or not the opacity will be applied using glColor(R,G,B,opacity) or glColor(opacity, opacity, opacity, opacity);
	     @since v0.8
	     */
    }

    /**
     @brief You can specify the blending fuction.
     @since v0.99.0
     */
    public interface CCBlendProtocol
    {
        // set the source blending function for the texture
        void setBlendFunc(ccBlendFunc blendFunc);

        // returns the blending function used for the texture
        ccBlendFunc getBlendFunc();
    }

    /** @brief CCNode objects that uses a Texture2D to render the images.
     The texture can have a blending function.
     If the texture has alpha premultiplied the default blending function is:
        src=GL_ONE dst= GL_ONE_MINUS_SRC_ALPHA
     else
	    src=GL_SRC_ALPHA dst= GL_ONE_MINUS_SRC_ALPHA
     But you can change the blending funtion at any time.
     @since v0.8.0
     */
    public interface CCTextureProtocol : CCBlendProtocol
    {
        // returns the used texture
        CCTexture2D getTexture();

        // sets a new texture. it will be retained
        void setTexture(CCTexture2D texture);
    }

    public interface CCLabelProtocol
    {
        // sets a new label using an string
        void setString(string label);

        // returns the string that is rendered
        string getString();
    }

    /** OpenGL projection protocol */
    public interface CCProjectionProtocol
    {
        /** Called by CCDirector when the porjection is updated, and "custom" projection is used
	    @since v0.99.5
	    */
        void updateProjection();
    }
}
