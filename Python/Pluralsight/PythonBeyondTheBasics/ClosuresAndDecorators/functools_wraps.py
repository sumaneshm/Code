import functools


def normal_noop(f):
    def wrap():
        return f()
    return wrap


@normal_noop
def normal_test_method():
    "This is a test method"
    print("A test method is one of the most useful methods ever written")


def proper_noop(f):
    # The decorator @functools.wraps will replace certain attributes of beloe defined "wrap" method such that
    # it will look similar to f.. (like replacing __name__, __doc__ etc
    @functools.wraps(f)
    def wrap():
        return f()
    return wrap


@proper_noop
def proper_test_method():
    "This is a test method"
    print("A test method is one of the most useful methods ever written")

def experiment1():
    # This will not print the expected "normal_test_method"'s attribute as we have decorated it with @normal_noop
    # in stead it will show normal_noop's details
    print(help(normal_test_method))

    print(help(proper_test_method))

if __name__ == '__main__':
    experiment1()



