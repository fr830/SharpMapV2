// Copyright 2005, 2006 - Morten Nielsen (www.iter.dk)
//
// This file is part of SharpMap.
// SharpMap is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// SharpMap is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with SharpMap; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeoAPI.Coordinates;
using GeoAPI.Geometries;
using NPack;
using NPack.Interfaces;

namespace SharpMap.SimpleGeometries
{
    /// <summary>
    /// A Polygon is a planar Surface, defined by 1 exterior boundary 
    /// and 0 or more interior boundaries. Each
    /// interior boundary defines a hole in the Polygon.
    /// </summary>
    /// <remarks>
    /// Vertices of rings defining holes in polygons 
    /// are in the opposite direction of the exterior ring.
    /// </remarks>
    [Serializable]
    public class Polygon : Surface, IPolygon
    {
        private LinearRing _exteriorRing;
        private List<LinearRing> _interiorRings = new List<LinearRing>();

        /// <summary>
        /// Instatiates a polygon based on one extorier ring 
        /// and a collection of interior rings.
        /// </summary>
        /// <param name="exteriorRing">Exterior <see cref="LinearRing"/></param>
        /// <param name="interiorRings">Interior LinearRings</param>
        internal protected Polygon(LinearRing exteriorRing, IEnumerable<LinearRing> interiorRings)
        {
            _exteriorRing = exteriorRing;

            if (interiorRings != null)
                _interiorRings.AddRange(interiorRings);
        }

        /// <summary>
        /// Instatiates a polygon based on one exterior ring.
        /// </summary>
        /// <param name="exteriorRing">Exterior ring</param>
        internal protected Polygon(LinearRing exteriorRing)
            : this(exteriorRing, null)
        {
        }

        /// <summary>
        /// Instatiates an empty polygon.
        /// </summary>
        internal protected Polygon(IEnumerable<ICoordinate> verticies)
            : this(new LinearRing(verticies)) { }

        /// <summary>
        /// Instatiates an empty polygon.
        /// </summary>
        internal protected Polygon()
            : this(new LinearRing()) { }

        /// <summary>
        /// Gets or sets the exterior ring of this Polygon
        /// </summary>
        /// <remarks>
        /// This method is supplied as part of the OpenGIS Simple Features Specification.
        /// </remarks>
        public LinearRing ExteriorRing
        {
            get { return _exteriorRing; }
            set { _exteriorRing = value; }
        }

        /// <summary>
        /// Gets or sets the interior rings of this Polygon
        /// </summary>
        public IList<LinearRing> InteriorRings
        {
            get { return _interiorRings; }
            //set { _interiorRings = value; }
        }

        /// <summary>
        /// Returns the Nth interior ring for this Polygon as a LineString
        /// </summary>
        /// <remarks>
        /// This method is supplied as part of the OpenGIS Simple Features Specification.
        /// </remarks>
        /// <param name="N"></param>
        /// <returns></returns>
        public LinearRing InteriorRing(Int32 N)
        {
            return _interiorRings[N];
        }

        /// <summary>
        /// Returns the number of interior rings in this Polygon
        /// </summary>
        /// <remarks>
        /// This method is supplied as part of the OpenGIS Simple Features Specification.
        /// </remarks>
        /// <returns></returns>
        public Int32 NumInteriorRing
        {
            get { return _interiorRings.Count; }
        }

        ///// <summary>
        ///// Transforms the polygon to image coordinates, based on the map
        ///// </summary>
        ///// <param name="map">Map to base coordinates on</param>
        ///// <returns>Polygon in image coordinates</returns>
        //public RenderPoint[] TransformToView(SharpMap.Map map)
        //{

        //    Int32 vertices = _ExteriorRing.Vertices.Count;
        //    for (Int32 i = 0; i < _InteriorRings.Count;i++)
        //        vertices += _InteriorRings[i].Vertices.Count;

