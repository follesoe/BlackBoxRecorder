WinForms Client
---------------

This project contains a WinForms client for the PostSharp.Samples.Librarian application.

Client layers may only access entities through business processes 
(according to the Service Oriented Architecture style). When the client
layer modified an entity object, it modifies only the message that will
be passed to the business layer. It does modify the entity directly
in database.

Access to entities and business processes may raise exceptions derived
from the PostSharp.Samples.Librarian.Framework.ValidationException class. These are
normal exceptions that stem from the inputs of the user and the current
state of the systems. The correct behavior of the client is to display
a message box with the exception message (which is supposed to be
end-user friendly).

When a method should catch such exceptions and display this dialog message,
it can use the [ExceptionMessageBox] custom attribute instead of
hard coding the exception handler.

