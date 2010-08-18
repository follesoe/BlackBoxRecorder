require 'albacore'

task :default => :build

task :build => [:clean, :msbuild, :tests, :copy, :zip]

desc "Cleaning build directory"
task :clean do
	rm_rf("build")
end

desc "Building BlackBoxRecorder"
msbuild :msbuild do |msb|
	msb.properties :configuration => :Release
	msb.targets :Clean, :Build
	msb.solution  = "BlackBox.sln"
end

desc "Running tests for BlackBox"
xunit :tests do |xunit|
	xunit.path_to_command = "lib/xunit/xunit.console.exe"
	xunit.assembly = "BlackBox.Tests/bin/release/BlackBox.Tests.dll"	
end

directory "build"

desc "Copying assemblies to build directory"
task :copy do
	mkdir "build"
	cp "License.txt", "build/License.txt"
	cp_r "BlackBox/bin/release/", "build/BlackBoxRecorder"
	cp_r "lib/PostSharp2/", "build/PostSharp2"
end

desc "Creating zip archive of built assemblies"
zip do |zip|
	zip.directories_to_zip "build"
	zip.output_file = "BlackBoxRecorder.zip"
end