        //    System.Drawing.PointF[] v = new System.Drawing.PointF[vertices];
        //    for (Int32 i = 0; i < _ExteriorRing.Vertices.Count; i++)
        //        v[i] = SharpMap.Utilities.Transform.WorldToMap(_ExteriorRing.Vertices[i], map);
        //    Int32 j = _ExteriorRing.Vertices.Count;
        //    for (Int32 k = 0; k < _InteriorRings.Count;k++)
        //    {
        //        for (Int32 i = 0; i < _InteriorRings[k].Vertices.Count; i++)
        //            v[j + i] = SharpMap.Utilities.Transform.WorldToMap(_InteriorRings[k].Vertices[i], map);
        //        j += _InteriorRings[k].Vertices.Count;
        //    }
        //    return v;
        //}

        #region "Inherited methods from abstract class Geometry"

        /// <summary>
        /// Determines if this Polygon and the specified Polygon object has the same values
        /// </summary>
        /// <param name="p">Polygon to compare with</param>
        /// <returns></returns>
        public Boolean Equals(Polygon p)
        {
            if (ReferenceEquals(p, null))
            {
                return false;
            }

            if (!p.ExteriorRing.Equals(ExteriorRing))
            {
                return false;
            }

            if (p.InteriorRings.Count != InteriorRings.Count)
            {
                return false;
            }

            for (Int32 i = 0; i < p.InteriorRings.Count; i++)
            {
                if (!p.InteriorRings[i].Equals(InteriorRings[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use 
        /// in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>A hash code for the current <see cref="GetHashCode"/>.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = ExteriorRing.GetHashCode();
            ;

            for (Int32 i = 0; i < InteriorRings.Count; i++)
            {
                hash = hash ^ InteriorRings[i].GetHashCode();
            }

            return hash;
        }

        /// <summary>
        /// If true, then this Geometry represents the empty point set, �, for the coordinate space. 
        /// </summary>
        /// <returns>Returns 'true' if this Geometry is the empty geometry</returns>
        public override Boolean IsEmpty
        {
            get
            {
                return (ExteriorRing == null) || (ExteriorRing.PointCount == 0);
            }
        }

        /// <summary>
        /// Returns 'true' if this Geometry has no anomalous geometric points, such as self
        /// intersection or self tangency. The description of each instantiable geometric class will include the specific
        /// conditions that cause an instance of that class to be classified as not simple.
        /// </summary>
        public override Boolean IsSimple
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns the closure of the combinatorial boundary of this Geometry. The
        /// combinatorial boundary is defined as described in section 3.12.3.2 of [1]. Because the result of this function
        /// is a closure, and hence topologically closed, the resulting boundary can be represented using
        /// representational geometry primitives
        /// </summary>
        public override IGeometry Boundary
        {
            get { throw new NotImplementedException(); }
        }

        public override Dimensions BoundaryDimension
        {
            get { return Dimensions.Curve; }
        }

        /// <summary>
        /// Returns the shortest distance between any two points in the two geometries
        /// as calculated in the spatial reference system of this Geometry.
        /// </summary>
        public override Double Distance(IGeometry geom)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a geometry that represents all points whose distance from this Geometry
        /// is less than or equal to distance. Calculations are in the Spatial Reference
        /// System of this Geometry.
        /// </summary>
        public override IGeometry Buffer(Double d)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Geometry�Returns a geometry that represents the convex hull of this Geometry.
        /// </summary>
        public override IGeometry ConvexHull()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a geometry that represents the point set intersection of this Geometry
        /// with anotherGeometry.
        /// </summary>
        public override IGeometry Intersection(IGeometry geom)
        {
            return FactoryInternal.SpatialOps.Intersection(this, geom);
        }

        /// <summary>
        /// Returns a geometry that represents the point set union 
        /// of this Geometry with another Geometry.
        /// </summary>
        public override IGeometry Union(IGeometry geometry)
        {
#warning fake the union using a GeometryCollection
            return FactoryInternal.SpatialOps.Union(this, geometry);
        }

        /// <summary>
        /// Returns a geometry that represents the point set difference of this Geometry with anotherGeometry.
        /// </summary>
        public override IGeometry Difference(IGeometry geometry)
        {
            if (geometry == null) throw new ArgumentNullException("geometry");

            if (IsEmpty)
            {
                return geometry;
            }

            if (geometry.IsEmpty)
            {
                return this;
            }

            return FactoryInternal.SpatialOps.Difference(
                ExtentsInternal, new Extents(FactoryInternal, geometry.Extents));
        }

        /// <summary>
        /// Returns a geometry that represents the point set symmetric difference of this Geometry with anotherGeometry.
        /// </summary>
        public override IGeometry SymmetricDifference(IGeometry geom)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// The area of this Surface, as measured in the spatial reference system of this Surface.
        /// </summary>
        public override Double Area
        {
            get
            {
                Double area = 0.0;
                area += _exteriorRing.Area;
                Boolean extIsClockwise = _exteriorRing.IsCcw();

                for (Int32 i = 0; i < _interiorRings.Count; i++)
                {
                    //opposite direction of exterior subtracts area
                    if (_interiorRings[i].IsCcw() != extIsClockwise)
                    {
                        area -= _interiorRings[i].Area;
                    }
                    else
                    {
                        area += _interiorRings[i].Area;
                    }
                }

                return area;
            }
        }

        /// <summary>
        /// The mathematical centroid for this Surface as a Point.
        /// The result is not guaranteed to be on this Surface.
        /// </summary>
        public override IPoint Centroid
        {
            get { return ExteriorRing.ExtentsInternal.GetCentroid(); }
        }

        /// <summary>
        /// A point guaranteed to be on this Surface.
        /// </summary>
        public override IPoint PointOnSurface
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns the bounding box of the object
        /// </summary>
        /// <returns>bounding box</returns>
        public override IExtents Extents
        {
            get
            {
                if (_exteriorRing == null || _exteriorRing.IsEmpty)
                {
                    return new Extents(FactoryInternal);
                }
                else
                {
                    return _exteriorRing.ExtentsInternal;
                }
            }
        }

        public override IEnumerable<Point> GetVertexes()
        {
            foreach (Point point in ExteriorRing.GetVertexes())
            {
                yield return point;
            }

            foreach (LinearRing ring in _interiorRings)
            {
                foreach (Point point in ring.GetVertexes())
                {
                    yield return point;
                }
            }
        }

        public override IEnumerable<Point> GetVertexes(ITransformMatrix<DoubleComponent> transform)
        {
            throw new NotImplementedException();
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a deep copy of the Polygon.
        /// </summary>
        /// <returns>A copy of the Polygon instance.</returns>
        public override Geometry Clone()
        {
            Polygon p = new Polygon();
            p.ExteriorRing = ExteriorRing.Clone() as LinearRing;

            foreach (LinearRing ring in InteriorRings)
            {
                p.InteriorRings.Add(ring.Clone() as LinearRing);
            }

            return p;
        }

        #endregion

        #region IPolygon Members

        ILineString IPolygon.ExteriorRing
        {
            get { return ExteriorRing; }
        }

        public Int32 InteriorRingsCount
        {
            get { return _interiorRings.Count; }
        }

        IEnumerable<ILineString> IPolygon.InteriorRings
        {
            get
            {
                return _interiorRings.ConvertAll<ILineString>(
                    delegate(LinearRing l)
                    {
                        return l;
                    });
            }
        }

        #endregion

        public override Int32 PointCount
        {
            get
            {
                Int32 pointCount = ExteriorRing.PointCount;

                foreach (LinearRing ring in _interiorRings)
                {
                    pointCount += ring.PointCount;
                }

                return pointCount;
            }
        }

        public override OgcGeometryType GeometryType
        {
            get { return OgcGeometryType.Polygon; }
        }

        protected override Boolean EqualsInternal(IGeometry other)
        {
            Polygon p = other as Polygon;
            Debug.Assert(p != null);

            if (p.InteriorRingsCount != InteriorRingsCount)
            {
                return false;
            }

            if (!p.ExteriorRing.Equals(ExteriorRing))
            {
                return false;
            }

            for (int i = 0; i < InteriorRingsCount; i++)
            {
                if (!p.InteriorRings[i].Equals(InteriorRings[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}