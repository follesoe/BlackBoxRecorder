// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace BlackBox.FluentPath {
    public class PathCollection : IEnumerable<Path> 
    {
        private readonly IEnumerable<string> _paths;
        private readonly PathCollection _previousPaths;

        /// <summary>
        /// Creates a collection of paths from a list of path strings.
        /// Avoid using directly, and use one of the methods on Path instead.
        /// </summary>
        /// <param name="paths">The list of path strings.</param>
        public PathCollection(IEnumerable<string> paths) : this(paths, null) 
        {
        }

        /// <summary>
        /// Creates a collection of paths from a list of path strings and a previous list of path strings.
        /// Avoid using directly, and use one of the methods on Path instead.
        /// </summary>
        /// <param name="paths">The list of path strings in the set.</param>
        /// <param name="previousPaths">The list of path strings in the previous set.</param>
        public PathCollection(IEnumerable<string> paths, PathCollection previousPaths) 
        {
            _paths = paths;
            _previousPaths = previousPaths;
        }

        public IEnumerator<Path> GetEnumerator() 
        {
            return new PathEnumerator(_paths);
        }

        IEnumerator IEnumerable.GetEnumerator() 
        {
            return new PathEnumerator(_paths);
        }

        /// <summary>
        /// Changes the path on each path in the set.
        /// Does not do any physical change to the file system.
        /// </summary>
        /// <param name="extensionTransformation">A function that maps each path to an extension.</param>
        /// <returns>The set</returns>
        public PathCollection ChangeExtension(Func<Path, string> extensionTransformation) 
        {
            var result = new HashSet<string>();
            foreach (var path in _paths) 
            {
                var p = new Path(path);
                result.Add(p.ChangeExtension(extensionTransformation(p)).ToString());
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Changes the path on each path in the set.
        /// Does not do any physical change to the file system.
        /// </summary>
        /// <param name="newExtension">The new extension.</param>
        /// <returns>The set</returns>
        public PathCollection ChangeExtension(string newExtension) 
        {
            return ChangeExtension(p => newExtension);
        }

        /// <summary>
        /// Combines each path in the set with the specified file or directory name.
        /// Does not do any physical change to the file system.
        /// </summary>
        /// <param name="directoryNameGenerator">A function that maps each path to a file or directory name.</param>
        /// <returns>The set</returns>
        public PathCollection Combine(Func<Path, string> directoryNameGenerator) 
        {
            var result = new HashSet<string>();
            foreach (var path in _paths) 
            {
                var p = new Path(path);
                if (p.IsDirectory) 
                {
                    result.Add(p.Combine(directoryNameGenerator(p)).ToString());
                }
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Combines each path in the set with the specified file or directory name.
        /// Does not do any physical change to the file system.
        /// </summary>
        /// <param name="directoryName">A file or directory name.</param>
        /// <returns>The set</returns>
        public PathCollection Combine(string directoryName) 
        {
            return Combine(p => directoryName);
        }

                /// <summary>
        /// Does a copy of all files and directories in the set.
        /// </summary>
        /// <param name="pathMapping">
        /// A function that determines the destination path for each source path.
        /// If the function returns a null path, the file or directory is not copied.
        /// </param>
        /// <returns>The set</returns>
        public PathCollection Copy(Func<Path, Path> pathMapping)
        {
            return Copy(pathMapping, false, false);    
        }

        /// <summary>
        /// Does a copy of all files and directories in the set.
        /// </summary>
        /// <param name="pathMapping">
        /// A function that determines the destination path for each source path.
        /// If the function returns a null path, the file or directory is not copied.
        /// </param>
        /// <param name="overwrite">True if destination files should be overwritten.</param>
        /// <param name="recursive">True if the copy should be deep and go into subdirectories recursively.</param>
        /// <returns>The set</returns>
        public PathCollection Copy(Func<Path, Path> pathMapping, bool overwrite, bool recursive) 
        {
            var result = new HashSet<string>();
            foreach (var path in _paths) 
            {
                var source = new Path(path);
                var dest = pathMapping(source);
                if (dest == null) continue;

                source.Copy(dest, overwrite, recursive);
                result.Add(dest.ToString());
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Creates subdirectories for each directory.
        /// </summary>
        /// <param name="directoryNameGenerator">
        /// A function that returns the new directory name for each path.
        /// If the function returns null, no directory is created.
        /// </param>
        /// <returns>The set</returns>
        public PathCollection CreateDirectory(Func<Path, string> directoryNameGenerator) {
            var result = new HashSet<string>();
            foreach (var path in _paths) {
                var p = new Path(path);
                var dest = directoryNameGenerator(p);
                if (dest != null) {
                    result.Add(
                        p.CreateSubDirectory(dest).ToString());
                }
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Creates subdirectories for each directory.
        /// </summary>
        /// <param name="directoryNameGenerator">
        /// A function that returns the new directory name for each path.
        /// If the function returns null, no directory is created.
        /// </param>
        /// <returns>The set</returns>
        public PathCollection CreateDirectory(Func<Path, Path> directoryNameGenerator) {
            var result = new HashSet<string>();
            foreach (var path in _paths) {
                var p = new Path(path);
                var dest = directoryNameGenerator(p);
                if (dest != null) {
                    result.Add(
                        Path.CreateDirectory(dest).ToString());
                }
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Creates subdirectories for each directory.
        /// </summary>
        /// <param name="directoryName">The name of the new directory.</param>
        /// <returns>The set</returns>
        public PathCollection CreateDirectory(string directoryName) {
            return CreateDirectory(p => directoryName);
        }

        /// <summary>
        /// Decrypts all files in the set.
        /// </summary>
        /// <returns>The set</returns>
        public PathCollection Decrypt() {
            foreach (var path in _paths) {
                new Path(path).Decrypt();
            }
            return this;
        }

        /// <summary>
        /// Deletes all files and folders in the set, including non-empty directories if recursive is true.
        /// </summary>
        /// <param name="recursive">If true, also deletes the contents of directories. Default is false.</param>
        /// <returns>The set of parent directories of all deleted file system entries.</returns>
        public PathCollection Delete(bool recursive) {
            var result = new HashSet<string>();
            foreach (var path in _paths) {
                var p = new Path(path);
                result.Add(p.Parent.ToString());
                p.Delete(recursive);
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Encrypts all files in the set.
        /// </summary>
        /// <returns>The set</returns>
        public PathCollection Encrypt() {
            foreach (var path in _paths) {
                new Path(path).Encrypt();
            }
            return this;
        }

        /// <summary>
        /// Filters the set according to the predicate.
        /// </summary>
        /// <param name="predicate">A predicate that returns true for the entries that must be in the returned set.</param>
        /// <returns>The filtered set.</returns>
        public PathCollection Where(Predicate<Path> predicate) {
            var result = new HashSet<string>();
            foreach (var path in _paths) {
                if (predicate(new Path(path))) {
                    result.Add(path);
                }
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Executes an action for each file or folder in the set.
        /// </summary>
        /// <param name="action">An action that takes the path of each entry as its parameter.</param>
        /// <returns>The set</returns>
        public PathCollection ForEach(Action<Path> action) {
            foreach(var path in _paths) {
                action(new Path(path));
            }
            return this;
        }

        /// <summary>
        /// Gets all the subdirectories of folders in the set that match the provided pattern and using the provided options.
        /// </summary>
        /// <param name="searchPattern">A search pattern such as "*.jpg". Default is "*".</param>
        /// <param name="recursive">True if subdirectories should also be searched recursively. Default is false.</param>
        /// <returns>The set of matching subdirectories.</returns>
        public PathCollection Directories(string searchPattern, bool recursive) {
            if(string.IsNullOrEmpty(searchPattern))
                searchPattern = "*";

            var result = new HashSet<string>();
            foreach (var path in _paths) {
                foreach (var dir in Directory.GetDirectories(path, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {
                    result.Add(dir);
                }
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Gets all the files under the directories of the set that match the pattern, going recursively into subdirectories if recursive is set to true.
        /// </summary>
        /// <param name="searchPattern">A search pattern such as "*.jpg". Default is "*".</param>
        /// <param name="recursive">If true, subdirectories are explored as well. Default is false.</param>
        /// <returns>The set of files that match the pattern.</returns>
        public PathCollection Files(string searchPattern, bool recursive) {
            if(string.IsNullOrEmpty(searchPattern))
                searchPattern = "*";

            var result = new HashSet<string>();
            foreach (var path in _paths) {
                foreach (var dir in Directory.GetFiles(path, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {
                    result.Add(dir);
                }
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Gets all the files and subdirectories under the directories of the set that match the pattern, going recursively into subdirectories if recursive is set to true.
        /// </summary>
        /// <param name="searchPattern">A search pattern such as "*.jpg". Default is "*".</param>
        /// <param name="recursive">If true, subdirectories are explored as well. Default is false.</param>
        /// <returns>The set of files and folders that match the pattern.</returns>
        public PathCollection FileSystemEntries(string searchPattern, bool recursive) {
            searchPattern = DefaultSearchPatternIfEmpty(searchPattern);
            var result = new HashSet<string>();
            foreach (var path in _paths) {
                foreach (var dir in DirectoryExtensions.GetFileSystemEntries(path, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {
                    result.Add(dir);
                }
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Maps all the paths in the set to a new set of paths using the provided mapping function.
        /// </summary>
        /// <param name="pathMapping">A function that takes a path and returns a transformed path.</param>
        /// <returns>The mapped set.</returns>
        public PathCollection Map(Func<Path, Path> pathMapping) {
            var result = new HashSet<string>();
            foreach(var path in _paths) {
                result.Add(pathMapping(new Path(path)).ToString());
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Moves all the files and folders in the set to new locations as specified by the mapping function.
        /// </summary>
        /// <param name="pathMapping">The function that maps from the current path to the new one.</param>
        /// <returns>The moved set.</returns>
        public PathCollection Move(Func<Path, Path> pathMapping) {
            var result = new HashSet<string>();
            foreach (var path in _paths) {
                var source = new Path(path);
                var dest = pathMapping(source);
                source.Move(dest);
                result.Add(dest.ToString());
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Opens all the files in the set and hands them to the provided action.
        /// </summary>
        /// <param name="action">The action to perform on the open files.</param>
        /// <param name="mode">The FileMode to use. Default is OpenOrCreate.</param>
        /// <param name="access">The FileAccess to use. Default is ReadWrite.</param>
        /// <param name="share">The FileShare to use. Default is None.</param>
        /// <returns>The set</returns>
        public PathCollection Open(Action<FileStream> action, FileMode mode, FileAccess access, FileShare share) {
            foreach (var path in _paths) {
                using (var stream = File.Open(path, mode, access, share)) {
                    action(stream);
                }
            }
            return this;
        }

        /// <summary>
        /// Opens all the files in the set and hands them to the provided action.
        /// </summary>
        /// <param name="action">The action to perform on the open streams.</param>
        /// <param name="mode">The FileMode to use. Default is OpenOrCreate.</param>
        /// <param name="access">The FileAccess to use. Default is ReadWrite.</param>
        /// <param name="share">The FileShare to use. Default is None.</param>
        /// <returns>The set</returns>
        public PathCollection Open(Action<FileStream, Path> action, FileMode mode, FileAccess access, FileShare share) {
            foreach (var path in _paths) {
                using (var stream = File.Open(path, mode, access, share)) {
                    action(stream, new Path(path));
                }
            }
            return this;
        }

        /// <summary>
        /// The previous set, from which the current one was created.
        /// </summary>
        /// <returns>The previous set.</returns>
        public PathCollection Previous() {
            return _previousPaths;
        }


        /// <summary>
        /// Reads all text in files in the set and hands the results to the provided action.
        /// </summary>
        /// <param name="action">An action that takes the contents of the file.</param>
        /// <returns>The set</returns>
        public PathCollection Read(Action<string> action) {
            foreach (var path in _paths) {
                action(File.ReadAllText(path));
            }
            return this;
        }

        /// <summary>
        /// Reads all text in files in the set and hands the results to the provided action.
        /// </summary>
        /// <param name="action">An action that takes the contents of the file.</param>
        /// <param name="encoding">The encoding to use when reading the file.</param>
        /// <returns>The set</returns>
        public PathCollection Read(Action<string> action, Encoding encoding) {
            foreach (var path in _paths) {
                action(File.ReadAllText(path, encoding));
            }
            return this;
        }

        /// <summary>
        /// Reads all text in files in the set and hands the results to the provided action.
        /// </summary>
        /// <param name="action">An action that takes the contents of the file and its path.</param>
        /// <returns>The set</returns>
        public PathCollection Read(Action<string, Path> action) {
            foreach (var path in _paths) {
                action(File.ReadAllText(path), new Path(path));
            }
            return this;
        }

        /// <summary>
        /// Reads all text in files in the set and hands the results to the provided action.
        /// </summary>
        /// <param name="action">An action that takes the contents of the file and its path.</param>
        /// <param name="encoding">The encoding to use when reading the file.</param>
        /// <returns>The set</returns>
        public PathCollection Read(Action<string, Path> action, Encoding encoding) {
            foreach (var path in _paths) {
                action(File.ReadAllText(path, encoding), new Path(path));
            }
            return this;
        }

        /// <summary>
        /// Reads all the bytes in a file and hands them to the provided action.
        /// </summary>
        /// <param name="action">An action that takes an array of bytes.</param>
        /// <returns>The set</returns>
        public PathCollection ReadBytes(Action<byte[]> action) {
            foreach (var path in _paths) {
                action(File.ReadAllBytes(path));
            }
            return this;
        }

        /// <summary>
        /// Reads all the bytes in a file and hands them to the provided action.
        /// </summary>
        /// <param name="action">An action that takes an array of bytes and a path.</param>
        /// <returns>The set</returns>
        public PathCollection ReadBytes(Action<byte[], Path> action) {
            foreach (var path in _paths) {
                action(File.ReadAllBytes(path), new Path(path));
            }
            return this;
        }

        /// <summary>
        /// Sets the access control security on all files and directories in the set.
        /// </summary>
        /// <param name="security">The security to apply.</param>
        /// <returns>The set</returns>
        public PathCollection AccessControl(FileSystemSecurity security) {
            return AccessControl(p => security);
        }

        /// <summary>
        /// Sets the access control security on all files and directories in the set.
        /// </summary>
        /// <param name="securityFunction">A function that returns the security for each path.</param>
        /// <returns>The set</returns>
        public PathCollection AccessControl(Func<Path, FileSystemSecurity> securityFunction) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.SetAccessControl(securityFunction(p));
            }
            return this;
        }

        /// <summary>
        /// Sets attributes on all files in the set.
        /// </summary>
        /// <param name="attributes">The attributes to set.</param>
        /// <returns>The set</returns>
        public PathCollection Attributes(FileAttributes attributes) {
            return Attributes(p => attributes);
        }

        /// <summary>
        /// Sets attributes on all files in the set.
        /// </summary>
        /// <param name="attributeFunction">A function that gives the attributes to set for each path.</param>
        /// <returns>The set</returns>
        public PathCollection Attributes(Func<Path, FileAttributes> attributeFunction) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.SetAttributes(attributeFunction(p));
            }
            return this;
        }

        /// <summary>
        /// Sets the creation time across the set.
        /// </summary>
        /// <param name="creationTime">The time to set.</param>
        /// <returns>The set</returns>
        public PathCollection CreationTime(DateTime creationTime) {
            return CreationTime(p => creationTime);
        }

        /// <summary>
        /// Sets the creation time across the set.
        /// </summary>
        /// <param name="creationTimeFunction">A function that returns the new creation time for each path.</param>
        /// <returns>The set</returns>
        public PathCollection CreationTime(Func<Path, DateTime> creationTimeFunction) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.SetCreationTime(creationTimeFunction(p));
            }
            return this;
        }

        /// <summary>
        /// Sets the UTC creation time across the set.
        /// </summary>
        /// <param name="creationTime">The time to set.</param>
        /// <returns>The set</returns>
        public PathCollection CreationTimeUtc(DateTime creationTimeUtc) {
            return CreationTimeUtc(p => creationTimeUtc);
        }

        /// <summary>
        /// Sets the UTC creation time across the set.
        /// </summary>
        /// <param name="creationTimeFunction">A function that returns the new time for each path.</param>
        /// <returns>The set</returns>
        public PathCollection CreationTimeUtc(Func<Path, DateTime> creationTimeFunctionUtc) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.SetCreationTimeUtc(creationTimeFunctionUtc(p));
            }
            return this;
        }

        /// <summary>
        /// Sets the last access time across the set.
        /// </summary>
        /// <param name="creationTime">The time to set.</param>
        /// <returns>The set</returns>
        public PathCollection LastAccessTime(DateTime lastAccessTime) {
            return LastAccessTime(p => lastAccessTime);
        }

        /// <summary>
        /// Sets the last access time across the set.
        /// </summary>
        /// <param name="creationTimeFunction">A function that returns the new time for each path.</param>
        /// <returns>The set</returns>
        public PathCollection LastAccessTime(Func<Path, DateTime> lastAccessTimeFunction) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.SetLastAccessTime(lastAccessTimeFunction(p));
            }
            return this;
        }

        /// <summary>
        /// Sets the UTC last access time across the set.
        /// </summary>
        /// <param name="creationTime">The time to set.</param>
        /// <returns>The set</returns>
        public PathCollection LastAccessTimeUtc(DateTime lastAccessTimeUtc) {
            return LastAccessTimeUtc(p => lastAccessTimeUtc);
        }

        /// <summary>
        /// Sets the UTC last access time across the set.
        /// </summary>
        /// <param name="creationTimeFunction">A function that returns the new time for each path.</param>
        /// <returns>The set</returns>
        public PathCollection LastAccessTimeUtc(Func<Path, DateTime> lastAccessTimeFunctionUtc) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.SetLastAccessTimeUtc(lastAccessTimeFunctionUtc(p));
            }
            return this;
        }

        /// <summary>
        /// Sets the last write time across the set.
        /// </summary>
        /// <param name="creationTime">The time to set.</param>
        /// <returns>The set</returns>
        public PathCollection LastWriteTime(DateTime lastWriteTime) {
            return LastWriteTime(p => lastWriteTime);
        }

        /// <summary>
        /// Sets the last write time across the set.
        /// </summary>
        /// <param name="creationTimeFunction">A function that returns the new time for each path.</param>
        /// <returns>The set</returns>
        public PathCollection LastWriteTime(Func<Path, DateTime> lastWriteTimeFunction) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.SetLastWriteTime(lastWriteTimeFunction(p));
            }
            return this;
        }

        /// <summary>
        /// Sets the UTC last write time across the set.
        /// </summary>
        /// <param name="creationTime">The time to set.</param>
        /// <returns>The set</returns>
        public PathCollection LastWriteTimeUtc(DateTime lastWriteTimeUtc) {
            return LastWriteTimeUtc(p => lastWriteTimeUtc);
        }

        /// <summary>
        /// Sets the UTC last write time across the set.
        /// </summary>
        /// <param name="creationTimeFunction">A function that returns the new time for each path.</param>
        /// <returns>The set</returns>
        public PathCollection LastWriteTimeUtc(Func<Path, DateTime> lastWriteTimeFunctionUtc) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.SetLastWriteTimeUtc(lastWriteTimeFunctionUtc(p));
            }
            return this;
        }

        /// <summary>
        /// Goes up one level on each path in the set.
        /// </summary>
        /// <returns>The new set</returns>
        public PathCollection Up() {
            return Up(1);
        }

        /// <summary>
        /// Goes up the specified number of levels on each path in the set.
        /// Never goes above the root of the drive.
        /// </summary>
        /// <param name="levels">The number of levels to go up.</param>
        /// <returns>The new set</returns>
        public PathCollection Up(int levels) {
            var result = new HashSet<string>();
            foreach (var path in _paths) {
                result.Add(new Path(path).Up(levels).ToString());
            }
            return new PathCollection(result, this);
        }

        /// <summary>
        /// Writes to all files in the set using UTF8.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="append">True if the text should be appended to the existing contents. Default is false.</param>
        /// <returns>The set</returns>
        public PathCollection Write(string text, bool append) {
            return Write(text, Encoding.UTF8, append);
        }

        /// <summary>
        /// Writes to all files in the set.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="append">True if the text should be appended to the existing contents. Default is false.</param>
        /// <returns>The set</returns>
        public PathCollection Write(string text, Encoding encoding, bool append) {
            return Write(p => text, encoding, append);
        }

        /// <summary>
        /// Writes to all files in the set.
        /// </summary>
        /// <param name="textFunction">A function that returns the text to write for each path.</param>
        /// <param name="append">True if the text should be appended to the existing contents. Default is false.</param>
        /// <returns>The set</returns>
        public PathCollection Write(Func<Path, string> textFunction, bool append) {
            return Write(textFunction, Encoding.UTF8, append);
        }

        /// <summary>
        /// Writes to all files in the set.
        /// </summary>
        /// <param name="textFunction">A function that returns the text to write for each path.</param>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="append">True if the text should be appended to the existing contents. Default is false.</param>
        /// <returns>The set</returns>
        public PathCollection Write(Func<Path, string> textFunction, Encoding encoding, bool append) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.Write(textFunction(p), encoding, append);
            }
            return this;
        }

        /// <summary>
        /// Writes to all files in the set.
        /// </summary>
        /// <param name="bytes">The byte array to write.</param>
        /// <returns>The set</returns>
        public PathCollection WriteBytes(byte[] bytes) {
            return WriteBytes(p => bytes);
        }

        /// <summary>
        /// Writes to all files in the set.
        /// </summary>
        /// <param name="byteFunction">A function that returns a byte array to write for each path.</param>
        /// <returns>The set</returns>
        public PathCollection WriteBytes(Func<Path, byte[]> byteFunction) {
            foreach (var path in _paths) {
                var p = new Path(path);
                p.WriteBytes(byteFunction(p));
            }
            return this;
        }

        private string DefaultSearchPatternIfEmpty(string pattern)
        {
            return string.IsNullOrEmpty(pattern) ? "*" : pattern;
        }
    }
}


