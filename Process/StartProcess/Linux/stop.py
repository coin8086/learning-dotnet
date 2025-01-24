#!/usr/bin/env python3

import os
import signal
import sys

def parse_args():
    args = sys.argv
    return int(args[1])

def main():
    pid = parse_args()
    pgid = os.getpgid(pid)
    print(pid)
    print(pgid)
    os.killpg(pgid, signal.SIGKILL)

if __name__ == '__main__':
    main()
