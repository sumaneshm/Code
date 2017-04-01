from functools import reduce
import operator


def experiment1():
    result = reduce(adder, [1, 2, 3, 4, 5])
    print("Final result : ", result)


def adder(x, y):
    print("Interim result : {} + {} = {}".format(x, y, x+y))
    return x + y

def experiment2():
    # we can provide an initial value to the start with which will be used if the there is
    # no elements
    print(reduce(operator.add, [], 10))     # will print 10 (initial value)
    print(reduce(operator.add, [1], 100))   # will print 101(initial value + first value)
    

if __name__ == '__main__':
    experiment2()