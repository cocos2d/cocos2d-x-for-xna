/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

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
    public class CCFadeOutUpTiles : CCFadeOutTRTiles
    {
        public virtual float testFunc(ccGridSize pos, float time)
        {
            CCPoint n = new CCPoint((float)(m_sGridSize.x * time), (float)(m_sGridSize.y * time));
            if (n.y == 0.0f)
            {
                return 1.0f;
            }

            return (float)Math.Pow(pos.y / n.y, 6);
        }

        public virtual void transformTile(ccGridSize pos, float distance)
        {
            ccQuad3 coords = originalTile(pos);
            CCPoint step = m_pTarget.Grid.Step;

            coords.bl.y += (step.y / 2) * (1.0f - distance);
            coords.br.y += (step.y / 2) * (1.0f - distance);
            coords.tl.y -= (step.y / 2) * (1.0f - distance);
            coords.tr.y -= (step.y / 2) * (1.0f - distance);

            setTile(pos, coords);
        }

        /// <summary>
        /// creates the action with the grid size and the duration 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static CCFadeOutUpTiles actionWithSize(ccGridSize gridSize, float time)
        {
            CCFadeOutUpTiles pAction = new CCFadeOutUpTiles();

            if (pAction != null)
            {
                if (pAction.initWithSize(gridSize, time))
                {
                    //pAction->autorelease();
                }
                else
                {
                    //CC_SAFE_RELEASE_NULL(pAction);
                }
            }

            return pAction;
        }
    }
}
