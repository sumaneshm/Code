__author__ = 'sumaneshm'


class ResultCalcultor:
    def __get__(self, instance, owner):
        return "PASS" if ((instance.m1 > 39) & (instance.m2 > 39) & (instance.m3 > 39)) else "FAIL"
    def __set__(self, instance, value):
        raise "Not valid to set result directly"

class TotalCalcultor:
    def __get__(self, instance, owner):
        return instance.m1 + instance.m2 + instance.m3
    def __set__(self, instance, value):
        raise "Total is readonly"

class AverageCalculator:
    def __get__(self, instance, owner):
        return instance.total / 3
    def __set__(self, instance, value):
        raise "Average is readonly"

class Student:
    def __init__(self, name, m1, m2, m3):
        self.name = name
        self.m1 = m1
        self.m2 = m2
        self.m3 = m3
    result = ResultCalcultor()
    total = TotalCalcultor()
    average = AverageCalculator()

students = [
    Student("Sumanesh", 10, 20, 30),
    Student("Aadhavan", 100, 89, 77),
    Student("Saveetha", 99, 50, 54),
    Student("Aghilan", 90, 80, 78),
    Student("Nila", 80, 88, 98)
]

def PrintDetails(s):
    print("Name : {0}, Result : {1}, Total : {2}, Average : {3:.2f}".format(s.name, s.result, s.total, s.average))

for s in students:
    PrintDetails(s)

s = Student("New Student", 20, 30, 45)
print(s.total)
#s.total = "Overridden"
