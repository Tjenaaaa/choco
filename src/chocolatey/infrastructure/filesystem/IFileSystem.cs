﻿// Copyright © 2017 - 2021 Chocolatey Software, Inc
// Copyright © 2011 - 2017 RealDimensions Software, LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//
// 	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace chocolatey.infrastructure.filesystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    ///   File System Interface
    /// </summary>
    public interface IFileSystem
    {
        #region Path

        /// <summary>
        ///   Combines strings into a path.
        /// </summary>
        /// <param name="leftItem">The first path to combine. </param>
        /// <param name="rightItems">string array of all other paths to combine.</param>
        /// <returns>The combined paths.</returns>
        string CombinePaths(string leftItem, params string[] rightItems);

        /// <summary>
        ///   Gets the full path.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain absolute path information.</param>
        /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
        string GetFullPath(string path);

        /// <summary>
        ///   Gets the path to the temporary folder for the current user.
        /// </summary>
        string GetTempPath();

        /// <summary>
        ///   Gets the path directory separator character
        /// </summary>
        /// <returns></returns>
        char GetPathDirectorySeparatorChar();

        /// <summary>
        /// Gets the path to an executable based on looking in current directory, next to the running process, then among the derivatives of Path and Pathext variables
        /// </summary>
        /// <param name="executableName">Name of the executable.</param>
        /// <remarks>Based loosely on http://stackoverflow.com/a/5471032/18475</remarks>
        /// <returns></returns>
        string GetExecutablePath(string executableName);

        /// <summary>
        /// Gets the location of the executing assembly
        /// </summary>
        /// <returns>The path to the executing assembly</returns>
        string GetCurrentAssemblyPath();

        #endregion

        #region File

        /// <summary>
        ///   Gets a list of files inside an existing directory, optionally by pattern and recursive search option.
        /// </summary>
        /// <param name="directoryPath">The path to a specified directory.</param>
        /// <param name="pattern">The search pattern.</param>
        /// <param name="option">The option specifies whether the search operation should include all subdirectories or only the current directory.</param>
        /// <returns>Returns the names of files (including their paths).</returns>
        IEnumerable<string> GetFiles(string directoryPath, string pattern = "*.*", SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Gets a list of files inside an existing directory with extensions and optionally recursive search option.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="extensions">The extensions.</param>
        /// <param name="option">The option.</param>
        /// <returns>Returns the names of files (including their paths).</returns>
        IEnumerable<string> GetFiles(string directoryPath, string[] extensions, SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        ///   Does the file exist?
        /// </summary>
        /// <param name="filePath">The file to check.</param>
        /// <returns>Boolean - true if the caller has the required permissions and path contains the name of an existing file; otherwise, false.</returns>
        bool FileExists(string filePath);

        /// <summary>
        ///   Returns the file name and extension of the specified path string.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The characters after the last directory character in path. If the last character of path is a directory or volume separator character, this method returns String.Empty. If path is Nothing, this method returns Nothing.</returns>
        string GetFileName(string filePath);

        /// <summary>
        ///   Gets the file name without extension.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The string returned by get_file_name, minus the last period (.) and all characters following it.</returns>
        string GetFilenameWithoutExtension(string filePath);

        /// <summary>
        ///   Gets the extension (including the ".").
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>he extension of the specified path (including the period "."), or Nothing, or String.Empty. If path is Nothing, get_file_extension returns Nothing. If path does not have extension information, get_file_extension returns String.Empty.</returns>
        string GetFileExtension(string filePath);

        /// <summary>
        ///   Determines the file information given a path to an existing file
        /// </summary>
        /// <param name="filePath">Path to an existing file</param>
        /// <returns>FileInfo object or reimplementation of a FileInfo object that works with greater than 260 chars</returns>
        dynamic GetFileInfoFor(string filePath);

        /// <summary>
        ///   Gets the file mod date.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>the modification date of the specified file.</returns>
        DateTime GetFileModifiedDate(string filePath);

        /// <summary>
        ///   Gets the size of the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The size, in bytes, of the current file.</returns>
        long GetFileSize(string filePath);

        /// <summary>
        ///   Determines the FileVersion of the file passed in
        /// </summary>
        /// <param name="filePath">Relative or full path to a file</param>
        /// <returns>A string representing the FileVersion of the passed in file</returns>
        string GetFileVersionFor(string filePath);

        /// <summary>
        ///   Determines if a file is a system file
        /// </summary>
        /// <param name="file">File to check - FileInfo or some representation of FileInfo</param>
        /// <returns>True if the file has the System attribute marked, otherwise false</returns>
        bool IsSystemFile(dynamic file);

        /// <summary>
        ///   Determines if a file is a read only file
        /// </summary>
        /// <param name="file">File to check - FileInfo or some representation of FileInfo</param>
        /// <returns>True if the file has the ReadOnly attribute marked, otherwise false</returns>
        bool IsReadOnlyFile(dynamic file);

        /// <summary>
        ///   Determines if a file is a hidden file
        /// </summary>
        /// <param name="file">File to check - FileInfo or some representation of FileInfo</param>
        /// <returns>True if the file has the Hidden attribute marked, otherwise false</returns>
        bool IsHiddenFile(dynamic file);

        /// <summary>
        ///   Determines if a file is encrypted or not
        /// </summary>
        /// <param name="file">File to check - FileInfo or some representation of FileInfo</param>
        /// <returns>True if the file has the Encrypted attribute marked, otherwise false</returns>
        bool IsEncryptedFile(dynamic file);

        /// <summary>
        ///   Determines the older of the file dates, Creation Date or Modified Date
        /// </summary>
        /// <param name="file">File to analyze - FileInfo or some representation of FileInfo</param>
        /// <returns>The oldest date on the file</returns>
        string GetFileDate(dynamic file);

        /// <summary>
        ///   Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="filePath">The name of the file to move. </param>
        /// <param name="newFilePath">The new path for the file. </param>
        void MoveFile(string filePath, string newFilePath);

        /// <summary>
        ///   Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="filePath">The name of the file to move. </param>
        /// <param name="newFilePath">The new path for the file. </param>
        /// <param name="isSilent">Whether we should log retries or not.</param>
        void MoveFile(string filePath, string newFilePath, bool isSilent);

        /// <summary>
        ///   Copies an existing file to a new file. Overwriting a file of the same name is allowed.
        /// </summary>
        /// <param name="sourceFilePath">The source file path. The file to copy.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="overwriteExisting">true if the destination file can be overwritten; otherwise, false.</param>
        void CopyFile(string sourceFilePath, string destinationFilePath, bool overwriteExisting);

        /// <summary>
        ///   Copies an existing file to a new file. Overwriting a file of the same name is allowed.
        /// </summary>
        /// <param name="sourceFilePath">The source file path. The file to copy.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="overwriteExisting">true if the destination file can be overwritten; otherwise, false.</param>
        /// <param name="isSilent">Whether we should log retries or not.</param>
        void CopyFile(string sourceFilePath, string destinationFilePath, bool overwriteExisting, bool isSilent);

        /// <summary>
        ///   Copies a file from one directory to another using FFI
        /// </summary>
        /// <param name="sourceFilePath">Where is the file now?</param>
        /// <param name="destinationFilePath">Where would you like it to go?</param>
        /// <param name="overwriteExisting">If there is an existing file already there, would you like to delete it?</param>
        /// <returns>true if copy was successful, otherwise false</returns>
        bool CopyFileUnsafe(string sourceFilePath, string destinationFilePath, bool overwriteExisting);

        /// <summary>
        ///   Replace an existing file.
        /// </summary>
        /// <param name="sourceFilePath">Where is the file now?</param>
        /// <param name="destinationFilePath">Where would you like it to go?</param>
        /// <param name="backupFilePath">Where should the existing file be placed? Null if nowhere.</param>
        void ReplaceFile(string sourceFilePath, string destinationFilePath, string backupFilePath);

        /// <summary>
        ///   Deletes the specified file.
        /// </summary>
        /// <param name="filePath">The name of the file to be deleted. Wildcard characters are not supported.</param>
        void DeleteFile(string filePath);

        /// <summary>
        ///   Creates a file
        /// </summary>
        /// <param name="filePath">Path to the file name</param>
        /// <returns>A file stream object for use after creating the file</returns>
        FileStream CreateFile(string filePath);

        /// <summary>
        ///   Returns the contents of a file
        /// </summary>
        /// <param name="filePath">Path to the file name</param>
        /// <returns>A string of the file contents</returns>
        string ReadFile(string filePath);

        /// <summary>
        /// Returns the contents of a file as bytes.
        /// </summary>
        /// <param name="filePath">The filepath.</param>
        /// <returns>A byte array of the file contents</returns>
        byte[] ReadFileBytes(string filePath);

        /// <summary>
        ///   Opens a file
        /// </summary>
        /// <param name="filePath">Path to the file name</param>
        /// <returns>A file stream object for use after accessing the file</returns>
        FileStream OpenFileReadonly(string filePath);

        /// <summary>
        ///   Opens a file exclusively
        /// </summary>
        /// <param name="filePath">Path to the file name</param>
        /// <returns>A file stream object for use after accessing the file</returns>
        FileStream OpenFileExclusive(string filePath);

        /// <summary>
        ///   Writes the file text to the specified path
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="fileText">The file text</param>
        void WriteFile(string filePath, string fileText);

        /// <summary>
        ///   Writes the file text to the specified path
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="fileText">The file text</param>
        /// <param name="encoding">The encoding</param>
        void WriteFile(string filePath, string fileText, Encoding encoding);

        /// <summary>
        ///   Writes a stream to a specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="getStream">A deferred function of getting the stream</param>
        void WriteFile(string filePath, Func<Stream> getStream);

        #endregion

        #region Directory

        /// <summary>
        ///   Gets the current working directory of the application.
        /// </summary>
        /// <returns>The path to the directory</returns>
        string GetCurrentDirectory();

        /// <summary>
        ///   Gets the names of subdirectories (including their paths) in the specified directory.
        /// </summary>
        /// <param name="directoryPath">The path for which an array of subdirectory names is returned. </param>
        /// <returns>An array of the names of subdirectories in "directory".</returns>
        IEnumerable<string> GetDirectories(string directoryPath);

        /// <summary>
        /// Gets a list of directories inside an existing directory by pattern, and optionally by recursive search option.
        /// </summary>
        /// <param name="directoryPath">The parent path</param>
        /// <param name="pattern">The search pattern.</param>
        /// <param name="option">The option specifies whether the search operation should include all subdirectories or only the current directory.</param>
        /// <returns>Returns the names of directories (including their paths).</returns>
        IEnumerable<string> GetDirectories(string directoryPath, string pattern, SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        ///   Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="directoryPath">The path to test.</param>
        /// <returns>True if path refers to an existing directory; otherwise, false.</returns>
        bool DirectoryExists(string directoryPath);

        /// <summary>
        ///   Gets the name of the directory.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Directory information for path, or Nothing if path denotes a root directory or is null. Returns String.Empty if path does not contain directory information.</returns>
        string GetDirectoryName(string filePath);

        /// <summary>
        ///   Returns a DirectoryInfo object from a string
        /// </summary>
        /// <param name="directoryPath">Full path to the directory you want the directory information for</param>
        /// <returns>DirectoryInfo object or reimplementation of a DirectoryInfo object that works with greater than 248 chars</returns>
        dynamic GetDirectoryInfo(string directoryPath);

        /// <summary>
        ///   Returns a DirectoryInfo object from a string to a filepath
        /// </summary>
        /// <param name="filePath">Full path to the file you want directory information for</param>
        /// <returns>DirectoryInfo object or reimplementation of a DirectoryInfo object that works with greater than 248 chars</returns>
        dynamic GetFileDirectoryInfo(string filePath);

        /// <summary>
        ///   Creates all directories and subdirectories in the specified path.
        /// </summary>
        /// <param name="directoryPath">The directory path to create. </param>
        void CreateDirectory(string directoryPath);

        /// <summary>
        ///   Moves a specified directory to a new location, providing the option to specify a new directory name.
        ///   Will fall back to do a file move until the first file is unable to be moved.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to move.</param>
        /// <param name="newDirectoryPath">The new path for the directory.</param>
        void MoveDirectory(string directoryPath, string newDirectoryPath);

        /// <summary>
        ///   Moves a specified directory to a new location, providing the option to specify a new directory name.
        ///   Will fall back to do a file move until the firlst file is unable to be moved if <paramref name="useFileMoveFallback"/>
        ///   is <c>true</c>
        /// </summary>
        /// <param name="directoryPath">The path of the directory to move.</param>
        /// <param name="newDirectoryPath">The new path for the directory.</param>
        /// <param name="useFileMoveFallback">Whether a fallback to move each individual files should be used if directory move fails.</param>
        /// <param name="isSilent">Whether we should log retries or not.</param>
        /// <remarks>Any underlying exception will be rethrown if <paramref name="useFileMoveFallback"/> is set to <c>false</c>.</remarks>
        void MoveDirectory(string directoryPath, string newDirectoryPath, bool useFileMoveFallback, bool isSilent);

        /// <summary>
        ///   Copies an existing directory to a new directory. Overwriting a directory of the same name is allowed.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source file directory. The directory to copy.</param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        /// <param name="overwriteExisting">true if the destination directory can be overwritten; otherwise, false.</param>
        void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath, bool overwriteExisting);

        /// <summary>
        ///   Copies an existing directory to a new directory. Overwriting a directory of the same name is allowed.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source file directory. The directory to copy.</param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        /// <param name="overwriteExisting">true if the destination directory can be overwritten; otherwise, false.</param>
        /// <param name="isSilent">Whether we should log retries or not.</param>
        void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath, bool overwriteExisting, bool isSilent);

        /// <summary>
        ///   Creates all directories and subdirectories in the specified path if they have not already been created.
        /// </summary>
        /// <param name="directoryPath">The directory path to create. </param>
        void EnsureDirectoryExists(string directoryPath);

        /// <summary>
        ///   Deletes a directory
        /// </summary>
        /// <param name="directoryPath">Path to the directory</param>
        /// <param name="recursive">Would you like to delete the directories inside of this directory? Almost always true.</param>
        void DeleteDirectory(string directoryPath, bool recursive);

        /// <summary>
        ///  Deletes a directory
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="recursive">Would you like to delete the directories inside of this directory? Almost always true.</param>
        /// <param name="overrideAttributes">Override the attributes, e.g. delete readonly and/or system files.</param>
        void DeleteDirectory(string directoryPath, bool recursive, bool overrideAttributes);

        /// <summary>
        ///  Deletes a directory
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="recursive">Would you like to delete the directories inside of this directory? Almost always true.</param>
        /// <param name="overrideAttributes">Override the attributes, e.g. delete readonly and/or system files.</param>
        /// <param name="isSilent">Should this method be silent? false by default</param>
        void DeleteDirectory(string directoryPath, bool recursive, bool overrideAttributes, bool isSilent);

        /// <summary>
        ///   Deletes a directory if it exists
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="recursive">Would you like to delete the directories inside of this directory? Almost always true.</param>
        void DeleteDirectoryChecked(string directoryPath, bool recursive);

        /// <summary>
        /// Deletes a directory if it exists
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="recursive">Would you like to delete the directories inside of this directory? Almost always true.</param>
        /// <param name="overrideAttributes">Override the attributes, e.g. delete readonly and/or system files.</param>
        void DeleteDirectoryChecked(string directoryPath, bool recursive, bool overrideAttributes);

        /// <summary>
        /// Deletes a directory if it exists
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="recursive">Would you like to delete the directories inside of this directory? Almost always true.</param>
        /// <param name="overrideAttributes">Override the attributes, e.g. delete readonly and/or system files.</param>
        /// <param name="isSilent">Should this method be silent? false by default</param>
        void DeleteDirectoryChecked(string directoryPath, bool recursive, bool overrideAttributes, bool isSilent);

        #endregion

        /// <summary>
        ///   Ensure file attributes are set on a specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="attributes">The attributes.</param>
        void EnsureFileAttributeSet(string path, FileAttributes attributes);

        /// <summary>
        ///   Ensure file attributes are removed from a specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="attributes">The attributes.</param>
        void EnsureFileAttributeRemoved(string path, FileAttributes attributes);


#pragma warning disable IDE1006
        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string combine_paths(string leftItem, params string[] rightItems);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_full_path(string path);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_temp_path();

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        char get_path_directory_separator_char();

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_executable_path(string executableName);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_current_assembly_path();

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        IEnumerable<string> get_files(string directoryPath, string pattern = "*.*", SearchOption option = SearchOption.TopDirectoryOnly);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        IEnumerable<string> get_files(string directoryPath, string[] extensions, SearchOption option = SearchOption.TopDirectoryOnly);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        bool file_exists(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_file_name(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_file_name_without_extension(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_file_extension(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        dynamic get_file_info_for(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        DateTime get_file_modified_date(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        long get_file_size(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_file_version_for(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        bool is_system_file(dynamic file);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        bool is_readonly_file(dynamic file);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        bool is_hidden_file(dynamic file);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        bool is_encrypted_file(dynamic file);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_file_date(dynamic file);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void move_file(string filePath, string newFilePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void copy_file(string sourceFilePath, string destinationFilePath, bool overwriteExisting);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        bool copy_file_unsafe(string sourceFilePath, string destinationFilePath, bool overwriteExisting);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void replace_file(string sourceFilePath, string destinationFilePath, string backupFilePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void delete_file(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        FileStream create_file(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string read_file(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        byte[] read_file_bytes(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        FileStream open_file_readonly(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        FileStream open_file_exclusive(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void write_file(string filePath, string fileText);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void write_file(string filePath, string fileText, Encoding encoding);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void write_file(string filePath, Func<Stream> getStream);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_current_directory();

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        IEnumerable<string> get_directories(string directoryPath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        IEnumerable<string> get_directories(string directoryPath, string pattern, SearchOption option = SearchOption.TopDirectoryOnly);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        bool directory_exists(string directoryPath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        string get_directory_name(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        dynamic get_directory_info_for(string directoryPath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        dynamic get_directory_info_from_file_path(string filePath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void create_directory(string directoryPath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void move_directory(string directoryPath, string newDirectoryPath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void copy_directory(string sourceDirectoryPath, string destinationDirectoryPath, bool overwriteExisting);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void create_directory_if_not_exists(string directoryPath);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void delete_directory(string directoryPath, bool recursive);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void delete_directory(string directoryPath, bool recursive, bool overrideAttributes);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void delete_directory(string directoryPath, bool recursive, bool overrideAttributes, bool isSilent);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void delete_directory_if_exists(string directoryPath, bool recursive);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void delete_directory_if_exists(string directoryPath, bool recursive, bool overrideAttributes);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void delete_directory_if_exists(string directoryPath, bool recursive, bool overrideAttributes, bool isSilent);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void ensure_file_attribute_set(string path, FileAttributes attributes);

        [Obsolete("This overload is deprecated and will be removed in v3.")]
        void ensure_file_attribute_removed(string path, FileAttributes attributes);
#pragma warning restore IDE1006
    }
}
