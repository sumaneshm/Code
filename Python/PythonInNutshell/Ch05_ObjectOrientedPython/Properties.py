__author__ = 'Sumanesh Magarabooshanam'


class Rectangle(object):
    def __init__(self, length, breadth):
        self.length = length
        self.breadth = breadth

    def getArea(self):
        return self.length * self.breadth
    area = property(getArea, doc="Calculates the area of this rectable")

r = Rectangle(10, 20)
print("Area of the rectangle : ", r.area)