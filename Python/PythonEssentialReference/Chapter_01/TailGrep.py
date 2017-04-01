import time


def tail(f):
    f.seek(0, 1)
    print("Starting to tail")
    while True:
        line = f.readline()
        if not line:
            time.sleep(0.1)
            continue
        yield line

def grep(lines, searchtext):
    for line in lines:
        if searchtext in line:
            yield line

print("Starting...")

log = grep(tail(open("C:\\Temp\\Log.txt")),"t")
for line in log:
    print(line)
