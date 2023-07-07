Create a script for listing file sizes.

The script would partially mimic behaviour of ls: without arguments it lists information about files in the current directory, when some arguments are provided, they are treated as list of files to print details about.

Example run can look like this:

./08/dir.sh /dev/random 08/dir.sh 08

/dev/random  <special>
08/dir.sh          312
08               <dir>

The second column will display file size for normal files, <dir> for directories and <special> for any other file. File size can be read through the stat(1) utility.

Nonexistent files should be announced as FILENAME: no such file or directory. to stderr.

You can safely assume that you will have access to all files provided on the command-line.

You will probably find the column utility useful, especially the following invocation:

column --table --table-noheadings --table-columns FILENAME,SIZE --table-right SIZE

You can assume that there filenames will be reasonable (e.g. without spaces). To simplify things, we will not check exit code to be different when some of the files were not found.
