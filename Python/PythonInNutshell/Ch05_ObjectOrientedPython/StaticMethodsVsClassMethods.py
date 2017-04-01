__author__ = 'Sumanesh Magarabooshanam'

class SomeClass:
    def some_instance_method(self,x):
        print("{0},{1}".format(self, x))

    @staticmethod
    def static_method(x):
        print(x)

    @classmethod
    def class_method(cls, x):
        print("{0},{1}".format(cls, x))


s = SomeClass()

print("Instance methods")       #Instance methods usage
s.some_instance_method("Calling a normal method using an instance")
SomeClass.some_instance_method(s, "Calling a normal method in a different way")

print("Calling static method")  #Calling Static methods
s.static_method("Calling static method using an instance")
SomeClass.static_method("Calling static method using the class")

print("Calling class methods")  #Calling class methods
s.class_method("Calling class method using an instance")
SomeClass.class_method("Calling class method using class")
