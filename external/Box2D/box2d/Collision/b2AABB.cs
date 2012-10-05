using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Collision
{
    public struct b2AABB
    {

        public bool IsValid()
        {
            b2Vec2 d = m_upperBound - m_lowerBound;
            bool valid = d.x >= 0.0f && d.y >= 0.0f;
            valid = valid && m_lowerBound.IsValid() && m_upperBound.IsValid();
            return valid;
        }

        /// Get the center of the AABB.
        public b2Vec2 GetCenter()
        {
            return 0.5f * (m_lowerBound + m_upperBound);
        }

        /// Get the extents of the AABB (half-widths).
        public b2Vec2 GetExtents()
        {
            return 0.5f * (m_upperBound - m_lowerBound);
        }

        /// Get the perimeter length
        public float GetPerimeter()
        {
            float wx = m_upperBound.x - m_lowerBound.x;
            float wy = m_upperBound.y - m_lowerBound.y;
            return 2.0f * (wx + wy);
        }

        /// Combine an AABB into this one.
        public void Combine(b2AABB aabb)
        {
            m_lowerBound = b2Math.b2Min(m_lowerBound, aabb.m_lowerBound);
            m_upperBound = b2Math.b2Max(m_upperBound, aabb.m_upperBound);
        }

        /// Combine two AABBs into this one.
        public void Combine(b2AABB aabb1, b2AABB aabb2)
        {
            m_lowerBound = b2Math.b2Min(aabb1.m_lowerBound, aabb2.m_lowerBound);
            m_upperBound = b2Math.b2Max(aabb1.m_upperBound, aabb2.m_upperBound);
        }

        /// Does this aabb contain the provided AABB.
        public bool Contains(b2AABB aabb)
        {
            bool result = true;
            result = result && m_lowerBound.x <= aabb.m_lowerBound.x;
            result = result && m_lowerBound.y <= aabb.m_lowerBound.y;
            result = result && aabb.m_upperBound.x <= m_upperBound.x;
            result = result && aabb.m_upperBound.y <= m_upperBound.y;
            return result;
        }

        public bool RayCast(b2RayCastOutput output, b2RayCastInput input)
        {
        }

        public b2Vec2 lowerBound { get { return (m_lowerBound); } set { m_lowerBound = value; } }
        public b2Vec2 upperBound { get { return (m_upperBound); } set { m_upperBound = value; } }

        private b2Vec2 m_lowerBound;    ///< the lower vertex
        private b2Vec2 m_upperBound;    ///< the upper vertex

    }
}
