import math
import traceback

class InclinationException(Exception):
    pass


def incline(dx, dy):
    try:
        return math.degrees(math.atan(dx / dy))
    except ZeroDivisionError as e:
        # explicit chaining happens here using "from 'original exception'"
        raise InclinationException("Slope cannot be vertical") from e


def experiment1():
    try:
        print(incline(10, 0))
    except InclinationException as e:
        # print(e)
        # print(e.__cause__) # original exception will be stored as __cause__ in case of explicit chaining
        # stack trace is __traceback__
        # caution : don't store the reference of __traceback__ as this will hold stack frame of all the calling
        # hierarchy preventing the objects in the calling functions from garbage collection
        traceback.print_tb(e.__traceback__)
        # this is recommended way to store the traceback by converting  it into string
        s = traceback.format_tb(e.__traceback__)

if __name__ == '__main__':
    experiment1()
    print("Continue")