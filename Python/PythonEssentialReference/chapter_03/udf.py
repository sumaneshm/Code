from functools import wraps


def tracer(f):
    @wraps(f)
    def wrap(*args, **kwargs):
        print("Going to call : ", *args, **kwargs)
        r = f(*args, **kwargs)
        print("Result : ", r)
        return r

    return wrap


@tracer
def foo(x, y=10):
    "foo accepts two parameters and adds them together"
    return x + y


def experiment1():
    f = foo

    print(f.__doc__)  # doc string shows the comment from the first line
    print(f.__name__)  # name of the function
    print(f.__dict__)  # dictionary containing function attributes
    print(f.__code__)  # byte compiled code
    print(f.__defaults__)  # default arguments list

    foo(21)


if __name__ == '__main__':
    experiment1()
