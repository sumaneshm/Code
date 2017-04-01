from pprint import pprint


# to check whether a variable is callable, we can check "callable"
def experiment1():
    # functions are callable
    def is_even(x):
        return x % 2 == 0

    print(callable(is_even))

    # lambdas are callable
    is_odd = lambda x: x % 2 == 1
    print(callable(is_odd))

    # classes are callable
    print(callable(list))

    # class methods are also callable
    print(callable(list.append))


def experiment2():
    hypervolume1(3, 4)
    hypervolume1(1)
    hypervolume1(1, 2, 3)
    # the following statement would fail because we are not passing anything,
    # however we are assuming that we need to iterate and next would
    # with an StopIteration error which is not really something the client
    # would expect and we are giving away implementation details.
    #       hypervolume1() - Throws StopIteration error
    #
    # one method to fix this issue is to wrap the call to next() inside try/catch
    # but it would look ugly and hence we will refactor it slightly to
    # hypervolume2 function and this would ensure that at least one value
    # is passed and if not, it will throw a more meaningful TypeError
    #       hypervolume2() - Throws TypeError


def hypervolume1(*lengths):
    i = iter(lengths)
    v = next(i)
    for length in i:
        v *= length
    print(v)


def hypervolume2(length, *lengths):
    v = length
    for i in lengths:
        v *= i
    print(v)


def experiment3():
    tag("img",src="test.jpg", size=12, alt="Alternate text")


def tag(name, **attributes):
    # when we declare a keyword argument using **, it will collect the arguments
    # list as a dictionary with the argument name as key, value as value
    result = '<' + name
    for key, value in attributes.items():
        result += ' {k}="{v}"'.format(k=key, v=value)
    result += '/>'

    print(result)


def experiment4():
    # we can pass a tuple as a parameter and request python to unpack the
    # parameters list
    t = (11, 12, 13, 14)

    print_args(*t)  # note * before t instructing to unpack the args

    k = {'red':21, 'green': 68, 'blue': 120, 'alpha': 52, 'gamma': 123}
    color(**k)
    # ** unpacks the dictionary to keyword arguments
    # normal argument names are first matched and all remaining are
    # bundled to the keyword arguments dictionary


def print_args(arg1, arg2, *args):
    print(arg1)
    print(arg2)
    print(args)


def color(red, blue, green, **kwargs):
    print("r = ", red)
    print("b = ", blue)
    print("g = ", green)
    print(kwargs)


def experiment5():
    print(trace(adder, 1, 4))


def adder(a, b):
    return a + b


def trace(f, *args, **kwargs):
    # trace can intercept the input/output to a function
    # yet another beauty of python on how it uses the dynamic power
    print("args=", args)
    print("kwargs=", kwargs)

    result = f(*args, **kwargs)

    print("result : ", result)
    return result

if __name__ == '__main__':
    experiment5()


# Rules :
# 1. *args must be declared before **kwargs
# 2. any arguments declared before *args are regular positional arguments
# 3. Any arguments after *args are to be passed as mandatory named arguments
# 4. **kwargs must be the last argument



