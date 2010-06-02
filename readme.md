BlackBoxRecorder - Characterization test generator
===================================================
BlackBoxRecorder is a tool that uses [AOP](http://en.wikipedia.org/wiki/Aspect-oriented_programming) 
techniques to automatically record and generate [characterization tests](http://en.wikipedia.org/wiki/Characterization_test) 
for legacy code (code without tests). You apply a **[Recording]**-attribute on the method you want to generate tests for, 
and a **[Dependency]**-attribute on any type calling external resources such as file system, web services and databases.

Read more about the project in the [Wiki](http://wiki.github.com/follesoe/BlackBoxRecorder/) 
or on the [project home page](http://follesoe.github.com/BlackBoxRecorder/).

## Important Information about PostSharp Dependency ##
BlackBoxRecorder uses PostSharp for aspect oriented programming. For licensing reasons we cannot include the PostSharp 
binaries in the source repository so it has to be [downloaded separately](http://www.sharpcrafters.com/downloads/postsharp-2.0/ctp-5). 

You can download the [ZIP package](http://www.sharpcrafters.com/downloads/postsharp-2.0/ctp-5/PostSharp-2.0.5.1204.zip) 
and simply extract PostSharp to the folder lib\PostSharp, or you can use the 
[MSI installer](http://www.sharpcrafters.com/downloads/postsharp-2.0/ctp-5/PostSharp-2.0.5.1204.exe) to install 
PostSharp on your system. The project got a custom build target that will look for PostSharp either in lib\PostSharp, 
or the default installation directory. 

Other than PostSharp necessary dependencies to build and run BlackBoxRecorder should be included in the repository.