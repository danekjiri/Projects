#!/bin/bash

set -ueo pipefail

ARG_COUNT="$#"
ARGUMENTS="$@"

no_arguments(){
  for file in $( ls $(pwd) ); do
    echo "$file $( file_type "$file" )"
  done
}

file_type(){
  local file="$1"
  local stat_type="$( stat -c %F ${file})"
  case "$stat_type" in
    regular*file)
      local size="$( stat -c %s ${file})"
      echo ${size}
      ;;
    directory)
      local is_dir="<dir>"
      echo ${is_dir}
      ;;
    *)
      local is_special="<special>"
      echo ${is_special}
      ;;
  esac
}

arguments(){
  while [ "$#" -gt 0 ]; do
    file="$1"
    if [ -e "$file" ]; then
      echo "$file $( file_type "$file" )"
    else
      echo "${file}: no such file or directory." >&2 
    fi  
    shift
  done 
}

prepare_data(){
  if [ "$ARG_COUNT" -eq 0 ]; then
    no_arguments
  else
    arguments ${ARGUMENTS}
  fi
}

#prepare_data
prepare_data | column --table --table-noheadings --table-columns FILENAME,SIZE --table-right SIZE

