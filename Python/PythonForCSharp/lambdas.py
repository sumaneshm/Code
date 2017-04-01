__author__ = 'sumaneshm'


def find_numbers(predicate):
    for i in range(0, 100):
        if (predicate(i)):
            yield i

def expect_numbers(a, b):
    return a + b

numbers = find_numbers(lambda i : i % 11 == 0)

for n in numbers:
    print(n , end=',');

print()
print("Result is {0}".format(expect_numbers("M ","Sumanesh")))