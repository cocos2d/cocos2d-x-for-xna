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

namespace cocos2d
{
    enum NodeTag
    {
        kCCNodeTagInvalid = -1,
    };

    /** @brief CCNode is the main element. Anything thats gets drawn or contains things that get drawn is a CCNode.
	The most popular CCNodes are: CCScene, CCLayer, CCSprite, CCMenu.

	The main features of a CCNode are:
	- They can contain other CCNode nodes (addChild, getChildByTag, removeChild, etc)
	- They can schedule periodic callback (schedule, unschedule, etc)
	- They can execute actions (runAction, stopAction, etc)

	Some CCNode nodes provide extra functionality for them or their children.

	Subclassing a CCNode usually means (one/all) of:
	- overriding init to initialize resources and schedule callbacks
	- create callbacks to handle the advancement of time
	- overriding draw to render the node

	Features of CCNode:
	- position
	- scale (x, y)
	- rotation (in degrees, clockwise)
	- CCCamera (an interface to gluLookAt )
	- CCGridBase (to do mesh transformations)
	- anchor point
	- size
	- visible
	- z-order
	- openGL z position

	Default values:
	- rotation: 0
	- position: (x=0,y=0)
	- scale: (x=1,y=1)
	- contentSize: (x=0,y=0)
	- anchorPoint: (x=0,y=0)

	Limitations:
	- A CCNode is a "void" object. It doesn't have a texture

	Order in transformations with grid disabled
	-# The node will be translated (position)
	-# The node will be rotated (rotation)
	-# The node will be scaled (scale)
	-# The node will be moved according to the camera values (camera)

	Order in transformations with grid enabled
	-# The node will be translated (position)
	-# The node will be rotated (rotation)
	-# The node will be scaled (scale)
	-# The grid will capture the screen
	-# The node will be moved according to the camera values (camera)
	-# The grid will render the captured screen

	Camera:
	- Each node has a camera. By default it points to the center of the CCNode.
	*/
    class CCNode : CCObject
    {
        public CCNode()
        {
        }

        public ~CCNode()
        {
        }


        // Properties

        // The z order of the node relative to it's "brothers": children of the same parent
        private int m_nZOrder;
        public int zOrder
        {
            // read only
            get
            {
                return m_nZOrder;
            }
        }

        /** The real openGL Z vertex.
			Differences between openGL Z vertex and cocos2d Z order:
			- OpenGL Z modifies the Z vertex, and not the Z order in the relation between parent-children
			- OpenGL Z might require to set 2D projection
			- cocos2d Z order works OK if all the nodes uses the same openGL Z vertex. eg: vertexZ = 0
			@warning: Use it at your own risk since it might break the cocos2d parent-children z order
			@since v0.8
			*/
        private float m_fVertexZ;
        public float vertexZ
        {
            get
            {
                return m_fVertexZ;
            }
            set
            {
                m_fVertexZ = value;
            }
        }

        // The rotation (angle) of the node in degrees. 0 is the default rotation angle. Positive values rotate node CW
        private float m_fRotation;
        public float rotation
        {
            get
            {
                return m_fRotation;
            }
            set
            {
                m_fRotation = value;
            }
        }

        /** The X skew angle of the node in degrees.
            This angle describes the shear distortion in the X direction.
            Thus, it is the angle between the Y axis and the left edge of the shape
            The default skewX angle is 0. Positive values distort the node in a CW direction.
        */
        private float m_fSkewX;
        public float skewX
        {
            get
            {
                return m_fSkewX;
            }
            set
            {
                m_fSkewX = value;
            }
        }

        /** The Y skew angle of the node in degrees.
            This angle describes the shear distortion in the Y direction.
            Thus, it is the angle between the X axis and the bottom edge of the shape
            The default skewY angle is 0. Positive values distort the node in a CCW direction.
        */
        private float m_fSkewY;
        public float skewY
        {
            get
            {
                return m_fSkewY;
            }
            set
            {
                m_fSkewY = value;
            }
        }

    }
}
