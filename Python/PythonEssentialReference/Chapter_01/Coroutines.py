# Coroutines are to give more than one input to a function

def print_matches(matchtext):
    print("Looking for " + matchtext)
    while True:
        # (yield) within braces means it will wait for user inputs
        line = (yield)
        if matchtext in line:
            print(line)


allmatchers = [
    print_matches("test"),
    print_matches("1")
]

for m in allmatchers:
    m.__next__()

print("Starting...")
for l in open("C:\\Temp\\Log.txt"):
    for m in allmatchers:
        m.send(l)

print("Completed")

for m in allmatchers:
    m.close()
