using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Collision.Shapes;

namespace Box2D.Collision
{
    enum b2ContactFeatureType
    {
        e_vertex = 0,
        e_face = 1
    }

    /// The features that intersect to form the contact point
    /// This must be 4 bytes or less.
    public struct b2ContactFeature
    {
        byte indexA;        ///< Feature index on shapeA
        byte indexB;        ///< Feature index on shapeB
        byte typeA;        ///< The feature type on shapeA
        byte typeB;        ///< The feature type on shapeB

        public b2ContactFeature(byte iA, byte iB, byte tA, byte tB)
        {
            indexA = iA;
            indexB = iB;
            typeA = tA;
            typeB = tB;
        }
        public static b2ContactFeature Zero = new b2ContactFeature(0, 0, 0, 0);
    }

    /// A manifold point is a contact point belonging to a contact
    /// manifold. It holds details related to the geometry and dynamics
    /// of the contact points.
    /// The local point usage depends on the manifold type:
    /// -e_circles: the local center of circleB
    /// -e_faceA: the local center of cirlceB or the clip point of polygonB
    /// -e_faceB: the clip point of polygonA
    /// This structure is stored across time steps, so we keep it small.
    /// Note: the impulses are used for internal caching and may not
    /// provide reliable contact forces, especially for high speed collisions.
    struct b2ManifoldPoint
    {
        public b2Vec2 localPoint;       ///< usage depends on manifold type
        public float normalImpulse;     ///< the non-penetration impulse
        public float tangentImpulse;    ///< the friction impulse
        public b2ContactFeature id;     ///< uniquely identifies a contact point between two shapes
    }

    /// A manifold for two touching convex shapes.
    /// Box2D supports multiple types of contact:
    /// - clip point versus plane with radius
    /// - point versus point with radius (circles)
    /// The local point usage depends on the manifold type:
    /// -e_circles: the local center of circleA
    /// -e_faceA: the center of faceA
    /// -e_faceB: the center of faceB
    /// Similarly the local normal usage:
    /// -e_circles: not used
    /// -e_faceA: the normal on polygonA
    /// -e_faceB: the normal on polygonB
    /// We store contacts in this way so that position correction can
    /// account for movement, which is critical for continuous physics.
    /// All contact scenarios must be expressed in one of these types.
    /// This structure is stored across time steps, so we keep it small.
    enum b2ManifoldType
    {
        e_circles,
        e_faceA,
        e_faceB
    }

    /// This is used to compute the current state of a contact manifold.
    public struct b2WorldManifold
    {
        /// Evaluate the manifold with supplied transforms. This assumes
        /// modest motion from the original state. This does not change the
        /// point count, impulses, etc. The radii must come from the shapes
        /// that generated the manifold.
        public void Initialize(b2Manifold[] manifold,
                        b2Transform xfA, float radiusA,
                        b2Transform xfB, float radiusB)
        {
            points = new b2Vec2[b2Settings.b2_maxManifoldPoints];
        }

        public b2Vec2 normal;      ///< world vector pointing from A to B
        public b2Vec2[] points;    ///< world contact point (point of intersection)
    }

    /// This is used for determining the state of contact points.
    public enum b2PointState
    {
        b2_nullState,        ///< point does not exist
        b2_addState,        ///< point was added in the update
        b2_persistState,    ///< point persisted across the update
        b2_removeState        ///< point was removed in the update
    }

    /// Used for computing contact manifolds.
    public struct b2ClipVertex
    {
        public b2Vec2 v;
        public b2ContactFeature id;
    };

    /// Ray-cast input data. The ray extends from p1 to p1 + maxFraction * (p2 - p1).
    public struct b2RayCastInput
    {
        public b2Vec2 p1, p2;
        public float maxFraction;
    };

    /// Ray-cast output data. The ray hits at p1 + fraction * (p2 - p1), where p1 and p2
    /// come from b2RayCastInput.
    public struct b2RayCastOutput
    {
        public b2Vec2 normal;
        public float fraction;
    }

    public class b2Manifold
    {
        public b2ManifoldPoint[] points = new b2ManifoldPoint[b2Settings.b2_maxManifoldPoints];    ///< the points of contact
        public b2Vec2 localNormal;                                ///< not use for Type::e_points
        public b2Vec2 localPoint;                                ///< usage depends on manifold type
        public b2ManifoldType type;
        public int pointCount;                                ///< the number of manifold points
    }

    public abstract class b2Collision
    {
        public static byte b2_nullFeature = byte.MaxValue;

        public static bool b2TestOverlap(b2AABB a, b2AABB b)
        {
            b2Vec2 d1, d2;
            d1 = b.lowerBound - a.upperBound;
            d2 = a.lowerBound - b.upperBound;

            if (d1.x > 0.0f || d1.y > 0.0f)
                return false;

            if (d2.x > 0.0f || d2.y > 0.0f)
                return false;

            return true;
        }


        /// Compute the point states given two manifolds. The states pertain to the transition from manifold1
        /// to manifold2. So state1 is either persist or remove while state2 is either add or persist.
        public void b2GetPointStates(b2PointState[] state1, b2PointState[] state2,
                              b2Manifold manifold1, b2Manifold manifold2)
        {
        }

