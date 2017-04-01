import math
import sys

import io


class TriangleError(Exception):
    def __init__(self, text, sides):
        super().__init__(text)
        self._sides = tuple(sides)

    @property
    def sides(self):
        return self._sides

    def __repr__(self):
        return "TriangleError : {!r} {!r}".format(self.args[0], self._sides)

    def __str__(self):
        return "'{}' of '{}'".format(self.args[0], self._sides)

def triangle_area(a, b, c):
    sides = sorted((a, b, c))

    if sides[2] > sides[0] + sides[1]:
        raise TriangleError("Illegal triangle", sides)

    p = (a + b + c) / 2
    area = math.sqrt(p * (p - a) * (p - b) * (p - c))
    return area


def experiment1():
    print(triangle_area(3, 4, 5))
    try:
        illegal = triangle_area(3, 4, 10)
    except TriangleError as e:

        try:
            print(e, file=sys.stdin)
        except io.UnsupportedOperation as f:
            # when a new exception is thrown, the __context__ has to contain InnerException
            # as demonstrated above. here, the inner context of f will be the outer exception e

            # this is "Implicit" chaining of exception
            print(f.__context__ is e)

if __name__ == '__main__':
    experiment1()
