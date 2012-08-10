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

namespace cocos2d
{
    public class CCTiledGrid3DAction : CCGridAction
    {
        /// <summary>
        /// returns the tile that belongs to a certain position of the grid
        /// </summary>
        public virtual ccQuad3 tile(ccGridSize pos)
        {
            return (tile(pos.x, pos.y));
        }
        /// <summary>
        /// returns the tile that belongs to a certain position of the grid
        /// </summary>
        public virtual ccQuad3 tile(int x, int y)
        {
            CCTiledGrid3D g = (CCTiledGrid3D)m_pTarget.Grid;
            if (g != null)
            {
                return g.tile(x,y);
            }
            return (null);
        }

        /// <summary>
        /// returns the non-transformed tile that belongs to a certain position of the grid. This 
        /// can return null if the scene is not setup and the update pipeline calls this method
        /// during an action update.
        /// </summary>
        public virtual ccQuad3 originalTile(ccGridSize pos)
        {
            return (originalTile(pos.x, pos.y));
        }

        /// <summary>
        /// returns the non-transformed tile that belongs to a certain position of the grid. This 
        /// can return null if the scene is not setup and the update pipeline calls this method
        /// during an action update.
        /// </summary>
        public virtual ccQuad3 originalTile(int x, int y)
        {
            CCTiledGrid3D g = (CCTiledGrid3D)m_pTarget.Grid;
            if (g != null)
            {
                return g.originalTile(x,y);
            }
            return (null);
        }

        /// <summary>
        /// sets a new tile to a certain position of the grid
        /// </summary>
        public virtual void setTile(ccGridSize pos, ccQuad3 coords)
        {
            setTile(pos.x, pos.y, coords);
        }

        /// <summary>
        /// sets a new tile to a certain position of the grid
        /// </summary>
        public virtual void setTile(int x, int y, ccQuad3 coords)
        {
            if (coords == null)
            {
                return;
            }
            CCTiledGrid3D g = (CCTiledGrid3D)m_pTarget.Grid;
            if (g != null)
            {
                g.setTile(x,y, coords);
            }
        }

        /// <summary>
        /// returns the grid using CCTileGrid3D.gridWithSize()
        /// </summary>
        /// <see cref="CCTileGrid3D"/>
        public override CCGridBase getGrid()
        {
            return CCTiledGrid3D.gridWithSize(m_sGridSize);
        }

        /// <summary>
        /// creates the action with size and duration. See CCGridAction.initWithSize().
        /// </summary>
        /// <seealso cref="CCGridAction"/>
        public static CCTiledGrid3DAction actionWithSize(ccGridSize gridSize, float duration)
        {
            CCTiledGrid3DAction action = new CCTiledGrid3DAction();
            action.initWithSize(gridSize, duration);
            return (action);
        }
    }
}
