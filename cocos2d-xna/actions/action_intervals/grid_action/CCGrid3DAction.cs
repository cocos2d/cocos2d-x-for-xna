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
    public class CCGrid3DAction : CCGridAction
    {
        /// <summary>
        /// returns the grid
        /// </summary>
        /// <returns></returns>
        public virtual CCGridBase getGrid()
        {
            //return CCGrid3D.gridWithSize(m_sGridSize);
            throw new NotImplementedException();
        }

        /// <summary>
        ///  returns the vertex than belongs to certain position in the grid
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public ccVertex3F vertex(ccGridSize pos)
        {
            //CCGrid3D g = (CCGrid3D*)m_pTarget->getGrid();
            //return g->vertex(pos);
            throw new NotImplementedException();
        }

        /// <summary>
        ///  returns the non-transformed vertex than belongs to certain position in the grid
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public ccVertex3F originalVertex(ccGridSize pos)
        {
            //CCGrid3D* g = (CCGrid3D*)m_pTarget->getGrid();
            //return g->originalVertex(pos);
            throw new NotImplementedException();
        }

        /// <summary>
        /// sets a new vertex to a certain position of the grid
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="vertex"></param>
        public void setVertex(ccGridSize pos, ccVertex3F vertex)
        {
            //CCGrid3D* g = (CCGrid3D*)m_pTarget->getGrid();
            //g->setVertex(pos, vertex);
            throw new NotImplementedException();
        }

        /// <summary>
        /// creates the action with size and duration 
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static CCGrid3DAction actionWithSize(ccGridSize gridSize, float duration)
        {
            throw new NotImplementedException("win32 is not implemented");
        }
    }
}
