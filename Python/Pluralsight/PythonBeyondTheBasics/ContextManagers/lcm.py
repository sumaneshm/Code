
# How Context Manager works.
# 1. With statement has an expression which should return an instance which implements context manager protocol
# 2. __enter__ method will be called on that instance
# 3. If it gets any exception, the process completes, i.e. with block statements will not be called
# 4. IMPORTANT !!! the return value of __enter__ is bound to the variable used in with statement
#       though in most of the cases, __enter__ will return itself, but beware it is possible to return something else too
# 5. Irrespective of how the with block statements gets completed, i.e. whether it throws any error, or normally
#       __exit__ method will be called accordingly
# 6. In case of any exceptions, __exit__ will re-raise that

class LogContextManager1:
    def __enter__(self):
        return "Something else is returned"

    def __exit__(self, exc_type, exc_val, exc_tb):
        pass


def experiment1():
    with LogContextManager1() as l:
        print(l)


class LogContextManager2:
    def __enter__(self):
        print("LogContextManager2.__enter__()")
        return self

    # Exception information will be passed if any to __exit__
    # exc_type = Exception type
    # exc_val =  Message most likely
    # exc_tb = Traceback of the exception
    def __exit__(self, exc_type, exc_val, exc_tb):
        print("LogContextManager2.__exit({}, {}, {})".format(exc_type, exc_val, exc_tb))

def experiment2():
    with LogContextManager2() as l:
        print(l)


def experiment3():
    with LogContextManager2() as l:
        raise ValueError("Something is wrong")


class LogContextManager3():
    def __init__(self, name):
        self._name = name
        print("__init__ : ", name)

    # even though we are returning a new instance of LogContextManager here,
    # when with block is done, python will call the __exit__ on the same instance on which __enter__ was called
    def __enter__(self):
        return LogContextManager3("Saveetha")

    def __exit__(self, exc_type, exc_val, exc_tb):
        print("__exit__ : ", self._name)

def experiment4():
    with LogContextManager3("Sumanesh") as l:
        # here name printed will be "Saveetha" as __entet__ method created a new object
        print("Inside with : ", l._name)

if __name__ == '__main__':
    experiment4()