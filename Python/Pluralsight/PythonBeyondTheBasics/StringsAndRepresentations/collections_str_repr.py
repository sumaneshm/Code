import reprlib

class Point2D:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def __str__(self):
        return "({}, {})".format(self.x, self.y)

    def __repr__(self):
        return "Point2D (x={}, y={})".format(self.x, self.y)

    def __format__(self, format_spec):
        return "[X={}, Y={}, Format_spec:{}]".format(self.x, self.y, format_spec)


def experiment1():
    l = [Point2D(i, i*2) for i in range(3)]
    print(l)    # it uses __str__ for the container and __repr__ for the individual items

    d = {i: Point2D(i, i*2) for i in range(3)}
    print(d)

    p = Point2D(12, 30)
    # this will call __format__ method on Point2D and whatever user has given after : will go as format_spec
    print("This is nicely formatted: {:abc}".format(p))

    print("Default Format : {}".format(p))          # this will call __format__
    print("Forced repr format : {!r}".format(p))    # will call __str__ on p
    print("Forced str format : {!s}".format(p))     # will call __repr__ on p


def experiment2():
    large = [Point2D(x, y) for x in range(1000) for y in range(1000)]
    # if we want to show the repr of this large, it will be extremely big, so we can use reprlib.repr instead
    # which will show only first few characters and a "..." if it is too big
    print(reprlib.repr(large))
    print("a{:10}b".format("s"))

if __name__ == '__main__':
    experiment2()