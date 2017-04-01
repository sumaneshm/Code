__author__ = 'sumaneshm'


class Student:
    def __init__(self,name, age):
        self.name = name
        self.age = age
    def Print(self):
        print("Student ", self.name, " age :", self.age)

class Employee:
    def __init__(self,name, rollNumber):
        self.name = name
        self.roll = rollNumber
    def Print(s):
        print("Employee ", s.name, " Roll : ", s.roll)

def Create(opt):
    if(opt == 1): return Student("Aadhavan", 5)
    if(opt == 2): return Employee("Sumanesh", "97CS28")


s = Create(1)
s.Print()

# print(s.roll)

e = Create(2)
e.Print()