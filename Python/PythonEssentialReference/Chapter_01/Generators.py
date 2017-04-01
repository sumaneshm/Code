# yield creates a class for thie loop and hence it is called as Generator

def countdown(n):
    print("Counting down...")
    while(n>0):
        yield n
        n -= 1

# we can navigate it using __next__
c = countdown(5)
print(c.__next__())
print(c.__next__())
print(c.__next__())
print(c.__next__())
print(c.__next__())

for i in countdown(10):
    print(i)

