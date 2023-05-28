# PROJECT-NAME

The program will look into `README.md` and `README` files for the first non-empty line (again stripping extra whitespace and leading `#` in *.md files).

When neither `README.md` or `README` are present, the program will try to find the top directory of a Git project and print its basename.

If the current directory is not a part of a Git project, the program will print the basename of the current directory.

```bash
project-name
# Prints 'NSWI177 Submission Repository'
cd 01
project-name
# Prints 'student-LOGIN'
cd ../../
project-name
# Prints directory name of the parent directory of your submission repository clone
```
