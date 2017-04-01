import contextlib
import sys

# @contextlib.contextmanager
# def meth():
#     __enter__():
#     try:
#         yield "Return value"
#         __exit__() -> Normal exit case
#     except:
#         __exit__() -> Exception exit case
#


@contextlib.contextmanager
def logger1():
    print("Entered the logger1")
    try:
        yield "I am the Context Manager return"
        print("logger1 is back in action properly")
    except Exception:
        print("Something went wrong and logger1 knows about it", sys.exc_info())
        # decorator style context manager won't throw the error back
        # if you need that error to be thrown, you have to explicitly throw it

def experiment1():
    with logger1() as x:
        print(x)
        raise "Something terrible"

if __name__ == '__main__':
    experiment1()
