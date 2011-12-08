/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
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
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Ball : CCSprite
    {
        public float radius()
        {
            return Texture.getContentSize().width / 2;
        }

        //BOOL initWithTexture(CCTexture2D* aTexture);
        //virtual void setTexture(CCTexture2D* newTexture);
        public void move(float delta)
        {
            this.position = new CCPoint(position.x + m_velocity.x * delta, position.y + m_velocity.y * delta);

            if (position.x > 320 - radius())
            {
                position = new CCPoint(320 - radius(), position.y);
                m_velocity.x *= -1;
            }
            else if (position.x < radius())
            {
                position = new CCPoint(radius(), position.y);
                m_velocity.x *= -1;
            }
        }

        public void collideWithPaddle(Paddle paddle)
        {
            CCRect paddleRect = paddle.rect();
            paddleRect.origin.x += paddle.position.x;
            paddleRect.origin.y += paddle.position.y;

            float lowY = CCRect.CCRectGetMinY(paddleRect);
            float midY = CCRect.CCRectGetMidY(paddleRect);
            float highY = CCRect.CCRectGetMaxY(paddleRect);

            float leftX = CCRect.CCRectGetMinX(paddleRect);
            float rightX = CCRect.CCRectGetMaxX(paddleRect);

            if (position.x > leftX && position.x < rightX)
            {

                bool hit = false;
                float angleOffset = 0.0f;

                if (position.y > midY && position.y <= highY + radius())
                {
                    position = new CCPoint(position.x, highY + radius());
                    hit = true;
                    angleOffset = (float)Math.PI / 2;
                }
                else if (position.y < midY && position.y >= lowY - radius())
                {
                    position = new CCPoint(position.x, lowY - radius());
                    hit = true;
                    angleOffset = -(float)Math.PI / 2;
                }

                if (hit)
                {
                    float hitAngle = (float)Math.Atan2(new CCPoint(paddle.position.x - position.x, paddle.position.y - position.y).y, new CCPoint(paddle.position.x - position.x, paddle.position.y - position.y).x) + angleOffset;

                    float scalarVelocity = (float)Math.Sqrt((double)(m_velocity.x * m_velocity.x + m_velocity.y * m_velocity.y)) * 1.05f;
                    float velocityAngle = -(float)Math.Atan2(m_velocity.y, m_velocity.x) + 0.5f * hitAngle;

                    m_velocity = new CCPoint(new CCPoint((float)Math.Cos(velocityAngle), (float)Math.Sin(velocityAngle)).x * scalarVelocity, new CCPoint((float)Math.Cos(velocityAngle), (float)Math.Sin(velocityAngle)).y * scalarVelocity);
                }
            }
        }

        public static Ball ballWithTexture(CCTexture2D aTexture)
        {
            Ball pBall = new Ball();
            pBall.initWithTexture(aTexture);
            //pBall->autorelease();

            return pBall;
        }

        CCPoint m_velocity;
        public CCPoint Velocity
        {
            get { return m_velocity; }
            set { m_velocity = value; }
        }
    }
}
