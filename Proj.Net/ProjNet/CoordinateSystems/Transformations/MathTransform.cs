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
using GeoAPI.CoordinateSystems.Transformations;
using NPack;
using NPack.Interfaces;

namespace ProjNet.CoordinateSystems.Transformations
{
	/// <summary>
	/// Abstract class for creating multi-dimensional coordinate points transformations.
	/// </summary>
	/// <remarks>
	/// If a client application wishes to query the source and target coordinate 
	/// systems of a transformation, then it should keep hold of the 
    /// <see cref="ICoordinateTransformation{TCoordinate}"/> interface, and use the contained 
	/// math transform object whenever it wishes to perform a transform.
	/// </remarks>
    public abstract class MathTransform<TCoordinate> : IMathTransform<TCoordinate>
	{
		#region IMathTransform Members

		/// <summary>
		/// Gets the dimension of input points.
		/// </summary>
		public virtual int SourceDimension
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the dimension of output points.
		/// </summary>
		public virtual int TargetDimension
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Tests whether this transform does not move any points.
		/// </summary>
		/// <returns></returns>
		public virtual Boolean IsIdentity
		{
            get
            {
                throw new NotImplementedException();
            }
		}

		/// <summary>
		/// Gets a Well-Known Text representation of this object.
		/// </summary>
		public abstract string Wkt { get; }

		/// <summary>
		/// Gets an XML representation of this object.
		/// </summary>
		public abstract string Xml { get; }

		public virtual IMatrix<DoubleComponent> Derivative(TCoordinate point)
		{
			throw new NotImplementedException();
		}

        public virtual IEnumerable<TCoordinate> GetCodomainConvexHull(IEnumerable<TCoordinate> points)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets flags classifying domain points within a convex hull.
		/// </summary>
		/// <remarks>
		/// The supplied ordinates are interpreted as a sequence of points, which 
		/// generates a convex hull in the source space. Conceptually, each of the 
		/// (usually infinite) points inside the convex hull is then tested against
		/// the source domain. The flags of all these tests are then combined. In 
		/// practice, implementations of different transforms will use different 
		/// short-cuts to avoid doing an infinite number of tests.
		/// </remarks>
		/// <param name="points"></param>
		/// <returns></returns>
        public virtual DomainFlags GetDomainFlags(IEnumerable<TCoordinate> points)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates the inverse transform of this object.
		/// </summary>
		/// <remarks>This method may fail if the transform is not one to one. However, all cartographic projections should succeed.</remarks>
		/// <returns></returns>
		public abstract IMathTransform<TCoordinate> Inverse();

		/// <summary>
		/// Transforms a coordinate point. The passed parameter point should not be modified.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
        public abstract TCoordinate Transform(TCoordinate point);

		/// <summary>
		/// Transforms a list of coordinate point ordinal values.
		/// </summary>
		/// <remarks>
		/// This method is provided for efficiently transforming many points. The supplied array 
		/// of ordinal values will contain packed ordinal values. For example, if the source 
		/// dimension is 3, then the ordinals will be packed in this order (x0,y0,z0,x1,y1,z1 ...).
		/// The size of the passed array must be an integer multiple of DimSource. The returned 
		/// ordinal values are packed in a similar way. In some DCPs. the ordinals may be 
		/// transformed in-place, and the returned array may be the same as the passed array.
		/// So any client code should not attempt to reuse the passed ordinal values (although
		/// they can certainly reuse the passed array). If there is any problem then the server
		/// implementation will throw an exception. If this happens then the client should not
		/// make any assumptions about the state of the ordinal values.
		/// </remarks>
		/// <param name="points"></param>
		/// <returns></returns>
        public abstract IEnumerable<TCoordinate> Transform(IEnumerable<TCoordinate> points);

		/// <summary>
		/// Reverses the transformation
		/// </summary>
		public abstract void Invert();

		/// <summary>
		/// Number of degrees per radian.
		/// </summary>
		protected const double DegreesPerRadian = 180 / Math.PI;

		/// <summary>
		/// Number of radians per degree.
		/// </summary>
		protected const double RadiansPerDegree = Math.PI / 180;

		/// <summary>
		/// Converts an angular measure in degrees into an equivilant measure
		/// in radians.
		/// </summary>
		/// <param name="degrees">The measure in degrees to convert.</param>
		/// <returns>
		/// The number of radians for the given <paramref name="degrees"/>
		/// measure.
		/// </returns>
		protected static double DegreesToRadians(double degrees)
		{
			return RadiansPerDegree * degrees;

		}

		/// <summary>
		/// Converts an angular measure in radians into an equivilant measure
		/// in degrees.
		/// </summary>
		/// <param name="radians">The measure in radians to convert.</param>
		/// <returns>
		/// The number of radians for the given <paramref name="radians"/>
		/// measure.
		/// </returns>
		protected static double RadiansToDegrees(double radians)
		{
			return DegreesPerRadian * radians;
		}

		#endregion
	}
}
