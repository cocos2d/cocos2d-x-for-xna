/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011      Fulcrum Mobile Network, Inc.

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
    public class CCNode : CCObject, SelectorProtocol
    {
        public CCNode()
        {
            // Only initialize the members that are not default value.

            m_fScaleX = 1.0f;
            m_fScaleY = 1.0f;
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

        /// <summary>
        /// allocates and initializes a node.
        /// The node will be created as "autorelease".
        /// </summary>
        public static CCNode node()
        {
            return new CCNode();
        }

        #region scene managment

        /// <summary>
        /// callback that is called every time the CCNode enters the 'stage'.
        /// If the CCNode enters the 'stage' with a transition, this callback is called when the transition starts.
        /// During onEnter you can't a "sister/brother" node.
        /// </summary>
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

        /// <summary>
        /// callback that is called when the CCNode enters in the 'stage'.
        /// If the CCNode enters the 'stage' with a transition, this callback is called when the transition finishes.
        /// @since v0.8
        /// </summary>
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

        /// <summary>
        ///  callback that is called every time the CCNode leaves the 'stage'.
        ///  If the CCNode leaves the 'stage' with a transition, this callback is called when the transition finishes.
        ///  During onExit you can't access a sibling node.
        /// </summary>
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

        #endregion

        #region composition: children

        protected List<CCNode> m_pChildren;
        /// <summary>
        /// Array of childrens
        /// </summary>
        public List<CCNode> children
        {
            // read only
            get
            {
                return m_pChildren;
            }
        }

        /// <summary>
        /// Adds a child to the container with z-order as 0.
        /// If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
        /// @since v0.7.1
        /// </summary>
        /// <param name="child"></param>
        public virtual void addChild(CCNode child)
        {
            Debug.Assert(child != null, "Argument must be no-null");
            addChild(child, child.zOrder, child.tag);
        }

        /// <summary>
        /// Adds a child to the container with a z-order
        /// If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
        /// @since v0.7.1
        /// </summary>
        public virtual void addChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Argument must be no-null");
            addChild(child, zOrder, child.tag);
        }

        /// <summary>
        /// Adds a child to the container with z order and tag
        /// If the child is added to a 'running' node, then 'onEnter' and 'onEnterTransitionDidFinish' will be called immediately.
        /// @since v0.7.1
        /// </summary>
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

        /// <summary>
        /// helper that reorder a child
        /// </summary>
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

            child.zOrder = z;
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

        #endregion

        #region composition: REMOVE

        /// <summary>
        /// Remove itself from its parent node. If cleanup is true, then also remove all actions and callbacks.
        /// If the node orphan, then nothing happens.
        /// @since v0.99.3
        /// </summary>
        public void removeFromParentAndCleanup(bool cleanup)
        {
            m_pParent.removeChild(this, cleanup);
        }

        /// <summary>
        /// Removes a child from the container. It will also cleanup all running actions depending on the cleanup parameter.
        /// "remove" logic MUST only be on this method
        /// If a class want's to extend the 'removeChild' behavior it only needs
        /// to override this method
        /// @since v0.7.1
        /// </summary>
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

        /// <summary>
        /// Removes a child from the container by tag value. It will also cleanup all running actions depending on the cleanup parameter
        /// @since v0.7.1
        /// </summary>
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

        /// <summary>
        /// Removes all children from the container and do a cleanup all running actions depending on the cleanup parameter.
        /// @since v0.7.1
        /// </summary>
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

        #endregion

        #region composition: GET

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

        #endregion

        /// <summary>
        /// Override this method to draw your own node.
        ///	The following GL states will be enabled by default:
        ///- glEnableClientState(GL_VERTEX_ARRAY);
        ///- glEnableClientState(GL_COLOR_ARRAY);
        ///	- glEnableClientState(GL_TEXTURE_COORD_ARRAY);
        ///	- glEnable(GL_TEXTURE_2D);
        ///AND YOU SHOULD NOT DISABLE THEM AFTER DRAWING YOUR NODE
        ///But if you enable any other GL state, you should disable it after drawing your node.
        /// </summary>
        public virtual void draw()
        {
            // override me
            // Only use- this function to draw your staff.
            // DON'T draw your stuff outside this method
        }

        /// <summary>
        /// recursive method that visit its children and draw them
        /// </summary>
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

        /// <summary>
        /// The update function
        /// </summary>
        public virtual void update(float dt)
        {

        }

        #region transformations

        /// <summary>
        /// performs OpenGL view-matrix transformation based on position, scale, rotation and other attributes.
        /// </summary>
        public void transform()
        {
            // transformations

#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
            // BEGIN alternative -- using cached transform
            //
            if (m_bIsTransformGLDirty)
            {
                //CCAffineTransform t = this.nodeToParentTransform();
                //TransformUtils.CGAffineToGL(t, m_pTransformGL);
                //m_bIsTransformGLDirty = false;
            }

            //glMultMatrixf(m_pTransformGL);
            //if (m_fVertexZ)
            //{
            //    glTranslatef(0, 0, m_fVertexZ);
            //}

            // XXX: Expensive calls. Camera should be integrated into the cached affine matrix
            //if (m_pCamera && !(m_pGrid && m_pGrid->isActive()))
            //{
            //    bool translate = (m_tAnchorPointInPixels.x != 0.0f || m_tAnchorPointInPixels.y != 0.0f);

            //    if (translate)
            //    {
            //        ccglTranslate(RENDER_IN_SUBPIXEL(m_tAnchorPointInPixels.x), RENDER_IN_SUBPIXEL(m_tAnchorPointInPixels.y), 0);
            //    }

            //    m_pCamera->locate();

            //    if (translate)
            //    {
            //        ccglTranslate(RENDER_IN_SUBPIXEL(-m_tAnchorPointInPixels.x), RENDER_IN_SUBPIXEL(-m_tAnchorPointInPixels.y), 0);
            //    }
            //}


            // END alternative

#else
                // BEGIN original implementation
                // 
                // translate
                if ( m_bIsRelativeAnchorPoint && (m_tAnchorPointInPixels.x != 0 || m_tAnchorPointInPixels.y != 0 ) )
                    glTranslatef( RENDER_IN_SUBPIXEL(-m_tAnchorPointInPixels.x), RENDER_IN_SUBPIXEL(-m_tAnchorPointInPixels.y), 0);

                if (m_tAnchorPointInPixels.x != 0 || m_tAnchorPointInPixels.y != 0)
                    glTranslatef( RENDER_IN_SUBPIXEL(m_tPositionInPixels.x + m_tAnchorPointInPixels.x), RENDER_IN_SUBPIXEL(m_tPositionInPixels.y + m_tAnchorPointInPixels.y), m_fVertexZ);
                else if ( m_tPositionInPixels.x !=0 || m_tPositionInPixels.y !=0 || m_fVertexZ != 0)
                    glTranslatef( RENDER_IN_SUBPIXEL(m_tPositionInPixels.x), RENDER_IN_SUBPIXEL(m_tPositionInPixels.y), m_fVertexZ );

                // rotate
                if (m_fRotation != 0.0f )
                    glRotatef( -m_fRotation, 0.0f, 0.0f, 1.0f );

                // skew
                if ( (skewX_ != 0.0f) || (skewY_ != 0.0f) ) 
                {
                    CCAffineTransform skewMatrix = CCAffineTransformMake( 1.0f, tanf(CC_DEGREES_TO_RADIANS(skewY_)), tanf(CC_DEGREES_TO_RADIANS(skewX_)), 1.0f, 0.0f, 0.0f );
                    GLfloat	glMatrix[16];
                    CCAffineToGL(&skewMatrix, glMatrix);															 
                    glMultMatrixf(glMatrix);
                }

                // scale
                if (m_fScaleX != 1.0f || m_fScaleY != 1.0f)
                    glScalef( m_fScaleX, m_fScaleY, 1.0f );

                if ( m_pCamera  && !(m_pGrid && m_pGrid->isActive()) )
                    m_pCamera->locate();

                // restore and re-position point
                if (m_tAnchorPointInPixels.x != 0.0f || m_tAnchorPointInPixels.y != 0.0f)
                    glTranslatef(RENDER_IN_SUBPIXEL(-m_tAnchorPointInPixels.x), RENDER_IN_SUBPIXEL(-m_tAnchorPointInPixels.y), 0);

                //
                // END original implementation
#endif
        }

        /// <summary>
        /// performs OpenGL view-matrix transformation of it's ancestors.
        /// Generally the ancestors are already transformed, but in certain cases (eg: attaching a FBO)
        /// it's necessary to transform the ancestors again.
        /// @since v0.7.2
        /// </summary>
        public void transformAncestors()
        {
            ///@todo
            throw new NotImplementedException();
        }

        #endregion

        #region boundingBox

        /// <summary>
        /// returns a "local" axis aligned bounding box of the node.
        /// The returned box is relative only to its parent.
        /// @since v0.8.2
        /// </summary>
        public CCRect boundingBox()
        {
            CCRect ret = boundingBoxInPixels();
            return ccMacros.CC_RECT_PIXELS_TO_POINTS(ret);
        }

        /// <summary>
        /// returns a "local" axis aligned bounding box of the node in pixels.
        /// The returned box is relative only to its parent.
        /// The returned box is in Points.
        /// @since v0.99.5
        /// </summary>
        public CCRect boundingBoxInPixels()
        {
            CCRect rect = new CCRect(0, 0, m_tContentSizeInPixels.width, m_tContentSizeInPixels.height);
            return CCAffineTransform.CCRectApplyAffineTransform(rect, nodeToParentTransform());
        }

        #endregion

        #region actions

        /// <summary>
        /// Executes an action, and returns the action that is executed.
        /// The node becomes the action's target.
        /// @warning Starting from v0.8 actions don't retain their target anymore.
        /// @since v0.7.1
        /// @return 
        /// </summary>
        /// <returns>An Action pointer</returns>
        public CCAction runAction(CCAction action)
        {
            Debug.Assert(action != null, "Argument must be non-nil");
            CCActionManager.sharedManager().addAction(action, this, !m_bIsRunning);
            return action;
        }

        /// <summary>
        /// Removes all actions from the running action list
        /// </summary>
        public void stopAllActions()
        {
            CCActionManager.sharedManager().removeAllActionsFromTarget(this);
        }

        /// <summary>
        /// Removes an action from the running action list
        /// </summary>
        public void stopAction(CCAction action)
        {
            CCActionManager.sharedManager().removeAction(action);
        }

        /// <summary>
        /// Removes an action from the running action list given its tag
        /// @since v0.7.1
        /// </summary>
        public void stopActionByTag(int tag)
        {
            Debug.Assert(tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");
            CCActionManager.sharedManager().removeActionByTag(tag, this);
        }

        /// <summary>
        /// Gets an action from the running action list given its tag
        /// @since v0.7.1
        /// @return
        /// </summary>
        /// <returns>the Action the with the given tag</returns>
        public CCAction getActionByTag(uint tag)
        {
            Debug.Assert((int)tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");
            return CCActionManager.sharedManager().getActionByTag(tag, this);
        }

        /// <summary>
        /// Returns the numbers of actions that are running plus the ones that are schedule to run (actions in actionsToAdd and actions arrays). 
        /// Composable actions are counted as 1 action. Example:
        /// If you are running 1 Sequence of 7 actions, it will return 1.
        /// If you are running 7 Sequences of 2 actions, it will return 7.
        /// </summary>
        public uint numberOfRunningActions()
        {
            return CCActionManager.sharedManager().numberOfRunningActionsInTarget(this);
        }

        #endregion

        #region timers/Schedule

        ///@todo
        //check whether a selector is scheduled
        // bool isScheduled(SEL_SCHEDULE selector);

        /// <summary>
        /// schedules the "update" method. It will use the order number 0. This method will be called every frame.
        /// Scheduled methods with a lower order value will be called before the ones that have a higher order value.
        /// Only one "update" method could be scheduled per node.
        /// @since v0.99.3
        /// </summary>
        public void sheduleUpdate()
        {
            scheduleUpdateWithPriority(0);
        }

        /// <summary>
        /// schedules the "update" selector with a custom priority. This selector will be called every frame.
        /// Scheduled selectors with a lower priority will be called before the ones that have a higher value.
        /// Only one "update" selector could be scheduled per node (You can't have 2 'update' selectors).
        /// @since v0.99.3
        /// </summary>
        /// <param name="priority"></param>
        public void scheduleUpdateWithPriority(int priority)
        {
            CCScheduler.sharedScheduler().scheduleUpdateForTarget(this, priority, !m_bIsRunning);
        }

        /// <summary>
        ///  unschedules the "update" method.
        /// @since v0.99.3
        /// </summary>
        public void unscheduleUpdate()
        {
            CCScheduler.sharedScheduler().unscheduleUpdateForTarget(this);
        }

        /// <summary>
        /// schedules a selector.
        /// The scheduled selector will be ticked every frame
        /// </summary>
        /// <param name="selector"></param>
        void schedule(SEL_SCHEDULE selector)
        {
            this.schedule(selector, 0);
        }

        /// <summary>
        /// schedules a custom selector with an interval time in seconds.
        ///If time is 0 it will be ticked every frame.
        ///If time is 0, it is recommended to use 'scheduleUpdate' instead.
        ///If the selector is already scheduled, then the interval parameter 
        ///will be updated without scheduling it again.
        /// </summary>
        void schedule(SEL_SCHEDULE selector, float interval)
        {
            CCScheduler.sharedScheduler().scheduleSelector(selector, this, interval, !m_bIsRunning);
        }

        /// <summary>
        /// unschedules a custom selector.
        /// </summary>
        void unschedule(SEL_SCHEDULE selector)
        {
            // explicit nil handling
            if (selector != null)
            {
                CCScheduler.sharedScheduler().unscheduleSelector(selector, this);
            }
        }

        /// <summary>
        /// unschedule all scheduled selectors: custom selectors, and the 'update' selector.
        /// Actions are not affected by this method.
        /// @since v0.99.3
        /// </summary>
        public void unsheduleAllSelectors()
        {
            CCScheduler.sharedScheduler().unscheduleAllSelectorsForTarget(this);
        }

        /// <summary>
        /// resumes all scheduled selectors and actions.
        /// Called internally by onEnter
        /// </summary>
        public void resumeSchedulerAndActions()
        {
            CCScheduler.sharedScheduler().resumeTarget(this);
            CCActionManager.sharedManager().resumeTarget(this);
        }

        /// <summary>
        /// pauses all scheduled selectors and actions.
        /// Called internally by onExit
        /// </summary>
        public void pauseSchedulerAndActions()
        {
            CCScheduler.sharedScheduler().pauseTarget(this);
            CCActionManager.sharedManager().pauseTarget(this);
        }

        #endregion

        #region transformation methods

        /// <summary>
        /// Returns the matrix that transform the node's (local) space coordinates into the parent's space coordinates.
        /// The matrix is in Pixels.
        /// @since v0.7.1
        /// </summary>
        public CCAffineTransform nodeToParentTransform()
        {
            if (m_bIsTransformDirty)
            {
                m_tTransform = CCAffineTransform.CCAffineTransformMakeIdentity();

                if (!m_bIsRelativeAnchorPoint && !CCPoint.CCPointEqualToPoint(m_tAnchorPointInPixels, new CCPoint()))
                {
                    m_tTransform = CCAffineTransform.CCAffineTransformTranslate(m_tTransform, m_tAnchorPointInPixels.x, m_tAnchorPointInPixels.y);
                }

                if (!CCPoint.CCPointEqualToPoint(m_tPositionInPixels, new CCPoint()))
                {
                    m_tTransform = CCAffineTransform.CCAffineTransformTranslate(m_tTransform, m_tPositionInPixels.x, m_tPositionInPixels.y);
                }

                if (m_fRotation != 0)
                {
                    m_tTransform = CCAffineTransform.CCAffineTransformRotate(m_tTransform, -ccMacros.CC_DEGREES_TO_RADIANS(m_fRotation));
                }

                if (m_fSkewX != 0 || m_fSkewY != 0)
                {
                    // create a skewed coordinate system
                    CCAffineTransform skew = CCAffineTransform.CCAffineTransformMake(1.0f,
                        (float)Math.Tan(ccMacros.CC_DEGREES_TO_RADIANS(m_fSkewY)),
                          (float)Math.Tan(ccMacros.CC_DEGREES_TO_RADIANS(m_fSkewX)), 1.0f, 0.0f, 0.0f);
                    // apply the skew to the transform
                    m_tTransform = CCAffineTransform.CCAffineTransformConcat(skew, m_tTransform);
                }

                if (!(m_fScaleX == 1 && m_fScaleY == 1))
                {
                    m_tTransform = CCAffineTransform.CCAffineTransformScale(m_tTransform, m_fScaleX, m_fScaleY);
                }

                if (!CCPoint.CCPointEqualToPoint(m_tAnchorPointInPixels, new CCPoint()))
                {
                    m_tTransform = CCAffineTransform.CCAffineTransformTranslate(m_tTransform, -m_tAnchorPointInPixels.x, -m_tAnchorPointInPixels.y);
                }

                m_bIsTransformDirty = false;
            }

            return m_tTransform;
        }

        /// <summary>
        /// Returns the matrix that transform parent's space coordinates to the node's (local) space coordinates.
        /// The matrix is in Pixels.
        /// @since v0.7.1
        /// </summary>
        public CCAffineTransform parentToNodeTransform()
        {
            if (m_bIsInverseDirty)
            {
                m_tInverse = CCAffineTransform.CCAffineTransformInvert(this.nodeToParentTransform());
                m_bIsInverseDirty = false;
            }

            return m_tInverse;
        }

        /// <summary>
        /// Retrusn the world affine transform matrix. The matrix is in Pixels.
        /// @since v0.7.1
        /// </summary>
        public CCAffineTransform nodeToWorldTransform()
        {
            CCAffineTransform t = this.nodeToParentTransform();

            CCNode p = m_pParent;
            while (p != null)
            {
                var temp = p.nodeToParentTransform();
                t = CCAffineTransform.CCAffineTransformConcat(t, temp);
                p = p.parent;
            }

            return t;
        }

        /// <summary>
        /// Returns the inverse world affine transform matrix. The matrix is in Pixels.
        ///@since v0.7.1
        /// </summary>
        public CCAffineTransform worldToNodeTransform()
        {
            return CCAffineTransform.CCAffineTransformInvert(this.nodeToWorldTransform());
        }

        #endregion

        #region convertToSpace

        /// <summary>
        /// Converts a Point to node (local) space coordinates. The result is in Points.
        /// @since v0.7.1
        /// </summary>
        public CCPoint convertToNodeSpace(CCPoint worldPoint)
        {
            CCPoint ret;
            if (CCDirector.sharedDirector().ContentScaleFactor == 1)
            {
                ret = CCAffineTransform.CCPointApplyAffineTransform(worldPoint, worldToNodeTransform());
            }
            else
            {
                ret = CCPointExtension.ccpMult(worldPoint, CCDirector.sharedDirector().ContentScaleFactor);
                ret = CCAffineTransform.CCPointApplyAffineTransform(ret, worldToNodeTransform());
                ret = CCPointExtension.ccpMult(ret, 1 / CCDirector.sharedDirector().ContentScaleFactor);
            }

            return ret;
        }

        /// <summary>
        /// Converts a Point to world space coordinates. The result is in Points.
        /// @since v0.7.1
        /// </summary>
        public CCPoint convertToWorldSpace(CCPoint nodePoint)
        {
            CCPoint ret;
            if (CCDirector.sharedDirector().ContentScaleFactor == 1)
            {
                ret = CCAffineTransform.CCPointApplyAffineTransform(nodePoint, nodeToWorldTransform());
            }
            else
            {
                ret = CCPointExtension.ccpMult(nodePoint, CCDirector.sharedDirector().ContentScaleFactor);
                ret = CCAffineTransform.CCPointApplyAffineTransform(ret, nodeToWorldTransform());
                ret = CCPointExtension.ccpMult(ret, 1 / CCDirector.sharedDirector().ContentScaleFactor);
            }

            return ret;
        }

        /// <summary>
        /// Converts a Point to node (local) space coordinates. The result is in Points.
        /// treating the returned/received node point as anchor relative.
        /// @since v0.7.1
        /// </summary>
        public CCPoint convertToNodeSpaceAR(CCPoint worldPoint)
        {
            CCPoint nodePoint = convertToNodeSpace(worldPoint);
            CCPoint anchorInPoints;
            if (CCDirector.sharedDirector().ContentScaleFactor == 1)
            {
                anchorInPoints = m_tAnchorPointInPixels;
            }
            else
            {
                anchorInPoints = CCPointExtension.ccpMult(m_tAnchorPointInPixels, 1 / CCDirector.sharedDirector().ContentScaleFactor);
            }

            return CCPointExtension.ccpSub(nodePoint, anchorInPoints);
        }

        /// <summary>
        /// Converts a local Point to world space coordinates.The result is in Points.
        /// treating the returned/received node point as anchor relative.
        /// @since v0.7.1
        /// </summary>
        public CCPoint convertToWorldSpaceAR(CCPoint nodePoint)
        {
            CCPoint anchorInPoints;
            if (CCDirector.sharedDirector().ContentScaleFactor == 1)
            {
                anchorInPoints = m_tAnchorPointInPixels;
            }
            else
            {
                anchorInPoints = CCPointExtension.ccpMult(m_tAnchorPointInPixels, 1 / CCDirector.sharedDirector().ContentScaleFactor);
            }

            CCPoint pt = CCPointExtension.ccpAdd(nodePoint, anchorInPoints);
            return convertToWorldSpace(pt);
        }

        /// <summary>
        /// convenience methods which take a CCTouch instead of CCPoint
        ///@since v0.7.1
        /// </summary>
        public CCPoint convertTouchToNodeSpace(CCTouch touch)
        {
            CCPoint point = touch.locationInView(touch.view());
            point = CCDirector.sharedDirector().convertToGL(point);
            return this.convertToNodeSpace(point);
        }

        /// <summary>
        /// converts a CCTouch (world coordinates) into a local coordiante. This method is AR (Anchor Relative).
        /// @since v0.7.1
        /// </summary>
        CCPoint convertTouchToNodeSpaceAR(CCTouch touch)
        {
            CCPoint point = touch.locationInView(touch.view());
            point = CCDirector.sharedDirector().convertToGL(point);
            return this.convertToNodeSpaceAR(point);
        }

        private CCPoint convertToWindowSpace(CCPoint nodePoint)
        {
            CCPoint worldPoint = this.convertToWorldSpace(nodePoint);
            return CCDirector.sharedDirector().convertToUI(worldPoint);
        }

        #endregion

        #region Properties: The main features of a CCNode

        protected CCPoint m_tPosition;
        /// <summary>
        /// Position (x,y) of the node in points. (0,0) is the left-bottom corner.
        /// </summary>
        public virtual CCPoint position
        {
            get
            {
                return m_tPosition;
            }
            set
            {
                m_tPosition = value;

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

        protected CCPoint m_tPositionInPixels;
        /// <summary>
        /// Position (x,y) of the node in pixels. (0,0) is the left-bottom corner.
        /// </summary>
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

        protected float m_fScale;
        /// <summary>
        /// The scale factor of the node. 1.0 is the default scale factor. It modifies the X and Y scale at the same time.
        /// </summary>
        public virtual float scale
        {
            get
            {
                return m_fScale;
            }
            set
            {
                m_fScaleX = m_fScaleY = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
                //#ifdef CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                //    m_bIsTransformGLDirty = true;
                //#endif
            }
        }

        protected float m_fScaleX;
        /// <summary>
        /// The scale factor of the node. 1.0 is the default scale factor. It only modifies the X scale factor.
        /// </summary>
        public virtual float scaleX
        {
            get
            {
                return m_fScaleX;
            }
            set
            {
                m_fScaleX = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
                //#ifdef CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                //    m_bIsTransformGLDirty = true;
                //#endif
            }
        }

        protected float m_fScaleY;
        /// <summary>
        /// The scale factor of the node. 1.0 is the default scale factor. It only modifies the Y scale factor.
        /// </summary>
        public virtual float scaleY
        {
            get
            {
                return m_fScaleY;
            }
            set
            {
                m_fScaleY = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
                //#ifdef CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                //    m_bIsTransformGLDirty = true;
                //#endif
            }
        }

        protected float m_fRotation;
        /// <summary>
        /// The rotation (angle) of the node in degrees. 0 is the default rotation angle. Positive values rotate node CW
        /// </summary>
        public virtual float rotation
        {
            get
            {
                return m_fRotation;
            }
            set
            {
                m_fRotation = value;
                m_bIsTransformDirty = m_bIsInverseDirty = true;
                //#ifdef CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                //    m_bIsTransformGLDirty = true;
                //#endif
            }
        }

        protected CCPoint m_tAnchorPoint;
        /// <summary>
        /// anchorPoint is the point around which all transformations and positioning manipulations take place.
        /// It's like a pin in the node where it is "attached" to its parent.
        /// The anchorPoint is normalized, like a percentage. (0,0) means the bottom-left corner and (1,1) means the top-right corner.
        /// But you can use values higher than (1,1) and lower than (0,0) too.
        /// The default anchorPoint is (0,0). It starts in the bottom-left corner. CCSprite and other subclasses have a different default anchorPoint.
        /// @since v0.8
        /// </summary>
        public virtual CCPoint anchorPoint
        {
            get
            {
                return m_tAnchorPoint;
            }
            set
            {
                if (!CCPoint.CCPointEqualToPoint(value, m_tAnchorPoint))
                {
                    m_tAnchorPoint = value;
                    m_tAnchorPointInPixels = CCPointExtension.ccp(m_tContentSizeInPixels.width * m_tAnchorPoint.x,
                        m_tContentSizeInPixels.height * m_tAnchorPoint.y);
                    m_bIsTransformDirty = m_bIsInverseDirty = true;
                    //#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                    //                    m_bIsTransformGLDirty = true;
                    //#endif
                }
            }
        }

        protected CCPoint m_tAnchorPointInPixels;
        /// <summary>
        /// The anchorPoint in absolute pixels.
        /// Since v0.8 you can only read it. If you wish to modify it, use anchorPoint instead
        /// </summary>
        public CCPoint anchorPointInPixels
        {
            // read only
            get
            {
                return m_tAnchorPointInPixels;
            }
            set
            {
                if (!CCPoint.CCPointEqualToPoint(value, m_tAnchorPoint))
                {
                    m_tAnchorPoint = value;
                    m_tAnchorPointInPixels = new CCPoint(m_tContentSizeInPixels.width * m_tAnchorPoint.x, m_tContentSizeInPixels.height * m_tAnchorPoint.y);
                    m_bIsTransformDirty = m_bIsInverseDirty = true;

#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                    m_bIsTransformGLDirty = true;
#endif
                }
            }
        }

        protected CCSize m_tContentSize;
        /// <summary>
        /// The untransformed size of the node in Points
        /// The contentSize remains the same no matter the node is scaled or rotated.
        /// All nodes has a size. Layer and Scene has the same size of the screen.
        /// @since v0.8
        /// </summary>
        public CCSize contentSize
        {
            get
            {
                return m_tContentSize;
            }
            set
            {
                if (!CCSize.CCSizeEqualToSize(value, m_tContentSize))
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

        protected CCSize m_tContentSizeInPixels;
        /// <summary>
        /// The untransformed size of the node in Pixels
        /// The contentSize remains the same no matter the node is scaled or rotated.
        /// All nodes has a size. Layer and Scene has the same size of the screen.
        /// @since v0.8
        /// </summary>
        public CCSize contentSizeInPixels
        {
            get
            {
                return m_tContentSizeInPixels;
            }
            set
            {
                if (!CCSize.CCSizeEqualToSize(value, m_tContentSizeInPixels))
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

        protected bool m_bIsVisible;
        /// <summary>
        /// Whether of not the node is visible. Default is YES
        /// </summary>
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

        protected int m_nZOrder;
        /// <summary>
        /// The z order of the node relative to it's "brothers": children of the same parent
        /// </summary>
        public int zOrder
        {
            get
            {
                return m_nZOrder;
            }
            private set
            {
                //used internally to alter the zOrder variable. DON'T call this method manually
                m_nZOrder = value;
            }
        }

        protected float m_fVertexZ;
        /// <summary>
        /// The real openGL Z vertex.
        /// Differences between openGL Z vertex and cocos2d Z order:
        /// OpenGL Z modifies the Z vertex, and not the Z order in the relation between parent-children
        /// OpenGL Z might require to set 2D projection
        /// cocos2d Z order works OK if all the nodes uses the same openGL Z vertex. eg: vertexZ = 0
        /// @warning: Use it at your own risk since it might break the cocos2d parent-children z order
        /// @since v0.8
        /// </summary>
        public virtual float vertexZ
        {
            get
            {
                return m_fVertexZ / CCDirector.sharedDirector().ContentScaleFactor;
            }
            set
            {
                m_fVertexZ = value * CCDirector.sharedDirector().ContentScaleFactor;
            }
        }

        protected float m_fSkewX;
        /// <summary>
        /// The X skew angle of the node in degrees.
        /// This angle describes the shear distortion in the X direction.
        /// Thus, it is the angle between the Y axis and the left edge of the shape
        /// The default skewX angle is 0. Positive values distort the node in a CW direction.
        /// </summary>
        public virtual float skewX
        {
            get
            {
                m_bIsTransformDirty = m_bIsInverseDirty = true;
                return m_fSkewX;
                //#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                //                m_bIsTransformGLDirty = true;
                //#endif
            }
            set
            {
                m_fSkewX = value;
            }
        }

        protected float m_fSkewY;
        /// <summary>
        /// The Y skew angle of the node in degrees.
        /// This angle describes the shear distortion in the Y direction.
        /// Thus, it is the angle between the X axis and the bottom edge of the shape
        /// The default skewY angle is 0. Positive values distort the node in a CCW direction.
        /// </summary>
        public virtual float skewY
        {
            get
            {
                m_bIsTransformDirty = m_bIsInverseDirty = true;
                return m_fSkewY;

                //#if CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
                //                m_bIsTransformGLDirty = true;
                //#endif
            }
            set
            {
                m_fSkewY = value;
            }
        }

        #endregion

        ///@todo add CCCamera
        /** A CCCamera object that lets you move the node using a gluLookAt
           
       @property(nonatomic,readonly) CCCamera* camera;
         */


        ///@todo
        /** A CCGrid object that is used when applying effects */
        /// @property(nonatomic,readwrite,retain) CCGridBase* grid;

        protected bool m_bIsRunning;
        /// <summary>
        /// whether or not the node is running
        /// </summary>
        public bool isRunning
        {
            // read only
            get
            {
                return m_bIsRunning;
            }
        }

        protected CCNode m_pParent;
        /// <summary>
        /// A weak reference to the parent
        /// </summary>
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

        protected bool m_bIsRelativeAnchorPoint;
        /// <summary>
        /// If YES the transformtions will be relative to it's anchor point.
        /// Sprites, Labels and any other sizeble object use it have it enabled by default.
        /// Scenes, Layers and other "whole screen" object don't use it, have it disabled by default.
        /// </summary>
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

        protected int m_nTag;
        /// <summary>
        /// A tag used to identify the node easily
        /// </summary>
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

        protected object m_pUserData;
        /// <summary>
        /// A custom user data pointer
        /// </summary>
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

        // transform
        CCAffineTransform m_tTransform, m_tInverse;

#if	CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
        float[] m_pTransformGL;
#endif

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
