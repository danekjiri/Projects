#!/bin/bash
 2
 3 set -ueo pipefail
 4
 5 OUTPUT_DIRECTORY="$1"
 6 shift
 7 if ! [ -d "$OUTPUT_DIRECTORY" ]; then
 8   mkdir "$OUTPUT_DIRECTORY"
 9 fi
10
11 exists(){
12   local timestamp="$1"
13   if ! [ -e "${OUTPUT_DIRECTORY}/${timestamp}.jpg" ]; then
14     return 1
15   fi
16   return 0
17 }
18
19 newname(){
20   local name=$1
21   local help_name=$1
22   local counter=1
23   while exists $help_name; do
24     help_name=${name}_${counter}
25     (( counter++ ))
26   done
27   echo "${help_name}.jpg"
28 }
29
30 for jpeg in $@; do
31   datetime=$( exiftool -printFormat '$DateTimeOriginal'\
32               -dateFormat "%Y-%m-%d_%H_%M_%S" "$jpeg" )
33   photo_name=$( newname "$datetime" )
34   cp "$jpeg" "${OUTPUT_DIRECTORY}/${photo_name}"
35 done