        /// Compute the collision manifold between two circles.
        public void b2CollideCircles(b2Manifold manifold,
                               b2CircleShape circleA, b2Transform xfA,
                               b2CircleShape circleB, b2Transform xfB)
        {
            manifold.pointCount = 0;

            b2Vec2 pA = b2Math.b2Mul(xfA, circleA.m_p);
            b2Vec2 pB = b2Math.b2Mul(xfB, circleB.m_p);

            b2Vec2 d = pB - pA;
            float distSqr = b2Math.b2Dot(d, d);
            float rA = circleA.m_radius, rB = circleB.m_radius;
            float radius = rA + rB;
            if (distSqr > radius * radius)
            {
                return;
            }

            manifold.type = b2ManifoldType.e_circles;
            manifold.localPoint = circleA.m_p;
            manifold.localNormal.SetZero();
            manifold.pointCount = 1;

            manifold.points[0].localPoint = circleB.m_p;
            manifold.points[0].id = b2ContactFeature.Zero;
        }

        /// Compute the collision manifold between a polygon and a circle.
        public void b2CollidePolygonAndCircle(b2Manifold manifold,
                                        b2PolygonShape polygonA, b2Transform xfA,
                                        b2CircleShape circleB, b2Transform xfB)
        {
            manifold.pointCount = 0;

            // Compute circle position in the frame of the polygon.
            b2Vec2 c = b2Math.b2Mul(xfB, circleB.m_p);
            b2Vec2 cLocal = b2Math.b2MulT(xfA, c);

            // Find the min separating edge.
            int normalIndex = 0;
            float separation = -b2Settings.b2_maxFloat;
            float radius = polygonA.m_radius + circleB.m_radius;
            int vertexCount = polygonA.m_vertexCount;
            b2Vec2[] vertices = polygonA.m_vertices;
            b2Vec2[] normals = polygonA.m_normals;

            for (int i = 0; i < vertexCount; ++i)
            {
                float s = b2Math.b2Dot(normals[i], cLocal - vertices[i]);

                if (s > radius)
                {
                    // Early out.
                    return;
                }

                if (s > separation)
                {
                    separation = s;
                    normalIndex = i;
                }
            }

            // Vertices that subtend the incident face.
            int vertIndex1 = normalIndex;
            int vertIndex2 = vertIndex1 + 1 < vertexCount ? vertIndex1 + 1 : 0;
            b2Vec2 v1 = vertices[vertIndex1];
            b2Vec2 v2 = vertices[vertIndex2];

            // If the center is inside the polygon ...
            if (separation < b2Settings.b2_epsilon)
            {
                manifold.pointCount = 1;
                manifold.type = b2ManifoldType.e_faceA;
                manifold.localNormal = normals[normalIndex];
                manifold.localPoint = 0.5f * (v1 + v2);
                manifold.points[0].localPoint = circleB.m_p;
                manifold.points[0].id = b2ContactFeature.Zero;
                return;
            }

            // Compute barycentric coordinates
            float u1 = b2Math.b2Dot(cLocal - v1, v2 - v1);
            float u2 = b2Math.b2Dot(cLocal - v2, v1 - v2);
            if (u1 <= 0.0f)
            {
                if (b2Math.b2DistanceSquared(cLocal, v1) > radius * radius)
                {
                    return;
                }

                manifold.pointCount = 1;
                manifold.type = b2ManifoldType.e_faceA;
                manifold.localNormal = cLocal - v1;
                manifold.localNormal.Normalize();
                manifold.localPoint = v1;
                manifold.points[0].localPoint = circleB.m_p;
                manifold.points[0].id = b2ContactFeature.Zero;
            }
            else if (u2 <= 0.0f)
            {
                if (b2Math.b2DistanceSquared(cLocal, v2) > radius * radius)
                {
                    return;
                }

                manifold.pointCount = 1;
                manifold.type = b2ManifoldType.e_faceA;
                manifold.localNormal = cLocal - v2;
                manifold.localNormal.Normalize();
                manifold.localPoint = v2;
                manifold.points[0].localPoint = circleB.m_p;
                manifold.points[0].id = b2ContactFeature.Zero;
            }
            else
            {
                b2Vec2 faceCenter = 0.5f * (v1 + v2);
                float separation = b2Math.b2Dot(cLocal - faceCenter, normals[vertIndex1]);
                if (separation > radius)
                {
                    return;
                }

                manifold.pointCount = 1;
                manifold.type = b2ManifoldType.e_faceA;
                manifold.localNormal = normals[vertIndex1];
                manifold.localPoint = faceCenter;
                manifold.points[0].localPoint = circleB.m_p;
                manifold.points[0].id = b2ContactFeature.Zero;
            }
        }

        /// Compute the collision manifold between two polygons.
        public void b2CollidePolygons(b2Manifold manifold,
                                b2PolygonShape polygonA, b2Transform xfA,
                                b2PolygonShape polygonB, b2Transform xfB)
        {
        }

        /// Compute the collision manifold between an edge and a circle.
        public void b2CollideEdgeAndCircle(b2Manifold manifold,
                                        b2EdgeShape polygonA, b2Transform xfA,
                                        b2CircleShape circleB, b2Transform xfB)
        {
        }

        /// Compute the collision manifold between an edge and a circle.
        public void b2CollideEdgeAndPolygon(b2Manifold manifold,
                                        b2EdgeShape edgeA, b2Transform xfA,
                                        b2PolygonShape circleB, b2Transform xfB)
        {
        }

        /// Clipping for contact manifolds.
        public int b2ClipSegmentToLine(b2ClipVertex[] vOut, b2ClipVertex[] vIn,
                                     b2Vec2 normal, float32 offset, int vertexIndexA)
        {
        }

        /// Determine if two generic shapes overlap.
        public bool b2TestOverlap(b2Shape shapeA, int indexA,
                             b2Shape shapeB, int indexB,
                            b2Transform xfA, b2Transform xfB)
        {
        }
    }
}
