
"""Module for reading a file on a filesystem."""

import os
from git import Repo

def try_to_find():
  functions = [find_readme, find_git_root, print_parent_dir]
  for function in functions:
    project_name = function()
    if (project_name != None):
      return project_name

def is_found(to_find, all_files): #checks if the file exists in directory, returns True/False
  for file in all_files:
    if (file == to_find):
      return True

  return False

def find_readme():
  all_files = os.listdir() #stores all subdirectories and files in current directory
  wanted = ["README.md", "README"]

  for name in wanted: #checks every filename stored in var wanted, opens and if contains non-empty line returns it
    if (is_found(name, all_files) == True):
      with open(name, "r") as readme_file:
        line = readme_file.readline()
        line = line.strip().lstrip('# ')
        if not (line == ""):
          return line
  return None 

def find_git_root():
  try:
    repo = Repo(".", search_parent_directories=True)
  except:
    return

  git_root_dir = repo.working_tree_dir
  git_root_dir_name = os.path.basename(git_root_dir)
  return git_root_dir_name

def print_parent_dir():
  return os.path.basename(os.getcwd()) 
