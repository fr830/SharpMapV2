// /*
//  *  The attached / following is part of SharpMap.Data.Providers.Kml
//  *  SharpMap.Data.Providers.Kml is free software � 2008 Newgrove Consultants Limited, 
//  *  www.newgrove.com; you can redistribute it and/or modify it under the terms 
//  *  of the current GNU Lesser General Public License (LGPL) as published by and 
//  *  available from the Free Software Foundation, Inc., 
//  *  59 Temple Place, Suite 330, Boston, MA 02111-1307 USA: http://fsf.org/    
//  *  This program is distributed without any warranty; 
//  *  without even the implied warranty of merchantability or fitness for purpose.  
//  *  See the GNU Lesser General Public License for the full details. 
//  *  
//  *  Author: John Diss 2009
//  * 
//  */
using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SharpMap.Entities.Ogc.Kml
{
    [XmlType(TypeName = "LatLonAltBoxType", Namespace = Declarations.SchemaVersion), Serializable]
    [XmlInclude(typeof (DataType))]
    [XmlInclude(typeof (AbstractTimePrimitiveType))]
    [XmlInclude(typeof (SchemaDataType))]
    [XmlInclude(typeof (ItemIconType))]
    [XmlInclude(typeof (AbstractLatLonBoxType))]
    [XmlInclude(typeof (OrientationType))]
    [XmlInclude(typeof (AbstractStyleSelectorType))]
    [XmlInclude(typeof (ResourceMapType))]
    [XmlInclude(typeof (LocationType))]
    [XmlInclude(typeof (AbstractSubStyleType))]
    [XmlInclude(typeof (RegionType))]
    [XmlInclude(typeof (AliasType))]
    [XmlInclude(typeof (AbstractViewType))]
    [XmlInclude(typeof (AbstractFeatureType))]
    [XmlInclude(typeof (AbstractGeometryType))]
    [XmlInclude(typeof (BasicLinkType))]
    [XmlInclude(typeof (PairType))]
    [XmlInclude(typeof (ImagePyramidType))]
    [XmlInclude(typeof (ScaleType))]
    [XmlInclude(typeof (LodType))]
    [XmlInclude(typeof (ViewVolumeType))]
    public class LatLonAltBoxType : AbstractLatLonBoxType
    {
        [XmlIgnore] private string __altitudeModeGroup;
        [XmlIgnore] private List<LatLonAltBoxObjectExtensionGroup> __LatLonAltBoxObjectExtensionGroup;
        [XmlIgnore] private List<string> __LatLonAltBoxSimpleExtensionGroup;
        [XmlIgnore] private double __maxAltitude;

        [XmlIgnore] public bool __maxAltitudeSpecified;
        [XmlIgnore] private double __minAltitude;

        [XmlIgnore] public bool __minAltitudeSpecified;

        public LatLonAltBoxType()
        {
            minAltitude = 0.0;
            maxAltitude = 0.0;
        }


        [XmlElement(ElementName = "minAltitude", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "double"
            , Namespace = Declarations.SchemaVersion)]
        public double minAltitude
        {
            get { return __minAltitude; }
            set
            {
                __minAltitude = value;
                __minAltitudeSpecified = true;
            }
        }


        [XmlElement(ElementName = "maxAltitude", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "double"
            , Namespace = Declarations.SchemaVersion)]
        public double maxAltitude
        {
            get { return __maxAltitude; }
            set
            {
                __maxAltitude = value;
                __maxAltitudeSpecified = true;
            }
        }

        [XmlElement(ElementName = "altitudeModeGroup", IsNullable = false, Form = XmlSchemaForm.Qualified,
            Namespace = Declarations.SchemaVersion)]
        public string altitudeModeGroup
        {
            get { return __altitudeModeGroup; }
            set { __altitudeModeGroup = value; }
        }

        [XmlElement(Type = typeof (string), ElementName = "LatLonAltBoxSimpleExtensionGroup", IsNullable = false,
            Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        public List<string> LatLonAltBoxSimpleExtensionGroup
        {
            get
            {
                if (__LatLonAltBoxSimpleExtensionGroup == null) __LatLonAltBoxSimpleExtensionGroup = new List<string>();
                return __LatLonAltBoxSimpleExtensionGroup;
            }
            set { __LatLonAltBoxSimpleExtensionGroup = value; }
        }

        [XmlElement(Type = typeof (LatLonAltBoxObjectExtensionGroup), ElementName = "LatLonAltBoxObjectExtensionGroup",
            IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        public List<LatLonAltBoxObjectExtensionGroup> LatLonAltBoxObjectExtensionGroup
        {
            get
            {
                if (__LatLonAltBoxObjectExtensionGroup == null)
                    __LatLonAltBoxObjectExtensionGroup = new List<LatLonAltBoxObjectExtensionGroup>();
                return __LatLonAltBoxObjectExtensionGroup;
            }
            set { __LatLonAltBoxObjectExtensionGroup = value; }
        }

        public new void MakeSchemaCompliant()
        {
            base.MakeSchemaCompliant();
        }
    }
}