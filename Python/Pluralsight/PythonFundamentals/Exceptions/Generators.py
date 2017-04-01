# generator are created when there is at least one yield inside the method
def gen123():
    print("--> Entered the definition")
    print("--> About to yield 1")
    yield 1
    print("--> About to yield 2")
    yield 2
    print("--> About to yield 3")
    yield 3
    print("--> Completed")


# we can get the next value in the pipeline by passing generator to the method
# "next"

g = gen123()
print("Created a generator")
print("Waiting for 1st element")
print(next(g))
print("Waiting for 2nd element")
print(next(g))
print("Waiting for 3rd element")
print(next(g))
print("Going to error routine now")

try:
    print(next(g))
except StopIteration as si:
    print("Completed the list", si)

# another most common way to access generator is for loop
for i in gen123():
    print(i)

