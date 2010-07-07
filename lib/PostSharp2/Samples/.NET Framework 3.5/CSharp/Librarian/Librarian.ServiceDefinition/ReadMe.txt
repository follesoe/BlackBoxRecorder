
PostSharp.Samples.Librarian Service Definition
----------------------------


This project contains the public interface of the server.


Entities
........

An *entity* is an object that is stored atomically in database. Entities can
be composed of many objects, which are *composed* into the entity main object.
That is, the lifetime of the composed object is controlled by the entity
main object.

Entities should be derived from the class PostSharp.Samples.Librarian.Framework.Entity.

By definition, entities cannot contain other entities. However, entities
can refer to other entities using the PostSharp.Samples.Librarian.Framework.EntityRef<T> class.

They should respect the following rules:

- Entities and their members (composed objects) should be serializable.

- Entities and their members should be cloneable. Members should either
  be value types either should implement the ICloneable interface.


Entities have standardly no behaviors. Modifying an entity field does not
change it in the database. Instead, entities are used to be parts of messages
exchanged between the different parts of the software. In order to change
an entity, client layers have to go through the process layer. And the process
layer should invoke explicitely CRUD operations on the data layer in order
to update an entity.


Information Objects
...................

Information objects (classes whose name ends with the 'Info' prefix) are not
entities, but are parts of messages sent by the process layer to the client
layers. They are used to aggregate many entities in a single message.



Validation
...........

Entities should be able to validate themselves. However, they should not be
able to validate their bindings to other entities (which should be ensured
by the business layer).

For this purpose, entities can implement the Validate virtual method.

In order to add validation to fields, custom attributes derived from 
FieldValidationAttribute can be used (for instance the [Required] 
custom attribute). When this attribute is used, the Validate method does
not need to be implemented. Additionally, when this attribute is used,
fields are validated when they are updated.