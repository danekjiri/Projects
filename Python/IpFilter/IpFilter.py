#!/usr/bin/env python3
"""Module providing regex function and cooperations with os/cli."""
import re
import sys

def filter_ip():
    """Function filtering the output of ip addr command providing just name and ip/mask."""
    for line in sys.stdin:
        match_name = re.search(r'^[0-9]+: (\w+)', line)
        match_ip = re.search(r'([0-9.]+)\/(\d+)\s+.*\s+(\S+)$', line)
        if match_name:
            name = match_name.group(1)
            print(f"{name} ", end='')
        if match_ip:
            ip_addr, mask = match_ip.group(1), match_ip.group(2)
            print(f"{ip_addr}/{mask}")

if __name__ == "__main__":
    filter_ip()

