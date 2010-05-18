BlackBoxRecorder - Characterization test generator
===================================================
BlackBoxRecorder is a tool that uses [AOP](http://en.wikipedia.org/wiki/Aspect-oriented_programming) 
techniques to automatically record and generate [characterization tests](http://en.wikipedia.org/wiki/Characterization_test) 
for legacy code (code without tests). You apply a **[Recording]**-attribute on the method you want to generate tests for, 
and a **[Dependency]**-attribute on any type calling external resources such as file system, web services and databases.

BlackBoxRecorder works best for code following the [anemic domain model anti-pattern](http://en.wikipedia.org/wiki/Anemic_Domain_Model). 
This pattern is often found in Java/.NET n-layered enterprise applications, where you have a business logic layer, a data access layer, 
and a shared set of dumb entity objects. The business logic layer operates directly on the entities, manipulating their values, or by 
creating new entity objects. Using BlackBoxRecorder you can get this kind of code under test by marking your business logic with 
recording-attribute, and your service agents or data access classes with the dependency-attribute.

Each method invocation of a method marked with the recording-attribute will be stored in an XML file. The XML file contains information 
about the method, copies of input parameters, output parameters and return values. If the recorded method invokes any method on a type 
marked with the dependency-attribute, the return values of the dependency will also be recorded in the same XML file.

For each method being recorded you will also get a generated unit tat that will play back the recording against the method, 
and compare the return values against the recorded return value by using reflection to walk the full object graph. 
The test will serve as a change detector (which in essence is what characterization test is), notifying you of any 
change resulting in change in the returned value. If the change was intended you can configure the generated test 
to ignore that property on next run. 

The generated tests supports automatic stubbing/mocking of the types marked with the dependency-attribute. 
When the test is played back, the dependency will return recorded values instead of calling the external resource. 
You do not have to change your code for this to work and even static methods are supported.