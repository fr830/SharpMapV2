﻿// Copyright 2006 - 2008: Rory Plaire (codekaizen@gmail.com)
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
using System.IO;
using GeoAPI.Coordinates;
using NPack.Interfaces;
using SharpMap.Symbology;

namespace SharpMap.Rendering
{
    public interface IScene<TCoordinate>
        where TCoordinate : ICoordinate<TCoordinate>, IEquatable<TCoordinate>,
                            IComparable<TCoordinate>, IConvertible,
                            IComputable<Double, TCoordinate> 
    {
        IPen CreatePen(StyleColor color, Single width, Single opacity, 
                       LineJoin lineJoin, LineCap lineCap, Single[] dashArray, 
                       Single dashOffset);
        IBrush CreateBrush(StyleColor color, Single opacity);
        IPath<TCoordinate> CreatePath(ICoordinateSequence<TCoordinate> coordinates);
        IFont CreateFont(String family, FontStyle style, FontWeight weight);
        IHalo CreateHalo(Double radius, StyleColor color);
        IBitmap CreateBitmap(Stream bitmapData, Size<TCoordinate> size);
        ISymbol<TCoordinate> CreateSymbol(Size<TCoordinate> size, Double opacity, Double rotation, TCoordinate anchorPoint, TCoordinate displacement);
    }
}
