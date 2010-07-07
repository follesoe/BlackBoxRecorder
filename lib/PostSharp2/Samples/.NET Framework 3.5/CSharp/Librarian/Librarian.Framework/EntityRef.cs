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
using System.Data.SqlTypes;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Librarian.Framework
{
    /// <summary>
    /// Reference to an entity.
    /// </summary>
    /// <typeparam name="T">Type of entities that can be references by the current type.</typeparam>
    /// <remarks>
    /// An <see cref="EntityRef{T}"/> is basically an <see cref="EntityKey{T}"/>, but it provides
    /// additional services for user code like automatic reference resolution, casting and so on.
    /// User code should always use entity references instead of entity keys.
    /// </remarks>
    /// <note>
    /// This class implements <see cref="ICloneable"/>. When a reference is cloned (instead of 'assigned'),
    /// the cached referenced entity is not copied. This allows the cloned object graph to be completely
    /// disconnected from the source graph.
    /// </note>
    [Serializable]
    public struct EntityRef<T> : IXmlSerializable, INullable, IEquatable<T>, IEquatable<EntityRef<T>>, ICloneable
        where T : BaseEntity
    {
        /// <summary>
        /// An empty instance.
        /// </summary>
        public static readonly EntityRef<T> Empty = new EntityRef<T>();

        private EntityKey entityKey;

        [NonSerialized] private T target;

        /// <summary>
        /// Initializes a new <see cref="EntityRef"/> from an entity.
        /// </summary>
        /// <param name="target">The entity that the new instance should reference.</param>
        public EntityRef( T target )
        {
            if ( target == null )
                throw new ArgumentNullException( "target" );

            this.entityKey = target.EntityKey;
            this.target = target;
        }

        /// <summary>
        /// Initializes a new <see cref="EntityRef"/> from an <see cref="EntityKey"/>.
        /// </summary>
        /// <param name="entityKey">An <see cref="EntityKey"/> (eventually null).</param>
        public EntityRef( EntityKey entityKey )
        {
            this.entityKey = entityKey;
            this.target = null;
        }

        /// <summary>
        /// Determines whether the current reference is null.
        /// </summary>
        /// <remarks>
        /// <b>false</b> if the current instance does reference an entity, <b>true</b> otherwise.
        /// </remarks>
        public bool IsNull { get { return this.entityKey.IsNull; } }


        /// <summary>
        /// Gets the entity key. 
        /// </summary>
        /// <remarks>
        /// This property is for the data layer. User code should normally not use it.
        /// </remarks>
        public EntityKey EntityKey { get { return this.entityKey; } }

        /// <summary>
        /// Gets the entity referenced to by the current reference.
        /// </summary>
        /// <remarks>
        /// The current structure caches the referenced entity. Uses the <see cref="GetVanilla"/>
        /// method to get a fresh entity directly from the source.
        /// </remarks>
        public T Entity
        {
            get
            {
                if ( this.target == null )
                {
                    this.target = this.GetVanillaEntity();
                }
                return this.target;
            }
        }

        /// <summary>
        /// Gets a 'vanilla', or fresh entity, directly from the source.
        /// </summary>
        /// <remarks>
        /// The 'source' entity depends on how the <see cref="Entity.EntityResolver"/>
        /// is configured. In the business layer, the source is the data layer. In
        /// the client layers, the source is the business layer.
        /// </remarks>
        /// <returns></returns>
        public T GetVanillaEntity()
        {
            return (T) Framework.BaseEntity.EntityResolver.GetEntity( this.entityKey );
        }


        /// <summary>
        /// Gets a string that described the current reference.
        /// </summary>
        /// <returns>A string that described the current reference</returns>
        public override string ToString()
        {
            return string.Format( "EntityRef<{0}> {1}", typeof(T).Name, this.entityKey.ToString() );
        }

        #region Equality

        public bool Equals( T target )
        {
            if ( target == null )
                throw new ArgumentNullException( "target" );
            return this.entityKey.Equals( target.EntityKey );
        }

        public bool Equals( EntityRef<T> other )
        {
            return this.entityKey.Equals( other.entityKey );
        }

        public override bool Equals( object obj )
        {
            if ( obj is T )
            {
                return this.Equals( (T) obj );
            }
            else if ( obj is EntityRef<T> )
            {
                return this.Equals( (EntityRef<T>) obj );
            }
            else
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            return this.entityKey.GetHashCode();
        }


        public static bool operator ==( EntityRef<T> left, EntityRef<T> right )
        {
            return left.Equals( right );
        }

        public static bool operator !=( EntityRef<T> left, EntityRef<T> right )
        {
            return !left.Equals( right );
        }

        public static implicit operator EntityRef<T>( T target )
        {
            return new EntityRef<T>( target );
        }

        public static implicit operator EntityRef<BaseEntity>(EntityRef<T> target)
        {
            return new EntityRef<BaseEntity>( target.entityKey );
        }

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.ReadXml( XmlReader reader )
        {
            ( (IXmlSerializable) this.entityKey ).ReadXml( reader );
        }

        void IXmlSerializable.WriteXml( XmlWriter writer )
        {
            ( (IXmlSerializable) this.entityKey ).WriteXml( writer );
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Gets a copy of the current reference that does <b>not</b> contain the cached referenced entity.
        /// </summary>
        /// <returns>An <see cref="EntityRef{T}"/>.</returns>
        public EntityRef<T> Clone()
        {
            return new EntityRef<T>( this.entityKey );
        }

        /// <summary>
        /// Gets a copy of the current reference that does <b>not</b> contain the cached referenced entity.
        /// </summary>
        /// <returns>An <see cref="EntityRef{T}"/>.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}