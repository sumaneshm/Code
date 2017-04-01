class Point2D:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    # simple representation of the object state intended normally for clients
    # default __str__ calls __repr__
    def __str__(self):
        return "({}, {})".format(self.x, self.y)

    # Unambiguous detailed representation of the the object state intended for developers
    #
    def __repr__(self):
        return "Point2D (x: {}, y: {})".format(self.x, self.y)


def experiment1():
    p = Point2D(10, 20)
    print("Str : ", str(p))
    print("Repr :", repr(p))

    print(p)    # uses "str" representation of the object


def experiment2():
    l = [Point2D(i, i*2) for i in range(3)]

if __name__ == '__main__':
    experiment1()