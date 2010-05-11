Business Processes
------------------

By design decision, client layers get access to entities and modify these entities only 
through business processes.

Typical activities of a business process is to:

- Execute business rules validating this current business process instance, using
  the Business Rules Manager (PostSharp.Samples.Librarian.Framework.BusinessRulesManager).

- Execute CRUD operations on entities (using the data layer).

- Eventually invoke other business processes.


Business processes should execute in a transaction. The [Transaction] custom attribute
should be applied to any method doing write access to the data layer. When a custom
attribute cannot be applied, business processes can use the TransactionScope class
of the System.Transactions namespace.



Business Rules
--------------

The namespace PostSharp.Samples.Librarian.BusinessRules contains *validation* business rules. Validation
business rules are simply conditions that can evaluate to true (if the rule is fulfilled) or 
false (if the rule is broken).


Validation business rules are invoked by the process layer. The
process layer does not know explicitely which rules should be invoked.
Instead, it raises a 'validation event' with a parameter (typically
the entity being modified).

Business rules should be derived from the class PostSharp.Samples.Librarian.Framework.BusinessRule.
Additionally, they should indicate to which validation event they wish
to react. They do this using the [BusinessRuleAppliesTo] custom attribute.

Business rules are managed by the class PostSharp.Samples.Librarian.Framework.BusinessRulesManager.
User code does not address directly busines rules.



Session and Session Factory
---------------------------

In order to get a 'Business Process' interface, the client should:

1. Get a Session Factory (typically by using explicitely Activator.GetObject).

2. Use this Session Factory to get a session by submitting credentials.

3. The session objects gets instances of business services.


We bind services to sessions in order to manage authentication and context information.
This approach is not ideal for internet applications but in our case this is not
a problem.