// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.AccessControl;
using System.Text;
using System.IO;

namespace BlackBox.FluentPath 
{

    [TypeConverter(typeof(PathConverter))]
    public class Path 
    {
        private readonly string _path;
        private readonly Path _previousPath;

        // Constructors

        /// <summary>
        /// Creates a new path from a string.
        /// You may also create a new path object from the Get method.
        /// </summary>
        /// <param name="path">The string representation of the path.</param>
        public Path(string path) : this(path, null) {
        }

        /// <summary>
        /// Creates a new path from a string.
        /// You may also create a new path object from the Get method.
        /// </summary>
        /// <param name="path">The string representation of the path.</param>
        /// <param name="previousPath">The previous path.</param>
        public Path(string path, Path previousPath) 
        {
            if (path == null) throw new InvalidOperationException("Path can't be null.");
            _path = path;
            _previousPath = previousPath;
        }

        // Conversion operators

        public static implicit operator string(Path path) 
        {
            return path.ToString();
        }

        public static implicit operator Path(string path) 
        {
            return new Path(path);
        }

        // Path properties

        /// <summary>
        /// The name of the directory for that path.
        /// This is the string representation of the parent directory path.
        /// </summary>
        public string DirectoryName 
        {
            get { return System.IO.Path.GetDirectoryName(_path); }
        }

        /// <summary>
        /// The extension for this path, including the ".".
        /// </summary>
        public string Extension 
        {
            get { return System.IO.Path.GetExtension(_path); }
        }

        /// <summary>
        /// The filename or folder name for this path, including the extension.
        /// </summary>
        public string FileName 
        {
            get { return System.IO.Path.GetFileName(_path); }
        }

