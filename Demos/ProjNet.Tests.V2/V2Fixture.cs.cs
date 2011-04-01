﻿namespace ProjNet.Tests.V2
{
    using System;
    using GeoAPI.Coordinates;
    using GeoAPI.CoordinateSystems;
    using GeoAPI.CoordinateSystems.Transformations;
    using NUnit.Framework;
    using SharpMap.Utilities;

    [TestFixture]
    public class V2Fixture
    {
        [Test]
        public void LatLonToGoogle()
        {
            IGeometryServices services = new GeometryServices();
            double[] data = new[] { -74.008573, 40.711946 };

            var coordinateSystemFactory = services.CoordinateSystemFactory;
            ICoordinateSystem source = CrsFor(4326, coordinateSystemFactory);
            ICoordinateSystem target = CrsFor(900913, coordinateSystemFactory);
            Assert.That(source, Is.Not.Null);
            Assert.That(target, Is.Not.Null);

            var transformationFactory = services.CoordinateTransformationFactory;
            ICoordinateTransformation transformation = transformationFactory.CreateFromCoordinateSystems(source, target);
            Assert.That(transformation, Is.Not.Null);
            IMathTransform mathTransform = transformation.MathTransform;
            Assert.That(mathTransform, Is.Not.Null);

            ICoordinateFactory coordinateFactory = services.CoordinateFactory;
            ICoordinate coordinate = coordinateFactory.Create(data);
            Assert.That(coordinate, Is.Not.Null);

            ICoordinate converted = mathTransform.Transform(coordinate);
            Assert.That(converted, Is.Not.Null);

            double x = converted[Ordinates.X];
            double y = converted[Ordinates.Y];
            Console.WriteLine("x: {0}, y: {1}", x, y);

            const double ex = -8238596.6606968148d;
            const double ey = 4969946.166007298d;
            Assert.That(x, Is.EqualTo(ex), String.Format("XConv error: {0}", (ex - x)));
            Assert.That(y, Is.EqualTo(ey), String.Format("YConv error: {0}", (ey - y)));
        }

        private static ICoordinateSystem CrsFor(int srid, ICoordinateSystemFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            switch (srid)
            {
                case 4326:
                    {
                        const string source =
                            "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]";
                        return factory.CreateFromWkt(source);
                    }

                case 3857:
                case 900913:
                    const string googleWktFromGeoserver = "PROJCS[\"WGS84 / Google Mercator\", GEOGCS[\"WGS 84\", DATUM[\"World Geodetic System 1984\", SPHEROID[\"WGS 84\", 6378137.0, 298.257223563, AUTHORITY[\"EPSG\",\"7030\"]], AUTHORITY[\"EPSG\",\"6326\"]], PRIMEM[\"Greenwich\", 0.0, AUTHORITY[\"EPSG\",\"8901\"]], UNIT[\"degree\", 0.017453292519943295], AXIS[\"Longitude\", EAST], AXIS[\"Latitude\", NORTH], AUTHORITY[\"EPSG\",\"4326\"]], PROJECTION[\"Mercator_1SP\"], PARAMETER[\"semi_minor\", 6378137.0], PARAMETER[\"latitude_of_origin\", 0.0], PARAMETER[\"central_meridian\", 0.0], PARAMETER[\"scale_factor\", 1.0], PARAMETER[\"false_easting\", 0.0], PARAMETER[\"false_northing\", 0.0], UNIT[\"m\", 1.0], AXIS[\"x\", EAST], AXIS[\"y\", NORTH], AUTHORITY[\"EPSG\",\"900913\"]]";
                    const string googleWktFromProjNetV2 = "PROJCS[\"Google Mercator\",GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137.0,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0.0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.017453292519943295],AXIS[\"Geodetic latitude\",NORTH],AXIS[\"Geodetic longitude\",EAST],AUTHORITY[\"EPSG\",\"4326\"]],PROJECTION[\"Mercator_1SP\"],PARAMETER[\"semi_minor\",6378137.0],PARAMETER[\"latitude_of_origin\",0.0],PARAMETER[\"central_meridian\",0.0],PARAMETER[\"scale_factor\",1.0],PARAMETER[\"false_easting\",0.0],PARAMETER[\"false_northing\",0.0],UNIT[\"m\",1.0],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"900913\"]]";
                    return factory.CreateFromWkt(googleWktFromProjNetV2);

                default:
                    var format = String.Format("SRID unmanaged: {0}", srid);
                    throw new ArgumentOutOfRangeException("srid", format);
            }
        }

        //private static ICoordinateSystem GetMercatorProjection(ICoordinateSystemFactory factory)
        //{
        //    var parameters = new List<ProjectionParameter> {
        //        new ProjectionParameter("semi_major", 6378137),
        //        new ProjectionParameter("semi_minor", 6378137),
        //        new ProjectionParameter("latitude_of_origin", 0.0),
        //        new ProjectionParameter("central_meridian", 0.0),
        //        new ProjectionParameter("scale_factor", 1.0),
        //        new ProjectionParameter("false_easting", 0.0),
        //        new ProjectionParameter("false_northing", 0.0)
        //    };
        //    var projection = factory.CreateProjection("Mercator", "mercator_1sp", parameters);
        //    var gcs = factory.CreateGeographicCoordinateSystem(
        //        "WGS 84",
        //        AngularUnit.Degrees,
        //        HorizontalDatum.WGS84,
        //        PrimeMeridian.Greenwich,
        //        new AxisInfo("north", AxisOrientationEnum.North),
        //        new AxisInfo("east", AxisOrientationEnum.East));
        //    var mercator = factory.CreateProjectedCoordinateSystem(
        //        "Mercator",
        //        gcs,
        //        projection,
        //        LinearUnit.Metre,
        //        new AxisInfo("East", AxisOrientationEnum.East),
        //        new AxisInfo("North", AxisOrientationEnum.North));
        //    return mercator;
        //}
    }
}