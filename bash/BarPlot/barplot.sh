#!/bin/sh
set -euo pipefail

: "${COLUMNS:=80}"
FILE=$1

if [ ! -s "${FILE}" ];then
  exit 0
fi

max_val=0
max_label_w=0
while read -r val label; do
  label_with_val="$label ($val)"
  label_with_val_w=${#label_with_val}
  [ "$label_with_val_w" -gt "$max_label_w" ] && max_label_w=$label_with_val_w
done < "$FILE"

max_val=$( cut -d " " -f1 <$FILE | sort -nr | head -n1 )
bar_w=$(($COLUMNS - $max_label_w - 3))
while read -r val label; do
  printf "%-*s | " $max_label_w "$label ($val)"
  lenght=$(printf "scale=0; (%d * %d) / %d\n" "$bar_w" "$val" "$max_val" | bc -l)
  printf "%*s\n" "$lenght" "" | tr ' ' '#'
done < "$FILE"

