__author__ = 'sumaneshm'


class BaseClass:
    B_static = 10
    def __init__(self):
        self.B_instance = 20

class DerivedClass(BaseClass):
    D_static = 10
    def __init__(self):
        self.D_instance = 20


b = BaseClass()
b.initialized_somewhere = "Test"

#ways to access instance variables which are initialized somewhere other than the class
print(b.initialized_somewhere)
print(b.__dict__['initialized_somewhere'])

print("Ways to access static variable")

print("Normal way : ", BaseClass.B_static)
print("As instance variable :" , b.B_static)
print("__dict__ on the defined class :" , BaseClass.__dict__["B_static"])
print("As instance variable on the derived class : ", DerivedClass.B_static)
# print(DerivedClass.__dict__["B_static"])      #Static variables can only accessed in __dict__ form on the class where it is defined

print("Instance variable vs Static variable")
b.B_static = "Overridden"
print("When overridden as instance, it should return that first : ", b.B_static)
print("When accessing the overridden variable using __dict__, it show correctly : ", b.__dict__["B_static"])
print("Accessing using the class name, gets the static value ", BaseClass.B_static)
print("Accessing using the class name, __dict__ gets static value : ", BaseClass.__dict__["B_static"])