        /// <summary>
        /// The filename or folder name for this path, without the extension.
        /// </summary>
        public string FileNameWithoutExtension 
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(_path); }
        }

        /// <summary>
        /// The fully qualified path string for this path.
        /// </summary>
        public string FullPath 
        {
            get { return System.IO.Path.GetFullPath(_path); }
        }

        /// <summary>
        /// True if the path has an extension.
        /// </summary>
        public bool HasExtension 
        {
            get { return System.IO.Path.HasExtension(_path); }
        }

        /// <summary>
        /// True if the path is fully-qualified.
        /// </summary>
        public bool IsRooted 
        {
            get { return System.IO.Path.IsPathRooted(_path); }
        }

        /// <summary>
        /// The parent path for this path.
        /// </summary>
        public Path Parent 
        {
            get 
            {
                var upStr = System.IO.Path.GetDirectoryName(ToString());
                return upStr == null ? this : new Path(upStr, this);
            }
        }

        /// <summary>
        /// The previous path that was used to create this one.
        /// For example, Path.Get("\foo\bar").Parent.Previous
        /// will return the "\foo\bar" path object.
        /// </summary>
        public Path Previous 
        {
            get { return _previousPath; }
        }

        /// <summary>
        /// The root directory of this path, such as "C:\".
        /// </summary>
        public string PathRoot 
        {
            get { return System.IO.Path.GetPathRoot(_path); }
        }

        // Directory & file properties

        /// <summary>
        /// The access control security information for the path.
        /// </summary>
        public FileSystemSecurity AccessControl 
        {
            get 
            {
                return IsDirectory ?
                            Directory.GetAccessControl(_path) :
                            (FileSystemSecurity)File.GetAccessControl(_path);
            }
            set 
            {
                if (IsDirectory) 
                {
                    Directory.SetAccessControl(_path, (DirectorySecurity)value);
                }
                else 
                {
                    File.SetAccessControl(_path, (FileSecurity)value);
                }
            }
        }

        /// <summary>
        /// The attributes for the file.
        /// </summary>
        public FileAttributes Attributes 
        {
            get 
            {
                return File.GetAttributes(_path);
            }
            set 
            {
                File.SetAttributes(_path, value);
            }
        }

        /// <summary>
        /// The creation time of this path.
        /// </summary>
        public DateTime CreationTime 
        {
            get 
            {
                return IsDirectory ?
                            Directory.GetCreationTime(_path) :
                            File.GetCreationTime(_path);
            }
            set 
            {
                if (IsDirectory) 
                {
                    Directory.SetCreationTime(_path, value);
                }
                else 
                {
                    File.SetCreationTime(_path, value);
                }
            }
        }

        /// <summary>
        /// The UTC creation time of this path.
        /// </summary>
        public DateTime CreationTimeUtc 
        {
            get 
            {
                return IsDirectory ?
                            Directory.GetCreationTimeUtc(_path) :
                            File.GetCreationTimeUtc(_path);
            }
            set 
            {
                if (IsDirectory) 
                {
                    Directory.SetCreationTimeUtc(_path, value);
                }
                else 
                {
                    File.SetCreationTimeUtc(_path, value);
                }
            }
        }

        /// <summary>
        /// The subdirectories under this path.
        /// </summary>
        public PathCollection Directories 
        {
            get { return GetDirectories(); }
        }

        /// <summary>
        /// True if the path exists in the file system.
        /// </summary>
        public bool Exists 
        {
            get
            {
                return IsDirectory ? Directory.Exists(_path) : File.Exists(_path);
            }
        }

        /// <summary>
        /// The files under this path.
        /// </summary>
        public PathCollection Files 
        {
            get { return GetFiles(_path); }
        }

        /// <summary>
        /// The last access time of this path.
        /// </summary>
        public DateTime LastAccessTime 
        {
            get 
            {
                return IsDirectory ?
                            Directory.GetLastAccessTime(_path) :
                            File.GetLastAccessTime(_path);
            }
            set 
            {
                if (IsDirectory) 
                {
                    Directory.SetLastAccessTime(_path, value);
                }
                else 
                {
                    File.SetLastAccessTime(_path, value);
                }
            }
        }

        /// <summary>
        /// The UTC last access time of this path.
        /// </summary>
        public DateTime LastAccessTimeUtc 
        {
            get 
            {
                return IsDirectory ?
                            Directory.GetLastAccessTimeUtc(_path) :
                            File.GetLastAccessTimeUtc(_path);
            }
            set 
            {
                if (IsDirectory) 
                {
                    Directory.SetLastAccessTimeUtc(_path, value);
                }
                else 
                {
                    File.SetLastAccessTimeUtc(_path, value);
                }
            }
        }

        /// <summary>
        /// The last write time of this path.
        /// </summary>
        public DateTime LastWriteTime 
        {
            get 
            {
                return IsDirectory ?
                            Directory.GetLastWriteTime(_path) :
                            File.GetLastWriteTime(_path);
            }
            set 
            {
                if (IsDirectory) 
                {
                    Directory.SetLastWriteTime(_path, value);
                }
                else 
                {
                    File.SetLastWriteTime(_path, value);
                }
            }
        }

        /// <summary>
        /// The UTC last write time of this path.
        /// </summary>
        public DateTime LastWriteTimeUtc 
        {
            get 
            {
                return IsDirectory ?
                            Directory.GetLastWriteTimeUtc(_path) :
                            File.GetLastWriteTimeUtc(_path);
            }
            set 
            {
                if (IsDirectory) 
                {
                    Directory.SetLastWriteTimeUtc(_path, value);
                }
                else {
                    File.SetLastWriteTimeUtc(_path, value);
                }
            }
        }

        /// <summary>
        /// True if this is the path of a directory in the file system.
        /// </summary>
        public bool IsDirectory 
        {
            get { return Directory.Exists(_path); }
        }

        // Overrides
        public override bool Equals(object obj) 
        {
            var path = obj as Path;
            if (path != null) return path.ToString() == ToString();
            var str = obj as string;
            if (str != null) return str == ToString();
            return false;
        }

        public override int GetHashCode() 
        {
            return _path.GetHashCode();
        }

        public override string ToString() 
        {
            return _path;
        }

        // Statics

        /// <summary>
        /// Creates a directory in the file system.
        /// </summary>
        /// <param name="directory">The path of the directory to create.</param>
        /// <returns>The path of the new directory.</returns>
        public static Path CreateDirectory(Path directory) 
        {
            return directory.CreateDirectory();
        }

        /// <summary>
        /// Creates a directory in the file system.
        /// </summary>
        /// <param name="directory">The path of the directory to create.</param>
        /// <param name="directorySecurity">The security to apply to the new directory.</param>
        /// <returns>The path of the new directory.</returns>
        public static Path CreateDirectory(Path directory, DirectorySecurity directorySecurity) 
        {
            return directory.CreateDirectory(directorySecurity);
        }

        /// <summary>
        /// Creates a directory in the file system.
        /// </summary>
        /// <param name="directoryName">The name of the directory to create.</param>
        /// <returns>The path of the new directory.</returns>
        public static Path CreateDirectory(string directoryName) 
        {
            return new Path(directoryName).CreateDirectory();
        }

        /// <summary>
        /// Creates a directory in the file system.
        /// </summary>
        /// <param name="directoryName">The name of the directory to create.</param>
        /// <param name="directorySecurity">The security to apply to the new directory.</param>
        /// <returns>The path of the new directory.</returns>
        public static Path CreateDirectory(string directoryName, DirectorySecurity directorySecurity) 
        {
            return new Path(directoryName).CreateDirectory(directorySecurity);
        }

        /// <summary>
        /// The current path for the application.
        /// </summary>
        public static Path Current 
        {
            get 
            {
                return new Path(Directory.GetCurrentDirectory());
            }
            set {
                Directory.SetCurrentDirectory(value.ToString());
            }
        }

        /// <summary>
        /// Creates a new path from its string representation.
        /// </summary>
        /// <param name="path">The string for the path.</param>
        /// <returns>The path object.</returns>
        public static Path Get(string path) 
        {
            return new Path(path);
        }

        // Path API

        /// <summary>
        /// Changes the extension for this path.
        /// </summary>
        /// <param name="newExtension">The new extension.</param>
        /// <returns></returns>
        public Path ChangeExtension(string newExtension) 
        {
            return new Path(System.IO.Path.ChangeExtension(ToString(), newExtension), this);
        }

        /// <summary>
        /// Combines the current path with another.
        /// </summary>
        /// <param name="path">The path to combine to the current one.</param>
        /// <returns>The combined path.</returns>
        public Path Combine(string path) 
        {
            return new Path(System.IO.Path.Combine(ToString(), path), this);
        }

        /// <summary>
        /// Goes up the specified number of levels.
        /// Never goes higher than the root.
        /// </summary>
        /// <param name="levels">The number of levels.</param>
        /// <returns>The new path.</returns>
        public Path Up(int levels) 
        {
            var str = ToString();
            for (var i = 0; i < levels; i++) 
            {
                var strUp = System.IO.Path.GetDirectoryName(str);
                if (strUp == null) 
                {
                    break;
                }
                str = strUp;
            }
            return new Path(str, this);
        }

        // Directory & file API

        /// <summary>
        /// Creates the current path as a new directory in the file system.
        /// </summary>
        /// <returns>The path.</returns>
        public Path CreateDirectory() 
        {
            Directory.CreateDirectory(_path);
            return this;
        }

        /// <summary>
        /// Creates the current path as a new directory in the file system.
        /// </summary>
        /// <param name="directorySecurity">The security to apply to the new directory.</param>
        /// <returns>The path.</returns>
        public Path CreateDirectory(DirectorySecurity directorySecurity) 
        {
            Directory.CreateDirectory(_path, directorySecurity);
            return this;
        }

        /// <summary>
        /// Creates a subdirectory to the current path in the file system.
        /// </summary>
        /// <param name="directoryName">The name of the directory to create.</param>
        /// <returns>The path of the new directory.</returns>
        public Path CreateSubDirectory(string directoryName) 
        {
            return Combine(directoryName).CreateDirectory();
        }

        /// <summary>
        /// Creates a subdirectory to the current path in the file system.
        /// </summary>
        /// <param name="directoryName">The name of the directory to create.</param>
        /// <param name="directorySecurity">The security to apply to the new directory.</param>
        /// <returns>The path of the new directory.</returns>
        public Path CreateSubDirectory(string directoryName, DirectorySecurity directorySecurity) 
        {
            return Combine(directoryName).CreateDirectory(directorySecurity);
        }

        /// <summary>
        /// Copies the file or folder for this path to another location.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <returns>The source path.</returns>
        public Path Copy(Path destination)
        {
            return Copy(destination, false, false);
        }

        /// <summary>
        /// Copies the file or folder for this path to another location.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <param name="overwrite">True if the destination can be overwritten.</param>
        /// <param name="recursive">True if the copy should be deep and include subdirectories recursively.</param>
        /// <returns>The source path.</returns>
        public Path Copy(Path destination, bool overwrite, bool recursive) 
        {
            return Copy(destination.ToString(), overwrite, recursive);
        }

        /// <summary>
        /// Copies the file or folder for this path to another location.
        /// </summary>
        /// <param name="destination">The destination path string.</param>
        /// <returns>The source path.</returns>
        public Path Copy(string destination)
        {
            return Copy(destination, false, false);
        }

        /// <summary>
        /// Copies the file or folder for this path to another location.
        /// </summary>
        /// <param name="destination">The destination path string.</param>
        /// <param name="overwrite">True if the destination can be overwritten. Default is false.</param>
        /// <param name="recursive">True if the copy should be deep and include subdirectories recursively. Default is false.</param>
        /// <returns>The source path.</returns>
        public Path Copy(string destination, bool overwrite, bool recursive) 
        {
            if (IsDirectory) 
            {
                CopyDirectory(_path, destination, overwrite, recursive);
            }
            else 
            {
                File.Copy(_path, System.IO.Path.GetFileName(_path), overwrite);
            }
            return this;
        }

        private static void CopyDirectory(string source, string destination, bool overwrite, bool recursive) 
        {
            if (recursive) 
            {
                foreach (var subdirectory in Directory.GetDirectories(source)) 
                {
                    CopyDirectory(subdirectory,
                                  System.IO.Path.Combine(destination, System.IO.Path.GetFileName(subdirectory)),
                                  true, overwrite);
                }
            }
            
            foreach (var file in Directory.GetFiles(source)) 
            {
                File.Copy(file, System.IO.Path.GetFileName(file), overwrite);
            }
        }

        /// <summary>
        /// Deletes this path from the file system.
        /// </summary>
        /// <returns>The path.</returns>
        public Path Delete() 
        {
            if (!IsDirectory) 
            {
                File.Delete(_path);
                return this;
            }
            return Delete(false);
        }

        /// <summary>
        /// Deletes this directory from the file system.
        /// </summary>
        /// <param name="recursive">True if subdirectories should be recursively deleted.</param>
        /// <returns>The path.</returns>
        public Path Delete(bool recursive) 
        {
            Directory.Delete(_path, recursive);
            return this;
        }

        /// <summary>
        /// Gets all subdirectories of this path.
        /// </summary>
        /// <returns>The set of subdirectory paths.</returns>
        public PathCollection GetDirectories()
        {
            return GetDirectories("*", false);
        }

        /// <summary>
        /// Gets all subdirectories of this path.
        /// </summary>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <returns>The set of subdirectory paths.</returns>
        public PathCollection GetDirectories(string searchPattern)
        {
            return GetDirectories(searchPattern, false);
        }

        /// <summary>
        /// Gets all subdirectories of this path.
        /// </summary>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <param name="recursive">True if subdirectories should be recursively included.</param>
        /// <returns>The set of subdirectory paths.</returns>
        public PathCollection GetDirectories(string searchPattern, bool recursive) 
        {
            return new PathCollection(
                Directory.GetDirectories(
                    _path, searchPattern,
                    recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly),
                new PathCollection(new[] {_path}));
        }

        /// <summary>
        /// Creates a set from all the subdirectories that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A function that returns true if the directory should be included.</param>
        /// <param name="recursive">True if subdirectories should be recursively included.</param>
        /// <returns>The set of directories that satisfy the predicate.</returns>
        public PathCollection GetDirectories(Predicate<Path> predicate, bool recursive) 
        {
            var result = new HashSet<string>();
            foreach (var dir in Directory.GetDirectories(_path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) 
            {
                if (predicate(new Path(dir))) 
                {
                    result.Add(dir);
                }
            }
            return new PathCollection(result, new PathCollection(new[] { _path }));
        }

        /// <summary>
        /// Gets all files under this path.
        /// </summary>
        /// <returns>The set of file paths.</returns>
        public PathCollection GetFiles()
        {
            return GetFiles("*", false);
        }

        /// <summary>
        /// Gets all files under this path.
        /// </summary>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <returns>The set of file paths.</returns>
        public PathCollection GetFiles(string searchPattern)
        {
            return GetFiles(searchPattern, false);
        }

        /// <summary>
        /// Gets all files under this path.
        /// </summary>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <param name="recursive">True if subdirectories should be recursively included.</param>
        /// <returns>The set of file paths.</returns>
        public PathCollection GetFiles(string searchPattern, bool recursive) 
        {
            return new PathCollection(
                Directory.GetFiles(
                    _path, searchPattern,
                    recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly),
                new PathCollection(new[] {_path}));
        }

        /// <summary>
        /// Creates a set from all the files under the path that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A function that returns true if the path should be included.</param>
        /// <param name="recursive">True if subdirectories should be recursively included.</param>
        /// <returns>The set of paths that satisfy the predicate.</returns>
        public PathCollection GetFiles(Predicate<Path> predicate, bool recursive) 
        {
            var result = new HashSet<string>();
            foreach (var file in Directory.GetFiles(_path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) 
            {
                if (predicate(new Path(file))) 
                {
                    result.Add(file);
                }
            }
            return new PathCollection(result, new PathCollection(new[] { _path }));
        }

        /// <summary>
        /// Gets all files and subdirectories under this path.
        /// </summary>
        /// <param name="recursive">True if subdirectories should be recursively included.</param>
        /// <returns>The set of file and subdirectory paths.</returns>
        public PathCollection GetFileSystemEntries(bool recursive)
        {
            return GetFileSystemEntries("*", recursive);
        }

        /// <summary>
        /// Gets all files and subdirectories under this path.
        /// </summary>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <param name="recursive">True if subdirectories should be recursively included.</param>
        /// <returns>The set of file and subdirectory paths.</returns>
        public PathCollection GetFileSystemEntries(string searchPattern, bool recursive) 
        {
            return new PathCollection(
                DirectoryExtensions.GetFileSystemEntries(
                    _path, searchPattern,
                    recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly),
                new PathCollection(new[] {_path}));
        }

        /// <summary>
        /// Creates a set from all the files and subdirectories under the path that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A function that returns true if the path should be included.</param>
        /// <param name="recursive">True if subdirectories should be recursively included.</param>
        /// <returns>The set of fils and subdirectories that satisfy the predicate.</returns>
        public PathCollection GetFileSystemEntries(Predicate<Path> predicate, bool recursive) 
        {
            var result = new HashSet<string>();
            foreach (var entry in DirectoryExtensions.GetFileSystemEntries(_path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) 
            {
                if (predicate(new Path(entry))) 
                {
                    result.Add(entry);
                }
            }
            return new PathCollection(result, new PathCollection(new[] { _path }));
        }

        /// <summary>
        /// Makes this path the current path for the application.
        /// </summary>
        /// <returns>The path.</returns>
        public Path MakeCurrent() 
        {
            Current = this;
            return this;
        }

        /// <summary>
        /// Moves the current path in the file system.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <returns>The destination path.</returns>
        public Path Move(Path destination) 
        {
            if (IsDirectory) 
            {
                Directory.Move(_path, destination.ToString());
            }
            else 
            {
                File.Move(_path, destination.ToString());
            }
            return destination;
        }

        /// <summary>
        /// Moves the current path in the file system.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <returns>The destination path.</returns>
        public Path Move(string destination) 
        {
            if (IsDirectory) 
            {
                Directory.Move(_path, destination);
            }
            else 
            {
                File.Move(_path, destination);
            }
            return new Path(destination, this);
        }

        /// <summary>
        /// Opens the file at this path and hands it over to the specified action.
        /// </summary>
        /// <param name="action">The action to perform on the open stream.</param>
        /// <returns>The path.</returns>
        public Path Open(Action<FileStream> action)
        {
            return Open(action, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }

        /// <summary>
        /// Opens the file at this path and hands it over to the specified action.
        /// </summary>
        /// <param name="action">The action to perform on the open stream.</param>
        /// <param name="mode">The FileMode to use.</param>
        /// <param name="access">The FileAccess to use.</param>
        /// <param name="share">The FileShare to use.</param>
        /// <returns>The path.</returns>
        public Path Open(Action<FileStream> action, FileMode mode, FileAccess access, FileShare share) 
        {
            using (var stream = File.Open(_path, mode, access, share)) 
            {
                action(stream);
            }
            return this;
        }

        /// <summary>
        /// Reads all the text in the file for this path.
        /// </summary>
        /// <returns>The text contents of the file.</returns>
        public string Read() 
        {
            return File.ReadAllText(_path);
        }

        /// <summary>
        /// Reads all the text in the file for this path.
        /// </summary>
        /// <param name="encoding">The encoding to use for reading the file.</param>
        /// <returns>The text contents of the file.</returns>
        public string Read(Encoding encoding) 
        {
            return File.ReadAllText(_path, encoding);
        }

        /// <summary>
        /// Reads all the contents of the file for this path.
        /// </summary>
        /// <returns>The binary contents of the file.</returns>
        public byte[] ReadBytes() 
        {
            return File.ReadAllBytes(_path);
        }

        /// <summary>
        /// Writes text into the file for this path.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <returns>The path.</returns>
        public Path Write(string text)
        {
            return Write(text, false);
        }

        /// <summary>
        /// Writes text into the file for this path.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="append">True if the text should be appended to the existing file.</param>
        /// <returns>The path.</returns>
        public Path Write(string text, bool append) 
        {
            if (append) 
            {
                File.AppendAllText(_path, text);
            }
            else 
            {
                File.WriteAllText(_path, text);
            }
            return this;
        }

        /// <summary>
        /// Writes text into the file for this path.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding to use when writing to the file.</param>
        /// <returns>The path.</returns>
        public Path Write(string text, Encoding encoding)
        {
            return Write(text, encoding, false);
        }

        /// <summary>
        /// Writes text into the file for this path.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding to use when writing to the file.</param>
        /// <param name="append">True if the text should be appended to the existing file.</param>
        /// <returns>The path.</returns>
        public Path Write(string text, Encoding encoding, bool append) 
        {
            if (append) 
            {
                File.AppendAllText(_path, text, encoding);
            }
            else 
            {
                File.WriteAllText(_path, text, encoding);
            }
            return this;
        }

        /// <summary>
        /// Writes binary contents into the file for this path.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <returns>The path.</returns>
        public Path WriteBytes(byte[] bytes) 
        {
            File.WriteAllBytes(_path, bytes);
            return this;
        }

        /// <summary>
        /// Decrypts the file for this path.
        /// </summary>
        /// <returns>The path.</returns>
        public Path Decrypt() 
        {
            File.Decrypt(_path);
            return this;
        }

        /// <summary>
        /// Encrypts the file for this path.
        /// </summary>
        /// <returns>The path.</returns>
        public Path Encrypt() 
        {
            File.Encrypt(_path);
            return this;
        }

        // Fluent setters

        /// <summary>
        /// Sets the access control security for this path.
        /// </summary>
        /// <param name="security">The access control security.</param>
        /// <returns>The path.</returns>
        public Path SetAccessControl(FileSystemSecurity security) 
        {
            AccessControl = security;
            return this;
        }

        /// <summary>
        /// Sets the attributes for this path.
        /// </summary>
        /// <param name="attributes">The attributes to set.</param>
        /// <returns>The path.</returns>
        public Path SetAttributes(FileAttributes attributes) 
        {
            Attributes = attributes;
            return this;
        }

        /// <summary>
        /// Sets the creation time for this path.
        /// </summary>
        /// <param name="creationTime">The time to set.</param>
        /// <returns>The path.</returns>
        public Path SetCreationTime(DateTime creationTime) 
        {
            CreationTime = creationTime;
            return this;
        }


        /// <summary>
        /// Sets the UTC creation time for this path.
        /// </summary>
        /// <param name="creationTimeUtc">The time to set.</param>
        /// <returns>The path.</returns>
        public Path SetCreationTimeUtc(DateTime creationTimeUtc) 
        {
            CreationTimeUtc = creationTimeUtc;
            return this;
        }

        /// <summary>
        /// Sets the last access time for this path.
        /// </summary>
        /// <param name="lastAccessTime">The time to set.</param>
        /// <returns>The path.</returns>
        public Path SetLastAccessTime(DateTime lastAccessTime) 
        {
            LastAccessTime = lastAccessTime;
            return this;
        }

        /// <summary>
        /// Sets the UTC last access time for this path.
        /// </summary>
        /// <param name="lastAccessTimeUtc">The time to set.</param>
        /// <returns>The path.</returns>
        public Path SetLastAccessTimeUtc(DateTime lastAccessTimeUtc) 
        {
            LastAccessTimeUtc = lastAccessTimeUtc;
            return this;
        }

        /// <summary>
        /// Sets the last write time for this path.
        /// </summary>
        /// <param name="lastWriteTime">The time to set.</param>
        /// <returns>The path.</returns>
        public Path SetLastWriteTime(DateTime lastWriteTime) 
        {
            LastWriteTime = lastWriteTime;
            return this;
        }

        /// <summary>
        /// Sets the UTC last write time for this path.
        /// </summary>
        /// <param name="lastWriteTimeUtc">The time to set.</param>
        /// <returns>The path.</returns>
        public Path SetLastWriteTimeUtc(DateTime lastWriteTimeUtc) 
        {
            LastWriteTimeUtc = lastWriteTimeUtc;
            return this;
        }
    }
}


