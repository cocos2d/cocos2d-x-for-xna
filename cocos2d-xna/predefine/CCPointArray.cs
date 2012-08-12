using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
/** An Array that contain control points.
 Used by CCCardinalSplineTo and (By) and CCCatmullRomTo (and By) actions.
@ingroup Actions
 */
    public class CCPointArray
{
    /** creates and initializes a Points array with capacity 
    @deprecated: This interface will be deprecated sooner or later.
    */
    public static CCPointArray arrayWithCapacity(int capacity)
    {
        return (new CCPointArray(new List<CCPoint>(capacity)));
    }
    
    /** creates and initializes a Points array with capacity */
    public static CCPointArray create(int capacity)
    {
        return (new CCPointArray(new List<CCPoint>(capacity)));
    }

    public static CCPointArray create(List<CCPoint> pts)
    {
        return (new CCPointArray(pts));
    }

    public CCPointArray()
    {
    }

    public CCPointArray copy()
    {
        CCPointArray copy = new CCPointArray(m_pControlPoints);
        return (copy);
    }

    public CCPointArray(List<CCPoint> copy)
    {
        m_pControlPoints = copy;
    }

    /** initializes a Catmull Rom config with a capacity hint */
    public bool initWithCapacity(int capacity)
    {
        m_pControlPoints = new List<CCPoint>(capacity);
        return (true);
    }
    
    /** appends a control point */
    public void addControlPoint(CCPoint controlPoint)
    {
        m_pControlPoints.Add(controlPoint);
    }
    
    /** inserts a controlPoint at index */
    public void insertControlPoint(CCPoint controlPoint, int index)
    {
        m_pControlPoints[index] = controlPoint;
    }
    
    /** replaces an existing controlPoint at index */
    public void replaceControlPoint(CCPoint controlPoint, int index)
    {
        // should create a new object of CCPoint
        // because developer always use this function like this
        // replaceControlPoint(ccp(x, y))
        // passing controlPoint is a temple object
        // and CCArray::insertObject() will retain the passing object, so it 
        // should be an object created in heap
        CCPoint temp = new CCPoint(controlPoint);
        m_pControlPoints[index] = temp;
    }
    
    /** get the value of a controlPoint at a given index */
    public CCPoint getControlPointAtIndex(int index)
    {
        return (m_pControlPoints[index]);
    }
    
    /** deletes a control point at a given index */
    public void removeControlPointAtIndex(int index)
    {
        m_pControlPoints.RemoveAt(index);
    }
    
    /** returns the number of objects of the control point array */
    public int count()
    {
        return m_pControlPoints.Count;
    }
    
    /** returns a new copy of the array reversed. User is responsible for releasing this copy */
    public CCPointArray reverse()
    {
        List<CCPoint> copy = new List<CCPoint>(m_pControlPoints);
        copy.Reverse();
        return (new CCPointArray(copy));
    }
    
    /** reverse the current control point array inline, without generating a new one */
    public void reverseInline()
    {
        int l = m_pControlPoints.Count;
        for (int i = 0; i < l / 2; ++i)
        {
            CCPoint h = m_pControlPoints[i];
            m_pControlPoints[i] = m_pControlPoints[l - i - 1];
            m_pControlPoints[l - i - 1] = h;
        }
    }

    public List<CCPoint> getControlPoints()
    {
        return m_pControlPoints; 
    }

    public void setControlPoints(List<CCPoint> controlPoints)
    {
        m_pControlPoints = controlPoints;
    }

    /** Array that contains the control points */
    private List<CCPoint> m_pControlPoints;

    }
}

