using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Oiski.Common.Files
{
    /// <summary>
    /// Defines a tool to manipulate the contents of a file
    /// </summary>
    public class FileHandler
    {
        /// <summary>
        /// Creates a new instance of type <see cref="FileHandler"/>
        /// </summary>
        /// <param name="_filePath">The path to the file the handler should manipulate</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public FileHandler ( string _filePath )
        {
            FilePath = _filePath;

            if ( !File.Exists (FilePath) )
            {
                File.Create (FilePath).Close ();
            }
        }

        /// <summary>
        /// The fully qualified file path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Write <paramref name="_text"/> to the file
        /// </summary>
        /// <param name="_text">The content to write</param>
        /// <param name="_append"><see langword="true"/> to append <paramref name="_text"/> to the file; <see langword="false"/> to override file</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void Write ( string _text, bool _append = false )
        {
            using ( StreamWriter file = new StreamWriter (FilePath, _append) )
            {
                file.Write (_text);
            }
        }

        /// <summary>
        /// Write a string <see langword="value"/> to the file, followed by a <i>newline terminator</i>
        /// </summary>
        /// <param name="_text">The content to write</param>
        /// <param name="_append"><see langword="true"/> to append <paramref name="_text"/> to the file; <see langword="false"/> to override file</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        public void WriteLine ( string _text, bool _append = false )
        {
            using ( StreamWriter file = new StreamWriter (FilePath, _append) )
            {
                file.WriteLine (_text);
            }
        }

        /// <summary>
        /// Insert <paramref name="_text"/> at the specified <paramref name="_lineNumber"/> in the file
        /// </summary>
        /// <param name="_text">The content to write</param>
        /// <param name="_lineNumber">The line index</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void InsertLine ( string _text, int _lineNumber )
        {
            string[] lines = ReadLines ();

            if ( _lineNumber < lines.Length )
            {
                string updatedFileContent = string.Empty;

                for ( int i = 0; i < lines.Length; i++ )
                {
                    if ( i == _lineNumber )
                    {
                        updatedFileContent += $"{_text}{( ( i != lines.Length - 1 ) ? ( Environment.NewLine ) : ( string.Empty ) )}";
                    }
                    else
                    {
                        updatedFileContent += $"{lines[ i ]}{( ( i != lines.Length - 1 ) ? ( Environment.NewLine ) : ( string.Empty ) )}";
                    }
                }

                Write (updatedFileContent, false);
            }
            else
            {
                throw new IndexOutOfRangeException ("Line Number must be positive and less than the length of the file in lines");
            }
        }

        /// <summary>
        /// Replace a line in the file with <paramref name="_text"/> at the specified <paramref name="_lineNumber"/>
        /// </summary>
        /// <param name="_text">The content to write</param>
        /// <param name="_lineNumber">The line index</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void UpdateLine ( string _text, int _lineNumber )
        {
            string[] lines = ReadLines ();

            if ( _lineNumber < lines.Length )
            {
                string updatedFileContent;

                string fileContent = ReadAll ();
                updatedFileContent = fileContent.Replace (lines[ _lineNumber ], _text);

                Write (updatedFileContent, false);
            }
            else
            {
                throw new IndexOutOfRangeException ("Line Number must be positive and less than the length of the file in lines");
            }
        }

        /// <summary>
        /// Delete a line in the file at the specified <paramref name="_lineNumber"/>
        /// </summary>
        /// <param name="_lineNumber">The line index</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public void DeleteLine ( int _lineNumber )
        {
            string[] lines = ReadLines ();

            if ( _lineNumber < lines.Length )
            {
                string updatedFileContent = string.Empty;

                for ( int i = 0; i < lines.Length; i++ )
                {
                    if ( lines[ i ] != lines[ _lineNumber ] )
                    {
                        updatedFileContent += $"{lines[ i ]}{( ( i != lines.Length - 1 ) ? ( Environment.NewLine ) : ( string.Empty ) )}";
                    }
                }

                Write (updatedFileContent, false);
            }
            else
            {
                throw new IndexOutOfRangeException ("Line Number must be positive and less than the length of the file in lines");
            }
        }

        /// <summary>
        /// Read the content of the file as lines
        /// </summary>
        /// <returns>The content of the file, seperated by a <i>newline termintor</i>, as a collection of <see langword="strings"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public string[] ReadLines ()
        {
            string[] lines;
            lines = ReadAll ().Split (Environment.NewLine);

            return lines;
        }

        /// <summary>
        /// Read the file
        /// </summary>
        /// <returns>The content of the file as a <see langword="string"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public string ReadAll ()
        {
            StreamReader reader = new StreamReader (FilePath);
            string fileContent = reader.ReadToEnd ();
            reader.Close ();

            return fileContent;
        }

        /// <summary>
        /// Find a specific line within the file
        /// </summary>
        /// <param name="_lineNumber">The line index</param>
        /// <returns>The line at the specified <paramref name="_lineNumber"/> as a <see langword="string"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public string FindLine ( int _lineNumber )
        {
            string[] lines = ReadLines ();

            if ( _lineNumber < lines.Length )
            {
                return lines[ _lineNumber ];
            }

            throw new IndexOutOfRangeException ("Line Number must be positive and less than the length of the file in lines");
        }

        /// <summary>
        /// Search for a specific key in the file
        /// </summary>
        /// <param name="_searchKey">The key to search for</param>
        /// <returns>The first line that contains the specified <paramref name="_searchKey"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public string FindLine ( string _searchKey )
        {
            if ( _searchKey != null )
            {
                string[] lines = ReadLines ();

                foreach ( string line in lines )
                {
                    if ( !string.IsNullOrWhiteSpace (line) && line.ToLower ().Contains (_searchKey.ToLower ()) )
                    {
                        return line;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// search for a specific key in the file
        /// </summary>
        /// <param name="_searchKey">The key to search for</param>
        /// <returns>A collection of <see langword="string"/> <see langword="values"/> that contains the specified <paramref name="_searchKey"/></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public IReadOnlyList<string> FindLines ( string _searchKey )
        {
            List<string> foundLines = null;

            if ( _searchKey != null )
            {
                string[] lines = ReadLines ();
                foundLines = new List<string> ();

                foreach ( string line in lines )
                {
                    if ( !string.IsNullOrWhiteSpace (line) && line.ToLower ().Contains (_searchKey.ToLower ()) )
                    {
                        foundLines.Add (line);
                    }
                }
            }

            return foundLines;
        }

        /// <summary>
        /// Find the line number of a specific line
        /// </summary>
        /// <param name="_line"></param>
        /// <returns>The zero-based index of the first occurence if <paramref name="_line"/> within the file, if found; Otherwise, -1</returns>
        public int GetLineNumber ( string _line )
        {
            return ReadLines ().ToList ().IndexOf (_line);
        }
    }
}
