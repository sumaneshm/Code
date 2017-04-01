__author__ = 'sumaneshm'

def fibinacci():
    current, nxt = 1, 1
    yield current

    while True:
        current, nxt = nxt, current + nxt
        if(current < 10000):
            yield current


print ("Going to start")

for i in fibinacci():
    print(i)

print("Everything completed")