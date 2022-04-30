import os
import sys
import subprocess

def show_usage():
    usage = '''
{bin} [options] <command>

Where options are:
-h Show this help
-w Wait the command to finish
-c Run by systemd-run
-s systemd scope name
-l systemd slice name
    '''
    print(usage.format(bin = sys.argv[0]))

def parse_args():
    wait, run_in_cgroup, scope, slice, command = False, False, None, None, None
    args = sys.argv
    try:
        idx = 1
        while idx < len(args):
            if args[idx] == '-h':
                show_usage()
                sys.exit(0)
            elif args[idx] == '-w':
                wait = True
            elif args[idx] == '-c':
                run_in_cgroup = True
            elif args[idx] == '-s':
                idx += 1
                scope = args[idx]
            elif args[idx] == '-l':
                idx += 1
                slice = args[idx]
            else:
                command = args[idx]
            if command:
                break
            idx += 1
    except IndexError:
        show_usage()
        sys.exit(1)

    if not command or (run_in_cgroup and not (scope and slice)):
        show_usage()
        sys.exit(1)

    return wait, run_in_cgroup, scope, slice, command

def main():
    wait, run_in_cgroup, scope, slice, command = parse_args()
    if run_in_cgroup:
        process = subprocess.Popen(
            "systemd-run --unit={0} --scope --slice={1} {2}".format(scope, slice, command),
            shell=True,
            preexec_fn=os.setsid)
    else:
        process = subprocess.Popen(command, shell=True, preexec_fn=os.setsid)

    print(process.pid)

    if wait:
        code = process.wait()
        print(code)

if __name__ == '__main__':
    main()
