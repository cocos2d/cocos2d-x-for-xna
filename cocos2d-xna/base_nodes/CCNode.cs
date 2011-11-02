/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
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
using System.Diagnostics;
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
    public class CCNode : CCObject
    {
        public CCNode()
        {
            // Only initialize the members that are not default value.

            m_tPosition = new CCPoint();
            m_tPositionInPixels = new CCPoint();
            m_bIsVisible = true;
            m_tAnchorPoint = new CCPoint();
            m_tAnchorPointInPixels = new CCPoint();
            m_tContentSize = new CCSize();
            m_tContentSizeInPixels = new CCSize();
            m_bIsRelativeAnchorPoint = true;
            m_bIsTransformDirty = true;
            m_bIsInverseDirty = true;
            m_pChildren = new List<CCNode>();

#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
            m_bIsTransformGLDirty = true;
#endif
        }

        ~CCNode()
        {
            //@todo CCLOGINFO( "cocos2d: deallocing" );
            foreach (CCNode node in m_pChildren)
            {
                node.parent = null;
            }
        }

        /** allocates and initializes a node.
		The node will be created as "autorelease".
		*/
        public static CCNode node()
        {
            return new CCNode();
        }

        //scene managment

        /** callback that is called every time the CCNode enters the 'stage'.
		If the CCNode enters the 'stage' with a transition, this callback is called when the transition starts.
		During onEnter you can't a "sister/brother" node.
		*/
        public virtual void onEnter()
        {
            foreach (CCNode node in m_pChildren)
            {
                if (node != null)
                {
                    node.onEnter();
                }              
            }

            resumeSchedulerAndActions();

            m_bIsRunning = true;
        }

        /** callback that is called when the CCNode enters in the 'stage'.
		If the CCNode enters the 'stage' with a transition, this callback is called when the transition finishes.
		@since v0.8
		*/
        public virtual void onEnterTransitionDidFinish()
        {
            foreach (CCNode node in m_pChildren)
            {
                if (node != null)
                {
                    node.onEnterTransitionDidFinish();
                }               
            }
        }

        /** callback that is called every time the CCNode leaves the 'stage'.
		If the CCNode leaves the 'stage' with a transition, this callback is called when the transition finishes.
		During onExit you can't access a sibling node.
		*/
        public virtual void onExit()
        {
            pauseSchedulerAndActions();

            m_bIsRunning = false;

            foreach (CCNode node in m_pChildren)
            {
                if (node != null)
                {
                    node.onExit();
                }                
            }
        }

        // composition: ADD

        /** Adds a child to the container with z-order as 0.
		If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
		@since v0.7.1
		*/
        public virtual void addChild(CCNode child)
        {
            Debug.Assert(child != null, "Argument must be no-null");
            addChild(child, child.zOrder, child.tag);
        }

        /** Adds a child to the container with a z-order
		If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
		@since v0.7.1
		*/
        public virtual void addChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Argument must be no-null");
            addChild(child, zOrder, child.tag);
        }

        /** Adds a child to the container with z order and tag
		If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
		@since v0.7.1
		*/
        public virtual void addChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null, "Argument must be non-null");
            Debug.Assert(child.m_pParent == null, "child already added. It can't be added again");

            insertChild(child, zOrder);

            this.m_nTag = tag;

            child.parent = this;

            if (m_bIsRunning)
            {
                child.onEnter();
                child.onEnterTransitionDidFinish();
            }
        }

        // composition: REMOVE

        /** Remove itself from its parent node. If cleanup is true, then also remove all actions and callbacks.
		If the node orphan, then nothing happens.
		@since v0.99.3
		*/
        public void removeFromParentAndCleanup(bool cleanup)
        {
            m_pParent.removeChild(this, cleanup);
        }

        /** Removes a child from the container. It will also cleanup all running actions depending on the cleanup parameter.
		
         * "remove" logic MUST only be on this method
         * If a class want's to extend the 'removeChild' behavior it only needs
         * to override this method
         * 
         * @since v0.7.1
       */
        public virtual void removeChild(CCNode child, bool cleanup)
        {
            // explicit nil handling
            if (m_pChildren == null)
            {
                return;
            }

            if (m_pChildren.Contains(child))
            {
                detachChild(child, cleanup);
            }
        }

        /** Removes a child from the container by tag value. It will also cleanup all running actions depending on the cleanup parameter
		@since v0.7.1
		*/
        public void removeChildByTag(int tag, bool cleanup)
        {
            Debug.Assert(tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");

            CCNode child = getChildByTag(tag);

            if (child == null)
            {
                Debug.WriteLine("cocos2d: removeChildByTag: child not found!");
            }
            else
            {
                removeChild(child, cleanup);
            }
        }

        /** Removes all children from the container and do a cleanup all running actions depending on the cleanup parameter.
		@since v0.7.1
		*/
        public virtual void removeAllChildrenWithCleanup(bool cleanup)
        {
            // not using detachChild improves speed here

            foreach (CCNode node in m_pChildren)
            {
                if (node != null)
                {
                    // IMPORTANT:
                    //  -1st do onExit
                    //  -2nd cleanup
                    if (m_bIsRunning)
                    {
                        node.onExit();
                    }

                    if (cleanup)
                    {
                        node.cleanup();
                    }

                    // set parent nil at the end
                    node.parent = null;
                }               
            }

            m_pChildren.Clear();
        }

        // composition: GET

        /** Gets a child from the container given its tag
        @return returns a CCNode object
        @since v0.7.1
        */
        public CCNode getChildByTag(int tag)
        {
            Debug.Assert(tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");

            foreach (CCNode node in m_pChildren)
            {
                if (node != null && node.m_nTag == tag)
                {
                    return node;
                }
            }

            return null;
        }

        /** Reorders a child according to a new z value.
		* The child MUST be already added.
		*/
        public virtual void reorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Child must be non-null");

            m_pChildren.Remove(child);

            insertChild(child, zOrder);
        }

        /** Stops all running actions and schedulers
		@since v0.8
		*/
        public virtual void cleanup()
        {
            // actions
            stopAllActions();
            unsheduleAllSelectors();

            // timers
            foreach (CCNode node in m_pChildren)
            {
                if (node != null)
                {
                    node.cleanup();
                }              
            }
        }

        // draw

        /** Override this method to draw your own node.
		The following GL states will be enabled by default:
		- glEnableClientState(GL_VERTEX_ARRAY);
		- glEnableClientState(GL_COLOR_ARRAY);
		- glEnableClientState(GL_TEXTURE_COORD_ARRAY);
		- glEnable(GL_TEXTURE_2D);

		AND YOU SHOULD NOT DISABLE THEM AFTER DRAWING YOUR NODE

		But if you enable any other GL state, you should disable it after drawing your node.
		*/
        public virtual void draw()
        {
            // override me
            // Only use- this function to draw your staff.
            // DON'T draw your stuff outside this method
        }

        // recursive method that visit its children and draw them
        public virtual void visit()
        {
            // quick return if not visible
            if (!m_bIsVisible)
            {
                return;
            }

            ///@todo
            // glPushMatrix();

            ///@todo
            /*
            if (m_pGrid && m_pGrid->isActive())
            {
                m_pGrid->beforeDraw();
                this->transformAncestors();
            }
             */

            transform();

            CCNode node;
            int i = 0;

            if ((m_pChildren != null) && (m_pChildren.Count > 0))
            {
                // draw children zOrder < 0
                for (; i < m_pChildren.Count; ++i)
                {
                    node = m_pChildren[i];
                    if (node != null && node.m_nZOrder < 0)
                    {
                        node.visit();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // self draw
            draw();

            // draw children zOrder >= 0
            if ((m_pChildren != null) && (m_pChildren.Count > 0))
            {
                for (; i < m_pChildren.Count; ++i)
                {
                    node = m_pChildren[i];

                    if (node != null)
                    {
                        node.visit();
                    }                   
                }
            }

            ///@todo
            /*
             * if (m_pGrid && m_pGrid->isActive())
 	           {
 		           m_pGrid->afterDraw(this);
	           }
 
	           glPopMatrix();
             */
        }

        // transformations

        // performs OpenGL view-matrix transformation based on position, scale, rotation and other attributes.
        public void transform()
        {
            ///@todo
            //throw new NotImplementedException();
        }

        /** performs OpenGL view-matrix transformation of it's ancestors.
		Generally the ancestors are already transformed, but in certain cases (eg: attaching a FBO)
		it's necessary to transform the ancestors again.
		@since v0.7.2
		*/
        public void transformAncestors()
        {
            ///@todo
            throw new NotImplementedException();
        }

        /** returns a "local" axis aligned bounding box of the node.
		The returned box is relative only to its parent.

		@since v0.8.2
		*/
        public CCRect boundingBox()
        {
            ///@todo
            throw new NotImplementedException();
        }

        /** returns a "local" axis aligned bounding box of the node in pixels.
		The returned box is relative only to its parent.
		The returned box is in Points.

		@since v0.99.5
		*/
        public CCRect boundingBoxInPixels()
        {
            ///@todo
            throw new NotImplementedException();
        }

        // actions

        ///@todo
        /** Executes an action, and returns the action that is executed.
		The node becomes the action's target.
		@warning Starting from v0.8 actions don't retain their target anymore.
		@since v0.7.1
		@return An Action pointer
		*/

       // CCAction* runAction(CCAction* action);

        // Removes all actions from the running action list
        public void stopAllActions()
        {
            ///@todo
            throw new NotImplementedException();
        }

        /// <summary>
        /// @todo
        /// </summary>
        // Removes an action from the running action list
        /*
        public void stopAction(CCAction action)
        {
        }
         * */

        /** Removes an action from the running action list given its tag
		@since v0.7.1
		*/
        public void stopActionByTag(int tag)
        {
            ///@todo
            throw new NotImplementedException();
        }

        ///@todo

        /** Gets an action from the running action list given its tag
		@since v0.7.1
		@return the Action the with the given tag
		*/
		/*CCAction* getActionByTag(int tag);
         */

        /** Returns the numbers of actions that are running plus the ones that are schedule to run (actions in actionsToAdd and actions arrays). 
		* Composable actions are counted as 1 action. Example:
		*    If you are running 1 Sequence of 7 actions, it will return 1.
		*    If you are running 7 Sequences of 2 actions, it will return 7.
		*/
        public uint numberOfRunningActions()
        {
            ///@todo
            throw new NotImplementedException();
        }

        // timers

        ///@todo
        //check whether a selector is scheduled
        // bool isScheduled(SEL_SCHEDULE selector);

        /** schedules the "update" method. It will use the order number 0. This method will be called every frame.
		Scheduled methods with a lower order value will be called before the ones that have a higher order value.
		Only one "update" method could be scheduled per node.

		@since v0.99.3
		*/
        public void sheduleUpdate()
        {
            ///@todo
            throw new NotImplementedException();
        }

        /** schedules the "update" selector with a custom priority. This selector will be called every frame.
		Scheduled selectors with a lower priority will be called before the ones that have a higher value.
		Only one "update" selector could be scheduled per node (You can't have 2 'update' selectors).

		@since v0.99.3
		*/
        public void scheduleUpdateWithPriority(int priority)
        {
            ///@todo
            throw new NotImplementedException();
        }

        /* unschedules the "update" method.

		@since v0.99.3
		*/
        public void unscheduleUpdate()
        {
            ///@todo
            throw new NotImplementedException();
        }

        ///@todo
        /** schedules a selector.
		The scheduled selector will be ticked every frame
		*/
        // void schedule(SEL_SCHEDULE selector);

        ///@todo
        /** schedules a custom selector with an interval time in seconds.
		If time is 0 it will be ticked every frame.
		If time is 0, it is recommended to use 'scheduleUpdate' instead.
		If the selector is already scheduled, then the interval parameter 
		will be updated without scheduling it again.
		*/
        // void schedule(SEL_SCHEDULE selector, ccTime interval);

        ///@todo
        /** unschedules a custom selector.*/
        // void unschedule(SEL_SCHEDULE selector);

        /** unschedule all scheduled selectors: custom selectors, and the 'update' selector.
		Actions are not affected by this method.
		@since v0.99.3
		*/
        public void unsheduleAllSelectors()
        {
            ///@todo
            throw new NotImplementedException();
        }

        /** resumes all scheduled selectors and actions.
		Called internally by onEnter
		*/
        public void resumeSchedulerAndActions()
        {
            ///@todo
            //throw new NotImplementedException();
        }

        /** pauses all scheduled selectors and actions.
		Called internally by onExit
		*/
        public void pauseSchedulerAndActions()
        {
            ///@todo
            throw new NotImplementedException();
        }

        /*
         * These functions are not needed. They are designed for no RTTI.
         * 
        virtual void selectorProtocolRetain(void);
		virtual void selectorProtocolRelease(void);

		virtual CCRGBAProtocol* convertToRGBAProtocol(void) { return NULL; }
		virtual CCLabelProtocol* convertToLabelProtocol(void) { return NULL; }
         */

        // transformation methods
        ///@todo

        /** Returns the matrix that transform the node's (local) space coordinates into the parent's space coordinates.
        The matrix is in Pixels.
        @since v0.7.1
        */
        // CCAffineTransform nodeToParentTransform(void);

        /** Returns the matrix that transform parent's space coordinates to the node's (local) space coordinates.
		The matrix is in Pixels.
		@since v0.7.1
		*/
        // 
        // CCAffineTransform parentToNodeTransform(void);

        /** Retrusn the world affine transform matrix. The matrix is in Pixels.
		@since v0.7.1
		*/
        // CCAffineTransform nodeToWorldTransform(void);

        /** Returns the inverse world affine transform matrix. The matrix is in Pixels.
		@since v0.7.1
		*/
        // CCAffineTransform worldToNodeTransform(void);

        /** Converts a Point to node (local) space coordinates. The result is in Points.
		@since v0.7.1
		*/
        public CCPoint convertToNodeSpace(CCPoint worldPoint)
        {
            ///@todo
            throw new NotImplementedException();
        }

        /** Converts a Point to world space coordinates. The result is in Points.
		@since v0.7.1
		*/
        public CCPoint convertToWorldSpace(CCPoint nodePoint)
        {
            ///@todo
            throw new NotImplementedException();
        }

        /** Converts a Point to node (local) space coordinates. The result is in Points.
		treating the returned/received node point as anchor relative.
		@since v0.7.1
		*/
        public CCPoint convertToNodeSpaceAR(CCPoint worldPoint)
        {
            ///@todo
            throw new NotImplementedException();
        }

        /** Converts a local Point to world space coordinates.The result is in Points.
		treating the returned/received node point as anchor relative.
		@since v0.7.1
		*/
        public CCPoint convertToWorldSpaceAR(CCPoint nodePoint)
        {
            ///@todo
            throw new NotImplementedException();
        }

        ///@todo
        /** convenience methods which take a CCTouch instead of CCPoint
		@since v0.7.1
		*/
        // public CCPoint convertTouchToNodeSpace(CCTouch touch);

        ///@todo
        /** converts a CCTouch (world coordinates) into a local coordiante. This method is AR (Anchor Relative).
		@since v0.7.1
		*/
        // CCPoint convertTouchToNodeSpaceAR(CCTouch* touch);

        // helper that reorder a child
        private void insertChild(CCNode child, int z)
        {            
            // Get last member
            CCNode a = m_pChildren.Count > 0 ? m_pChildren[m_pChildren.Count - 1] : null;

            if (a == null || a.zOrder <= z)
            {
                m_pChildren.Add(child);
            }
            else
            {
                int index = 0;
                foreach (CCNode node in m_pChildren)
                {
                    if (node != null && node.m_nZOrder > z)
                    {
                        m_pChildren.Insert(index, child);
                        break;
                    }

                    ++index;
                }
            }

            child.setZOrder(z);
        }

        // used internally to alter the zOrder variable. DON'T call this method manually
        private void setZOrder(int z)
        {
            m_nZOrder = z;
        }

        private void detachChild(CCNode child, bool doCleanup)
        {
            // IMPORTANT:
            //  -1st do onExit
            //  -2nd cleanup
            if (m_bIsRunning)
            {
                child.onExit();
            }

            // If you don't do cleanup, the child's actions will not get removed and the
            // its scheduledSelectors_ dict will not get released!
            if (doCleanup)
            {
                child.cleanup();
            }

            // set parent nil at the end
            child.parent = null;

            m_pChildren.Remove(child);
        }

        private CCPoint convertToWindowSpace(CCPoint nodePoint)
        {
            ///@todo
            throw new NotImplementedException();
        }

        // Properties

        // The z order of the node relative to it's "brothers": children of the same parent
        protected int m_nZOrder;
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
        protected float m_fVertexZ;
        public virtual float vertexZ
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                /// @todo
                throw new NotImplementedException();
            }
        }

        // The rotation (angle) of the node in degrees. 0 is the default rotation angle. Positive values rotate node CW
        protected float m_fRotation;
        public virtual float rotation
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
                /// @todo
            }
        }

        /** The X skew angle of the node in degrees.
            This angle describes the shear distortion in the X direction.
            Thus, it is the angle between the Y axis and the left edge of the shape
            The default skewX angle is 0. Positive values distort the node in a CW direction.
        */
        protected float m_fSkewX;
        public virtual float skewX
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                /// @todo
                throw new NotImplementedException();
            }
        }

        /** The Y skew angle of the node in degrees.
            This angle describes the shear distortion in the Y direction.
            Thus, it is the angle between the X axis and the bottom edge of the shape
            The default skewY angle is 0. Positive values distort the node in a CCW direction.
        */
        protected float m_fSkewY;
        public virtual float skewY
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                ///@todo
                throw new NotImplementedException();
            }
        }

        // The scale factor of the node. 1.0 is the default scale factor. It modifies the X and Y scale at the same time.
        protected float m_fScale;
        public virtual float scale
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                ///@todo
                throw new NotImplementedException();
            }
        }

        // The scale factor of the node. 1.0 is the default scale factor. It only modifies the X scale factor.
        protected float m_fScaleX;
        public virtual float scaleX
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                ///@todo
                throw new NotImplementedException();
            }
        }

        // The scale factor of the node. 1.0 is the default scale factor. It only modifies the Y scale factor.
        protected float m_fScaleY;
        public virtual float scaleY
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                ///@todo
                throw new NotImplementedException();
            }
        }

        // Position (x,y) of the node in points. (0,0) is the left-bottom corner.
        protected CCPoint m_tPosition;
        public virtual CCPoint position
        {
            get
            {
                return m_tPosition;
            }
            set
            {
                if (ccMacros.CC_CONTENT_SCALE_FACTOR() == 1)
                {
                    m_tPositionInPixels = m_tPosition;
                }
                else
                {
                    m_tPositionInPixels = CCPointExtension.ccpMult(value, ccMacros.CC_CONTENT_SCALE_FACTOR());
                }

                m_bIsTransformDirty = m_bIsInverseDirty = true;


#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
	            m_bIsTransformGLDirty = true;
#endif

            }
        }

        // Position (x,y) of the node in pixels. (0,0) is the left-bottom corner.
        protected CCPoint m_tPositionInPixels;
        public virtual CCPoint positionInPixels
        {
            get
            {
                return m_tPositionInPixels;
            }
            set
            {
                m_tPositionInPixels = value;

                if (ccMacros.CC_CONTENT_SCALE_FACTOR() == 1)
                {
                    m_tPosition = m_tPositionInPixels;
                }
                else
                {
                    m_tPosition = CCPointExtension.ccpMult(value, 1 / ccMacros.CC_CONTENT_SCALE_FACTOR());
                }

                m_bIsTransformDirty = m_bIsInverseDirty = true;

#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX  
                m_bIsTransformGLDirty = true;
#endif // CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
            }
        }

        ///@todo add CCCamera
        /** A CCCamera object that lets you move the node using a gluLookAt
           
       @property(nonatomic,readonly) CCCamera* camera;
         */

        // Array of children
        protected List<CCNode> m_pChildren;
        public List<CCNode> children
        {
            // read only
            get
            {
                return m_pChildren;
            }
        }

        ///@todo
        /** A CCGrid object that is used when applying effects */
        /// @property(nonatomic,readwrite,retain) CCGridBase* grid;

        // Whether of not the node is visible. Default is YES
        protected bool m_bIsVisible;
        public virtual bool visible
        {
            get
            {
                return m_bIsVisible;
            }
            set
            {
                m_bIsVisible = value;
            }
        }

        /** anchorPoint is the point around which all transformations and positioning manipulations take place.
            It's like a pin in the node where it is "attached" to its parent.
            The anchorPoint is normalized, like a percentage. (0,0) means the bottom-left corner and (1,1) means the top-right corner.
            But you can use values higher than (1,1) and lower than (0,0) too.
            The default anchorPoint is (0,0). It starts in the bottom-left corner. CCSprite and other subclasses have a different default anchorPoint.
            @since v0.8
        */
        protected CCPoint m_tAnchorPoint;
        public virtual CCPoint anchorPoint
        {
            get
            {
                return m_tAnchorPoint;
            }
            set
            {
                if (! CCPoint.CCPointEqualToPoint(value, m_tAnchorPoint))
                {
                    m_tAnchorPoint = value;
                    m_tAnchorPointInPixels = CCPointExtension.ccp(m_tContentSizeInPixels.width * m_tAnchorPoint.x, 
                        m_tContentSizeInPixels.height * m_tAnchorPoint.y);
                    m_bIsTransformDirty = m_bIsInverseDirty = true;
#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
		            m_bIsTransformGLDirty = true;
#endif
                    
                }
            }
        }

        /** The anchorPoint in absolute pixels.
            Since v0.8 you can only read it. If you wish to modify it, use anchorPoint instead
        */
        protected CCPoint m_tAnchorPointInPixels;
        public CCPoint anchorPointInPixels
        {
            // read only
            get
            {
                return m_tAnchorPointInPixels;
            }
        }

        /** The untransformed size of the node in Points
            The contentSize remains the same no matter the node is scaled or rotated.
            All nodes has a size. Layer and Scene has the same size of the screen.
            @since v0.8
        */
        protected CCSize m_tContentSize;
        public CCSize contentSize
        {
            get
            {
                return m_tContentSize;
            }
            set
            {
                if (! CCSize.CCSizeEqualToSize(value, m_tContentSize))
                {
                    m_tContentSize = value;

                    if (ccMacros.CC_CONTENT_SCALE_FACTOR() == 1)
                    {
                        m_tContentSizeInPixels = m_tContentSize;
                    }
                    else
                    {
                        m_tContentSizeInPixels = new CCSize(value.width * ccMacros.CC_CONTENT_SCALE_FACTOR(),
                            value.height * ccMacros.CC_CONTENT_SCALE_FACTOR());
                    }

                    m_tAnchorPointInPixels = CCPointExtension.ccp(m_tContentSizeInPixels.width * m_tAnchorPoint.x,
                        m_tContentSizeInPixels.height * m_tAnchorPoint.y);
                    m_bIsTransformDirty = m_bIsInverseDirty = true;
#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                    m_bIsTransformGLDirty = true;
#endif
                }
            }
        }

        /** The untransformed size of the node in Pixels
            The contentSize remains the same no matter the node is scaled or rotated.
            All nodes has a size. Layer and Scene has the same size of the screen.
            @since v0.8
        */
        protected CCSize m_tContentSizeInPixels;
        public CCSize contentSizeInPixels
        {
            get
            {
                return m_tContentSizeInPixels;
            }
            set
            {
                if (! CCSize.CCSizeEqualToSize(value, m_tContentSizeInPixels))
                {
                    m_tContentSizeInPixels = value;

                    if (ccMacros.CC_CONTENT_SCALE_FACTOR() == 1)
                    {
                        m_tContentSize = m_tContentSizeInPixels;
                    }
                    else
                    {
                        m_tContentSize = new CCSize(value.width / ccMacros.CC_CONTENT_SCALE_FACTOR(),
                            value.height / ccMacros.CC_CONTENT_SCALE_FACTOR());
                    }

                    m_tAnchorPointInPixels = CCPointExtension.ccp(m_tContentSizeInPixels.width * m_tAnchorPoint.x,
                        m_tContentSizeInPixels.height * m_tAnchorPoint.y);
                    m_bIsTransformDirty = m_bIsInverseDirty = true;

#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                    m_bIsTransformGLDirty = true;
#endif // CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                }
            }
        }

        // whether or not the node is running
        protected bool m_bIsRunning;
        public bool isRunning
        {
            // read only
            get
            {
                return m_bIsRunning;
            }
        }

        // A weak reference to the parent
        protected CCNode m_pParent;
        public CCNode parent
        {
            get
            {
                return m_pParent;
            }
            set
            {
                m_pParent = value;
            }
        }

        /** If YES the transformtions will be relative to it's anchor point.
          * Sprites, Labels and any other sizeble object use it have it enabled by default.
          * Scenes, Layers and other "whole screen" object don't use it, have it disabled by default.
          */
        protected bool m_bIsRelativeAnchorPoint;
        public virtual bool isRelativeAnchorPoint
        {
            get
            {
                return m_bIsRelativeAnchorPoint;
            }
            set
            {
                m_bIsRelativeAnchorPoint = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
	            m_bIsTransformGLDirty = true;
#endif
            }
        }

        // A tag used to identify the node easily
        protected int m_nTag;
        public int tag
        {
            get
            {
                return m_nTag;
            }
            set
            {
                m_nTag = value;
            }
        }

        // A custom user data pointer
        protected object m_pUserData;
        public object userData
        {
            get
            {
                return m_pUserData;
            }
            set
            {
                m_pUserData = value;
            }
        }

        // internal member variables


        // To reduce memory, place bools that are not properties here:
        protected bool m_bIsTransformDirty;
        protected bool m_bIsInverseDirty;


        /*
         * CCAffineTransform m_tTransform, m_tInverse;
         */
#if	CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
        // @todo
        // float[]	transformGL;
#endif
          
#if	CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
        protected bool m_bIsTransformGLDirty;
#endif
        
    }
}
