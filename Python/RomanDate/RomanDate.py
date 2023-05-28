#!/bin/env python3

import roman
import dateparser
import sys

def get_date_list():
  arg = sys.argv[1:]
  if ( len(arg) > 0 ):
    arg_string = ' '.join(map(str, arg))
    date = dateparser.parse(arg_string)
  else:
    date = dateparser.parse("now") 
  
  try:
    output_list = [date.day, date.month, date.year]
  except AttributeError as e:
    sys.stderr.write(f"Time `{arg_string}\' not recognized.")
    sys.exit(1)

  return output_list


def print_date_roman(date_list):
  roman_list = list()

  for i in date_list:  
    roman_list.append(roman.toRoman(i))

  print(f"{roman_list[0]}.{roman_list[1]}.{roman_list[2]}")

def main():
  exit_code = 0
  date_list = get_date_list()
  print_date_roman(date_list)

  sys.exit(exit_code)

if __name__ == "__main__":
  main()

