#region Released to Public Domain by Gael Fraiteur
/*----------------------------------------------------------------------------*
 *   This file is part of samples of PostSharp.                                *
 *                                                                             *
 *   This sample is free software: you have an unlimited right to              *
 *   redistribute it and/or modify it.                                         *
 *                                                                             *
 *   This sample is distributed in the hope that it will be useful,            *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of            *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                      *
 *                                                                             *
 *----------------------------------------------------------------------------*/
#endregion

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Librarian.Framework
{
    /// <summary>
    /// Primary key of an entity.
    /// </summary>
    [Serializable]
    public struct EntityKey : IEquatable<EntityKey>, IXmlSerializable
    {
        private string key;

        /// <summary>
        /// Initializes a new <see cref="EntityKey"/>.
        /// </summary>
        /// <param name="key">key</param>
        public EntityKey( string key )
        {
            this.key = key;
        }

        /// <summary>
        /// Determines whether the current key is null (i.e. whether it refers to no entity).
        /// </summary>
        public bool IsNull { get { return string.IsNullOrEmpty( key ); } }

        /// <summary>
        /// Gets a string representing the current <see cref="EntityKey"/>.
        /// </summary>
        /// <returns>A string representing the current object.</returns>
        public override string ToString()
        {
            return this.key;
        }

        #region Equality

        /// <summary>
        /// Determines whether the current <see cref="EntityKey"/> is equal to another.
        /// </summary>
        /// <param name="other">Another <see cref="EntityKey"/>.</param>
        /// <returns><b>true</b> if both objects are equal, otherwise <b>false</b>.</returns>
        public bool Equals( EntityKey other )
        {
            return string.Compare( this.key, other.key, StringComparison.InvariantCulture ) == 0;
        }

        /// <summary>
        /// Determines whether the current <see cref="EntityKey"/> is equal to another object.
        /// </summary>
        /// <param name="obj">Another object.</param>
        /// <returns><b>true</b> if both objects are equal, otherwise <b>false</b>.</returns>
        public override bool Equals( object obj )
        {
            if ( obj == null || !( obj is EntityKey ) )
            {
                return false;
            }
            else
            {
                return this.Equals( (EntityKey) obj );
            }
        }

        /// <summary>
        /// Get the hash code of this <see cref="EntityKey"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.key == null ? 0 : this.key.GetHashCode();
        }

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.ReadXml( XmlReader reader )
        {
            this.key = reader.ReadElementContentAsString();
        }

        void IXmlSerializable.WriteXml( XmlWriter writer )
        {
            if ( this.key != null )
            {
                writer.WriteValue( this.key );
            }
        }

        #endregion
    }
}