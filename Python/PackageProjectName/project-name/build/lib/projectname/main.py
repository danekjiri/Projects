#!/usr/bin/env python3

"""Module with main CLI interface."""

import sys

import projectname.project_name as project_name

def main():
  res = project_name.try_to_find()
  print(res)
 
if __name__ == "__main__":
  main()
