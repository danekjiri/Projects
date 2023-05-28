#!/usr/bin/env python3

import sys
import datetime
import dateutil
from dateutil.parser import parse

def is_date(string): #checks date format
    try:
        parse(string)
        return True
    except ValueError:
        return False

def print_day(data):
    for line in data:
        if len(line) <= 1: continue #empty line
        line = line.split(" ", 1) #separates first col and rest of sentence
        date = line[0]
        if (is_date(date) == True):
            year, month, day = (int(x) for x in date.split("-")) #date extraction
            day = datetime.date(year, month, day) #right format
            line[0] = day.strftime("%A") #changes date with day name

        if len(line) > 1:
            line[1] = " " + line[1] #if not only date, split deleted one white-space
        for word in line: #reprints the input
            print(word, end='')

    return

def main():
    exit_code = 0

    if len(sys.argv) == 1: #called with stdin
        print_day(sys.stdin)
    else:
        for filename in sys.argv[1:]: #called with given files 
            try:
                with open(filename, "r") as file:
                    print_day(file)
            except IOError as err: #if cannot open file
                print(f"{filename}: error reading file", file=sys.stderr)
                exit_code = 1

    sys.exit(exit_code)
    return

if __name__ == "__main__":
    main()

