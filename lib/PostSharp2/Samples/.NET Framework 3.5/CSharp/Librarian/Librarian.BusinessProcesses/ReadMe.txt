Namespace PostSharp.Samples.Librarian.BusinessProcesses
-------------------------------------

This namespace contains business processes. By design decision, client layers 
get access to entities and modify these entities only through business processes.

Typical activities of a business process is to:

- Execute business rules validating this current business process instance, using
  the Business Rules Manager (PostSharp.Samples.Librarian.Framework.BusinessRulesManager).

- Execute CRUD operations on entities (using the data layer).

- Eventually invoke other business processes.


Business processes should execute in a transaction. The [Transaction] custom attribute
should be applied to any method doing write access to the data layer. When a custom
attribute cannot be applied, business processes can use the TransactionScope class
of the System.Transactions namespace.

